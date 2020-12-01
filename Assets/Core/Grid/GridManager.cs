using System.Linq;
using NotTimeTravel.Core.Block;
using NotTimeTravel.Core.Model;
using NotTimeTravel.Core.Resource;
using NotTimeTravel.Core.Speech;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NotTimeTravel.Core.Grid
{
    public class GridManager : MonoBehaviour
    {
        public Tilemap source;
        public Tilemap top;
        public Tilemap left;
        public Tilemap bottom;
        public Tilemap right;
        public Tilemap color;
        public Tilemap decalLight;
        public Tilemap decalDark;
        public Tilemap physicsColor;
        public Tilemap background;
        public ColorableGridSection[] sections;

        private AssetManager _assetManager;

        private void Start()
        {
            _assetManager = GlobalInstanceManager.GetAssetManager();
        }

#if UNITY_EDITOR
        [ContextMenu("Generate")]
        public void Generate()
        {
            Start();

            foreach (BlockManager blockManager in GlobalInstanceManager.GetGameManager().gameObject
                .GetComponentsInChildren<BlockManager>())
            {
                Vector3 position = blockManager.gameObject.transform.position;
                float blockX = position.x;
                float blockY = position.y;
                ColorableGridSection gridSection =
                    sections.First(section =>
                        blockX >= section.originX && blockX <= section.targetX && blockY >= section.originY &&
                        blockY <= section.targetY);

                if (gridSection != null)
                {
                    blockManager.colorScheme = blockManager.primaryOverride
                        ? gridSection.colorScheme
                        : gridSection.secondaryColorScheme;
                }

                blockManager.tint = _assetManager.GetMainColor(blockManager.colorScheme);

                EditorUtility.SetDirty(blockManager);
                blockManager.CreateBlockSprites();
            }

            foreach (ColorableGridSection gridSection in sections)
            {
                for (int i = gridSection.originX; i < gridSection.targetX; i++)
                {
                    for (int j = gridSection.originY; j < gridSection.targetY; j++)
                    {
                        Erase(i, j);

                        if (!HasTile(i, j))
                        {
                            if (HasTile(i, j, background))
                            {
                                Vector3Int position = new Vector3Int(i, j, 0);

                                if (Random.Range(0, 20) > 18)
                                {
                                    int variant = Random.Range(1, 13);
                                    decalLight.SetTile(position,
                                        _assetManager.GetDecalTile(gridSection.backgroundColorScheme, true, variant));
                                    decalDark.SetTile(position,
                                        _assetManager.GetDecalTile(gridSection.backgroundColorScheme, false, variant));
                                }

                                color.SetTile(position,
                                    _assetManager.GetMainColorTile(gridSection.backgroundColorScheme));
                            }

                            continue;
                        }

                        Add(i, j, gridSection.colorScheme, gridSection);
                    }
                }
            }
        }

        private bool HasTile(int x, int y, Tilemap whereToLook = null)
        {
            return (whereToLook == null ? source : whereToLook).GetTile(new Vector3Int(x, y, 0)) != null;
        }

        private bool IsOutside(int x, int y, ColorableGridSection gridSection)
        {
            return x < gridSection.originX || x >= gridSection.targetX || y < gridSection.originY ||
                   y >= gridSection.targetY;
        }

        private void Erase(int x, int y)
        {
            foreach (Tilemap tilemap in new[] {top, left, bottom, right, color, decalLight, decalDark, physicsColor})
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }

        private void Add(int x, int y, string colorScheme, ColorableGridSection gridSection)
        {
            Vector3Int position = new Vector3Int(x, y, 0);
            physicsColor.SetTile(position, _assetManager.GetMainColorTile(colorScheme));

            if (Random.Range(0, 10) > 8)
            {
                int variant = Random.Range(1, 13);
                decalLight.SetTile(position, _assetManager.GetDecalTile(colorScheme, true, variant));
                decalDark.SetTile(position, _assetManager.GetDecalTile(colorScheme, false, variant));
            }

            AddTop(x, y, colorScheme, gridSection);
            AddLeft(x, y, colorScheme, gridSection);
            AddBottom(x, y, colorScheme, gridSection);
            AddRight(x, y, colorScheme, gridSection);
        }

        private void AddTop(int x, int y, string colorScheme, ColorableGridSection gridSection)
        {
            if (HasTile(x, y + 1) && !IsOutside(x, y + 1, gridSection))
            {
                return;
            }

            top.SetTile(new Vector3Int(x, y, 0), _assetManager.GetTile(colorScheme, "Top"));
        }

        private void AddLeft(int x, int y, string colorScheme, ColorableGridSection gridSection)
        {
            if (HasTile(x - 1, y) && !IsOutside(x - 1, y, gridSection))
            {
                return;
            }

            left.SetTile(new Vector3Int(x, y, 0), _assetManager.GetTile(colorScheme, "Left"));
        }

        private void AddBottom(int x, int y, string colorScheme, ColorableGridSection gridSection)
        {
            if (HasTile(x, y - 1) && !IsOutside(x, y - 1, gridSection))
            {
                return;
            }

            bottom.SetTile(new Vector3Int(x, y, 0), _assetManager.GetTile(colorScheme, "Bottom"));
        }

        private void AddRight(int x, int y, string colorScheme, ColorableGridSection gridSection)
        {
            if (HasTile(x + 1, y) && !IsOutside(x + 1, y, gridSection))
            {
                return;
            }

            right.SetTile(new Vector3Int(x, y, 0), _assetManager.GetTile(colorScheme, "Right"));
        }
#endif
    }
}