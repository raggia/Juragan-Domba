using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class ItemChairField : EventSingleChairField<IItem>
    {
        
    }

    public partial class NewEventTicketDefinition
    {
        public void Execute(IItem value)
        {
            ActionHandler<IItem>.ExecuteAction(name, value);
        }
    }

    public partial class NewEventBus
    {
        public void RegisterItemChair()
        {
            RegisterSingleChair<IItem>();
        }
        public void UnRegisterItemChair()
        {
            UnRegisterSingleChair<IItem>();
        }
    }
}
