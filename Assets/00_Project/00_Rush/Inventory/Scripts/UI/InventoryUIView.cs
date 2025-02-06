using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Rush
{
    public interface IInventoryView : IItemStoreable, ISetIdentifier
    {
        
    }
    public class InventoryUIView : UIView, IInventoryView
    {
        [SerializeField]
        private AssetReferenceGameObject m_ItemViewAsset;
        [SerializeField]
        private Transform m_ItemSlotSpawnRoot;
        [SerializeField]
        private float m_DelayItemSorting;
        [SerializeField]
        private ItemInfoView m_ItemInfoView;
        [SerializeField, ReadOnly]
        private Inventory m_Inventory;
        [SerializeField, ReadOnly]
        private InventorySettingSingleton m_InventorySetting;

        [SerializeField]
        private UnityEvent<IItem> m_OnItemViewAdded = new();
        [SerializeField]
        private UnityEvent<IItem> m_OnItemViewRemoved = new();

        [SerializeField, ReadOnly]
        private List<ItemView> m_SpawnedItemViews = new();
        public virtual Inventory GetInventory()
        {
            if (m_Inventory == null)
            {
                m_Inventory = PlayerSingleton.Instance.Inventory;
            }
            return m_Inventory;
        }
        public override void Show(float overideDelay = 0)
        {
            base.Show(overideDelay);
            SetInventory(GetInventory());

        }
        public override void Hide(float overideDelay = 0)
        {
            base.Hide(overideDelay);
            foreach (var item in m_SpawnedItemViews)
            {
                item.Hide();
            }
        }
        protected ItemView GetItemView(string id)
        {
            ItemView match = m_SpawnedItemViews.Find(x => x.Id == id);
/*            if (match == null)
            {
                Debug.LogError($"There is no item slot with Id {id} exist on the list");
                match = null;
            }*/
            return match;
        }

        private void SetInventory(Inventory inventory)
        {
            CoroutineUtility.BeginCoroutine($"{GetInstanceID()}/{nameof(SettingInventory)}", SettingInventory(inventory));
        }

        private IEnumerator SettingInventory(Inventory inventory)
        {
            m_Inventory = inventory;
            for (int i = 0; i < m_Inventory.Items.Count; i++)
            {
                IItem item = m_Inventory.Items[i];
                
                yield return CoroutineUtility.BeginCoroutineReturn($"{item.Id}_{inventory.Id}_{nameof(AddItemViewInternal)}", AddItemViewInternal(item));
            }
        }

        protected virtual ItemView SpawnNewItemView(IItem item)
        {
            ItemView spawn = Instantiate(AddressableUtility.GetPrefab<ItemView>(m_ItemViewAsset.AssetGUID), m_ItemSlotSpawnRoot, false);
            spawn.Init(item);
            return spawn;
        }

        protected virtual IEnumerator AddItemViewInternal(IItem item)
        {
            ItemView view = GetItemView(item.Id);

            if (view != null)
            {
                //view = GetItemView(item.Id);
                view.SetText(item.Amount.ToString());
                //view.Show();
            }
            else
            {
                if (!AddressableUtility.HasPrefab(m_ItemViewAsset.AssetGUID))
                {
                    yield return AddressableUtility.LoadingPrefab(m_ItemViewAsset.AssetGUID);
                }

                ItemView itemView = SpawnNewItemView(item);
                view = itemView;
                view.AddAction(() => OnItemInfoSelected(item));
                yield return new WaitUntil(() => view.Initialized);
                m_SpawnedItemViews.Add(view);
            }

            yield return new WaitForSeconds(m_DelayItemSorting);
            view.Show();
            OnItemViewAdded(item);
        }
        public void AddItem(IItem item)
        {
            Debug.Log($"Add Item with Id {item.Id}");
            CoroutineUtility.BeginCoroutine($"{GetTaskId(item)}_{nameof(AddItemViewInternal)}", AddItemViewInternal(item));
        }

        protected virtual string GetTaskId(IItem item) 
        {
            return $"{item.Id}/{Random.Range(-1000, 1000)}";
        }
        public void RemoveItem(IItem item)
        {
            RemoveItemInternal(item);
            
        }
        private void RemoveItemInternal(IItem item)
        {
            ItemView match = GetItemView(item.Id);
            if (match != null)
            {
                OnItemViewRemoved(item);
                m_SpawnedItemViews.Remove(match);
                Destroy(match.gameObject);
            }

        }

        public void SetItemSlotAmount(IItem item)
        {
            ItemView match = GetItemView(item.Id);
            if (match != null)
            {
                match.SetText(item.Amount.ToString());
            }
        }

        public void SetId(string id)
        {
            m_Id = id;
        }

        private void OnItemInfoSelected(IItem item)
        {
            m_ItemInfoView.SetItemView(item);
        }

        protected virtual void OnItemViewAdded(IItem item)
        {
            m_OnItemViewAdded?.Invoke(item);
        }
        protected virtual void OnItemViewRemoved(IItem item)
        {
            m_OnItemViewRemoved?.Invoke(item);
        }

        public void ClearItemView(List<IItem> remove)
        {
            foreach (var item in remove)
            {
                RemoveItemInternal(item);
            }
        }
        public void ClearItemView()
        {
            foreach (var item in m_SpawnedItemViews)
            {
                ItemView itemView = GetItemView(item.Id);
                m_SpawnedItemViews.Remove(item);
                Destroy(itemView.gameObject);
            }
        }
    }
}
