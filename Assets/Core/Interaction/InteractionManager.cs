using System.Collections;
using NotTimeTravel.Core.Input;
using NotTimeTravel.Core.Logic;
using NotTimeTravel.Core.Model;
using NotTimeTravel.Core.Player;
using NotTimeTravel.Core.Speech;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NotTimeTravel.Core.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        public bool isActive;
        public Canvas canvas;
        public Canvas messageCanvas;
        public CanvasGroup panel;
        public CanvasGroup messagePanel;
        public string action;
        public string key;
        public TextMeshProUGUI messageText;
        public TextMeshProUGUI actionText;
        public TextMeshProUGUI keyText;
        public float animationDuration = .2f;
        public UnityEvent onInteract;
        public Times times;
        public Color popupColor;
        public Color messagePopupColor;
        public Color popupTextColor;
        public Color messageTextColor;
        public bool requireCharacterToLook;
        public bool faceRightWhenStandingInPosition;

        private bool _canInteract;
        private bool _hasInteracted;
        private Conditions _conditions;
        private GameActions _gameActions;
        private RectTransform _popupTransform;
        private RectTransform _messagePopupTransform;
        private Image _popup;
        private Image _messagePopup;
        private bool _animatingPopup;
        private bool _animatingMessagePopup;
        private bool _shouldShowMessage;

        private void Start()
        {
            _conditions = GetComponent<Conditions>();
            _gameActions = GetComponent<GameActions>();
            _popupTransform = canvas.GetComponent<RectTransform>();
            _messagePopupTransform = messageCanvas.GetComponent<RectTransform>();
            _popup = canvas.GetComponentInChildren<Image>();
            _messagePopup = messageCanvas.GetComponentInChildren<Image>();
            UpdateText(true);
            UpdateColor();
            InputManager.OnInteract(OnInteract);
            SetCanvasActiveStatus(false);
        }

        private void FixedUpdate()
        {
            if (!_shouldShowMessage)
            {
                if (panel.alpha == 1)
                {
                    StartCoroutine(Animate(0));
                }

                if (messagePanel.alpha == 1)
                {
                    StartCoroutine(AnimateMessagePanel(0));
                }
            }
        }

        private void UpdateColor()
        {
            Color actionTextColor = popupTextColor;
            actionTextColor.a = .8f;
            _popup.color = popupColor;
            actionText.color = actionTextColor;
            keyText.color = popupTextColor;
        }

        private void UpdateMessageColor()
        {
            _messagePopup.color = messagePopupColor;
            messageText.color = messageTextColor;
        }

        private void UpdateText(bool force)
        {
            if (actionText.text == action && !force)
            {
                return;
            }

            Vector2 values = actionText.GetPreferredValues(action);
            _popupTransform.sizeDelta = new Vector2(values.x + 0.4f, _popupTransform.rect.height);
            keyText.SetText(key);
            actionText.SetText(action);
        }

        private void UpdateMessageText()
        {
            if (_conditions.AreCleared())
            {
                return;
            }

            string message = _conditions.GetMessage();

            if (messageText.text == message)
            {
                return;
            }

            Vector2 values = messageText.GetPreferredValues(message);
            float newWidth = values.x + 0.4f;

            Rect rect = _messagePopupTransform.rect;
            _messagePopupTransform.sizeDelta = new Vector2(newWidth, rect.height);
            messageText.SetText(message);
        }

        private bool ShouldBeAllowedToInteract()
        {
            return isActive && IsConditionCleared() && (!_hasInteracted || times == Times.Always);
        }

        private bool IsConditionCleared()
        {
            return _conditions.AreCleared();
        }

        public void SetCanvasActiveStatus(bool active)
        {
            _shouldShowMessage = active;

            if (!active)
            {
                StartCoroutine(Animate(0));
                StartCoroutine(AnimateMessagePanel(0));
            }

            if (active && !ShouldBeAllowedToInteract())
            {
                if (!IsConditionCleared())
                {
                    UpdateMessageColor();
                    UpdateMessageText();
                    StartCoroutine(AnimateMessagePanel(1));
                    StartCoroutine(Animate(0));
                }

                return;
            }

            if (IsConditionCleared())
            {
                StartCoroutine(AnimateMessagePanel(0));
            }

            _canInteract = active;
            StartCoroutine(Animate(active ? 1f : 0f));
        }

        private IEnumerator AnimateMessagePanel(float target)
        {
            if (messagePanel.alpha == target || _animatingMessagePopup)
            {
                yield break;
            }

            _animatingMessagePopup = true;

            yield return Transition.Transition.TransitionFloat(animationDuration, 1, messagePanel.alpha, target,
                newAlpha => messagePanel.alpha = newAlpha);

            _animatingMessagePopup = false;
        }

        private IEnumerator Animate(float target)
        {
            if (panel.alpha == target || _animatingPopup)
            {
                yield break;
            }

            _animatingPopup = true;

            yield return Transition.Transition.TransitionFloat(animationDuration, 1, panel.alpha, target,
                newAlpha => panel.alpha = newAlpha);

            _animatingPopup = false;
        }

        private void OnDestroy()
        {
            InputManager.OnInteract(OnInteract, true);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("MainCharacter"))
            {
                return;
            }

            SetCanvasActiveStatus(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("MainCharacter"))
            {
                return;
            }

            SetCanvasActiveStatus(false);
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (!_canInteract || !context.performed)
            {
                return;
            }

            InteractNow();
        }

        public void InteractNow()
        {
            if (!ShouldBeAllowedToInteract())
            {
                return;
            }

            SetCanvasActiveStatus(false);
            _hasInteracted = true;

            if (requireCharacterToLook)
            {
                CharacterController2D characterController =
                    GlobalInstanceManager.GetMainCharacter().GetComponent<CharacterController2D>();
                characterController.MakeSureFacing(faceRightWhenStandingInPosition);
            }

            onInteract.Invoke();
            _gameActions.Invoke();

            StartCoroutine(Animate(0));
            StartCoroutine(AnimateMessagePanel(0));
        }

        public void Refresh()
        {
            _hasInteracted = false;
        }
    }
}