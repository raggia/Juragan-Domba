using NaughtyAttributes.Test;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial class NewEventBus : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private EventChairField[] m_Chairs;

        [SerializeField]
        private UnityEvent m_OnRegister = new();
        [SerializeField]
        private UnityEvent m_OnUnregister = new();
        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            UnRegister();
        }
        private void Register()
        {
            RegisterVoidChair();
            RegisterSingleChair<string>();
            RegisterSingleChair<bool>();
            RegisterSingleChair<float>();
            RegisterSingleChair<AudioClip>();
            RegisterSingleChair<Transform>();
            RegisterDoubleChairs<string, Vector2>();

            m_OnRegister?.Invoke();
        }

        private void UnRegister()
        {
            UnRegisterVoidChair();
            UnRegisterSingleChair<string>();
            UnRegisterSingleChair<bool>();
            UnRegisterSingleChair<float>();
            UnRegisterSingleChair<AudioClip>();
            UnRegisterSingleChair<Transform>();
            UnRegisterDoubleChairs<string, Vector2>();

            m_OnUnregister?.Invoke();
        }

        private void RegisterVoidChair()
        {
            foreach (var chair in m_Chairs)
            {
                if (chair is VoidChairField newChair)
                {
                    newChair.Register(newChair.Listen);
                }
            }
        }
        private void RegisterSingleChair<T>()
        {
            foreach (var chair in m_Chairs)
            {
                if (chair is EventSingleChairField<T> newChair)
                {
                    newChair.Register(newChair.Listen);
                }
            }
        }
        private void RegisterDoubleChairs<T, T1>()
        {
            foreach (var chair in m_Chairs)
            {
                if (chair is EventDoubleChairsField<T, T1> newChair)
                {
                    newChair.Register(newChair.Listen);
                }
            }
        }
        private void RegisterTripleChairs<T, T1, T2>()
        {
            foreach (var chair in m_Chairs)
            {
                if (chair is EventTripleChairsField<T, T1, T2> newChair)
                {
                    newChair.Register(newChair.Listen);
                }
            }
        }
        private void UnRegisterVoidChair()
        {
            foreach (var chair in m_Chairs)
            {
                if (chair is VoidChairField newChair)
                {
                    newChair.UnRegister(newChair.Listen);
                }
            }
        }
        private void UnRegisterSingleChair<T>()
        {
            foreach (var chair in m_Chairs)
            {
                if (chair is EventSingleChairField<T> newChair)
                {
                    newChair.Register(newChair.Listen);
                }
            }
        }
        private void UnRegisterDoubleChairs<T, T1>()
        {
            foreach (var chair in m_Chairs)
            {
                if (chair is EventDoubleChairsField<T, T1> newChair)
                {
                    newChair.UnRegister(newChair.Listen);
                }
            }
        }
        private void UnRegisterTripleChairs<T, T1, T2>()
        {
            foreach (var chair in m_Chairs)
            {
                if (chair is EventTripleChairsField<T, T1, T2> newChair)
                {
                    newChair.UnRegister(newChair.Listen);
                }
            }
        }
    }
}
