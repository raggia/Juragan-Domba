using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public partial class StatChair : EventSingleChairField<Stat>
    {

    }
    public partial class NewEventTicketDefinition
    {
        public void Execute(Stat stat)
        {
            ActionHandler<Stat>.ExecuteAction(name, stat);
        }
    }
    public partial class NewEventBus
    {
        public void RegisterStat()
        {
            RegisterSingleChair<Stat>();    
        }
        public void UnRegisterStat()
        {
            UnRegisterSingleChair<Stat>();
        }
    }

    public partial class StatAgent : MonoBehaviour
    {
        [SerializeField]
        private StatNameDefinition m_Definition;

        [SerializeField]
        private UnityEvent<Stat> m_OnTake = new();

        public void Take(float take)
        {
            OnTakeInvoke(take);
        }
        private void OnTakeInvoke(float take)
        {
            Stat stat = new Stat(m_Definition, take);
            m_OnTake?.Invoke(stat);
        }

    }
}
