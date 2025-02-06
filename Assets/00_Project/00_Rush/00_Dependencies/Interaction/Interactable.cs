using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace Rush
{
    public enum EInteractMode
    {
        Click,
        Hold,
    }
    public interface IInteractable : IContactable, IIdentifier, IBusy
    {
        bool DeContactWhenInteract { get; }
        string GetInteractText();
        void InteractBehaviour(GameObject val = null);

        void SetInteractButton(InteractButtonView spawned);
    }


    public class Interactable : Contactable, IInteractable
    {
        [SerializeField]
        private bool m_CanInteract = true;
        [SerializeField]
        private bool m_OverideInteractButtonSetting;
        [SerializeField, ShowIf(nameof(m_OverideInteractButtonSetting)), AllowNesting]
        private InteractButtonViewSetting m_ButtonSetting;
        [SerializeField]
        private bool m_SetBusyOnInteract = false;
        [SerializeField, ReadOnly]
        protected bool m_IsBusy = false;
        [SerializeField]
        protected bool m_DeContactWhenInteract = true;

        [SerializeField]
        private string m_InteractText;
        [SerializeField]
        private UnityEvent<bool> m_OnBusyChanged = new();
        [SerializeField]
        private UnityEvent<GameObject> m_OnInteracted = new();

        [SerializeField, ReadOnly]
        private InteractButtonView m_SpawnedInteractButton;

        protected virtual string IdInternal => GetInstanceID().ToString() + gameObject.name;

        public string Id => IdInternal;
        public bool IsBusy => m_IsBusy;
        public bool DeContactWhenInteract => m_DeContactWhenInteract;

        [SerializeField, ReadOnly]
        private InteractButtonSpawnerView m_InteractButtonSpawnerView;
        protected InteractButtonSpawnerView GetInteractButtonSpawnerViewInternal()
        {
            if (m_InteractButtonSpawnerView == null)
            {
                InteractButtonSpawnerView match = CanvasSingleton.Instance.GetPanel<GameplayPanel>().InteractButtonSpawnerView;
                m_InteractButtonSpawnerView = match;
            }
            return m_InteractButtonSpawnerView;
        }

        public virtual string GetInteractText()
        {
            return m_InteractText;
        }
        protected virtual bool CanInteractInternal(GameObject other, out IInteractable interactable)
        {
            bool can = other.TryGetComponent(out IInteractable match) && m_CanInteract && !m_IsBusy;
            if (can)
            {
                interactable = match;
            }
            else
            {
                interactable = null;
            }
            Debug.Log($"Can Interact {can}, with {match}");
            return can;
        }
        public virtual void InteractBehaviour(GameObject other = null)
        {
            InteractBehaviourInternal(other);
        }
        protected virtual void InteractBehaviourInternal(GameObject other = null)
        {
            if (!CanInteractInternal(other, out IInteractable interactable)) return;
            OnInteractBehaviourInvokeInternal(interactable);
        }
        protected virtual void OnInteractBehaviourInvokeInternal(IInteractable interactable)
        {
            m_OnInteracted?.Invoke(interactable.GetSelf());
            Debug.Log($"{gameObject.name} Interacted with {interactable.GetSelf().name}");
            SetBusyInternal(true);
            
        }
        public void SetBusy(bool isBusy)
        {
            SetBusyInternal(isBusy);
        }

        protected void SetBusyInternal(bool isBusy)
        {
            m_IsBusy = false;
            if (!m_SetBusyOnInteract) return;
            m_IsBusy = isBusy;
            OnBusyChangedInvoke(m_IsBusy);
            Debug.Log($"{gameObject.name} is Busy set to {m_IsBusy}");
            GetInteractButtonSpawnerViewInternal().SetAllInteractableButton(!m_IsBusy);

        }

        public void TryDecontact()
        {
            TryDeContactInternal();
        }

        protected void TryDeContactInternal()
        {
            if (!m_DeContactWhenInteract) return;
            DeSpawnButton(this);

            SetCanContactInternal(false);
        }

        protected void DeSpawnButton(IInteractable interactable)
        {
            GetInteractButtonSpawnerViewInternal().DeSpawnButton(interactable);
        }

        public void SetInteractButton(InteractButtonView spawned)
        {
            m_SpawnedInteractButton = spawned;

            m_SpawnedInteractButton.SetId(IdInternal.ToString());
            m_SpawnedInteractButton.SetText(GetInteractText());

            if (m_OverideInteractButtonSetting)
            {
                m_SpawnedInteractButton.SetFrame(m_ButtonSetting.Frame);
            }
        }

        private void OnBusyChangedInvoke(bool busy)
        {
            m_OnBusyChanged?.Invoke(busy);
        }
    }

}
