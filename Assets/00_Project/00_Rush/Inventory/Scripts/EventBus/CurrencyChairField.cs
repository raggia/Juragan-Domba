using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class CurrencyChairField : EventSingleChairField<ICurrency>
    {
        
    }

    public partial class NewEventTicketDefinition
    {
        public void Execute(ICurrency value)
        {
            ActionHandler<ICurrency>.ExecuteAction(name, value);
        }
    }

    public partial class NewEventBus
    {
        public void RegisterICurrencyChair()
        {
            RegisterSingleChair<ICurrency>();
        }
        public void UnRegisterICurrencyChair()
        {
            UnRegisterSingleChair<ICurrency>();
        }
    }
}
