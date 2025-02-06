using Rush;
using Unity.VisualScripting;
using UnityEngine;

namespace Rush
{
    public partial class CanvasView : View
    {
        [SerializeField]
        private Canvas m_Canvas;

        public void RegisterCamera()
        {
            m_Canvas.worldCamera = Camera.main;
        }
        public RenderMode GetRenderMode()
        {
            return m_Canvas.renderMode;
        }
    }
}

