using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public partial class StatModifierField
    {
        [SerializeField]
        private AStatModifierDefinition m_ModifierDefinition;
        [SerializeField]
        private int m_Level;
        [SerializeField]
        private int m_Stack;

        [SerializeField]
        private float m_CurrentEffDuration;

        [SerializeField]
        private UnityEvent m_OnEffRemoved;
         
        public StatModifierField(AStatModifierDefinition defi, int level, UnityAction onEffRemoved)
        {
            m_ModifierDefinition = defi;
            m_Level = level;
            m_Stack = defi.AddStackAtFirstInflict;
            m_CurrentEffDuration = defi.EffectDuration;
            PlayDurationInternal(onEffRemoved);
        }
        public void PlayDuration(UnityAction onEffRemoved)
        {
            PlayDurationInternal(onEffRemoved);
        }
        private void PlayDurationInternal(UnityAction onEffRemoved)
        {
            RemoveOnEffRemoved(onEffRemoved);
            CoroutineUtility.BeginCoroutine($"{m_ModifierDefinition.Id}", PlayingDuration(onEffRemoved));
        }

        private IEnumerator PlayingDuration(UnityAction onEffRemoved)
        {
            AddOnEffRemoved(onEffRemoved);
            while (m_Stack > 1)
            {
                m_CurrentEffDuration -= Time.deltaTime;
                if (m_ModifierDefinition.RemovingEffMechanism == ERemovingEffMechanism.StopEffectOnDurationEnd)
                {
                    m_Stack = 0;
                }
                else
                {
                    if (m_CurrentEffDuration < 0)
                    {
                        m_Stack -= m_ModifierDefinition.RemovedStackOnDurationEnd;
                        m_CurrentEffDuration = m_ModifierDefinition.EffectDuration;
                    }
                }
                yield return null;
            }
            OnEffRemovedInvoke();
        }
        private void OnEffRemovedInvoke()
        {
            m_OnEffRemoved?.Invoke();
        }
        private void AddOnEffRemoved(UnityAction onEffRemoved)
        {
            m_OnEffRemoved?.AddListener(onEffRemoved);
        }
        private void RemoveOnEffRemoved(UnityAction onEffRemoved)
        {
            m_OnEffRemoved?.RemoveListener(onEffRemoved);
        }

        private Stats GetFinalStatsInternal()
        {
            Stats newStats = m_ModifierDefinition.GetFinalStatsByLevel(m_Level);
            foreach (Stat stat in m_ModifierDefinition.GetFinalAddStatEachStack(m_Stack, m_Level).StatList) 
            {
                newStats.AddStatValue(stat.Definition, stat.Value);
            }
            return newStats;
        }

        public int GetLevel()
        {
            return m_Level;
        }
        public int GetStack()
        {
            return m_Stack;
        }
        public Stats GetFinalStats()
        {
            return GetFinalStatsInternal();
        }
        public Stat GetFinalStat(StatNameDefinition defi)
        {
            return GetFinalStatsInternal().GetStat(defi);
        }
        public float GetFinalStatValue(StatNameDefinition defi)
        {
            return GetFinalStatsInternal().GetStatValue(defi);
        }
        public AStatModifierDefinition Definition 
        { 
            get 
            { 
                return m_ModifierDefinition; 
            } 
        }

        public bool CanStack
        {
            get
            {
                return m_ModifierDefinition.CanStack;
            }
        }
        public int AddStackEachAtFirstInflict
        {
            get
            {
                return m_ModifierDefinition.AddStackAtFirstInflict;
            }
        }
        public int AddStackEachInflict
        {
            get
            {
                return m_ModifierDefinition.AddStackEachInflict;
            }
        }
        public void SetStack(int set)
        {
            m_Stack = set;
        }
        public void SetLevel(int set)
        {
            m_Level = set;
        }
        public void AddStack(int add)
        {
            m_Stack += add;
        }
        public void AddLevel(int add)
        {
            m_Level += add;
        }
    }
}
