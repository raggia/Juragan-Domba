using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public abstract partial class AProgressionLevelController<T> : MonoBehaviour where T : AProgressionStatDefinition
    {
        [SerializeField]
        T m_ProgressDefinition;
        [SerializeField]
        private int m_StartingLevel;
        [SerializeField]
        private int m_MaxLevel;
        [SerializeField, ReadOnly]
        private int m_CurrentLevel;

        [SerializeField, ReadOnly]
        private Stats m_CurrentProgress = new();
        [SerializeField, ReadOnly]
        private Stats m_ThresoldProgress = new();

        public void Init()
        {
            InitInternal();
        }
        private void InitInternal()
        {
            foreach (Stat stat in m_ProgressDefinition.GetBaseStatsList())
            {
                Stat defaultStat = new Stat(stat.Definition, 0f);
                m_CurrentProgress.AddNewStat(defaultStat);
                m_ThresoldProgress.AddNewStat(defaultStat);
            }

            //[To Do] load Current Level
            //[To Do] load Current Progress by Current Level

            InitThresholdProgress();
        }
        private void InitThresholdProgress()
        {
            foreach (Stat stat in GetProgressionStatsInternal())
            {
                float thresholdValueByLevel = m_ProgressDefinition.GetFinalStatValueByLevel(stat.Definition, m_CurrentLevel);
                Stat thresholdByLevel = new Stat(stat.Definition, thresholdValueByLevel);
                SetThresholdProgress(thresholdByLevel);
            }
        }
        private void SetCurrentLevelInternal(int level)
        {
            m_CurrentLevel = level;
        }

        private List<Stat> GetProgressionStatsInternal()
        {
            return m_ProgressDefinition.GetBaseStatsList();
        }
        public void LoadCurrentLevel(object result = null)
        {
            if (result == null)
            {
                SetCurrentLevelInternal(m_StartingLevel);
            }
            else
            {
                if (result is int level)
                {
                    SetCurrentLevelInternal(level);
                }
                else
                {
                    SetCurrentLevelInternal(m_StartingLevel);
                }
            }
        }
        public void LoadCurrentProgress(object result = null)
        {
            if (result == null)
            {
                if (result is Stats stats)
                {

                }
            }
        }

        private void SetThresholdProgress(Stat stat)
        {
            if (!HasThresholdProgress(stat.Definition))
            {
                m_ThresoldProgress.AddNewStat(stat);
            }
            m_ThresoldProgress.AddStatValue(stat.Definition, stat.Value);
        }

        private bool HasCurrentProgress(StatNameDefinition defi)
        {
            return m_CurrentProgress.HasStat(defi);
        }

        private bool HasThresholdProgress(StatNameDefinition defi)
        {
            return m_ThresoldProgress.HasStat(defi);
        }

        public void AddCurrentProgress(Stat stat)
        {
            if (!HasCurrentProgress(stat.Definition))
            {
                m_CurrentProgress.AddNewStat(stat);
            }
            m_CurrentProgress.AddStatValue(stat.Definition, stat.Value);
        }


    }
}
