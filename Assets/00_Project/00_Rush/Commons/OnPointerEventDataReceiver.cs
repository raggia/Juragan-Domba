using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Rush
{
    [System.Serializable]
    public partial class OnPointerEventDataField
    {
        [SerializeField]
        private UnityEvent<Vector2> m_OnSetPointerPosition = new();
        public void SetPosition(BaseEventData eventData) 
        { 
            PointerEventData pointerEventData = eventData as PointerEventData;
            m_OnSetPointerPosition?.Invoke(pointerEventData.position);
        }
    }
    public class OnPointerEventDataReceiver : MonoBehaviour
    {
        [SerializeField]
        private OnPointerEventDataField m_OnPointerEventDataField;

        public void SetPosition(BaseEventData eventData)
        {
            m_OnPointerEventDataField.SetPosition(eventData);
        }
    }
}
