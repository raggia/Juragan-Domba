using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public class TryStats : MonoBehaviour
    {
        [SerializeField]
        private float m_Attack = 10f;
        [SerializeField]
        private float m_Defend = 10f;

        [SerializeField]
        private UnityEvent<float> m_OnAttackChanged = new();
        [SerializeField]
        private UnityEvent<float> m_DefendChanged = new();

        private void Start()
        {
            m_OnAttackChanged?.Invoke(m_Attack);
            m_DefendChanged?.Invoke(m_Defend);
        }

        public void AddAttack(float add)
        {
            m_Attack += add;
            m_OnAttackChanged?.Invoke(m_Attack);
        }
        public void AddDefend(float defend)
        {
            m_Defend += defend;
            m_DefendChanged.Invoke(m_Defend);
        }


    }
}
