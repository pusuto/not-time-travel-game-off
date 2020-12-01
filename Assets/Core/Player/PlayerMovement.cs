using System;
using NotTimeTravel.Core.Input;
using NotTimeTravel.Core.Model;
using NotTimeTravel.Core.Speech;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NotTimeTravel.Core.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController2D controller;
        public float runSpeed = 100f;
        public float pullingSpeedMultiplier;
        public Animator animator;
        public Transform pullingAnchor;

        private float _horizontalMovement;
        private bool _jump;
        private Rigidbody2D _thingToPull;
        private bool _isPulling;
        private bool _isTryingToPull;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int JumpingUpUpwards = Animator.StringToHash("JumpingUpUpwards");
        private static readonly int JumpingUpDownwards = Animator.StringToHash("JumpingUpDownwards");
        private static readonly int JumpingForwardUpwards = Animator.StringToHash("JumpingForwardUpwards");
        private static readonly int JumpingForwardDownwards = Animator.StringToHash("JumpingForwardDownwards");
        private static readonly int Pushing = Animator.StringToHash("Pushing");
        private static readonly int Pulling = Animator.StringToHash("Pulling");

        public bool IsPulling(Rigidbody2D which)
        {
            return _isPulling && GetComponentInChildren<FixedJoint2D>().connectedBody == which;
        }

        private void Start()
        {
            if (!GetComponent<CharacterManager>().isMain)
            {
                return;
            }

            GlobalInstanceManager.GetStateManager().StateChanged += OnCannotMove;

            InputManager.OnMove(OnMove);
            InputManager.OnJump(OnJump);
            InputManager.OnPull(OnPull);
        }

        private void OnCannotMove(object caller, EventArgs change)
        {
            if (change is StateChangeArgs<bool> boolChange && boolChange.PropertyName == "CanMove")
            {
                if (!boolChange.NewValue)
                {
                    StopMoving();
                }
            }
        }

        private void OnDestroy()
        {
            InputManager.OnMove(OnMove, true);
            InputManager.OnJump(OnJump, true);
            InputManager.OnPull(OnPull, true);
        }

        private void FixedUpdate()
        {
            if (_horizontalMovement == 0 || !_isTryingToPull)
            {
                StopPulling();
            }

            animator.SetBool(Pushing, _thingToPull != null && _horizontalMovement != 0 && !_isPulling);
            animator.SetBool(Pulling, _isTryingToPull && _isPulling);

            if (_thingToPull != null && _isTryingToPull)
            {
                if (_thingToPull.gameObject.transform.position.x > gameObject.transform.position.x)
                {
                    if (_horizontalMovement >= 0 || !controller.IsFacingRight())
                    {
                        StopPulling();
                    }
                    else
                    {
                        Pull();
                    }
                }
                else
                {
                    if (_horizontalMovement <= 0 || controller.IsFacingRight())
                    {
                        StopPulling();
                    }
                    else
                    {
                        Pull();
                    }
                }
            }

            float velocityY = GetComponent<Rigidbody2D>().velocity.normalized.y;

            RaycastHit2D hit =
                Physics2D.Raycast(pullingAnchor.position, Vector2.right, 0.1f, LayerMask.GetMask("Box"));

            _thingToPull = hit.rigidbody;

            animator.SetBool(JumpingUpUpwards, false);
            animator.SetBool(JumpingUpDownwards, false);
            animator.SetBool(JumpingForwardUpwards, false);
            animator.SetBool(JumpingForwardDownwards, false);

            bool goingForward = _horizontalMovement != 0;

            if (!controller.IsGrounded())
            {
                if (velocityY > 0)
                {
                    animator.SetBool(goingForward ? JumpingForwardUpwards : JumpingUpUpwards, true);
                }
                else if (velocityY < 0)
                {
                    animator.SetBool(goingForward ? JumpingForwardDownwards : JumpingUpDownwards, true);
                }
            }

            float multiplier = hit.rigidbody != null && hit.rigidbody.mass == 4
                ? 0
                : pullingSpeedMultiplier;
            float movementMultiplier = _isPulling ? multiplier : 1;

            controller.Move((_horizontalMovement * movementMultiplier) * Time.fixedDeltaTime, _jump && !_isPulling,
                _isPulling);
            _jump = false;
        }

        public void MoveEast()
        {
            _horizontalMovement = runSpeed;
            animator.SetFloat(Speed, Mathf.Abs(_horizontalMovement));
        }

        public void MoveWest()
        {
            _horizontalMovement = -runSpeed;
            animator.SetFloat(Speed, Mathf.Abs(_horizontalMovement));
        }

        public void StopMoving()
        {
            _horizontalMovement = 0;
            animator.SetFloat(Speed, 0);
        }

        public void JumpNow()
        {
            controller.UpdateHoldingJump(true);
            _jump = true;
        }

        public void StopHoldingJump()
        {
            controller.UpdateHoldingJump(false);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!GlobalInstanceManager.GetStateManager().CanMove)
            {
                return;
            }

            GlobalInstanceManager.GetStateManager().HasMoved = true;
            Vector2 movement = context.ReadValue<Vector2>();
            float movementX = movement.x;

            if (movementX == 0)
            {
                StopMoving();

                return;
            }

            if (movementX > 0)
            {
                MoveEast();
            }
            else
            {
                MoveWest();
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!GlobalInstanceManager.GetStateManager().CanMove)
            {
                return;
            }

            if (context.performed)
            {
                GlobalInstanceManager.GetStateManager().HasJumped = true;
                JumpNow();
            }

            if (context.canceled)
            {
                StopHoldingJump();
            }
        }

        public void OnPull(InputAction.CallbackContext context)
        {
            if (!GlobalInstanceManager.GetStateManager().CanMove)
            {
                return;
            }

            if (context.canceled)
            {
                _isTryingToPull = false;

                return;
            }

            _isTryingToPull = true;
        }

        private void Pull()
        {
            FixedJoint2D joint = GetComponentInChildren<FixedJoint2D>();

            _isPulling = true;
            joint.connectedBody = _thingToPull;
        }

        private void StopPulling()
        {
            FixedJoint2D joint = GetComponentInChildren<FixedJoint2D>();

            joint.connectedBody = null;
            _isPulling = false;
        }

        public void SetHorizontalMovement(float movement)
        {
            _horizontalMovement = movement;
        }

        public float GetHorizontalMovement()
        {
            return _horizontalMovement;
        }

        public void StopAllMovement()
        {
            StopMoving();
            StopHoldingJump();
        }
    }
}