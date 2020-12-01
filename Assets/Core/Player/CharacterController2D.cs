using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NotTimeTravel.Core.Player
{
    public class CharacterController2D : MonoBehaviour
    {
        [SerializeField] private float jumpForce = 600f; // Amount of force added when the player jumps.

        [SerializeField]
        private float continuousJumpForce = 1f; // Amount of force added when the player holds the jump button.

        [SerializeField] private float decayRate = 10f;

        [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f; // How much to smooth out the movement

        [SerializeField] private bool airControl; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character

        [SerializeField] private Transform groundCheck; // A position marking where to check if the player is grounded.
        [SerializeField] private GameObject flippableTransform; // Which part of the character should be flipped.

        public UnityEvent onJump;
        
        private const float GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
        private bool _grounded; // Whether or not the player is grounded.
        private Rigidbody2D _rigidBody2D;
        private bool _facingRight = true; // For determining which way the player is currently facing.
        private Vector3 _velocity = Vector3.zero;
        private bool _canHoldJump;
        private bool _isHoldingJump;

        [Header("Events")] [Space] public UnityEvent onLandEvent;

        [System.Serializable]
        public class BoolEvent : UnityEvent<bool>
        {
        }

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();

            if (onLandEvent == null)
                onLandEvent = new UnityEvent();
        }

        private void FixedUpdate()
        {
            bool wasGrounded = _grounded;
            _grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders =
                // ReSharper disable once Unity.PreferNonAllocApi
                Physics2D.OverlapCircleAll(groundCheck.position, GroundedRadius, whatIsGround);
            foreach (Collider2D t in colliders)
            {
                if (t.gameObject == gameObject) continue;

                _grounded = true;
                if (!wasGrounded)
                    onLandEvent.Invoke();
            }
        }


        public void Move(float move, bool jump, bool isPulling = false)
        {
            //only control the player if grounded or airControl is turned on
            if (_grounded || airControl)
            {
                // Move the character by finding the target velocity
                Vector2 velocity = _rigidBody2D.velocity;
                Vector3 targetVelocity = new Vector2(move * 10f, velocity.y);
                // And then smoothing it out and applying it to the character
                _rigidBody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity,
                    movementSmoothing);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !_facingRight && !isPulling)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && _facingRight && !isPulling)
                {
                    // ... flip the player.
                    Flip();
                }
            }

            // If the player should jump...
            if (_grounded && jump)
            {
                // Add a vertical force to the player.
                _grounded = false;

                StopAllCoroutines();
                StartCoroutine(DoJump());
                onJump.Invoke();
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            _facingRight = !_facingRight;

            // Multiply the player's x local scale by -1.
            Transform objectTransform = flippableTransform.transform;
            Vector3 theScale = objectTransform.localScale;
            theScale.x *= -1;
            objectTransform.localScale = theScale;
        }

        private IEnumerator DoJump()
        {
            _canHoldJump = true;
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
            _rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);

            //can be any value, maybe this is a start ascending force, up to you
            float currentForce = continuousJumpForce;

            while (_canHoldJump && _isHoldingJump && currentForce > 0)
            {
                _rigidBody2D.AddForce(Vector2.up * currentForce);

                currentForce -= decayRate * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        public void UpdateHoldingJump(bool holding)
        {
            if (!holding)
            {
                _canHoldJump = false;
            }

            _isHoldingJump = holding;
        }

        public bool IsGrounded()
        {
            return _grounded;
        }

        public void MakeSureFacing(bool right)
        {
            if (right && !_facingRight)
            {
                Flip();
            }

            if (!right && _facingRight)
            {
                Flip();
            }
        }

        public bool IsFacingRight()
        {
            return _facingRight;
        }
    }
}