using System;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.State
{
    public class StateManager : MonoBehaviour
    {
        private static StateManager _instance;

        public bool HasMoved { get; set; }
        public bool HasJumped { get; set; }

        private bool _canMove;

        public bool CanMove
        {
            get => _canMove;
            set
            {
                bool oldValue = _canMove;
                _canMove = value;
                OnStateChanged(new StateChangeArgs<bool>()
                {
                    PropertyName = "CanMove",
                    OldValue = oldValue,
                    NewValue = value
                });
            }
        }

        private bool _hasLantern;

        public bool HasLantern
        {
            get => _hasLantern;
            set
            {
                bool oldValue = _hasLantern;
                _hasLantern = value;
                OnStateChanged(new StateChangeArgs<bool>()
                {
                    PropertyName = "HasLantern",
                    OldValue = oldValue,
                    NewValue = value
                });
            }
        }

        private bool _canUsePower;

        public bool CanUsePower
        {
            get => _canUsePower;
            set
            {
                bool oldValue = _canUsePower;
                _canUsePower = value;
                OnStateChanged(new StateChangeArgs<bool>()
                {
                    PropertyName = "CanUsePower",
                    OldValue = oldValue,
                    NewValue = value
                });
            }
        }

        public event EventHandler StateChanged;

        protected virtual void OnStateChanged(EventArgs e)
        {
            EventHandler handler = StateChanged;
            handler?.Invoke(this, e);
        }
    }
}