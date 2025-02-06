using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rush
{
    [System.Serializable]
    public class InteractButtonViewSetting : ButtonViewSetting
    {
        
    }
    public class InteractButtonView : ButtonUIView, ISetIdentifier
    {
        public void SetId(string id)
        {
            m_Id = id;
        }
    }
}

