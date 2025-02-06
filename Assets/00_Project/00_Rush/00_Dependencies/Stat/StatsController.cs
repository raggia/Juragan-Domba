using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial interface IStatController
    {
        // klo perlu
    }
    public partial class StatsController : MonoBehaviour, IStatController
    {
        [SerializeField]
        private int m_Level = 2;
        [SerializeField]
        private MainStatsDefinition m_MainStats;
        [SerializeField]
        private List<StatModifierField> m_StatModifiers;
        [SerializeField]
        private UnityEvent<Stat> m_OnStatValueChanged = new();

        [SerializeField]
        private UnityEvent<Stats> m_OnStatsInitialized = new();

        private void Start()
        {
            Init();
        }
        private void Init()
        {
            OnStatsInitialized(GetFinalStatsInternal());
        }
        public void SetLevel(int level)
        {
            m_Level = level;
            Init();
        }
        private void OnStatsInitialized(Stats stats)
        {
            m_OnStatsInitialized?.Invoke(stats);
        }
        private Stats GetFinalStatsInternal()
        {
            Stats stats = GetFinalMainStatsByLevelInternal(m_Level);
            foreach (Stat stat in GetFinalModifierStatsInternal().StatList)
            {
                stats.AddStatValue(stat.Definition, stat.Value);
            }
            return stats;
        }
        private Stats GetFinalMainStatsByLevelInternal(int level)
        {
            return m_MainStats.GetFinalStatsByLevel(level);
        }
        private Stats GetFinalModifierStatsInternal()
        {
            Stats stats = new Stats();
            foreach (StatModifierField field in m_StatModifiers)
            {
                foreach (Stat stat in field.GetFinalStats().StatList)
                {
                    stats.AddStatValue(stat.Definition, stat.Value);
                }
            }
            return stats;
        }

        private float GetMainStatValue(StatNameDefinition defi, int level)
        {
            return m_MainStats.GetFinalStatValueByLevel(defi, level);
        }

        private StatModifierField GetStatModifier(AStatModifierDefinition modifier)
        {
            StatModifierField match = m_StatModifiers.Find(x => x.Definition == modifier);
            if (match == null) 
            {
                Debug.LogError($"There is no modifier {modifier.Label} on list");
                return default;
            }
            return match;
        }
        private int GetAddStackAtFirstInflictModifier(AStatModifierDefinition defi)
        {
            return GetStatModifier(defi).AddStackEachAtFirstInflict;
        }
        private int GetAddStackEachInflictModifier(AStatModifierDefinition defi)
        {
            return GetStatModifier(defi).AddStackEachInflict;
        }
        private bool HasModifier(AStatModifierDefinition defi)
        {
            return m_StatModifiers.Contains(GetStatModifier(defi));
        }

        private bool CanStackModifier(AStatModifierDefinition defi)
        {
            return GetStatModifier(defi).CanStack;
        }
        private void SetStackModifier(AStatModifierDefinition defi, int set)
        {
            GetStatModifier(defi).SetStack(set);
        }
        private void AddStackModifier(AStatModifierDefinition defi, int add)
        {
            GetStatModifier(defi).AddStack(add);
        }
        public void AddModifier(StatModifierField modifier)
        {
            if (HasModifier(modifier.Definition))
            {
                if (modifier.CanStack)
                {
                    AddStackModifier(modifier.Definition, modifier.AddStackEachInflict);
                }
            }
            else
            {
                m_StatModifiers.Add(modifier);
            }
        }
    }
}
