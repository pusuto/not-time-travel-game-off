using NotTimeTravel.Core.Door;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Elevator
{
    public class ElevatorManager : MonoBehaviour
    {
        private DoorManager _door;

        private void Start()
        {
            _door = GetComponent<DoorManager>();
        }

        public void Activate()
        {
            _door.SetStatus(DoorStatus.Closed);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("MainCharacter"))
            {
                return;
            }

            other.gameObject.transform.parent = transform;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("MainCharacter"))
            {
                return;
            }

            other.gameObject.transform.parent = null;
        }
    }
}