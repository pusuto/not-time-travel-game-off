using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotTimeTravel.Core.Logic
{
    public class Conditions : MonoBehaviour
    {
        public bool AreCleared()
        {
            return GetUncleared().ToArray().Length == 0;
        }

        public string GetMessage()
        {
            return string.Join(", ", GetUncleared().Select(condition => condition.GetMessage()));
        }

        private IEnumerable<ICondition> GetUncleared()
        {
            return GetConditions().Where(condition => !condition.IsCleared());
        }

        private IEnumerable<ICondition> GetConditions()
        {
            return GetComponents<ICondition>().Where(condition => condition.IsActive());
        }
    }
}