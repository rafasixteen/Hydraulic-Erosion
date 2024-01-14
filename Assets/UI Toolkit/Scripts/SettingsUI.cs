using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private NoiseData NoiseData;
        [SerializeField] private TerrainData TerrainData;
        [SerializeField] private ErosionData ErosionData;

        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            #region NoiseData

            EnumField NoiseType = root.Q<EnumField>("NoiseType");
            EnumField FractalType = root.Q<EnumField>("FractalType");
            Button RandomSeed = root.Q<Button>("RandomSeed");
            IntegerField Seed = root.Q<IntegerField>("Seed");
            SliderInt Octaves = root.Q<SliderInt>("Octaves");
            Slider Lacunarity = root.Q<Slider>("Lacunarity");
            Slider Gain = root.Q<Slider>("Gain");
            Slider Frequency = root.Q<Slider>("Frequency");

            NoiseType.value = NoiseData.NoiseType;
            FractalType.value = NoiseData.FractalType;
            Seed.value = NoiseData.Seed;
            Octaves.value = NoiseData.Octaves;
            Lacunarity.value = NoiseData.Lacunarity;
            Gain.value = NoiseData.Gain;
            Frequency.value = NoiseData.Frequency;

            NoiseType.RegisterValueChangedCallback(OnNoiseTypeChanged);
            FractalType.RegisterValueChangedCallback(OnFractalTypeChanged);
            RandomSeed.clicked += () => RandomButton_clicked(Seed);
            Seed.RegisterValueChangedCallback(OnSeedChanged);
            Octaves.RegisterValueChangedCallback(OnOctavesChanged);
            Lacunarity.RegisterValueChangedCallback(OnLacunarityChanged);
            Gain.RegisterValueChangedCallback(OnGainChanged);
            Frequency.RegisterValueChangedCallback(OnFrequencyChanged);

            #endregion

            #region TerrainData

            SliderInt Resolution = root.Q<SliderInt>("Resolution");
            SliderInt Size = root.Q<SliderInt>("Size");
            Slider Multiplier = root.Q<Slider>("Multiplier");

            Resolution.value = TerrainData.Resolution;
            Size.value = TerrainData.Size;
            Multiplier.value = TerrainData.HeightMultiplier;

            Resolution.RegisterValueChangedCallback(OnResolutionChanged);
            Size.RegisterValueChangedCallback(OnSizeChanged);
            Multiplier.RegisterValueChangedCallback(OnHeightMultiplierChanged);

            #endregion

            #region ErosionData

            Button Erode = root.Q<Button>("Erode");
            SliderInt Iterations = root.Q<SliderInt>("Iterations");
            SliderInt IterationsPerFrame = root.Q<SliderInt>("IterationsPerFrame");
            Toggle Animate = root.Q<Toggle>("Animate");

            Iterations.value = ErosionData.Iterations;
            IterationsPerFrame.value = ErosionData.IterationsPerFrame;
            Animate.value = ErosionData.Animate;

            Erode.clicked += Erode_clicked;
            Iterations.RegisterValueChangedCallback(OnIterationsChanged);
            IterationsPerFrame.RegisterValueChangedCallback(OnIterationsPerFrameChanged);
            Animate.RegisterValueChangedCallback(OnAnimateChanged);

            #endregion
        }

        private void RandomButton_clicked(IntegerField seedField)
        {
            NoiseData.RandomSeed();
            seedField.value = NoiseData.Seed;
        }

        private void OnSeedChanged(ChangeEvent<int> evt)
        {
            NoiseData.Seed = evt.newValue;
            EventManager.SettingsUIChanged();
        }

        private void Erode_clicked()
        {
            EventManager.ErodeButtonClicked();
        }

        private void OnAnimateChanged(ChangeEvent<bool> evt)
        {
            ErosionData.Animate = evt.newValue;
        }

        private void OnIterationsPerFrameChanged(ChangeEvent<int> evt)
        {
            ErosionData.IterationsPerFrame = evt.newValue;
        }

        private void OnIterationsChanged(ChangeEvent<int> evt)
        {
            ErosionData.Iterations = evt.newValue;
        }

        private void OnNoiseTypeChanged(ChangeEvent<Enum> evt)
        {
            NoiseData.NoiseType = (FastNoiseLite.NoiseType)evt.newValue;
            EventManager.SettingsUIChanged();
        }

        private void OnFractalTypeChanged(ChangeEvent<Enum> evt)
        {
            NoiseData.FractalType = (FastNoiseLite.FractalType)evt.newValue;
            EventManager.SettingsUIChanged();
        }

        private void OnOctavesChanged(ChangeEvent<int> evt)
        {
            NoiseData.Octaves = evt.newValue;
            EventManager.SettingsUIChanged();
        }

        private void OnLacunarityChanged(ChangeEvent<float> evt)
        {
            NoiseData.Lacunarity = evt.newValue;
            EventManager.SettingsUIChanged();
        }

        private void OnGainChanged(ChangeEvent<float> evt)
        {
            NoiseData.Gain = evt.newValue;
            EventManager.SettingsUIChanged();
        }

        private void OnFrequencyChanged(ChangeEvent<float> evt)
        {
            NoiseData.Frequency = evt.newValue;
            EventManager.SettingsUIChanged();
        }

        private void OnResolutionChanged(ChangeEvent<int> evt)
        {
            TerrainData.Resolution = evt.newValue;
            EventManager.SettingsUIChanged();
        }

        private void OnSizeChanged(ChangeEvent<int> evt)
        {
            TerrainData.Size = evt.newValue;
            EventManager.SettingsUIChanged();
        }

        private void OnHeightMultiplierChanged(ChangeEvent<float> evt)
        {
            TerrainData.HeightMultiplier = evt.newValue;
            EventManager.SettingsUIChanged();
        }
    }
}