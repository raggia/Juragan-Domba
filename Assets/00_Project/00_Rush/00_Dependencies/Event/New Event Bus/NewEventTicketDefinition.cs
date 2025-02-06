using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [CreateAssetMenu(fileName = "New Event Ticket", menuName = "Rush/New Event Bus/Ticket")]
    public partial class NewEventTicketDefinition : UnitDefinition
    {
        public void Register(UnityAction action)
        {
            ActionHandler.RegisterAction(name, action);
        }
        public void Register<T>(UnityAction<T> action)
        {
            ActionHandler<T>.RegisterAction(name, action);
        }
        public void Register<T, T1>(UnityAction<T, T1> action)
        {
            ActionHandler<T, T1>.RegisterAction(name, action);
        }
        public void Register<T, T1, T2>(UnityAction<T, T1, T2> action)
        {
            ActionHandler<T, T1, T2>.RegisterAction(name, action);
        }
        public void UnRegister(UnityAction action)
        {
            ActionHandler.UnRegisterAction(name, action);
        }
        public void UnRegister<T>(UnityAction<T> action)
        {
            ActionHandler<T>.UnRegisterAction(name, action);
        }
        public void UnRegister<T, T1>(UnityAction<T, T1> action)
        {
            ActionHandler<T, T1>.UnRegisterAction(name, action);
        }
        public void UnRegister<T, T1, T2>(UnityAction<T, T1, T2> action)
        {
            ActionHandler<T, T1, T2>.UnRegisterAction(name, action);
        }
        public void Execute()
        {
            ActionHandler.ExecuteAction(name);
        }
        public void Execute(string value)
        {
            ActionHandler<string>.ExecuteAction(name, value);
        }
        public void Execute(float value)
        {
            ActionHandler<float>.ExecuteAction(name, value);
        }
        public void Execute(bool value)
        {
            ActionHandler<bool>.ExecuteAction(name, value);
        }
        public void Execute(string value, Vector2 value1)
        {
            ActionHandler<string, Vector2>.ExecuteAction(name, value, value1);
        }
        public void Execute(AudioClip value)
        {
            ActionHandler<AudioClip>.ExecuteAction(name, value);
        }
        public void Execute(Transform value)
        {
            ActionHandler<Transform>.ExecuteAction(name, value);
        }
    }
}
