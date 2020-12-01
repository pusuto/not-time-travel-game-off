using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace NotTimeTravel.Core.Grid
{
    public class HiddenPassageManager : MonoBehaviour
    {
        public UnityEngine.Grid grid;
        public Light2D lightToHide;
        public float duration;
        public UnityEvent onDiscover;
        public GameObject[] thingsToEnableOnActivate;

        private bool _wasDiscovered;
        private static readonly int VisibilityPercentage = Shader.PropertyToID("VisibilityPercentage");

        public bool WasDiscovered()
        {
            return _wasDiscovered;
        }

        private void Start()
        {
            foreach (GameObject thingToActivate in thingsToEnableOnActivate)
            {
                thingToActivate.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("MainCharacter"))
            {
                return;
            }

            if (_wasDiscovered)
            {
                return;
            }

            _wasDiscovered = true;

            onDiscover.Invoke();

            foreach (Transform child in grid.transform)
            {
                StartCoroutine(ShowPassage(child.GetComponent<Renderer>()));
            }


            foreach (GameObject thingToActivate in thingsToEnableOnActivate)
            {
                thingToActivate.SetActive(true);
            }
        }

        private IEnumerator ShowPassage(Renderer tilemapRenderer)
        {
            yield return Transition.Transition.TransitionFloat(duration, 1,
                tilemapRenderer.material.GetFloat(VisibilityPercentage), 0,
                newVisibilityPercentage =>
                {
                    tilemapRenderer.material.SetFloat(VisibilityPercentage, newVisibilityPercentage);
                    foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
                    {
                        Color spriteRendererColor = spriteRenderer.color;
                        spriteRendererColor.a = newVisibilityPercentage;
                        spriteRenderer.color = spriteRendererColor;
                    }
                });

            Destroy(gameObject);
            Destroy(lightToHide);
        }
    }
}