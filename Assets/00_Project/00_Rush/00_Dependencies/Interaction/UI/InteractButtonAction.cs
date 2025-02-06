using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Rush
{
    [System.Serializable]
    public class AssetReferenceInteractButton : AssetReferenceT<InteractButtonView>
    {
        public AssetReferenceInteractButton(string guid) : base(guid)
        {

        }

    }
    [System.Serializable]
    public partial class InteractButtonAction : IInitializer<InteractButtonAction>, ISetIdentifier
    {
        [SerializeField, ReadOnly]
        private string m_Id;
        [SerializeField, ReadOnly]
        private IInteractable m_InteractorAction;
        [SerializeField, ReadOnly]
        private IInteractable m_InteractableAction;
        [SerializeField, ReadOnly]
        private AssetReferenceGameObject m_InteractButtonAsset;

        [SerializeField, ReadOnly]
        private Transform m_InteractButtonTransform;

        private InteractButtonView m_SpawnedInteractButton;
        [SerializeField, ReadOnly]
        private bool m_Initialized = false;

        private AsyncOperationHandle<GameObject> m_AsyncOperationHandle;
        public InteractButtonAction(IInteractable interactorAction, IInteractable interactableAction, AssetReferenceGameObject interactButtonAsset, Transform interactButtonTransform)
        {
            m_Id = interactableAction.Id;
            m_InteractorAction = interactorAction;
            m_InteractableAction = interactableAction;
            m_InteractButtonAsset = interactButtonAsset;
            m_InteractButtonTransform = interactButtonTransform;
        }

        public bool Initialized
        {
            get
            {
                m_Initialized = m_SpawnedInteractButton != null;
                return m_Initialized;
            }
        }

        public void Init(InteractButtonAction val = null)
        {
            m_InteractorAction = val.m_InteractorAction;
            m_InteractableAction = val.m_InteractableAction;
            m_InteractButtonAsset = val.m_InteractButtonAsset;

            m_AsyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(m_InteractButtonAsset);

            CoroutineUtility.BeginCoroutine($"{m_Id}_{nameof(Initializing)}", Initializing());
        }

        public void SetId(string id)
        {
            m_Id = id;
        }

        private IEnumerator Initializing()
        {
            yield return m_AsyncOperationHandle;
            switch (m_AsyncOperationHandle.Status)
            {
                case AsyncOperationStatus.None:
                    break;
                case AsyncOperationStatus.Succeeded:
                    OnInitializedSuccesInvoke();
                    break;
                case AsyncOperationStatus.Failed:
                    break;
                default:
                    yield break;
            }
        }

        private void OnInitializedSuccesInvoke()
        {
            if (m_SpawnedInteractButton == null)
            {
                GameObject button = GameObject.Instantiate(m_AsyncOperationHandle.Result, m_InteractButtonTransform, false);
                if (button.TryGetComponent(out InteractButtonView buttonView))
                {
                    m_SpawnedInteractButton = buttonView;
                }
            }

            m_SpawnedInteractButton.ClearAction();

            m_SpawnedInteractButton.AddAction(() => m_InteractorAction.InteractBehaviour(m_InteractableAction.GetSelf()));
            m_SpawnedInteractButton.AddAction(() => m_InteractableAction.InteractBehaviour(m_InteractorAction.GetSelf()));

            m_Initialized = m_SpawnedInteractButton != null;
        }
    }
}
