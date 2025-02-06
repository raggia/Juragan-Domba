using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class Stat 
    {
        [SerializeField]
        private StatNameDefinition m_Definition;
        [SerializeField]
        private float m_Value;

        public StatNameDefinition Definition => m_Definition;
        public float Value => m_Value;

        public Stat(StatNameDefinition definition, float value)
        {
            m_Definition = definition;
            m_Value = value;
        }

        public void SetValue(float set)
        {
            m_Value = set;
        }
        public void AddValue(float add)
        {
            m_Value += add;
        }
        public void MultiplyValue(float multiply)
        {
            m_Value *= multiply;
        }

    }
    [System.Serializable]
    public partial class Stats
    {
        
        [SerializeField]
        private List<Stat> m_StatList = new();
        private Stat GetStatInternal(StatNameDefinition defi)
        {
            Stat stat = m_StatList.Find(x => x.Definition == defi);
            if (stat == null)
            {
                //Debug.LogError($"No stat with definition {defi} on the list");
                return new Stat(defi, 0f);
            }
            return stat;
        }
        public Stat GetStat(StatNameDefinition defi)
        {
            return GetStatInternal(defi);
        }
        public List<Stat> StatList
        {
            get
            {
                return m_StatList;
            }
        }

        public bool HasStat(StatNameDefinition defi)
        {
            return HasStatInternal(defi);
        }

        private bool HasStatInternal(StatNameDefinition defi)
        {
            return m_StatList.Contains(GetStatInternal(defi));
        }

        public float GetStatValue(StatNameDefinition defi, bool isMultiplyer = false, float injectValue = 1f)
        {
            if (HasStatInternal(defi))
            {
                return GetStatInternal(defi).Value;
            }
            else
            {
                if (isMultiplyer)
                {
                    return 1f * injectValue;
                }
                else
                {
                    return 0;
                }
            }
        }

        public void AddNewStat(Stat stat)
        {
            AddNewStatInternal(stat);
        }
        public void RemoveNewStat(Stat stat)
        {
            RemoveNewStatInternal(stat);
        }
        private void AddNewStatInternal(Stat stat)
        {
            if (!HasStatInternal(stat.Definition))
            {
                m_StatList.Add(stat);
            }
        }
        private void RemoveNewStatInternal(Stat stat)
        {
            if (HasStatInternal(stat.Definition))
            {
                m_StatList.Remove(stat);
            }
        }

        public void SetStatValue(StatNameDefinition defi, float set)
        {
            if (!HasStatInternal(defi))
            {
                AddNewStatInternal(new Stat(defi, set));
            }
            else
            {
                GetStatInternal(defi).SetValue(set);
            }
            
        }
        public void AddStatValue(StatNameDefinition defi, float add)
        {
            if (!HasStatInternal(defi))
            {
                AddNewStatInternal(new Stat(defi, add));
            }
            else
            {
                GetStatInternal(defi).AddValue(add);
            }
        }
        public void MultiplyValue(StatNameDefinition defi, float mutiply)
        {
            if (!HasStatInternal(defi))
            {
                AddNewStatInternal(new Stat(defi, mutiply));
            }
            else
            {
                GetStatInternal(defi).MultiplyValue(mutiply);
            }
        }

    }
}
