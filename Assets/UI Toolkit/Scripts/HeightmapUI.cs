using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class HeightmapUI : MonoBehaviour
    {
        [SerializeField] private Renderer noiseTextureRenderer;
        private VisualElement Heightmap;

        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            Heightmap = root.Q<VisualElement>("Heightmap");

            EventManager.OnHeightmapTextureCreated += DrawHeightmapTexture;
        }

        private void OnDestroy()
        {
            EventManager.OnHeightmapTextureCreated -= DrawHeightmapTexture;
        }

        private void DrawHeightmapTexture()
        {
            Heightmap.style.backgroundImage = (StyleBackground)noiseTextureRenderer.material.mainTexture;
        }
    }
}

