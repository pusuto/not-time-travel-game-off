using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace NotTimeTravel.Core.Prop
{
    public class Candle : MonoBehaviour
    {
        public float minChange = .05f;
        public float maxChange = .2f;
        public float minSecondsToChange = .2f;
        public float maxSecondsToChange = .7f;

        private Light2D _light;
        private bool _changing;
        private float _startingIntensity;

        public void Start()
        {
            _light = GetComponent<Light2D>();
            _startingIntensity = _light.intensity;
        }

        public void Update()
        {
            if (_changing)
            {
                return;
            }

            float randomChange = Random.Range(minChange, maxChange);
            float targetIntensity =
                _light.intensity > _startingIntensity ? _light.intensity - randomChange : _light.intensity + randomChange;
            float changeInSeconds = Random.Range(minSecondsToChange, maxSecondsToChange);

            StartCoroutine(ChangeLightIntensity(targetIntensity, changeInSeconds));
        }

        private IEnumerator ChangeLightIntensity(float targetIntensity, float changeInSeconds)
        {
            _changing = true;
            float elapsedTime = 0f;

            while (elapsedTime <= changeInSeconds)
            {
                float newIntensity =
                    Mathf.Lerp(_light.intensity, targetIntensity, elapsedTime / changeInSeconds);
                _light.intensity = newIntensity;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _changing = false;
        }
    }
}