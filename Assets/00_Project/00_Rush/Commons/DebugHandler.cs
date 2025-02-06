using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    [Serializable]
    public partial class DebugHandlerField
    {
        public bool Enable = false;
    }
    public partial class DebugHandler : MonoBehaviour
    {
        [SerializeField] private DebugHandlerField _debugHandlerField;
        private void Awake()
        {
            Debug.unityLogger.logEnabled = _debugHandlerField.Enable;
        }
    }
}

