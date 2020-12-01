using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace NotTimeTravel.Core.Prop
{
    public class LightWave : MonoBehaviour
    {
        public float speed;
        public float range;
        public Light2D light2D;

        private float _initialRadius;

        private void Start()
        {
            _initialRadius = light2D.pointLightOuterRadius;
        }

        private void Update()
        {
            light2D.pointLightOuterRadius = _initialRadius + Mathf.PingPong(Time.time * speed, range);
        }
    }
}