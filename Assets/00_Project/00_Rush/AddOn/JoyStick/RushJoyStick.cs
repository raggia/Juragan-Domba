using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public class RushJoyStick : MonoBehaviour
    {
        [SerializeField]
        private string m_HorizontalName = "horizontal";
        [SerializeField]
        private string m_VerticalName = "vertical";

        [SerializeField]
        private string m_InputValueName = "Move";
        [SerializeField] 
        private Joystick m_Joystick;

        [SerializeField, ReadOnly] 
        private float m_X;
        [SerializeField, ReadOnly] 
        private float m_Y;
        [SerializeField, ReadOnly]
        private Vector2 m_InputValue;

        [SerializeField] 
        private UnityEvent<float> OnHorizontal = new UnityEvent<float>();
        [SerializeField] 
        private UnityEvent<float> OnVertical = new UnityEvent<float>();
        [SerializeField] 
        private UnityEvent<object, object> OnHorizontalName = new UnityEvent<object, object>();
        [SerializeField] 
        private UnityEvent<object, object> OnVerticalName = new UnityEvent<object, object>();
        [SerializeField]
        private UnityEvent<string, Vector2> m_OnInputValueName = new();
        [SerializeField]
        private UnityEvent<object, object> m_OnInputValueNameConvert = new();

        private void Update()
        {
            UpdateX();
            UpdateY();
            UpdateInputValue();
        }

        public void UpdateX()
        {
            m_X = m_Joystick.Horizontal;
            OnHorizontal?.Invoke(m_Joystick.Horizontal);
            OnHorizontalName?.Invoke(m_HorizontalName, m_X);
        }
        public void UpdateY()
        {
            m_Y = m_Joystick.Vertical;
            OnVertical?.Invoke(m_Joystick.Vertical);
            OnVerticalName?.Invoke(m_VerticalName, m_Y);
        }

        private void UpdateInputValue()
        {
            m_InputValue = new Vector2(m_X, m_Y);
            m_OnInputValueName?.Invoke(m_InputValueName, m_InputValue);
            m_OnInputValueNameConvert?.Invoke(m_InputValueName, m_InputValue);
        }
    }
}

