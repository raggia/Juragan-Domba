using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public class Vector2Modifier 
    {
        [SerializeField] 
        private Vector2 m_Value;
        public UnityEvent<Vector2> m_OnValueChange = new();
        public Vector2 GetValue() => m_Value;
        public void SetValue(Vector2 newValue)
        {
            m_Value = newValue;
            OnValueChange();
        }
        public void OnValueChange()
        {
            m_OnValueChange?.Invoke(m_Value);
        }
    }
}
