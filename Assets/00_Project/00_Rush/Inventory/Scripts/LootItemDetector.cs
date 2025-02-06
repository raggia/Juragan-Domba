using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public class LootItemDetector : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private List<ItemObject> m_Items = new();

        [SerializeField, ReadOnly]
        private ItemObject m_StandByItem;

        [SerializeField]
        private UnityEvent<ItemObject> m_OnItemDetected = new();
        [SerializeField]
        private UnityEvent<ItemObject> m_OnItemReleased = new();
        [SerializeField]
        private UnityEvent<GameObject> m_OnSetStandbyItem = new();
        [SerializeField]
        private UnityEvent<IItem> m_OnItemCollect = new();

        private ItemObject GetItemObject(int instanceId)
        {
            ItemObject match = m_Items.Find(x => x.GetInstanceID() == instanceId);
            if (match == null)
            {
                return null;
            }  
            return match;
        }
        public void DetectItemObject(GameObject detect)
        {
            if (detect.TryGetComponent(out ItemObject itemObject))
            {
                if (!m_Items.Contains(itemObject))
                {
                    m_Items.Add(itemObject);
                    OnItemDetectedInvoke(itemObject);
                }
            }
        }
        public void ReleaseItemObject(GameObject detect)
        {
            if (detect.TryGetComponent(out ItemObject itemObject))
            {
                if (m_Items.Contains(itemObject))
                {
                    OnItemReleasedInvoke(itemObject);
                    m_Items.Remove(itemObject);
                }
            }
        }

        public void Collects()
        {
            foreach (var item in m_Items)
            {
                PlayerSingleton.Instance.AddItem(item);
            }
        }
        public void SetStandbyItem(GameObject detect)
        {
            //CollectInternal(detect);
            if (detect.TryGetComponent(out ItemObject item))
            {
                m_StandByItem = item;
                OnSetStandbyItemInvoked(detect);
            }
        }
        public void Collect(ItemObject item)
        {
            CollectInternal(item);
        }

        private void CollectInternal(ItemObject item)
        {
            if (m_Items.Contains(item))
            {
                ItemObject match = GetItemObject(item.GetInstanceID());
                PlayerSingleton.Instance.AddItem(match.Collect());
                OnItemCollectedInvoke(match);
            }
        }

        public void CollectStandbyItem()
        {
            CollectInternal(m_StandByItem);
        }

        private void OnSetStandbyItemInvoked(GameObject item)
        {
            m_OnSetStandbyItem?.Invoke(item);
            Debug.Log($"Item is Set {item.name}");
        }

        private void OnItemDetectedInvoke(ItemObject item)
        {
            m_OnItemDetected?.Invoke(item);
        }
        private void OnItemReleasedInvoke(ItemObject item)
        {
            m_OnItemReleased?.Invoke(item);
        }
        private void OnItemCollectedInvoke(ItemObject item)
        {
            
            m_OnItemCollect?.Invoke(item);
            m_StandByItem = null;
            m_Items.Remove(item);
        }
    }
}
