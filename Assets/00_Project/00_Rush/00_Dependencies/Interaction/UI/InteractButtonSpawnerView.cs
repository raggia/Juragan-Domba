using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Rush
{
    public class InteractButtonSpawnerView : UIView
    {
        [SerializeField]
        private AssetReferenceGameObject m_InteractButtonAsset;
        [SerializeField]
        private Transform m_ButtonSpawnPost;

        private readonly Dictionary<IInteractable, InteractButtonView> m_SpawnedInteracButtonViewPool = new();
        [SerializeField, ReadOnly]
        private InteractButtonView m_InteractButtonViewPrefab;

        private AsyncOperationHandle<GameObject> m_AsyncOperationHandle;

        public AssetReferenceGameObject GetInteractButtonAsset()
        {
            return m_InteractButtonAsset;
        }
        public Transform ButtonSpawnPost => m_ButtonSpawnPost;
        public void SetInteractableButton(IInteractable interactable, bool set)
        {
            if (m_SpawnedInteracButtonViewPool.TryGetValue(interactable, out InteractButtonView interactButtonView))
            {
                interactButtonView.SetInteractableButton(set);
            }
        }
        public void SetAllInteractableButton(bool set)
        {
            foreach (InteractButtonView button in m_SpawnedInteracButtonViewPool.Values)
            {
                button.SetInteractableButton(set);
            }
        }
        public void SetAsLastSibling(InteractButtonView button)
        {
            button.transform.SetAsLastSibling();
        }
        public void SpawnInteractButtonView(IInteractable interactor, IInteractable interactable)
        {
            int random = Random.Range(-1000, 1000);
            CoroutineUtility.BeginCoroutine($"{random}/{m_Id}_{gameObject.name}_{nameof(SpawningInteractButtonView)}", SpawningInteractButtonView(interactor, interactable));
        }
        private IEnumerator SpawningInteractButtonView(IInteractable interactor, IInteractable interactable)
        {
            if (m_InteractButtonViewPrefab == null)
            {
                yield return CoroutineUtility.BeginCoroutineReturn($"{m_Id}_{gameObject.name}_{nameof(LoadingInteractButtonAsset)}", LoadingInteractButtonAsset());
            }

            if (!m_SpawnedInteracButtonViewPool.ContainsKey(interactable))
            {
                RegisterInteractButton(interactor, interactable);
            }

            InteractButtonView stored = m_SpawnedInteracButtonViewPool[interactable];
            
            stored.Show();

            SetAsLastSibling(stored);
        }
        private IEnumerator LoadingInteractButtonAsset()
        {
            m_AsyncOperationHandle = m_InteractButtonAsset.LoadAssetAsync<GameObject>();
            yield return m_AsyncOperationHandle;
            OnInteractButtonLoaded(m_AsyncOperationHandle.Result);
        }

        private void OnInteractButtonLoaded(GameObject asset)
        {
            if (asset.TryGetComponent(out InteractButtonView button))
            {
                m_InteractButtonViewPrefab = button;
            }
        }
        
        public void DeSpawnButton(IInteractable interactable)
        {
            if (m_SpawnedInteracButtonViewPool.TryGetValue(interactable, out InteractButtonView matchButton))
            {
                matchButton.Hide();
            }
        }

        private void RegisterInteractButton(IInteractable interactor, IInteractable interactable)
        {
            if (!m_SpawnedInteracButtonViewPool.ContainsKey(interactable))
            {
                InteractButtonView button = Instantiate(m_InteractButtonViewPrefab, ButtonSpawnPost, false);
                interactable.SetInteractButton(button);

                button.AddAction(() => interactor.InteractBehaviour(interactable.GetSelf()));
                button.AddAction(() => interactable.InteractBehaviour(interactor.GetSelf()));

                m_SpawnedInteracButtonViewPool.Add(interactable, button);
                Debug.Log($"Register Interact Button {interactable.Id}_{interactable.GetInteractText()}/{interactable.GetSelf().name}");
            }
        }
        private void UnRegisterInteractButton(IInteractable interactable)
        {
            if (m_SpawnedInteracButtonViewPool.ContainsKey(interactable))
            {
                m_SpawnedInteracButtonViewPool.Remove(interactable);
            }
        }

    }


}

