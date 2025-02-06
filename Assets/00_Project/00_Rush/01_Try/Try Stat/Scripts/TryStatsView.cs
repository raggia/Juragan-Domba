using TMPro;
using UnityEngine;

namespace Rush.Training
{
    public class TryStatsView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_AttackValueText;
        [SerializeField]
        private TextMeshProUGUI m_DefendValueText;

        public void SetAttackValueText(float attackValue)
        {
            m_AttackValueText.text = attackValue.ToString();
        }
        public void SetDefendValueText(float defendValue)
        {
            m_DefendValueText.text = defendValue.ToString();
        }

    }
}
