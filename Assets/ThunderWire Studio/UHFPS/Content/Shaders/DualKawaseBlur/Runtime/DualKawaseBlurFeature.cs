using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

namespace UHFPS.Rendering
{
    public class DualKawaseBlurFeature : ScriptableRendererFeature
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public Shader shader;

        private Material material;
        private DualKawaseBlurPass pass;

        public override void Create()
        {
            if (shader == null) shader = Shader.Find("Hidden/CustomPostEffect/DualKawaseBlur");
            if (material == null) material = CoreUtils.CreateEngineMaterial(shader);
            pass = new DualKawaseBlurPass(renderPassEvent, material);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (pass != null) renderer.EnqueuePass(pass);
        }
    }
}