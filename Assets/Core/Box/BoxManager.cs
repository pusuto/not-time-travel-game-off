using NotTimeTravel.Core.Player;
using NotTimeTravel.Core.Speech;
using UnityEngine;

namespace NotTimeTravel.Core.Box
{
    public class BoxManager : MonoBehaviour
    {
        public Transform ray;
        public AudioSource moveSound;
        public AudioSource thudSound;

        private Rigidbody2D _rigidbody;
        private PlayerMovement _playerMovement;
        private bool _isFalling;
        private bool _hasPlayedThud;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            if (GlobalInstanceManager.GetMainCharacter() == null)
            {
                return;
            }
            
            _playerMovement = GlobalInstanceManager.GetMainCharacter().GetComponent<PlayerMovement>();
        }

        private void FixedUpdate()
        {
            CheckForMove();
            CheckForThud();
        }

        private void CheckForMove()
        {
            if (_playerMovement == null)
            {
                return;
            }

            Vector2 velocity = _rigidbody.velocity;
            bool velocityCheck = velocity.x != 0 && Mathf.Abs(velocity.y) < 5;

            if (velocityCheck || _playerMovement.IsPulling(_rigidbody))
            {
                if (!moveSound.isPlaying)
                {
                    moveSound.Play();
                }
            }
            else
            {
                moveSound.Stop();
            }
        }

        private void CheckForThud()
        {
            RaycastHit2D hit =
                Physics2D.Raycast(transform.position, Vector2.down, 2f, LayerMask.GetMask("Ground"));

            float velocityY = _rigidbody.velocity.y;

            if (velocityY == 0)
            {
                _hasPlayedThud = false;
            }

            if (_isFalling && hit.rigidbody != null)
            {
                _isFalling = false;
                _hasPlayedThud = true;
                thudSound.PlayOneShot(thudSound.clip);
            }
            else
            {
                if (velocityY < -10 && !_hasPlayedThud)
                {
                    _isFalling = true;
                }
            }
        }
    }
}