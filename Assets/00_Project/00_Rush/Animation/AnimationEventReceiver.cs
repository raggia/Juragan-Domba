using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public class AnimationEventField
    {
        public static string StateNameProperty = nameof(m_EventName);
        public static string StateEventProperty = nameof(m_OnAnimationEvent);

        [SerializeField]
        private string m_EventName;
        [SerializeField]
        private UnityEvent m_OnAnimationEvent;
        public string GetEventName()
        {
            return m_EventName;
        }
        public void OnAnimationEventInvoke()
        {
            m_OnAnimationEvent?.Invoke();
            Debug.Log($"AnimationEvent with [{m_EventName}] is Triggred");
        }

        public AnimationEventField(string eventName)
        {
            m_EventName = eventName;
        }
    }
    public class AnimationEventReceiver : MonoBehaviour
    {
        [SerializeField]
        private List<AnimationEventField> m_Events = new();

        private AnimationEventField GetEventInternal(string eventName)
        {
            AnimationEventField target = m_Events.Find(x => x.GetEventName() == eventName);
            if (target == null)
            {
                target = new AnimationEventField(eventName);
                m_Events.Add(target);
                Debug.LogWarning($"No AnimationEvent Name [{eventName}] exist on list");
            }
            return target;
        }

        public void OnAnimationEventInvoked(string eventName)
        {
            GetEventInternal(eventName).OnAnimationEventInvoke();
        }
    }
}

