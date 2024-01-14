using Assets.Scripts.Meshes.Generators;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class Preview : MonoBehaviour
    {
        [SerializeField] private Renderer noiseTextureRenderer;

        [SerializeField] private MeshFilter terrainMeshFilter;
        [SerializeField] private MeshFilter terrainWallsMeshFilter;

        [SerializeField] private NoiseData noiseData;
        [SerializeField] private TerrainData terrainData;
        [SerializeField] private ErosionData erosionData;

        [SerializeField] private ComputeShader noiseComputeShader;
        [SerializeField] private ComputeShader erosionComputeShader;

        private NoiseGenerator noiseGenerator;
        private TextureGenerator textureGenerator;
        private TerrainGenerator terrainGenerator;
        private TerrainWallsGenerator terrainWallsGenerator;
        private Erosion erosion;

        private void Awake()
        {
            noiseData.Initialize();
            terrainData.Initialize();
            erosionData.Initialize();

            noiseGenerator = new(noiseData, terrainData, noiseComputeShader);
            textureGenerator = new(noiseTextureRenderer, terrainData);
            terrainGenerator = new(terrainData);
            terrainWallsGenerator = new(terrainData);
            erosion = new(noiseData, terrainData, erosionData, erosionComputeShader);

            EventManager.OnSettingsUIChanged += Generate;
            EventManager.OnErodeButtonClicked += ErodeButtonClicked;

            EventManager.SettingsUIChanged();
        }

        private void OnDestroy()
        {
            EventManager.OnSettingsUIChanged -= Generate;
        }

        private void Update()
        {
            if (erosionData.Animate)
            {
                DebugUI.ErosionCompleted(() => erosion.Erode(erosionData.IterationsPerFrame), erosionData.IterationsPerFrame);
                UpdatePreview();
            }
        }

        private void ErodeButtonClicked()
        {
            DebugUI.ErosionCompleted(() => erosion.Erode(erosionData.Iterations), erosionData.Iterations);
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            textureGenerator.ReinitializeTexture();

            DebugUI.TextureCompleted(textureGenerator.UpdatePixels);

            textureGenerator.DrawTexture(terrainData.Size);

            EventManager.HeightmapTextureCreated();

            Mesh terrainMesh = terrainGenerator.GenerateMesh();
            terrainWallsGenerator.UpdateTerrainMeshVertices(terrainMesh.vertices);
            Mesh terrainWallsMesh = terrainWallsGenerator.GenerateMesh();

            terrainMeshFilter.sharedMesh = terrainMesh;
            terrainWallsMeshFilter.sharedMesh = terrainWallsMesh;
        }

        private void Generate()
        {
            DebugUI.GenerationCompleted(noiseGenerator.Generate);
            DebugUI.NormalizationCompleted(() => noiseGenerator.Normalize(0f, 1f));

            UpdatePreview();
        }
    }
}