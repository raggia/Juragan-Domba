namespace Rush
{
    //make your own class
    public partial class CustomEvent
    {

    }

    // Inherit your Field from EventChairField (Single,Double,Triple)
    [System.Serializable]
    public partial class CustomChairField : EventSingleChairField<CustomEvent>
    {

    }

    // Write partial Class for NewEventTicket
    // Then Write your own, public void Execute(CustomEvent value) Function
    public partial class NewEventTicketDefinition
    {
        public void Execute(CustomEvent value)
        {
            ActionHandler<CustomEvent>.ExecuteAction(name, value);
        }
    }
    // Write partial Class for NewEventBus
    // Then Write your own, public void RegisterCustomChair() Function
    // And public void UnRegisterCustomChair() Function
    public partial class NewEventBus
    {
        // Place this func on NewEventBus.OnRegisterEvent
        
        public void RegisterCustomChair()
        {
            RegisterSingleChair<CustomEvent>();
        }
        // Place this func on NewEventBus.OnUnRegisterEvent
        public void UnRegisterCustomChair()
        {
            UnRegisterSingleChair<CustomEvent>();
        }
    }

    // Then place your ticket to other event, and Choose Execute Func
}
