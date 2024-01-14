using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class TextureGenerator
    {
        private readonly TerrainData terrainData;
        private readonly Renderer textureRenderer;

        private NativeArray<Color32> pixels;
        private readonly Texture2D noiseTexture;

        public TextureGenerator(Renderer textureRenderer, TerrainData terrainData)
        {
            this.textureRenderer = textureRenderer;
            this.terrainData = terrainData;

            noiseTexture = new(1, 1)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
        }

        /// <summary>
        /// Reinitializes The Texture With New Heightmap Resolution
        /// </summary>
        public void ReinitializeTexture()
        {
            pixels = new(terrainData.Heightmap.Length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            noiseTexture.Reinitialize(terrainData.Resolution, terrainData.Resolution);
        }

        public void UpdatePixels()
        {
            Jobs.UpdatePixels.ScheduleParallel(terrainData, pixels, default);
            noiseTexture.SetPixelData(pixels, 0);
            pixels.Dispose();
        }

        public void DrawTexture(int size)
        {
            noiseTexture.Apply();

            textureRenderer.sharedMaterial.mainTexture = noiseTexture;
            textureRenderer.transform.localScale = new Vector3(size / 10f, 1, size / 10f);
        }
    }
}