using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public enum GeneralValueType
    {
        String = 0,
        Float = 1,
        Integer = 2,
        Bool = 3,
    }
    [Serializable]
    public class StringModifier
    {
        [SerializeField] private string m_Value;
        public UnityEvent<string> m_OnValueChange = new UnityEvent<string>();
        public string GetValue() => m_Value;
        public void SetValue(string newValue)
        {
            m_Value = newValue;
            OnValueChange();
        }
        public void AddValue(string addValue)
        {
            m_Value += addValue;
            OnValueChange();
        }
        public void OnValueChange()
        {
            m_OnValueChange?.Invoke(m_Value);
        }

    }
    [Serializable]
    public class FloatModifier
    {
        [SerializeField] private float m_Value;
        [SerializeField] private bool m_IsClamp;
        [ShowIf(nameof(m_IsClamp))]
        [AllowNesting]
        [SerializeField] private float m_MinValue;
        [ShowIf(nameof(m_IsClamp))]
        [AllowNesting]
        [SerializeField] private float m_MaxValue;
        public UnityEvent<float> m_OnValueChange = new UnityEvent<float>();
        public float GetValue() => m_Value;
        public void SetValue(float newValue)
        {
            
            m_Value = newValue;
            OnValueChange();
            
        }
        public void AddValue(float addValue)
        {
            m_Value += addValue;
            OnValueChange();
        }
        public void Multiply(float multiplicity)
        {
            m_Value *= multiplicity;
            OnValueChange();
        }
        public void OnValueChange()
        {
            Clamping();
            m_OnValueChange?.Invoke(m_Value);
        }
        private void Clamping()
        {
            if (!m_IsClamp) return;
            m_Value = Mathf.Clamp(m_Value, m_MinValue, m_MaxValue);
        }
    }
    [Serializable]
    public class IntModifier
    {
        [SerializeField] private int m_Value;
        [SerializeField] private bool m_IsClamp;

        [ShowIf(nameof(m_IsClamp))]
        [AllowNesting]
        [SerializeField] private float m_MinValue;
        [ShowIf(nameof(m_IsClamp))]
        [AllowNesting]
        [SerializeField] private float m_MaxValue;
        public UnityEvent<int> m_OnValueChange = new UnityEvent<int>();
        public int GetValue() => m_Value;
        public void SetValue(int newValue)
        {
            m_Value = newValue;
            OnValueChange();
        }
        public void AddValue(int addValue)
        {
            m_Value += addValue;
            OnValueChange();
        }
        public void Multiply(int multiplicity)
        {
            m_Value *= multiplicity;
            OnValueChange();
        }
        public void OnValueChange()
        {
            Clamping();
            m_OnValueChange?.Invoke(m_Value);
        }
        private void Clamping()
        {
            if (!m_IsClamp) return;
            m_Value = (int)Mathf.Clamp(m_Value, m_MinValue, m_MaxValue);
        }
    }
    [Serializable]
    public class BoolModifier
    {
        [SerializeField] private bool m_Value = false;
        public UnityEvent<bool> m_OnValueChange = new UnityEvent<bool>();
        public bool GetValue() => m_Value;
        public void SetValue(bool newValue)
        {
            m_Value = newValue;
            OnValueChange();
        }
        public void Toggle()
        {
            m_Value = !m_Value;
            OnValueChange();
        }
        public void OnValueChange()
        {
            m_OnValueChange?.Invoke(m_Value);
        }
    }

    [Serializable]
    public class GeneralValue 
    {
        public GeneralValueType Type;
        [ShowIf(nameof(Type), GeneralValueType.String)]
        [AllowNesting]
        public StringModifier PowerString;

        [ShowIf(nameof(Type), GeneralValueType.Float)]
        [AllowNesting]
        public FloatModifier PowerFloat;

        [ShowIf(nameof(Type), GeneralValueType.Integer)]
        [AllowNesting]
        public IntModifier PowerInt;

        [ShowIf(nameof(Type), GeneralValueType.Bool)]
        [AllowNesting]
        public BoolModifier PowerBool;
    }

}
