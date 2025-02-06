using NaughtyAttributes;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [Serializable]
    public class VoidField
    {
        public EventTicketDefinition RegisterTarget;
        public UnityEvent RegisterEvent = new UnityEvent();
        public void Register(UnityAction action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.Register(action);
        }
        public void Listen()
        {
            RegisterEvent?.Invoke();
        }
        public void UnRegister(UnityAction action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.UnRegister(action);
        }
    }
    [Serializable]
    public class BoolField
    {
        public EventTicketDefinition RegisterTarget;
        public UnityEvent<bool> RegisterEvent = new UnityEvent<bool>();
        public void Register(UnityAction<bool> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.Register(action);
        }
        public void Listen(bool value)
        {
            RegisterEvent?.Invoke(value);
        }
        public void UnRegister(UnityAction<bool> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.UnRegister(action);
        }
    }
    [Serializable]
    public class FloatField
    {
        public EventTicketDefinition RegisterTarget;
        public UnityEvent<float> RegisterEvent = new UnityEvent<float>();
        public void Register(UnityAction<float> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.Register(action);
        }
        public void Listen(float value)
        {
            RegisterEvent?.Invoke(value);
        }
        public void UnRegister(UnityAction<float> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.UnRegister(action);
        }
    }
    [Serializable]
    public class Vector2Field
    {
        public EventTicketDefinition RegisterTarget;
        public UnityEvent<Vector2> RegisterEvent = new();
        public void Register(UnityAction<Vector2> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.Register(action);
        }
        public void Listen(Vector2 value)
        {
            RegisterEvent?.Invoke(value);
        }
        public void UnRegister(UnityAction<Vector2> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.UnRegister(action);
        }
    }
    [Serializable]
    public class IntBoolField
    {
        public EventTicketDefinition RegisterTarget;
        public UnityEvent<int, bool> RegisterEvent = new();
        public void Register(UnityAction<int, bool> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.Register(action);
        }
        public void Listen(int value, bool value1)
        {
            RegisterEvent?.Invoke(value, value1);
        }
        public void UnRegister(UnityAction<int, bool> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.UnRegister(action);
        }
    }
    [Serializable]
    public class StringVector2Field
    {
        public EventTicketDefinition RegisterTarget;
        public UnityEvent<string, Vector2> RegisterEvent = new ();
        public void Register(UnityAction<string, Vector2> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.Register(action);
        }
        public void Listen(string value, Vector2 value1)
        {
            RegisterEvent?.Invoke(value, value1);
        }
        public void UnRegister(UnityAction<string, Vector2> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.UnRegister(action);
        }
    }
    [Serializable]
    public class ObjectField
    {
        public EventTicketDefinition RegisterTarget;
        public UnityEvent<object> RegisterEvent = new UnityEvent<object>();
        public void Register(UnityAction<object> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.Register(action);
        }
        public void Listen(object value)
        {
            RegisterEvent?.Invoke(value);
        }
        public void UnRegister(UnityAction<object> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.UnRegister(action);
        }
    }
    [Serializable]
    public class ObjectDoubleField
    {
        public EventTicketDefinition RegisterTarget;
        public UnityEvent<object, object> RegisterEvent = new UnityEvent<object, object>();
        public void Register(UnityAction<object, object> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.Register(action);
        }
        public void Listen(object value, object value1)
        {
            RegisterEvent?.Invoke(value, value1);
        }
        public void UnRegister(UnityAction<object, object> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.UnRegister(action);
        }
    }
    [Serializable]
    public class ObjectTripleField
    {
        public EventTicketDefinition RegisterTarget;
        public UnityEvent<object, object, object> RegisterEvent = new UnityEvent<object, object, object>();
        public void Register(UnityAction<object, object, object> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.Register(action);
        }
        public void Listen(object value, object value1, object value2)
        {
            RegisterEvent?.Invoke(value, value1, value2);
        }
        public void UnRegister(UnityAction<object, object, object> action)
        {
            if (RegisterTarget == null) return;
            RegisterTarget.UnRegister(action);
        }
    }
    
    [Serializable]
    public class TargetField
    {
        public ValueType EventValue;

        [ShowIf(nameof(EventValue), ValueType.None)]
        [AllowNesting]
        public VoidField EventField;

        [ShowIf(nameof(EventValue), ValueType.Bool)]
        [AllowNesting]
        public BoolField EventFieldBool;
        [ShowIf(nameof(EventValue), ValueType.Float)]
        [AllowNesting]
        public FloatField EventFieldFloat;

        [ShowIf(nameof(EventValue), ValueType.Vector2)]
        [AllowNesting]
        public Vector2Field EventFieldVector2;

        [ShowIf(nameof(EventValue), ValueType.IntBool)]
        [AllowNesting]
        public IntBoolField EventFieldIntBool;

        [ShowIf(nameof(EventValue), ValueType.StringVector2)]
        [AllowNesting]
        public StringVector2Field EventFieldStringVector2;

        [ShowIf(nameof(EventValue), ValueType.Object)]
        [AllowNesting]
        public ObjectField EventFieldObject;

        [ShowIf(nameof(EventValue), ValueType.ObjectDouble)]
        [AllowNesting]
        public ObjectDoubleField EventFieldObjectDouble;

        [ShowIf(nameof(EventValue), ValueType.ObjectTriple)]
        [AllowNesting]
        public ObjectTripleField EventFieldObjectTripple;

        public void RegisterAll()
        {
            switch(EventValue)
            {
                case ValueType.None:
                    Register(Listen);
                    break;
                case ValueType.Bool: 
                    RegisterBool(ListenBool);
                    break;
                case ValueType.Float: 
                    RegisterFloat(ListenFloat); 
                    break;
                case ValueType.Vector2:
                    RegisterVector2(ListenVector2);
                    break;
                case ValueType.IntBool:
                    RegisterIntBool(ListenIntBool);
                    break;
                case ValueType.StringVector2:
                    RegisterStringVector2(ListenStringVector2);
                    break;
                case ValueType.Object: 
                    RegisterObject(ListenObject);
                    break;
                case ValueType.ObjectDouble: 
                    RegisterObjectDouble(ListenObjectDouble); 
                    break;
                case ValueType.ObjectTriple:
                    RegisterObjectTripple(ListenObjectTripple);
                    break;
            }
        }
        public void UnRegisterAll()
        {
            switch (EventValue)
            {
                case ValueType.None:
                    UnRegister(Listen);
                    break;
                case ValueType.Bool:
                    UnRegisterIntBool(ListenBool);
                    break;
                case ValueType.Float:
                    UnRegisterFloat(ListenFloat);
                    break;
                case ValueType.Vector2:
                    UnRegisterVector2(ListenVector2);
                    break;
                case ValueType.IntBool:
                    UnRegisterIntBool(ListenIntBool);
                    break;
                case ValueType.StringVector2:
                    UnRegisterStringVector2(ListenStringVector2);
                    break;
                case ValueType.Object:
                    UnRegisterObject(ListenObject);
                    break;
                case ValueType.ObjectDouble:
                    UnRegisterObjectDouble(ListenObjectDouble);
                    break;
                case ValueType.ObjectTriple:
                    UnRegisterObjectTripple(ListenObjectTripple);
                    break;
            }
        }

        private void Register(UnityAction action)
        {
            EventField.Register(action);
        }
        private void RegisterBool(UnityAction<bool> action)
        {
            EventFieldBool.Register(action);
        }
        private void RegisterFloat(UnityAction<float> action)
        {
            EventFieldFloat.Register(action);
        }
        private void RegisterVector2(UnityAction<Vector2> action)
        {
            EventFieldVector2.Register(action);
        }
        private void RegisterIntBool(UnityAction<int, bool> action)
        {
            EventFieldIntBool.Register(action);
        }
        private void RegisterStringVector2(UnityAction<string, Vector2> action)
        {
            EventFieldStringVector2.Register(action);
        }
        private void RegisterObject(UnityAction<object> action)
        {
            EventFieldObject.Register(action);
        }
        private void RegisterObjectDouble(UnityAction<object, object> action)
        {
            EventFieldObjectDouble.Register(action);
        }
        private void RegisterObjectTripple(UnityAction<object, object, object> action)
        {
            EventFieldObjectTripple.Register(action);
        }
        private void Listen()
        {
            EventField.Listen();
        }
        private void ListenBool(bool value)
        {
            EventFieldBool.Listen(value);
        }
        private void ListenFloat(float value)
        {
            EventFieldFloat.Listen(value);
        }
        private void ListenVector2(Vector2 value)
        {
            EventFieldVector2.Listen(value);
        }
        private void ListenIntBool(int value, bool value1)
        {
            EventFieldIntBool.Listen(value, value1);
        }
        private void ListenStringVector2(string value, Vector2 value1)
        {
            EventFieldStringVector2.Listen(value, value1);
        }
        private void ListenObject(object value)
        {
            EventFieldObject.Listen(value);
        }
        private void ListenObjectDouble(object value, object value1)
        {
            EventFieldObjectDouble.Listen(value, value1);
        }
        private void ListenObjectTripple(object value, object value1, object value2)
        {
            EventFieldObjectTripple.Listen(value, value1, value2);
        }
        private void UnRegister(UnityAction action)
        {
            EventField.UnRegister(action);
        }
        private void UnRegisterIntBool(UnityAction<bool> action)
        {
            EventFieldBool.UnRegister(action);
        }
        private void UnRegisterFloat(UnityAction<float> action)
        {
            EventFieldFloat.UnRegister(action);
        }
        private void UnRegisterVector2(UnityAction<Vector2> action)
        {
            EventFieldVector2.UnRegister(action);
        }
        private void UnRegisterIntBool(UnityAction<int, bool> action)
        {
            EventFieldIntBool.UnRegister(action);
        }
        private void UnRegisterStringVector2(UnityAction<string, Vector2> action)
        {
            EventFieldStringVector2.UnRegister(action);
        }
        private void UnRegisterObject(UnityAction<object> action)
        {
            EventFieldObject.UnRegister(action);
        }
        private void UnRegisterObjectDouble(UnityAction<object, object> action)
        {
            EventFieldObjectDouble.UnRegister(action);
        }
        private void UnRegisterObjectTripple(UnityAction<object, object, object> action)
        {
            EventFieldObjectTripple.UnRegister(action);
        }
    }

    [Serializable]
    public class EventBusField
    {
        [SerializeField] 
        private List<TargetField> m_TargetRegisters;

        public void Register()
        {
            foreach (TargetField targetEvent in m_TargetRegisters)
            {
                targetEvent.RegisterAll();
            }
        }
        public void UnRegister()
        {
            foreach (TargetField targetEvent in m_TargetRegisters)
            {
                targetEvent.UnRegisterAll();
            }
        }
    }
    public partial class EventBus : MonoBehaviour
    {
        [SerializeField] 
        private EventBusField m_EventBusField;
        private void OnEnable()
        {
            m_EventBusField.Register();
        }
        private void OnDisable()
        {
            m_EventBusField.UnRegister();
        }
    }
}

