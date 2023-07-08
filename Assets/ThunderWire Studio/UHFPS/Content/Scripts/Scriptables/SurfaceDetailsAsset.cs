using System;
using System.Linq;
using UnityEngine;
using UHFPS.Runtime;

namespace UHFPS.Scriptable
{
    [CreateAssetMenu(fileName = "SurfaceDetails", menuName = "UHFPS/Game/Surface Details")]
    public class SurfaceDetailsAsset : ScriptableObject
    {
        public enum SurfaceDetectionEnum { Tag, Texture, Both }

        public SurfaceDetails[] Surfaces;

        [Serializable]
        public struct SurfaceDetails
        {
            public string SurfaceName;
            public Surface SurfaceProperties;
            public Footsteps FootstepProperties;
            public ImpactMark ImpactProperties;
            public Multipliers MultiplierProperties;
        }

        [Serializable]
        public struct Surface
        {
            public Tag SurfaceTag;
            public Texture2D[] SurfaceTextures;
        }

        [Serializable]
        public struct Footsteps
        {
            public AudioClip[] SurfaceFootsteps;
            public AudioClip[] SurfaceLandSteps;
        }

        [Serializable]
        public struct ImpactMarkType
        {
            public GameObject[] ImpactMarks;
            public AudioClip[] ImpactSounds;
        }

        [Serializable]
        public struct ImpactMark
        {
            public GameObject[] SurfaceBulletmarks;
            public AudioClip[] BulletmarkImpacts;

            public ImpactMarkType SlashImpactType;
            public ImpactMarkType StabImpactType;
        }

        [Serializable]
        public sealed class Multipliers
        {
            public float FootstepsMultiplier = 1f;
            public float LandStepsMultiplier = 1f;
        }

        /// <summary>
        /// Get Surface Details using the selected detection type.
        /// </summary>
        public SurfaceDetails? GetSurfaceDetails(GameObject surfaceUnder, Vector3 hitPosition, SurfaceDetectionEnum surfaceDetection)
        {
            SurfaceDetails? surfaceDetails = null;

            if (surfaceUnder != null)
            {
                if (surfaceUnder.TryGetComponent(out Terrain terrain))
                {
                    surfaceDetails = GetTerrainSurfaceDetails(terrain, hitPosition);
                }
                else if (surfaceDetection == SurfaceDetectionEnum.Tag)
                {
                    surfaceDetails = GetTagSurfaceDetails(surfaceUnder);
                }
                else if (surfaceDetection == SurfaceDetectionEnum.Texture)
                {
                    surfaceDetails = GetTexSurfaceDetails(surfaceUnder);
                }
                else if (surfaceDetection == SurfaceDetectionEnum.Both)
                {
                    surfaceDetails = GetSurfaceDetails(surfaceUnder);
                }
            }

            if (surfaceDetails == null && Surfaces.Length > 0)
                surfaceDetails = Surfaces[0];

            return surfaceDetails;
        }

        /// <summary>
        /// Get Surface Details which contains a specified Tag.
        /// </summary>
        public SurfaceDetails? GetSurfaceDetails(string tag)
        {
            foreach (var surface in Surfaces)
            {
                if (surface.SurfaceProperties.SurfaceTag == tag)
                {
                    return surface;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Surface Details which contains a specified Texture.
        /// </summary>
        public SurfaceDetails? GetSurfaceDetails(Texture2D texture)
        {
            foreach (var surface in Surfaces)
            {
                if (surface.SurfaceProperties.SurfaceTextures.Any(x => x == texture))
                {
                    return surface;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Surface Details which contains a specified Textures.
        /// </summary>
        public SurfaceDetails? GetSurfaceDetails(Texture2D[] textures)
        {
            foreach (var surface in Surfaces)
            {
                if (surface.SurfaceProperties.SurfaceTextures.Any(x => textures.Any(y => x == y)))
                {
                    return surface;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Surface Details by a Tag from the GameObject.
        /// </summary>
        public SurfaceDetails? GetTagSurfaceDetails(GameObject gameObject)
        {
            return GetSurfaceDetails(gameObject.tag);
        }

        /// <summary>
        /// Get Surface Details by a Texture from the GameObject.
        /// </summary>
        public SurfaceDetails? GetTexSurfaceDetails(GameObject gameObject)
        {
            if(gameObject.TryGetComponent(out MeshRenderer renderer))
            {
                Texture texture = renderer.material.mainTexture;
                return GetSurfaceDetails((Texture2D)texture);
            }

            return null;
        }

        /// <summary>
        /// Get Surface Details by a Texture or Tag from the GameObject.
        /// </summary>
        public SurfaceDetails? GetSurfaceDetails(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out MeshRenderer renderer))
            {
                Texture texture = renderer.material.mainTexture;
                SurfaceDetails? surface = GetSurfaceDetails((Texture2D)texture);
                if (surface != null) return surface;
            }

            SurfaceDetails? tagSurface = GetSurfaceDetails(gameObject.tag);
            if(tagSurface != null) return tagSurface;

            return null;
        }

        /// <summary>
        /// Get Surface Details at the world Terrain position.
        /// </summary>
        public SurfaceDetails? GetTerrainSurfaceDetails(Terrain terrain, Vector3 worldPos)
        {
            Texture2D terrainTexture = TerrainPosToTex(terrain, worldPos);
            if(terrainTexture != null)
            {
                var surfaceDetails = GetSurfaceDetails(terrainTexture);
                if(surfaceDetails.HasValue) return surfaceDetails.Value;
            }

            return null;
        }

        private Texture2D TerrainPosToTex(Terrain terrain, Vector3 worldPos)
        {
            float[] mix = TerrainTextureMix(terrain, worldPos);
            TerrainLayer[] terrainLayers = terrain.terrainData.terrainLayers;

            float maxMix = 0;
            int maxIndex = 0;

            for (int n = 0; n < mix.Length; n++)
            {
                if (mix[n] > maxMix)
                {
                    maxIndex = n;
                    maxMix = mix[n];
                }
            }

            if (terrainLayers.Length > 0 && terrainLayers.Length >= maxIndex)
                return terrainLayers[maxIndex].diffuseTexture;

            return null;
        }

        private float[] TerrainTextureMix(Terrain terrain, Vector3 worldPos)
        {
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPos = terrain.transform.position;

            int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
            int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

            float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
            float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

            for (int n = 0; n < cellMix.Length; n++)
            {
                cellMix[n] = splatmapData[0, 0, n];
            }

            return cellMix;
        }
    }
}