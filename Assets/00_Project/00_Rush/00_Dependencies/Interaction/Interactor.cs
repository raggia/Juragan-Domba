using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial interface IInteractor : IInteractable
    {
        
    }

    public partial class GameplayPanel
    {
        [SerializeField]
        private InteractButtonSpawnerView m_InteractButtonSpawners;

        public InteractButtonSpawnerView InteractButtonSpawnerView => m_InteractButtonSpawners;
    }
    public partial class Interactor : Interactable, IInteractor
    {
        private bool IsInteractable(IContactable other, out IInteractable interactable)
        {
            if (other is IInteractable match)
            {
                interactable = match;
                return true;
            }
            else
            {
                interactable = null;
                return false;
            }
        }
        protected override void OnContactedBehaviourInvoke(IContactable other)
        {
            base.OnContactedBehaviourInvoke(other);
            if (IsInteractable(other, out IInteractable interactable))
            {
                SpawnInteractButton(interactable);
            }
        }
        protected override void OnDeContactBehaviourInvoke(IContactable other)
        {
            base.OnDeContactBehaviourInvoke(other);
            if (IsInteractable(other, out IInteractable interactable))
            {
                DeSpawnButton(interactable);
            }
        }

        private void SpawnInteractButton(IInteractable interactable)
        {
            GetInteractButtonSpawnerViewInternal().SpawnInteractButtonView(this, interactable);
        }

    }
    
}

