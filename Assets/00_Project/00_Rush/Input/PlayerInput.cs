using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public enum InputValueType
    {
        Float = 0,
        Bool = 1,
        Vector2 = 2,
    }
    public enum InputTriggerType
    {
        None,
        KeyPress,
        KeyDown,
        KeyUp,
    }
    [Serializable]
    public class InputJoyStick
    {
        //[SerializeField] private 
    }
    [Serializable]
    public class InputReader
    {
        [SerializeField] 
        private string m_InputName;
        [SerializeField] 
        private bool m_Enabled;
        [SerializeField] 
        private InputTriggerType m_TriggerType;
        [SerializeField] 
        private InputValueType m_InputValueType;

        [ShowIf(nameof(m_InputValueType), InputValueType.Float)]
        [AllowNesting]
        [SerializeField] 
        private FloatModifier m_InputFloat = new();
        [ShowIf(nameof(m_InputValueType), InputValueType.Bool)]
        [AllowNesting]
        [SerializeField] 
        private BoolModifier m_InputBool = new();
        [ShowIf(nameof(m_InputValueType), InputValueType.Vector2)]
        [AllowNesting]
        [SerializeField]
        private Vector2Modifier m_InputVector2 = new();

        public string GetName() => m_InputName;
        public void HandleInput()
        {
            if (!m_Enabled) return;
            switch (m_TriggerType)
            {
                case (InputTriggerType.KeyPress):
                    HandlePressed();
                    break;
                case (InputTriggerType.KeyDown):
                    HandleKeyDown();
                    break;
                case (InputTriggerType.KeyUp):
                    HandleKeyUp();
                    break;
                    
            }
        }
        private void HandlePressed()
        {
            switch(m_InputValueType)
            {
                case InputValueType.Float:
                    float axesValueFloat = Input.GetAxis(m_InputName);
                    m_InputFloat.SetValue(axesValueFloat);
                    break; 
                case InputValueType.Bool:
                    bool axesValueBool = Input.GetButton(m_InputName);
                    m_InputBool.SetValue(axesValueBool);
                    break;
            } 
        }
        private void HandleKeyDown()
        {
            bool axesValueBool = Input.GetButtonDown(m_InputName);
            if (axesValueBool)
            {
                switch (m_InputValueType)
                {
                    case InputValueType.Float:
                        m_InputFloat.SetValue(1);
                        break;
                    case InputValueType.Bool:
                        m_InputBool.SetValue(true);
                        break;
                }
            }
        }
        private void HandleKeyUp()
        {
            bool axesValueBool = Input.GetButtonUp(m_InputName);
            if (axesValueBool)
            {
                switch (m_InputValueType)
                {
                    case InputValueType.Float:
                        m_InputFloat.SetValue(0);
                        break;
                    case InputValueType.Bool:
                        m_InputBool.SetValue(false);
                        break;
                }
            }
        }
        public void SetFloat(float set)
        {
            m_InputFloat.SetValue(set);
        }
        public void SetBool(bool set)
        {
            m_InputBool.SetValue(set);
        }
        public void SetVector2(Vector2 set)
        {
            m_InputVector2.SetValue(set);
        }
    }
    [Serializable]
    public class PlayerInputField
    {
        [SerializeField]
        private bool m_EnableUpdate;
        [SerializeField] 
        private List<InputReader> m_InputReaders = new List<InputReader>();
        public void HandleKeyBoardInput()
        {
            if (!m_EnableUpdate) return;
            foreach (var handler in m_InputReaders)
            {
                handler.HandleInput();
            }
        }
        private InputReader GetInput(string name)
        {
            InputReader input = m_InputReaders.Find(x => x.GetName() == name);
            return input;
        }
        public void SetFloat(string name, float set)
        {
            InputReader i = GetInput(name);
            i.SetFloat(set);
        }
        public void SetBool(string name, bool set)
        {
            InputReader i = GetInput(name);
            i.SetBool(set);
        }
        public void SetVector2(string name, Vector2 set)
        {
            InputReader i = GetInput(name);
            i.SetVector2(set);
        }
    }
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] 
        private PlayerInputField m_InputField;

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            m_InputField.HandleKeyBoardInput();
        }

        public void SetFloat(string name, float set)
        {
            m_InputField.SetFloat(name, set);
        }
        public void SetBool(string name, bool set)
        {
            m_InputField.SetBool(name, set);
        }
        
        public void SetFloat(object name, object set)
        {
            if (name is string && set is float)
            {
                m_InputField.SetFloat((string)name, (float)set);
            }
                
        }
        public void SetBool(object name, object set)
        {
            if (name is string && set is bool)
            {
                m_InputField.SetBool((string)name, (bool)set);
            }
            
        }
        // on Drag n drop Event
        public void SetVector2(string name, Vector2 set)
        {
            m_InputField.SetVector2(name, set);
            // kepanggil
        }

        // on Drag n drop Event
        public void SetVector2(object name, object set)
        {
            if (name is string newName && set is Vector2 newSet)
            {
                m_InputField.SetVector2(newName, newSet);
            }
        }
    }
}

