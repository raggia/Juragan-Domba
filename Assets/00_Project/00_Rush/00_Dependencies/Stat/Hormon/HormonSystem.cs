using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public partial class HormonThreshold
    {
        [SerializeField]
        private ThresholdExecute m_ThresholdExucte;
        [SerializeField, Range(0f, 1f)]
        private float m_ThresholdRate;
        [SerializeField]
        private UnityEvent<float> m_OnThreshold;
        public float Rate => m_ThresholdRate;
        public void TriggerThresold(float rate)
        {
            switch (m_ThresholdExucte)
            { 
                case ThresholdExecute.Equal:
                    if (rate == m_ThresholdRate)
                    {
                        OnThresholdInvoke(m_ThresholdExucte);
                    }
                    break;
                case ThresholdExecute.NotEqual:
                    if (rate != m_ThresholdRate)
                    {
                        OnThresholdInvoke(m_ThresholdExucte);
                    }
                    break;
                case ThresholdExecute.GreaterThan:
                    if (rate > m_ThresholdRate)
                    {
                        OnThresholdInvoke(m_ThresholdExucte);
                    }
                    break;
                case ThresholdExecute.LessThan:
                    if (rate < m_ThresholdRate)
                    {
                        OnThresholdInvoke(m_ThresholdExucte);
                    }
                    break;
                case ThresholdExecute.GreaterThanEqual:
                    if (rate >= m_ThresholdRate)
                    {
                        OnThresholdInvoke(m_ThresholdExucte);
                    }
                    break;
                case ThresholdExecute.LessThanEqual:
                    if (rate <= m_ThresholdRate)
                    {
                        OnThresholdInvoke(m_ThresholdExucte);
                    }
                    break;
            }
        }

        public void OnThresholdInvoke(ThresholdExecute threshold)
        {
            m_OnThreshold?.Invoke(m_ThresholdRate);
            Debug.Log($"Threshold Triggered on {m_ThresholdRate} {threshold}");
        }
    }
    [System.Serializable]
    public partial class Hormon
    {
        [SerializeField]
        private StatNameDefinition m_Definition;
        [SerializeField, ReadOnly]
        private bool m_Initialized = false;
        [SerializeField]
        private float m_Value;
        [SerializeField]
        private float m_MaxValue;
        [SerializeField]
        private UnityEvent<float> m_OnValueChanged = new();
        [SerializeField]
        private UnityEvent<float> m_OnRateChanged = new();

        [SerializeField]
        private List<HormonThreshold> m_Threshold = new();
        public StatNameDefinition Definition => m_Definition;
        public float Value => m_Value;
        public void SetInitialized(bool set)
        {
            m_Initialized = set;
        }
        private void TriggerThreshold(float threshold)
        {
            if (!m_Initialized) return;
            foreach (HormonThreshold hormon in m_Threshold)
            {
                hormon.TriggerThresold(threshold);
            }
        }
        public void SetValue(float set)
        {
            m_Value = set;
            ClampValue();
            OnValueChangedInvoke(m_Value);
        }
        public void AddValue(float set)
        {
            m_Value += set;
            ClampValue();
            OnValueChangedInvoke(m_Value);
        }
        public void SetMaxValue(float set)
        {
            m_MaxValue = set;
            ClampValue();
        }
        public void AddMaxValue(float set)
        {
            m_MaxValue += set;
            ClampValue();
            
        }

        private void ClampValue()
        {
            m_Value = Mathf.Clamp(m_Value, 0f, m_MaxValue);
            TriggerThreshold(GetRate());
            OnRateChangedInvoke(GetRate());
        }

        public float GetRate()
        {
            return m_Value / m_MaxValue;
        }

        private void OnValueChangedInvoke(float val)
        {
            m_OnValueChanged?.Invoke(val);
            
        }
        private void OnRateChangedInvoke(float val)
        {
            m_OnRateChanged?.Invoke(val);
        }
    }
    public partial class HormonSystem : MonoBehaviour
    {
        [SerializeField]
        private List<Hormon> m_Hormons = new();

        [SerializeField]
        private UnityEvent<Hormon> m_OnValueChanged = new();
        public void Init(Stats stats)
        {
            foreach (Stat stat in stats.StatList)
            {
                if (HasHormon(stat.Definition))
                {
                    GetHormonInternal(stat.Definition).SetMaxValue(stat.Value);
                    GetHormonInternal(stat.Definition).SetValue(stat.Value);
                    GetHormonInternal(stat.Definition).SetInitialized(true);
                }
            }
        }
        public void SetMaxValue(Stat stat)
        {
            GetHormonInternal(stat.Definition).SetMaxValue(stat.Value);
        }
        private bool HasHormon(StatNameDefinition defi)
        {
            return m_Hormons.Contains(GetHormonInternal(defi));
        }
        private Hormon GetHormonInternal(StatNameDefinition defi)
        {
            Hormon match = m_Hormons.Find(x => x.Definition == defi);
            if (match == null)
            {
                Debug.LogWarning($"There is no {defi} hormon in {m_Hormons}");
                match = null;
            }
            return match;
        }

        /*private float GetMaxValue(StatNameDefinition defi)
        {
            return m_HormonDefinition.GetFinalStat
        }*/
        public float GetValue(StatNameDefinition defi)
        {
            return GetHormonInternal(defi).Value;
        }

        public void SetValue(Stat stat)
        {
            GetHormonInternal(stat.Definition).SetValue(stat.Value);
            OnValuechangedInvoke(GetHormonInternal(stat.Definition));
        }
        public void AddValue(Stat stat)
        {
            GetHormonInternal(stat.Definition).AddValue(stat.Value);
            OnValuechangedInvoke(GetHormonInternal(stat.Definition));
        }

        private void OnValuechangedInvoke(Hormon hormon)
        {
            m_OnValueChanged?.Invoke(hormon);
        }
    }
}
