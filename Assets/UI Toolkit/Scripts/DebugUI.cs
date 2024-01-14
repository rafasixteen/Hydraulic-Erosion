using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class DebugUI : MonoBehaviour
    {
        VisualElement root;
        bool isVisible = true;

        private Label fps;
        private float updateTimer = 0f;
        [SerializeField, Min(0)] private float updateInterval = 0.5f;

        private void Awake()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            root.visible = isVisible;

            fps = root.Q<Label>("Fps");

            Label Generation = root.Q<Label>("Generation");
            Label Normalization = root.Q<Label>("Normalization");
            Label Texture = root.Q<Label>("Texture");

            Label TerrainVertices = root.Q<Label>("TerrainVertices");
            Label TerrainTriangles = root.Q<Label>("TerrainTriangles");
            Label TerrainNormals = root.Q<Label>("TerrainNormals");

            Label TerrainWallVertices = root.Q<Label>("TerrainWallVertices");
            Label TerrainWallTriangles = root.Q<Label>("TerrainWallTriangles");
            Label TerrainWallNormals = root.Q<Label>("TerrainWallNormals");

            Label Took = root.Q<Label>("Took");
            Label AvgPerDroplet = root.Q<Label>("AvgPerDroplet");

            OnGenerationCompleted += (double time) => { Generation.text = $"Generation: {time} ms"; };
            OnNormalizationCompleted += (double time) => { Normalization.text = $"Normalization: {time} ms"; };
            OnTextureCompleted += (double time) => { Texture.text = $"Texture: {time} ms"; };

            OnTerrainVerticesCompleted += (double time) => { TerrainVertices.text = $"Vertices: {time} ms"; };
            OnTerrainTrianglesCompleted += (double time) => { TerrainTriangles.text = $"Triangles: {time} ms"; };
            OnTerrainNormalsCompleted += (double time) => { TerrainNormals.text = $"Normals: {time} ms"; };

            OnTerrainWallVerticesCompleted += (double time) => { TerrainWallVertices.text = $"Vertices: {time} ms"; };
            OnTerrainWallTrianglesCompleted += (double time) => { TerrainWallTriangles.text = $"Triangles: {time} ms"; };
            OnTerrainWallNormalsCompleted += (double time) => { TerrainWallNormals.text = $"Normals: {time} ms"; };

            OnErosionCompleted += (double time, int iterations) => { Took.text = $"Took: {time} ms"; AvgPerDroplet.text = $"Avg Per Droplet: {time / iterations:F4} ms"; };
        }

        private void Update()
        {
            UpdateFPS();

            if (Input.GetKeyDown(KeyCode.F3))
            {
                isVisible = !isVisible;

                if (isVisible)
                {
                    root.visible = true;
                }
                else
                {
                    root.visible = false;
                }
            }
        }

        private void UpdateFPS()
        {
            updateTimer += Time.deltaTime;

            if (updateTimer >= updateInterval)
            {
                DrawFPS();
                updateTimer = 0f;
            }
        }

        private void DrawFPS()
        {
            int fpsValue = Mathf.RoundToInt(1f / Time.deltaTime);
            Color color;

            if (fpsValue >= 60)
            {
                color = Color.green;
            }
            else if (fpsValue >= 30)
            {
                color = Color.yellow;
            }
            else
            {
                color = Color.red;
            }

            fps.text = $"FPS: {fpsValue}";
            fps.style.color = color;
        }

        #region Events

        private static event Action<double> OnGenerationCompleted;
        public static void GenerationCompleted(Action method)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnGenerationCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static event Action<double> OnNormalizationCompleted;
        public static void NormalizationCompleted(Action method)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnNormalizationCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static event Action<double> OnTextureCompleted;
        public static void TextureCompleted(Action method)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnTextureCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static event Action<double> OnTerrainVerticesCompleted;
        public static void TerrainVerticesCompleted(Action method)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnTerrainVerticesCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static event Action<double> OnTerrainTrianglesCompleted;
        public static void TerrainTrianglesCompleted(Action method)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnTerrainTrianglesCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static event Action<double> OnTerrainNormalsCompleted;
        public static void TerrainNormalsCompleted(Action method)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnTerrainNormalsCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static event Action<double> OnTerrainWallVerticesCompleted;
        public static void TerrainWallVerticesCompleted(Action method)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnTerrainWallVerticesCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static event Action<double> OnTerrainWallTrianglesCompleted;
        public static void TerrainWallTrianglesCompleted(Action method)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnTerrainWallTrianglesCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static event Action<double> OnTerrainWallNormalsCompleted;
        public static void TerrainWallNormalsCompleted(Action method)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnTerrainWallNormalsCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static event Action<double, int> OnErosionCompleted;
        public static void ErosionCompleted(Action method, int iterations)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            method.Invoke();
            stopwatch.Stop();

            OnErosionCompleted?.Invoke(stopwatch.Elapsed.TotalMilliseconds, iterations);
        }

        #endregion
    }
}