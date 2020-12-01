using NotTimeTravel.Core.Speech;

#if UNITY_EDITOR

namespace NotTimeTravel.Core.Util
{
    public static class TilemapUtil
    {
        [UnityEditor.MenuItem("Generate tiles and shadows", menuItem = "Tools/Generate tiles and shadows")]
        public static void GenerateTilesAndShadows()
        {
            GlobalInstanceManager.GetAssetManager().SetAndUpdate();
            ShadowCaster2DGenerator.GenerateShadowCasters();
        }
    }
}

#endif