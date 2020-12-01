using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotTimeTravel.Core.Logic
{
    public class GameActions : MonoBehaviour
    {
        public void Invoke()
        {
            foreach (IGameAction gameAction in GetGameActions())
            {
                gameAction.Invoke();
            }
        }
        
        private IEnumerable<IGameAction> GetGameActions()
        {
            return GetComponents<IGameAction>().Where(gameAction => gameAction.IsActive()).ToArray();
        }
    }
}