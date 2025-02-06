using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    [Serializable]
    public class JoyStickUIViewField
    {
        [SerializeField] 
        private GameObject m_LeftJoystickContent;
        [SerializeField] 
        private GameObject m_RightJoystickContent;

        public void SetLeftJoyStickContentActive(bool val)
        {
            m_LeftJoystickContent.SetActive(val);
        }
        public void SetRightJoyStickContentActive(bool val)
        {
            m_RightJoystickContent.SetActive(val);
        }
    }
    public class JoyStickUIView : View
    {
        [SerializeField]
        private JoyStickUIViewField m_JoyStickUIViewField;
        public void SetLeftJoyStickContentActive(bool val)
        {
            m_JoyStickUIViewField.SetLeftJoyStickContentActive(val);
        }
        public void SetRightJoyStickContentActive(bool val)
        {
            m_JoyStickUIViewField.SetRightJoyStickContentActive(val);
        }
    }
}

