using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial interface ILevel
    {
        void Init(float loadExp);
        void SetExperience(float val);
        void AddExperience(float add);
    }
    [System.Serializable ]
    public partial class LevelField
    {
        [SerializeField]
        private LevelNameDefinition m_Definition;
        
        [SerializeField, ReadOnly]
        private int m_CurrentLevel;
        [SerializeField, ReadOnly]
        private float m_Experience;
        [SerializeField, ReadOnly]
        private float m_MaxExperience;
        [SerializeField]
        private UnityEvent<float> m_OnExperienceChanged;
        [SerializeField]
        private UnityEvent<float> m_OnMaxExperienceChanged;
        [SerializeField]
        private UnityEvent<int> m_OnCurrentLevelChanged;

        [SerializeField, ReadOnly]
        private List<float> m_ExperienceList = new();
        public float GetStartExperience() => m_Definition.StartExperience;
        public void Init(float loadExp)
        {
            SetExperienceInternal(loadExp);
        }

        public void Validate()
        {
            List<float> list = new();
            float maxExpEachLevel = m_Definition.StartExperience;
            list.Add(maxExpEachLevel);
            for (int i = 0; i < m_Definition.MaxLevel - 1; i++)
            {
                maxExpEachLevel *= m_Definition.ExponentialGrowth;
                maxExpEachLevel = Mathf.Round(maxExpEachLevel);
                list.Add(maxExpEachLevel);
            }
            m_ExperienceList = new List<float>(list);
        }

        public void SetExperience(float val)
        {
            SetExperienceInternal(val);
        }
        private void SetExperienceInternal(float val)
        {
            m_Experience = val;
            OnExperienceChangedInvoke();
        }
        public void AddExperience(float add)
        {
            m_Experience += add;
            OnExperienceChangedInvoke();
        }
        private void SetMaxExperience(float val)
        {
            m_MaxExperience = val;
            OnMaxExperienceChangedInvoke();
        }

        private void OnExperienceChangedInvoke()
        {
            m_OnExperienceChanged?.Invoke(m_Experience);
            AdjustLevel();
        }

        private void OnMaxExperienceChangedInvoke()
        {
            m_OnMaxExperienceChanged?.Invoke(m_MaxExperience);
        }

        private void AdjustLevel()
        {
            /*if (m_Experience > m_ExperienceList[m_CurrentLevel])
            {
                AddLevelInternal(1);
                SetMaxExperience(m_ExperienceList[m_CurrentLevel + 1]);
            }*/

            for (int i = 0; i < m_ExperienceList.Count; i++)
            {
                if (m_Experience < m_ExperienceList[i])
                {
                    SetLevelInternal(i+1);
                    SetMaxExperience(m_ExperienceList[i]);
                    return;
                }
                else
                {
                    SetLevelInternal(m_ExperienceList.Count);
                    SetMaxExperience(m_ExperienceList[m_ExperienceList.Count - 1]);
                }
            }
        }

        private void AddLevelInternal(int add)
        {
            m_CurrentLevel += add;
            OnCurrentLevelChangedInvoke();
        }
        private void SetLevelInternal(int set)
        {
            m_CurrentLevel = set;
            OnCurrentLevelChangedInvoke();
        }
        private void OnCurrentLevelChangedInvoke()
        {
            m_OnCurrentLevelChanged?.Invoke(m_CurrentLevel);
        }

        
    }
}
