using UnityEngine;

namespace NotTimeTravel.Core.PressurePlate
{
    public class ConnectedPlateManager : MonoBehaviour
    {
        public PressurePlateManager otherPressurePlate;

        public void FixedUpdate()
        {
            if (otherPressurePlate == null)
            {
                return;
            }

            Vector2 force = GetComponentInChildren<SpringJoint2D>().reactionForce;
            try
            {
                otherPressurePlate.GetComponentInChildren<SpringJoint2D>().GetComponent<Rigidbody2D>()
                    .AddForce(-1 * force);
            }
            catch
            {
                // ignored
            }
        }
    }
}