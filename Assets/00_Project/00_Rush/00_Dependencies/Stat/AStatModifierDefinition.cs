using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public enum ERemovingEffMechanism
    {
        StopEffectOnDurationEnd = 0,
        RemoveStackOnDurationEnd = 1,
    }
    [System.Serializable]
    public abstract partial class AStatModifierDefinition : AStatsDefinition
    {
        [Header("Modifier Stat Field")]
        [SerializeField]
        private bool m_CanStack;
        [SerializeField]
        private int m_MaxStacks;
        [SerializeField]
        private int m_AddStackAtFirstInflict = 1;
        [SerializeField]
        private int m_AddStackEachInflict = 1;

        [SerializeField]
        private Stats m_AddStatEachStack;
        [SerializeField]
        private Stats m_IncreaseAddStatEachStackByLevel;
        [SerializeField]
        private float m_EffectDuration = 10f;
        [SerializeField]
        private ERemovingEffMechanism m_RemovingEffMechanism;
        [SerializeField, ShowIf(nameof(m_RemovingEffMechanism), ERemovingEffMechanism.RemoveStackOnDurationEnd), AllowNesting]
        private int m_RemovedStackOnDurationEnd = 1;

        public Stats GetFinalAddStatEachStack(int stack, int level)
        {
            Stats newStats = new Stats();
            foreach (Stat stat in m_AddStatEachStack.StatList)
            {
                float addtional = GetFinalIncreaseAddStatEachStackByLevel(level - 1).GetStatValue(stat.Definition);
                newStats.AddStatValue(stat.Definition, stat.Value + addtional);
                newStats.MultiplyValue(stat.Definition, stack);
            }
            return newStats;
        }
        private Stats GetFinalIncreaseAddStatEachStackByLevel(int level)
        {
            Stats newStats = new Stats();
            foreach (Stat stat in m_IncreaseAddStatEachStackByLevel.StatList)
            {
                newStats.MultiplyValue(stat.Definition, level);
            }
            return newStats;
        }
        public bool CanStack
        {
            get
            {
                return m_CanStack;
            }
        }

        public int MaxStacks
        {
            get { return m_MaxStacks; }
        }

        public int AddStackAtFirstInflict
        {
            get { return m_AddStackAtFirstInflict; }
        }
        public int AddStackEachInflict
        {
            get { return m_AddStackEachInflict; }
        }

        public float EffectDuration
        {
            get { return m_EffectDuration; }
        }

        public ERemovingEffMechanism RemovingEffMechanism
        {
            get { return m_RemovingEffMechanism; }
        }

        public int RemovedStackOnDurationEnd
        {
            get { return m_RemovedStackOnDurationEnd; }
        }
    }
}
