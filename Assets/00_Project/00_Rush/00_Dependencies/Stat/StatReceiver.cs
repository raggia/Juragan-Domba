using UnityEngine;

namespace Rush
{
    public class StatReceiver : MonoBehaviour
    {
        [SerializeField]
        private Stats m_Stats;

        public void Init(Stats stats)
        {
            foreach (Stat stat in stats.StatList)
            {
                m_Stats.SetStatValue(stat.Definition, stat.Value);
            }
        }
    }
}
