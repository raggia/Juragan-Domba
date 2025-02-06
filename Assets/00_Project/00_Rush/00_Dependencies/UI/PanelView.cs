using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public partial class PanelView : UIView
    {
        [SerializeField]
        private bool m_IsBusyPanel;
        public bool IsBusy => m_IsBusyPanel;
        public void SetIsBusy(bool val)
        {
            m_IsBusyPanel = val;
        }
    }
}
