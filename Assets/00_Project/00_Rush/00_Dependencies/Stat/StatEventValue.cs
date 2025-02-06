using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public class StatEventValue : MonoBehaviour
    {
        [SerializeField]
        private StatNameDefinition m_StatName;
        [SerializeField]
        private UnityEvent<float> m_OnValueChanged;
        public void OnValueChangedInvoked(Stats stats)
        {
            foreach (Stat stat in stats.StatList)
            {
                if (stat.Definition == m_StatName)
                {
                    m_OnValueChanged?.Invoke(stat.Value);
                }
            }
        }
        public void OnValueChangedInvoked(Stat stat)
        {
            m_OnValueChanged?.Invoke(stat.Value);
        }
    }
}
