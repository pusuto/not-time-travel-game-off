using NotTimeTravel.Core.Resource;
using UnityEngine;

namespace NotTimeTravel.Core.Block
{
    public class BlockManager : MonoBehaviour
    {
        public bool skipBackground;
        public string colorScheme;
        public bool primaryOverride;
        public Color tint;
        public SpriteRenderer background;
        public SpriteRenderer top;
        public SpriteRenderer left;
        public SpriteRenderer bottom;
        public SpriteRenderer right;
        public bool hasTop;
        public bool hasLeft;
        public bool hasBottom;
        public bool hasRight;

        private static readonly int Tint = Shader.PropertyToID("Tint");

        private void Start()
        {
            if (!skipBackground) return;

            background.material.SetColor(Tint, tint);
        }

#if UNITY_EDITOR
        [ContextMenu("Create block sprites")]
        public void CreateBlockSprites()
        {
            AssetManager am = GameObject.FindGameObjectWithTag("World").GetComponent<AssetManager>();

            if (!skipBackground)
            {
                background.sprite = am.GetMainColorTile(colorScheme).sprite;
                background.color = am.GetMainColor(colorScheme);
            }

            top.sprite = am.GetTile(colorScheme, "Top").sprite;
            top.color = am.GetBorderColor(colorScheme, "Top");
            left.sprite = am.GetTile(colorScheme, "Left").sprite;
            left.color = am.GetBorderColor(colorScheme, "Left");
            bottom.sprite = am.GetTile(colorScheme, "Bottom").sprite;
            bottom.color = am.GetBorderColor(colorScheme, "Bottom");
            right.sprite = am.GetTile(colorScheme, "Right").sprite;
            right.color = am.GetBorderColor(colorScheme, "Right");

            top.gameObject.SetActive(hasTop);
            left.gameObject.SetActive(hasLeft);
            bottom.gameObject.SetActive(hasBottom);
            right.gameObject.SetActive(hasRight);
        }
#endif
    }
}