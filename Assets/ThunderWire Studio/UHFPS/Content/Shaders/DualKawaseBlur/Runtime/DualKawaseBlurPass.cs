using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

namespace UHFPS.Rendering
{
    public class DualKawaseBlurPass : ScriptableRenderPass
    {
        internal struct Level
        {
            internal int down;
            internal int up;
        }

        private static readonly int BlurOffset = Shader.PropertyToID("_Offset");
        private const string PROFILER_TAG = "DualKawaseBlur";

        internal Level[] m_Pyramid;
        const int k_MaxPyramidSize = 16;
        private readonly Material material;

        public DualKawaseBlurPass(RenderPassEvent renderPassEvent, Material material)
        {
            this.renderPassEvent = renderPassEvent;
            this.material = material;

            m_Pyramid = new Level[k_MaxPyramidSize];
            for (int i = 0; i < k_MaxPyramidSize; i++)
            {
                m_Pyramid[i] = new Level
                {
                    down = Shader.PropertyToID("_BlurMipDown" + i),
                    up = Shader.PropertyToID("_BlurMipUp" + i)
                };
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.isSceneViewCamera) return;
            if (!renderingData.cameraData.postProcessEnabled) return;

            VolumeStack stack = VolumeManager.instance.stack;
            DualKawaseBlur blurVolume = stack.GetComponent<DualKawaseBlur>();
            if (blurVolume == null || !blurVolume.IsActive()) return;

            var cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler(PROFILER_TAG)))
            {
                Camera camera = renderingData.cameraData.camera;
                int tw = (int)(camera.pixelWidth / blurVolume.RTDownScaling.value);
                int th = (int)(camera.pixelHeight / blurVolume.RTDownScaling.value);

                material.SetFloat(BlurOffset, Mathf.Sqrt(blurVolume.BlurRadius.value));

#if UNITY_2022_1_OR_NEWER
                var cameraColor = renderingData.cameraData.renderer.cameraColorTargetHandle.rt;
#else
			    var cameraColor = renderingData.cameraData.renderer.cameraColorTarget;
#endif

                // Downsample
                RenderTargetIdentifier lastDown = cameraColor;
                for (int i = 0; i < blurVolume.Iteration.value; i++)
                {
                    int mipDown = m_Pyramid[i].down;
                    int mipUp = m_Pyramid[i].up;
                    cmd.GetTemporaryRT(mipDown, tw, th, 0, FilterMode.Bilinear);
                    cmd.GetTemporaryRT(mipUp, tw, th, 0, FilterMode.Bilinear);
                    cmd.Blit(lastDown, mipDown, material, 0);

                    lastDown = mipDown;
                    tw = Mathf.Max(tw / 2, 1);
                    th = Mathf.Max(th / 2, 1);
                }

                // Upsample
                int lastUp = m_Pyramid[blurVolume.Iteration.value - 1].down;
                for (int i = blurVolume.Iteration.value - 2; i >= 0; i--)
                {
                    int mipUp = m_Pyramid[i].up;
                    cmd.Blit(lastUp, mipUp, material, 1);
                    lastUp = mipUp;
                }

                // Render blurred texture in blend pass
                cmd.Blit(lastUp, cameraColor, material, 1);

                // Cleanup
                int originalLastUp = m_Pyramid[blurVolume.Iteration.value - 1].down;
                for (int i = 0; i < blurVolume.Iteration.value; i++)
                {
                    if (m_Pyramid[i].down != originalLastUp)
                        cmd.ReleaseTemporaryRT(m_Pyramid[i].down);
                    if (m_Pyramid[i].up != originalLastUp)
                        cmd.ReleaseTemporaryRT(m_Pyramid[i].up);
                }
            }
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
    }
}