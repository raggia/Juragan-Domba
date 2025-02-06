using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public abstract class EventChairField 
    {
        
    }
    [System.Serializable]
    public partial class VoidChairField : EventChairField
    {
        [SerializeField]
        private NewEventTicketDefinition m_EventTicket;
        public NewEventTicketDefinition EventTicket => m_EventTicket;

        [SerializeField]
        public UnityEvent m_OnRegister = new();
        public void Register(UnityAction action)
        {
            if (m_EventTicket == null) return;
            m_EventTicket.Register(action);
        }
        public void Listen()
        {
            m_OnRegister?.Invoke();
        }
        public void UnRegister(UnityAction action)
        {
            if (m_EventTicket == null) return;
            m_EventTicket.UnRegister(action);
        }
    }

    [System.Serializable]
    public abstract class EventSingleChairField<T> : EventChairField
    {
        [SerializeField]
        private NewEventTicketDefinition m_EventTicket;
        public NewEventTicketDefinition EventTicket => m_EventTicket;

        [SerializeField]
        public UnityEvent<T> m_OnRegister = new();
        public void Register(UnityAction<T> action)
        {
            if (m_EventTicket == null) return;
            m_EventTicket.Register(action);
        }
        public void Listen(T value)
        {
            m_OnRegister?.Invoke(value);
        }
        public void UnRegister(UnityAction<T> action)
        {
            if (m_EventTicket == null) return;
            m_EventTicket.UnRegister(action);
        }
    }
    [System.Serializable]
    public abstract class EventDoubleChairsField<T, T1> : EventChairField
    {
        [SerializeField]
        private NewEventTicketDefinition m_EventTicket;
        public NewEventTicketDefinition EventTicket => m_EventTicket;

        [SerializeField]
        public UnityEvent<T, T1> m_OnRegister = new();
        public void Register(UnityAction<T, T1> action)
        {
            if (m_EventTicket == null) return;
            m_EventTicket.Register(action);
        }
        public void Listen(T value, T1 value1)
        {
            m_OnRegister?.Invoke(value, value1);
        }
        public void UnRegister(UnityAction<T, T1> action)
        {
            if (m_EventTicket == null) return;
            m_EventTicket.UnRegister(action);
        }
    }
    [System.Serializable]
    public abstract class EventTripleChairsField<T, T1, T2> : EventChairField
    {
        [SerializeField]
        private NewEventTicketDefinition m_EventTicket;
        public NewEventTicketDefinition EventTicket => m_EventTicket;

        [SerializeField]
        public UnityEvent<T, T1, T2> m_OnRegister = new();
        public void Register(UnityAction<T, T1, T2> action)
        {
            if (m_EventTicket == null) return;
            m_EventTicket.UnRegister(action);
        }
        public void Listen(T value, T1 value1, T2 value2)
        {
            m_OnRegister?.Invoke(value, value1, value2);
        }
        public void UnRegister(UnityAction<T, T1, T2> action)
        {
            if (m_EventTicket == null) return;
            m_EventTicket.UnRegister(action);
        }
    }
}
