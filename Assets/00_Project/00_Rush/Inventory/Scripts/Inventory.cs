using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public partial interface IItemCollector
    {
        List<Item> Items { get; }
        void Collects(List<IItem> items);
    }
    public partial interface IItemStoreable
    {
        void AddItem(IItem item);
        void RemoveItem(IItem item);
    }
    public partial interface IInventory : IItemStoreable, IItemCollector, IIdentifier
    {
       
    }

    public partial class Inventory : MonoBehaviour, IInventory, ISetIdentifier
    {
        [SerializeField]
        private string m_Id;
        [SerializeField]
        private bool m_RemoveItemWhenReachZero = false;

        [SerializeField]
        protected List<Currency> m_Currencies = new();
        [SerializeField]
        protected List<Item> m_Items = new();

        [SerializeField]
        private UnityEvent<ICurrency> m_OnCurrencyAmountChanged = new();
        [SerializeField]
        protected UnityEvent<IItem> m_OnNewItemAdded = new();
        [SerializeField]
        protected UnityEvent<IItem> m_OnItemAmountChanged = new();
        [SerializeField]
        protected UnityEvent<IItem> m_OnItemAmountCollected = new();
        [SerializeField]
        protected UnityEvent<IItem> m_OnItemRemoved = new();

        [SerializeField] 
        protected UnityEvent<IItem> m_OnPaySucces = new();
        [SerializeField]
        protected UnityEvent<IItem> m_OnPayFail = new();

        public List<Item> Items => m_Items;

        public string Id => m_Id;

        private void Start()
        {
            foreach (var currency in m_Currencies)
            {
                SetCurrencyAmount(currency);
            }
            
        }

        public List<IItem> GetItems()
        {
            List<IItem> i = new();
            foreach (var item in m_Items)
            {
                i.Add(item);
            }
            return i;
        }

        protected Item GetItemInternal(string id)
        {
            Item match = m_Items.Find(x => x.Id == id);
            if (match == null)
            {
                match = null;
            }
            return match;
        }
        public Item GetItem(string id)
        {
            return GetItemInternal(id);
        }
        private Currency GetCurrencyInternal(string id)
        {
            Currency match = m_Currencies.Find(x => x.Id == id);
            if (match == null)
            {
                match = null;
            }
            return match;
        }

        public Currency GetCurrency(string id)
        {
            return GetCurrencyInternal(id);
        }
        public int GetCurrencyAmount(string id)
        {
            return GetCurrencyAmountInternal(id);
        }
        private int GetCurrencyAmountInternal(string id)
        {
            return GetCurrencyInternal(id).Amount;
        }

        public void AddCurrencyAmount(ICurrency currency)
        {
            AddCurrencyAmountInternal(currency);
        }
        protected void AddCurrencyAmountInternal(ICurrency currency)
        {
            if (m_Currencies.Contains(GetCurrencyInternal(currency.Id)))
            {
                ICurrency match = GetCurrencyInternal(currency.Id);
                match.AddAmount(currency.Amount);
                OnCurrencyAmountChangedInvoke(match);
            }
        }

        public void SetCurrencyAmount(ICurrency currency)
        {
            SetCurrencyAmountInternal(currency);
        }
        public void SetCurrencyAmountInternal(ICurrency currency)
        {
            if (m_Currencies.Contains(GetCurrencyInternal(currency.Id)))
            {
                ICurrency match = GetCurrencyInternal(currency.Id);
                match.SetAmount(currency.Amount);
                OnCurrencyAmountChangedInvoke(match);
            }
        }
        protected virtual void OnCurrencyAmountChangedInvoke(ICurrency currency)
        {
            m_OnCurrencyAmountChanged?.Invoke(currency);
        }
        public void SetItem(IItem item)
        {
            SetItemInternal(item);
        }
        private void SetItemInternal(IItem item)
        {
            if (m_Items.Contains(GetItemInternal(item.Id)))
            {
                GetItemInternal(item.Id).SetAmount(item.Amount);
                OnItemAmountChangedInvoke(GetItemInternal(item.Id));
            }
            if (!m_RemoveItemWhenReachZero) return;
            if (GetItemInternal(item.Id).Amount < 1)
            {
                RemoveItemInternal(item);
            }

        }
        // used for add or reduce amount, and remove automatically on 0
        public void AddItem(IItem item) 
        {
            AddItemInternal(item);
        }

        private void AddItemInternal(IItem item)
        {

            if (m_Items.Contains(GetItemInternal(item.Id)))
            {
                GetItemInternal(item.Id).AddAmount(item.Amount);
                OnItemAmountChangedInvoke(GetItemInternal(item.Id));
                OnItemAmountCollectedInvoke(item);
            }
            else
            {
                NewAddItemInternal(item);
            }

            if (!m_RemoveItemWhenReachZero) return;
            if (GetItemInternal(item.Id).Amount < 1)
            {
                RemoveItemInternal(item);
            }
        }
        public void NewAddItem(IItem item)
        {
            NewAddItemInternal(item);
        }
        private void NewAddItemInternal(IItem item)
        {
            Item match = new (item.Definition, item.SlotId, item.Amount);
            m_Items.Add(match);
            OnNewItemAddedInvoke(match);
            OnItemAmountCollectedInvoke(item);
        }
        private void RemoveItemInternal(IItem item)
        {
            Debug.Log($"Remove Item {item.Id} from list");
            //Item match = GetItem(item.Id);
            if (m_Items.Contains(GetItemInternal(item.Id)))
            {
                OnItemRemovedInvoke(GetItemInternal(item.Id));
                m_Items.Remove(GetItemInternal(item.Id));
            }
        }

        public void RemoveItem(IItem item)
        {
            RemoveItemInternal(item);
        }

        // untuk item baru yang masuk
        protected virtual void OnNewItemAddedInvoke(IItem item)
        {
            m_OnNewItemAdded?.Invoke(item);
            Debug.Log($"Add New Item {item.Label}, amount {item.Amount}");
        }

        // untuk item yang sudah ada di inventory
        protected virtual void OnItemAmountChangedInvoke(IItem item)
        {
            m_OnItemAmountChanged?.Invoke(item);
            
        }
        // untuk item yang sedang masuk ke inventory entah item baru atau sama
        protected virtual void OnItemAmountCollectedInvoke(IItem item)
        {
            m_OnItemAmountCollected?.Invoke(item);
        }
        protected virtual void OnItemRemovedInvoke(IItem item)
        {
            m_OnItemRemoved?.Invoke(item);
        }

        public void SetId(string id)
        {
            m_Id = id;
        }

        public void Collects(List<IItem> items)
        {
            CollectsInternal(items);
        }

        protected virtual void CollectsInternal(List<IItem> items)
        {
            foreach (var item in items)
            {

                AddItemInternal(item);
            }
        }

        private bool HasEnoughtCurrency(Currency other)
        {
            return other.Amount <= GetCurrencyAmountInternal(other.Id);
        }

        public void PayCart(Currency other, List<IItem> carts)
        {
            if (HasEnoughtCurrency(other))
            {
                Currency pay = new Currency(other.Definition, "", - other.Amount);
                AddCurrencyAmountInternal(pay);
                CollectsInternal(carts);
                OnPaySuccessInvoke(other);
            }
            else
            {
                OnPayFailInvoke(other);
            }
        }

        public void ClearItems()
        {
            m_Items.Clear();
        }

        private void OnPaySuccessInvoke(IItem item)
        {
            m_OnPaySucces?.Invoke(item);
        }

        private void OnPayFailInvoke(IItem item)
        {
            m_OnPayFail?.Invoke(item);
        }

        private CurrencyDefinition GetCurrencyDefinition(string id)
        {
            return InventorySettingSingleton.Instance.GetCurrencyDefinition(id);
        }

        private InventoryUIView GetInventoryUIView()
        {
            InventoryUIView inventoryUIView = CanvasSingleton.Instance.GetPanel<CharacterPanel>().GetComponentInChildren<InventoryUIView>(true);
            return inventoryUIView;
        }
    }
}
