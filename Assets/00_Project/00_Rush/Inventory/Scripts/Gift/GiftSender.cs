using UnityEngine;
using UnityEngine.Events;

namespace Rush
{

    public partial class GiftSender : MonoBehaviour
    {
        [SerializeField]
        private Item m_GiftHold;

        [SerializeField]
        private GiftTaker m_GiftTakerStandby;

        [SerializeField]
        private UnityEvent<GiftTaker> m_OnSetGiftTaker = new();
        [SerializeField]
        private UnityEvent<float> m_OnGiftTaken = new();
        [SerializeField]
        private UnityEvent<Item> m_OnGiftDenied = new();

        public void SetGiftHold(IItem set)
        {
            SetGiftHoldInternal(set);
        }

        public void SetGiftHoldInternal(IItem set)
        {
            m_GiftHold = new Item(set);
        }
        private void AddGiftHoldInternal(IItem item)
        {
            if (m_GiftHold.Definition == null)
            {
                SetGiftHoldInternal(item);
            }
            else
            {
                if (m_GiftHold.Definition == item.Definition)
                {
                    m_GiftHold.AddAmount(item.Amount);
                }
            }
        }

        private void RemoveGiftHoldInternal(IItem item)
        {
            m_GiftHold.AddAmount(- item.Amount);

            if (item.Amount < 1)
            {
                m_GiftHold.SetDefinition(null);
            }
            PlayerSingleton.Instance.SetItem(m_GiftHold);
        }

        public void SetGiftTakerStandby(GameObject target)
        {
            if (target.TryGetComponent(out GiftTaker taker))
            {
                m_GiftTakerStandby = taker;
                OnSetGiftTakerStandby(taker);
            }
            
        }

        public void SendGift()
        {
            if (m_GiftHold.Definition == null) return;
            if (m_GiftTakerStandby.HasFavorite(m_GiftHold))
            {
                m_GiftTakerStandby.TakeGift(m_GiftHold);
                RemoveGiftHoldInternal(m_GiftTakerStandby.GetFavorite(m_GiftHold));
                OnGiftTakenInvoke(m_GiftTakerStandby.GetValue(m_GiftHold));

                Debug.Log($"The Give {m_GiftHold.Label} has been Taken ");
            }
            else
            {
                OnGiftDeniedInvoke(m_GiftHold);
                m_GiftTakerStandby.OnGiftDeniedInvoke(m_GiftHold);
                Debug.Log($"The Give {m_GiftHold.Label} has been Denied ");
            }
            Debug.Log($"Try Send Gift");
        }

        private void OnGiftTakenInvoke(float val)
        {
            m_OnGiftTaken?.Invoke(val);
        }
        private void OnGiftDeniedInvoke(Item item)
        {
            m_OnGiftDenied?.Invoke(item);
        }
        private void OnSetGiftTakerStandby(GiftTaker taker)
        {
            m_OnSetGiftTaker?.Invoke(taker);
        }
    }
}
