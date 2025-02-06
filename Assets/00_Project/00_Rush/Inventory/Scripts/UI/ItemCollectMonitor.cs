using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rush
{
    public partial class ItemCollectMonitor : UIView
    {
        
        [SerializeField]
        private AssetReferenceGameObject m_ItemCollectViewAsset;
        [SerializeField]
        private float m_ItemShowDuration = 3f;
        [SerializeField]
        private List<ItemCollectView> m_ItemCollectViews = new ();

        private ItemCollectView GetItemCollectView(IItem item)
        {
            ItemCollectView view = m_ItemCollectViews.Find(x => x.GetItem() == item);
            if (view == null)
            {
                Debug.LogError($"There is no {item.Id} exist on {m_ItemCollectViews}");
                view = null;
            }
            return view;
        }
        public void ShowItemCollecView(IItem item)
        {
            int taskrandom = Random.Range(-100, 100);
            CoroutineUtility.BeginCoroutine($"{name}/{taskrandom}/{nameof(ShowingItemCollectView)}", ShowingItemCollectView(item));
        }

        private IEnumerator ShowingItemCollectView(IItem item)
        {
            if (!AddressableUtility.HasPrefab(m_ItemCollectViewAsset.AssetGUID))
            {
                yield return AddressableUtility.LoadingPrefab(m_ItemCollectViewAsset.AssetGUID);
            }
            ItemCollectView view = SpawnItemCollectView(item);
            
            m_ItemCollectViews.Add(view);
            yield return new WaitForEndOfFrame();
            view.Show();
            yield return new WaitForSeconds(m_ItemShowDuration);
            view.Hide();
            yield return new WaitForSeconds(m_ItemShowDuration);
            GameObject targetRelease = view.gameObject;

            m_ItemCollectViews.Remove(view);
            Destroy(targetRelease);
            //AddressableSingleton.Instance.ReleaseGameObjectInstance(m_ItemCollectViewAsset.AssetGUID, targetRelease);
        }

        private void ShowView(UIView view)
        {
            view.Show();
        }
        private void HideView(UIView view)
        {
            view.Hide();
        }

        private ItemCollectView SpawnItemCollectView(IItem item)
        {
            ItemCollectView prefab = AddressableUtility.GetPrefab<ItemCollectView>(m_ItemCollectViewAsset.AssetGUID);
            ItemCollectView spawn = Instantiate(prefab, m_Content.transform, false);
            spawn.SetUp(item);
            return spawn;
        }
    }
}
