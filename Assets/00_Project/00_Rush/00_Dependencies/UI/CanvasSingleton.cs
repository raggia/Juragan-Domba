
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public partial class CanvasSingleton : Singleton<CanvasSingleton>
    {
        [SerializeField]
        private List<CanvasView> m_CanvasViews = new();
        [SerializeField]
        private List<PanelView> m_PanelCollection = new();

        public void RegisterCamera()
        {
            foreach (CanvasView canvas in m_CanvasViews)
            {
                canvas.RegisterCamera();
            }
        }

        public T GetPanel<T>(string id) where T : PanelView
        {
            PanelView target = m_PanelCollection.Find(x => x.Id == id);
            return (T)target;
        }

        public T GetPanel<T>() where T : PanelView
        {
            PanelView target = m_PanelCollection.Find(x => x.GetType() == typeof(T));
            return (T)target;
        }

        public PanelView GetPanel(string id)
        {
            PanelView target = m_PanelCollection.Find(x => x.Id == id);
            return target;
        }

        private CanvasView GetCanvasView(RenderMode mode)
        {
            CanvasView match = m_CanvasViews.Find(x => x.GetRenderMode() == mode);
            return match;
        }
        public void ShowCanvas(RenderMode mode, float delay = 0f)
        {
            GetCanvasView(mode).Show(delay);
        }
        public void HideCanvas(RenderMode mode, float delay = 0f)
        {
            GetCanvasView(mode).Hide(delay);
        }
        public void ShowThenHideCanvasByDuration(RenderMode mode, float duration, float showDelay = 0f, float hideDelay = 0f)
        {
            GetCanvasView(mode).ShowThenHideByDuration(duration, showDelay, hideDelay);
        }
        public void ShowThenHideCanvasByDuration(RenderMode mode, float duration)
        {
            GetCanvasView(mode).ShowThenHideByDuration(duration);
        }
    }
}

