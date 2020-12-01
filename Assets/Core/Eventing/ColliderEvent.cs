using NotTimeTravel.Core.Logic;
using NotTimeTravel.Core.Model;
using UnityEngine;
using UnityEngine.Events;

namespace NotTimeTravel.Core.Eventing
{
    public class ColliderEvent : MonoBehaviour
    {
        public UnityEvent onCollide;
        public Times times;
        public ColliderActor actor;

        private int _timesCalled;
        private Conditions _conditions;
        private GameActions _actions;

        private void Start()
        {
            _conditions = GetComponent<Conditions>();
            _actions = GetComponent<GameActions>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (actor == ColliderActor.MainCharacter && !other.CompareTag("MainCharacter"))
            {
                return;
            }

            if (actor == ColliderActor.Box && !other.CompareTag("Box"))
            {
                return;
            }

            if (times == Times.Once && _timesCalled > 0)
            {
                return;
            }

            if (_conditions != null && !_conditions.AreCleared())
            {
                return;
            }

            _timesCalled++;
            onCollide.Invoke();

            if (_actions != null)
            {
                _actions.Invoke();
            }
        }
    }
}