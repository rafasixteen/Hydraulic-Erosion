using System;

public static class EventManager
{
    public static event Action<int> OnTerrainChanged;
    public static void TerrainSizeChanged(int terrainSize)
    {
        OnTerrainChanged?.Invoke(terrainSize);
    }

    public static event Action OnSettingsUIChanged;
    public static void SettingsUIChanged()
    {
        OnSettingsUIChanged?.Invoke();
    }

    public static event Action OnHeightmapTextureCreated;
    public static void HeightmapTextureCreated()
    {
        OnHeightmapTextureCreated?.Invoke();
    }

    public static event Action OnErodeButtonClicked;
    public static void ErodeButtonClicked()
    {
        OnErodeButtonClicked?.Invoke();
    }
}
