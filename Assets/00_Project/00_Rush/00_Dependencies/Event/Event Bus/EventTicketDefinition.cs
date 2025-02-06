using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public enum ValueType
    {
        None,
        Bool,
        Float,
        Vector2,
        IntBool,
        StringVector2,
        /*String,
        Int,
        
        Bool,
        Vector2,
        Vector3,
        Component,
        GameObject,
        Transform,
        Sprite,
        AudioClip,
        ScriptableObject,*/
        Object,
        ObjectDouble,
        ObjectTriple,
    }
    [Serializable]
    public class EventPassangerSOField
    {
        [Header("Executed when placed on Followers")]
        [Space]
        public ValueType ExecuteValue = ValueType.None;

        [Header("__________FOLLOWERS__________")]

        [ShowIf(nameof(ExecuteValue), ValueType.None)]
        [AllowNesting]
        public UnityEvent VoidValue;

        [ShowIf(nameof(ExecuteValue), ValueType.Float)]
        [AllowNesting]
        public UnityEvent<float> FloatValue;

        [ShowIf(nameof(ExecuteValue), ValueType.Vector2)]
        [AllowNesting]
        public UnityEvent<Vector2> Vector2Value;

        [ShowIf(nameof(ExecuteValue), ValueType.Bool)]
        [AllowNesting]
        public UnityEvent<bool> BoolValue;

        [ShowIf(nameof(ExecuteValue), ValueType.IntBool)]
        [AllowNesting]
        public UnityEvent<int, bool> IntBoolValue;

        [ShowIf(nameof(ExecuteValue), ValueType.StringVector2)]
        [AllowNesting]
        public UnityEvent<string, Vector2> StringVector2Value;

        [ShowIf(nameof(ExecuteValue), ValueType.Object)]
        [AllowNesting]
        public UnityEvent<object> ObjectValue;

        [ShowIf(nameof(ExecuteValue), ValueType.ObjectDouble)]
        [AllowNesting]
        public UnityEvent<object, object> ObjectDoubleValue;

        [ShowIf(nameof(ExecuteValue), ValueType.ObjectTriple)]
        [AllowNesting]
        public UnityEvent<object, object, object> ObjectTripleValue;
    }
    [CreateAssetMenu(fileName = "UniqueId_ClassName_FunctionName", menuName = "Rush/Event Bus/Event Ticket", order = 1)]
    public class EventTicketDefinition : UnitDefinition
    {
        [SerializeField] private EventPassangerSOField m_GameEventField;

        public void Register(UnityAction regAction)
        {
            ActionHandler.RegisterAction(name + GetInstanceID(), regAction);
        }
        public void Register<T>(UnityAction<T> regAction)
        {
            ActionHandler<T>.RegisterAction(name + GetInstanceID(), regAction);
        }
        public void Register<T, T1>(UnityAction<T, T1> regAction)
        {
            ActionHandler<T, T1>.RegisterAction(name + GetInstanceID(), regAction);
        }
        public void Register<T, T1, T2>(UnityAction<T, T1, T2> regAction)
        {
            ActionHandler<T, T1, T2>.RegisterAction(name + GetInstanceID(), regAction);
        }
        public void Execute()
        {
            ActionHandler.ExecuteAction(name + GetInstanceID());
            m_GameEventField.VoidValue?.Invoke();
        }
        public void Execute<T>(T value)
        {
            ActionHandler<T>.ExecuteAction(name + GetInstanceID(), value);
            m_GameEventField.ObjectValue?.Invoke(value);

        }
        public void Execute(object value)
        {
            ActionHandler<object>.ExecuteAction(name + GetInstanceID(), value);
            m_GameEventField.ObjectValue?.Invoke(value);

        }
        public void Execute(object value, object value1)
        {
            ActionHandler<object, object>.ExecuteAction(name + GetInstanceID(), value, value1);
            m_GameEventField.ObjectDoubleValue?.Invoke(value, value1);

        }
        public void Execute(object value, object value1, object value2)
        {
            ActionHandler<object, object, object>.ExecuteAction(name + GetInstanceID(), value, value1, value2);
            m_GameEventField.ObjectTripleValue?.Invoke(value1, value1, value2);

        }
        public void Execute(bool value)
        {
            ActionHandler<bool>.ExecuteAction(name + GetInstanceID(), value);
            m_GameEventField.BoolValue?.Invoke(value);

        }
        public void Execute(float value)
        {
            ActionHandler<float>.ExecuteAction(name + GetInstanceID(), value);
            m_GameEventField.FloatValue.Invoke(value);

        }
        public void Execute(Vector2 value)
        {
            ActionHandler<Vector2>.ExecuteAction(name + GetInstanceID(), value);
            m_GameEventField.Vector2Value.Invoke(value);

        }
        public void Execute(int value, bool value1)
        {
            ActionHandler<int, bool>.ExecuteAction(name + GetInstanceID(), value, value1);
            m_GameEventField.IntBoolValue.Invoke(value, value1);

        }
        public void Execute(string value, Vector2 value1)
        {
            ActionHandler<string, Vector2>.ExecuteAction(name + GetInstanceID(), value, value1);
            m_GameEventField.StringVector2Value.Invoke(value, value1);

        }
        
        public void UnRegister(UnityAction unRegAction)
        {
            ActionHandler.UnRegisterAction(name + GetInstanceID(), unRegAction);
        }
        public void UnRegister<T>(UnityAction<T> unRegAction)
        {
            ActionHandler<T>.UnRegisterAction(name + GetInstanceID(), unRegAction);
        }
        public void UnRegister<T, T1>(UnityAction<T, T1> unRegAction)
        {
            ActionHandler<T, T1>.UnRegisterAction(name + GetInstanceID(), unRegAction);
        }
        public void UnRegister<T, T1, T2>(UnityAction<T, T1, T2> unRegAction)
        {
            ActionHandler<T, T1, T2>.UnRegisterAction(name + GetInstanceID(), unRegAction);
        }
    }
}

