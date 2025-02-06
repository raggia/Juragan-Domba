using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Rush
{
    [System.Serializable]
    public partial class AssetReferenceGameObjectGroup
    {
        [SerializeField]
        private AssetReferenceGameObject m_Asset;
        
        [SerializeField, ReadOnly]
        private GameObject m_Prefab;

        //[SerializeField]
        //private UnityEvent<GameObject> m_OnInstantiateDone = new();

        private Dictionary<int, GameObject> m_Spawneds = new();
        public AssetReferenceGameObject Asset => m_Asset;
        public GameObject Prefab => m_Prefab;
        public void InstantiateObject(Transform spawn, bool world = false)
        {
            if (m_Asset != null)
            {
                m_Asset.InstantiateAsync(spawn, world).Completed += OnObjectInstantiated;
            }
            else
            {
                Debug.LogError("Prefab Reference is null. Please assign an addressable asset.");
            }
        }
        private void OnObjectInstantiated(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject newObject = obj.Result;
                int instanceId = newObject.GetInstanceID();
                m_Spawneds[instanceId] = newObject; // Add to the dictionary
                Debug.Log($"Object instantiated successfully! Instance ID: {instanceId}");
                //m_OnInstantiateDone.Invoke(newObject);
            }
            else
            {
                Debug.LogError("Failed to instantiate object.");
            }
        }
        public void ReleaseObjectByInstanceId(int instanceId)
        {
            if (m_Spawneds.TryGetValue(instanceId, out GameObject obj))
            {
                if (obj != null)
                {
                    Addressables.ReleaseInstance(obj);
                    m_Spawneds.Remove(instanceId); // Remove from the dictionary
                    Debug.Log($"Object with Instance ID {instanceId} released.");
                }
                else
                {
                    Debug.LogWarning($"Object with Instance ID {instanceId} is already null.");
                }
            }
            else
            {
                Debug.LogError($"No object found with Instance ID {instanceId}.");
            }
        }

        public bool HasPrefab()
        {
            return m_Prefab != null;
        }
        private IEnumerator LoadingPrefab()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(m_Asset);
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                yield break;
            }
            GameObject prefab = handle.Result;
            m_Prefab = prefab;
        }

        public Coroutine LoadPrefab()
        {
            string taskId = $"{m_Asset.AssetGUID}/{nameof(LoadingPrefab)}";
            return CoroutineUtility.BeginCoroutineReturn(taskId, LoadingPrefab());
        }
    }
    public partial class AddressableSingleton : Singleton<AddressableSingleton>  
    {
        [SerializeField]
        private List<AssetReferenceGameObjectGroup> m_AssetGameObjects = new ();

        [SerializeField, ReadOnly]
        private Dictionary<string, GameObject> m_InstantiatedObj = new();
        private AssetReferenceGameObjectGroup GetAssetGameObject(string assetId)
        {
            AssetReferenceGameObjectGroup match = m_AssetGameObjects.Find(x => x.Asset.AssetGUID == assetId);
            if (match == null)
            {
                Debug.LogError($"There is no {assetId} exist on {nameof(m_AssetGameObjects)}");
                match = null;
            }
            return match;
        }

        public void InsantiateAsync(string assetId, Transform spawn, bool world = false)
        {
            GetAssetGameObject(assetId).InstantiateObject(spawn, world);
        }
        public void ReleaseInstance(string assetId, GameObject obj)
        {
            GetAssetGameObject(assetId).ReleaseObjectByInstanceId(obj.GetInstanceID());
        }
        public bool HasPrefab(string assetId)
        {
            return GetAssetGameObject(assetId).HasPrefab();
        }
        public Coroutine LoadingPrefab(string assetId)
        {
            return GetAssetGameObject(assetId).LoadPrefab();
        }

        public void ReleaseGameObjectInstance(string assetId, GameObject instance)
        {
            GetAssetGameObject(assetId).Asset.ReleaseInstance(instance);
        }

        public T GetPrefab<T>(string assetId) where T : Component
        {
            GameObject go = GetAssetGameObject(assetId).Prefab;
            return go.GetComponent<T>();
        }

    }

    public static partial class AddressableUtility
    {
        public static Coroutine LoadingPrefab(string assetId)
        {
            return AddressableSingleton.Instance.LoadingPrefab(assetId);
        }
        public static bool HasPrefab(string assetId)
        {
            return AddressableSingleton.Instance.HasPrefab(assetId);
        }
        public static T GetPrefab<T>(string assetId) where T : Component
        {
            return AddressableSingleton.Instance.GetPrefab<T>(assetId);
        }
        public static void ReleaseGameObjectInstance(string assetId, GameObject instance)
        {
            AddressableSingleton.Instance.ReleaseGameObjectInstance(assetId, instance);
        }

        public static void InsantiateAsync(string assetId, Transform spawn, bool world = false)
        {
            AddressableSingleton.Instance.InsantiateAsync(assetId, spawn, world);
        }
        public static void ReleaseInstance(string assetId, GameObject obj)
        {
            AddressableSingleton.Instance.ReleaseInstance(assetId, obj);
        }

    }
}
