using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Rush
{
    public partial class AddressableSingleton
    {
        /*[Header("_Stat_View_")]
        [SerializeField]
        private AssetReferenceGameObject m_StatViewAsset;
        [SerializeField, ReadOnly]
        private StatView m_StatViewPrefab;
        public AssetReferenceGameObject StatViewAsset => m_StatViewAsset;
        public StatView StatViewPrefab => m_StatViewPrefab;
        public bool HasStatViewPrefab()
        {
            return m_StatViewPrefab != null;
        }
        public IEnumerator LoadingStatViewPrefab()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(m_StatViewAsset);
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                yield break;
            }
            GameObject prefab = handle.Result;
            if (prefab.TryGetComponent(out StatView statView))
            {
                m_StatViewPrefab = statView;
            }
        }*/
    }
    public partial class StatMonitor : UIView
    {
        [SerializeField]
        private AssetReferenceGameObject m_StatViewAsset;
        [SerializeField]
        private List<StatView> m_Views = new ();

        public override void Hide(float overideDelay = 0)
        {
            foreach (var view in m_Views)
            {
                view.Hide();
            }
            base.Hide(overideDelay);

        }
        public void SetUp(List<Stat> stats)
        {
            ShowInternal();
            CoroutineUtility.BeginCoroutine($"{GetInstanceID()}/{nameof(SettingUp)}", SettingUp(stats));
        }

        private IEnumerator SettingUp(List<Stat> stats)
        {
            foreach (Stat stat in stats)
            {
                string taskId = $"{GetInstanceID()}/{stat.Definition.Id}/{nameof(AddingStatView)}";
                yield return CoroutineUtility.BeginCoroutineReturn(taskId, AddingStatView(stat));
            }
        }

        private StatView SpawnStatView()
        {
            StatView prefab = AddressableUtility.GetPrefab<StatView>(m_StatViewAsset.AssetGUID);
            return Instantiate(prefab, m_Content.transform, false);
        }

        private bool HasStatView(Stat stat)
        {
            bool has = false;
            foreach (StatView view in m_Views)
            {
                if (view.Definition == stat.Definition)
                {
                    has = true;
                }
            }
            return has;
        }

        private StatView GetStatView(Stat stat)
        {
            StatView match = m_Views.Find(x => x.Definition == stat.Definition);
            return match;
        }

        private IEnumerator AddingStatView(Stat stat)
        {
            StatView view = null;
            if (!AddressableUtility.HasPrefab(m_StatViewAsset.AssetGUID))
            {
                yield return AddressableUtility.LoadingPrefab(m_StatViewAsset.AssetGUID);
            }
            if (!HasStatView(stat))
            {
                view = SpawnStatView();
            }
            else
            {
                view = GetStatView(stat);
            }
            view.SetUp(stat);
        }


    }
}
