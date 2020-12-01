using System.Linq;
using NotTimeTravel.Core.Grid;
using NotTimeTravel.Core.Model;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace NotTimeTravel.Core.Resource
{
    public class AssetManager : MonoBehaviour
    {
        public ColorScheme[] colorSchemes;

#if UNITY_EDITOR
        public Tile GetMainColorTile(ColorScheme color)
        {
            return AssetDatabase.LoadAssetAtPath<Tile>($"Assets/Textures/Tilesets/WallTiles/Color{color.name}.asset");
        }

        public Tile GetMainColorTile(string scheme)
        {
            return GetMainColorTile(GetColorByName(scheme));
        }


        public Tile GetTile(ColorScheme scheme, string direction, int variant)
        {
            return AssetDatabase.LoadAssetAtPath<Tile>(
                $"Assets/Textures/Tilesets/WallTiles/Color{scheme.name}/{direction}{variant}.asset");
        }

        public Tile GetTile(ColorScheme scheme, string direction)
        {
            return GetTile(scheme, direction, Random.Range(0, 9));
        }

        public Tile GetTile(string scheme, string direction)
        {
            return GetTile(GetColorByName(scheme), direction);
        }

        public Tile GetDecalTile(ColorScheme scheme, bool isLight, int variant)
        {
            string lightOrDark = isLight ? "Light" : "Dark";
            return AssetDatabase.LoadAssetAtPath<Tile>(
                $"Assets/Textures/Tilesets/WallTiles/Color{scheme.name}/Decal{lightOrDark}{variant}.asset");
        }

        public Tile GetDecalTile(string scheme, bool isLight, int variant)
        {
            return GetDecalTile(GetColorByName(scheme), isLight, variant);
        }

        public Tile GetDecalTile(ColorScheme scheme, bool isLight)
        {
            return GetDecalTile(scheme, isLight, Random.Range(1, 13));
        }

        public Tile GetDecalTile(string scheme, bool isLight)
        {
            return GetDecalTile(GetColorByName(scheme), isLight);
        }

        public Color GetMainColor(ColorScheme color)
        {
            return color.color;
        }

        public Color GetMainColor(string scheme)
        {
            return GetMainColor(GetColorByName(scheme));
        }

        public Color GetBorderColor(ColorScheme colorScheme, string direction)
        {
            return direction == "Top" || direction == "Left" ? colorScheme.lightColor : colorScheme.darkColor;
        }

        public Color GetBorderColor(string scheme, string direction)
        {
            return GetBorderColor(GetColorByName(scheme), direction);
        }

        [ContextMenu("Set colors")]
        public void SetColors()
        {
            foreach (ColorScheme colorScheme in colorSchemes)
            {
                GetMainColorTile(colorScheme).color = GetMainColor(colorScheme);

                foreach (string direction in new[] {"Top", "Left", "Bottom", "Right"})
                {
                    for (int variant = 0; variant <= 8; variant++)
                    {
                        GetTile(colorScheme, direction, variant).color = GetBorderColor(colorScheme, direction);
                    }
                }

                for (int variant = 1; variant <= 12; variant++)
                {
                    GetDecalTile(colorScheme, true, variant).color = colorScheme.lightColor;
                    GetDecalTile(colorScheme, false, variant).color = colorScheme.darkColor;
                }
            }
        }

        [ContextMenu("Update colors")]
        public void UpdateColors()
        {
            foreach (GridManager gridManager in GetComponentsInChildren<GridManager>())
            {
                gridManager.Generate();
            }
        }

        [ContextMenu("Set and update")]
        public void SetAndUpdate()
        {
            SetColors();
            UpdateColors();
        }

        private ColorScheme GetColorByName(string schemeName)
        {
            return colorSchemes.First(scheme => scheme.name == schemeName);
        }
#endif
    }
}