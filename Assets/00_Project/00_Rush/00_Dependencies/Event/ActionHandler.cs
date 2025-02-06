using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial class ActionField
    {
        private string m_ActionName;
        private UnityAction m_Action;
        public string GetActionName() => m_ActionName;
        public void SetActionName(string newName) => m_ActionName = newName;
        public UnityAction GetAction() => m_Action;
        public void AddAction(UnityAction addAction)
        {
            m_Action += addAction;
        }
        public void RemoveAction(UnityAction removeAction)
        {
            m_Action -= removeAction;
        }
    }
    public partial class ActionField<T>
    {
        private string m_ActionName;
        private UnityAction<T> m_Action;
        private T m_Value;
        public string GetActionName() => m_ActionName;
        public void SetActionName(string newName) => m_ActionName = newName;
        public UnityAction<T> GetAction() => m_Action;
        public T GetValue() => m_Value;
        public void AddAction(UnityAction<T> addAction)
        {
            m_Action += addAction;
        }
        public void RemoveAction(UnityAction<T> removeAction)
        {
            m_Action -= removeAction;
        }
        public void SetValue(T newValue)
        {
            m_Value = newValue;
        }
    }
    public partial class ActionField<T, T1>
    {
        private string m_ActionName;
        private UnityAction<T, T1> m_Action;
        public string GetActionName() => m_ActionName;
        public void SetActionName(string newName) => m_ActionName = newName;
        public UnityAction<T, T1> GetAction() => m_Action;
        public void AddAction(UnityAction<T, T1> addAction)
        {
            m_Action += addAction;
        }
        public void RemoveAction(UnityAction<T, T1> removeAction)
        {
            m_Action -= removeAction;
        }
    }
    public partial class ActionField<T, T1, T2>
    {
        private string m_ActionName;
        private UnityAction<T, T1, T2> m_Action;
        public string GetActionName() => m_ActionName;
        public void SetActionName(string newName) => m_ActionName = newName;
        public UnityAction<T, T1, T2> GetAction() => m_Action;
        public void AddAction(UnityAction<T, T1, T2> addAction)
        {
            m_Action += addAction;
        }
        public void RemoveAction(UnityAction<T, T1, T2> removeAction)
        {
            m_Action -= removeAction;
        }
    }
    public static partial class ActionHandler
    {
        private static List<ActionField> m_ActionFields = new List<ActionField>();
        public static ActionField GetActionField(string uniqueName)
        {
            ActionField field = m_ActionFields.Find(x => x.GetActionName().Equals(uniqueName));
            if (field == null) return null;
            return field;
        }
        public static void RegisterAction(string uniqueName, UnityAction regAction)
        {
            ActionField ac = GetActionField(uniqueName);;
            if (ac == null)
            {
                ActionField newAc = new ActionField();
                newAc.SetActionName(uniqueName);
                newAc.AddAction(regAction);
                m_ActionFields.Add(newAc);
            }
            else
            {
                ac.AddAction(regAction);
            }
        }
        public static void ExecuteAction(string uniqueName)
        {
            ActionField ac = GetActionField(uniqueName);
            if (ac == null)
            {
                Debug.Log($"No action name {uniqueName}");
                return;
            }
            Debug.Log($"Start action Name {uniqueName}");
            ac.GetAction()?.Invoke();
        }
        public static void UnRegisterAction(string uniqueName, UnityAction unReg)
        {
            ActionField ac = GetActionField(uniqueName);
            if (ac == null) return;
            ac.RemoveAction(unReg); 
        }
    }
    public static partial class ActionHandler<T>
    {
        private static List<ActionField<T>> m_ActionFields = new List<ActionField<T>>();
        public static ActionField<T> GetActionField(string uniqueName)
        {
            ActionField<T> field = m_ActionFields.Find(x => x.GetActionName().Equals(uniqueName));
            if (field == null) return null;
            return field;
        }
        public static void RegisterAction(string uniqueName, UnityAction<T> regAction)
        {
            ActionField<T> ac = GetActionField(uniqueName);
            if (ac == null)
            {
                ActionField<T> newAc = new ActionField<T>();
                newAc.SetActionName(uniqueName);
                newAc.AddAction(regAction);
                m_ActionFields.Add(newAc);
            }
            else
            {
                ac.AddAction(regAction);
            }
        }
        // deprecated
        public static void RegisterValue(string uniqueName, T regValue)
        {
            ActionField<T> ac = GetActionField(uniqueName);
            if (ac == null)
            {
                ActionField<T> newAc = new ActionField<T>();
                newAc.SetActionName(uniqueName);
                newAc.SetValue(regValue);
                m_ActionFields.Add(newAc);
            }
            else
            {
                ac.SetValue(regValue);
            }
        }
        public static T GetValue(string uniqueName)
        {
            ActionField<T> ac = GetActionField(uniqueName);
            if (ac != null)
            {
                return ac.GetValue();
            }
            else
            {
                return default(T);
            }
        }
        public static void ExecuteAction(string uniqueName, T value)
        {
            ActionField<T> ac = GetActionField(uniqueName);
            if (ac == null)
            {
                Debug.Log($"No action name {uniqueName}");
                return;
            }
            Debug.Log($"Start action Name {uniqueName}. value {value}");
            ac.GetAction()?.Invoke(value);
        }
        public static void UnRegisterAction(string uniqueName, UnityAction<T> unReg)
        {
            ActionField<T> ac = GetActionField(uniqueName);
            if (ac == null) return;
            ac.RemoveAction(unReg);
        }
    }
    public static partial class ActionHandler<T, T1, T2>
    {
        private static List<ActionField<T, T1, T2>> m_ActionFields = new List<ActionField<T, T1, T2>>();
        public static ActionField<T, T1, T2> GetActionField(string uniqueName)
        {
            ActionField<T, T1, T2> field = m_ActionFields.Find(x => x.GetActionName().Equals(uniqueName));
            if (field == null) return null;
            return field;
        }
        public static void RegisterAction(string uniqueName, UnityAction<T, T1, T2> regAction)
        {
            ActionField<T, T1, T2> ac = GetActionField(uniqueName);
            if (ac == null)
            {
                ActionField<T, T1, T2> newAc = new ActionField<T, T1, T2>();
                newAc.SetActionName(uniqueName);
                newAc.AddAction(regAction);
                m_ActionFields.Add(newAc);
            }
            else
            {
                ac.AddAction(regAction);
            }
        }
        public static void ExecuteAction(string uniqueName, T value, T1 value1, T2 value2)
        {
            ActionField<T, T1, T2> ac = GetActionField(uniqueName);
            if (ac == null)
            {
                Debug.Log($"No action name {uniqueName}");
                return;
            }
            Debug.Log($"Start action Name {uniqueName}. value {value}, {value1}, {value2}");
            ac.GetAction()?.Invoke(value, value1, value2);
        }
        public static void UnRegisterAction(string uniqueName, UnityAction<T, T1, T2> unReg)
        {
            ActionField<T, T1, T2> ac = GetActionField(uniqueName);
            if (ac == null) return;
            ac.RemoveAction(unReg);
        }
    }
    public static partial class ActionHandler<T, T1>
    {
        private static List<ActionField<T, T1>> m_ActionFields = new List<ActionField<T, T1>>();
        public static ActionField<T, T1> GetActionField(string uniqueName)
        {
            ActionField<T, T1> field = m_ActionFields.Find(x => x.GetActionName().Equals(uniqueName));
            if (field == null) return null;
            return field;
        }
        public static void RegisterAction(string uniqueName, UnityAction<T, T1> regAction)
        {
            ActionField<T, T1> ac = GetActionField(uniqueName);
            if (ac == null)
            {
                ActionField<T, T1> newAc = new ActionField<T, T1>();
                newAc.SetActionName(uniqueName);
                newAc.AddAction(regAction);
                m_ActionFields.Add(newAc);
            }
            else
            {
                ac.AddAction(regAction);
            }
        }
        public static void ExecuteAction(string uniqueName, T value, T1 value1)
        {
            ActionField<T, T1> ac = GetActionField(uniqueName);
            if (ac == null)
            {
                Debug.Log($"No action name {uniqueName}");
                return;
            }
            Debug.Log($"Start action Name {uniqueName}. value {value}, {value1}");
            ac.GetAction()?.Invoke(value, value1);
        }
        public static void UnRegisterAction(string uniqueName, UnityAction<T, T1> unReg)
        {
            ActionField<T, T1> ac = GetActionField(uniqueName);
            if (ac == null) return;
            ac.RemoveAction(unReg);
        }
    }
}

