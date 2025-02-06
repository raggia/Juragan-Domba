using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public partial interface IStatsDefinition
    {
        Stats GetFinalStatsByLevel(int level);
    }

    [System.Serializable]
    public abstract partial class AStatsDefinition : UnitDefinition, IStatsDefinition
    {
        [Header("Base Stat Field")]
        [SerializeField]
        private Stats m_BaseStats;
        [SerializeField]
        private Stats m_AddStatsEachLevel;

        [SerializeField]
        private Stats m_ExponentialStatsEachLevel;
        private Stats GetFinalAddStatsEachLevel(int level)
        {
            Stats finalStat = new Stats();
            foreach (Stat stat in m_AddStatsEachLevel.StatList)
            {
                float finalValue = stat.Value * (level - 1);
                Stat newStat = new Stat(stat.Definition, finalValue);
                finalStat.AddNewStat(newStat);
            }
            return finalStat;
        }

        private Stats GetFinalExponentialStatsEachLevel(int level)
        {
            Stats finalStat = new Stats();
            foreach (Stat stat in m_ExponentialStatsEachLevel.StatList)
            {
                float newStatValue = m_BaseStats.GetStatValue(stat.Definition);
                float multiplyValue = m_ExponentialStatsEachLevel.GetStatValue(stat.Definition, true);
                Debug.Log($"Final Expo Definition Value {stat.Definition} {multiplyValue}");
                Stat newStat = new Stat(stat.Definition, newStatValue);
                for (int i = 0; i < level - 1; i++)
                {
                    //newStatValue *= multiplyValue;
                    newStat.MultiplyValue(multiplyValue);
                    Debug.Log($"New Stat Definition Value {stat.Definition} {newStatValue}");
                    //finalStat.MultiplyValue()
                }
                finalStat.AddNewStat(newStat);
            }   
            return finalStat;
        }

        protected virtual Stats GetFinalStatsByLevelInternal(int level)
        {
            Stats finalStat = new Stats();
            foreach (Stat stat in m_BaseStats.StatList)
            {
                float baseEachLevelValue = m_BaseStats.GetStatValue(stat.Definition);
                float addEachlevelValue = GetFinalAddStatsEachLevel(level).GetStatValue(stat.Definition);
                
                float multiplyEachLevelValue = GetFinalExponentialStatsEachLevel(level).GetStatValue(stat.Definition, true, baseEachLevelValue);
                
                Debug.Log($"Final Expo Value {multiplyEachLevelValue}");
                float finalValue = multiplyEachLevelValue + addEachlevelValue;
                Stat newStat = new Stat(stat.Definition, finalValue);
                finalStat.AddNewStat(newStat);
            }
            return finalStat;
        }

        public float GetFinalStatValueByLevel(StatNameDefinition defi, int level)
        {
            return GetFinalStatsByLevelInternal(level).GetStatValue(defi);
        }

        public Stats GetFinalStatsByLevel(int level)
        {
            return GetFinalStatsByLevelInternal(level);
        }

        public Stat GetFinalStat(StatNameDefinition defi, int level)
        {
            return GetFinalStatsByLevel(level).GetStat(defi);
        }

        public List<Stat> GetBaseStatsList()
        {
            return m_BaseStats.StatList;
        }

        public Sprite GetIcon(StatNameDefinition defi)
        {
            return defi.Icon;
        }
        public string GetName(StatNameDefinition defi)
        {
            return defi.Label;
        }
        public string GetDescription(StatNameDefinition defi)
        {
            return defi.Description;
        }
    }
}
