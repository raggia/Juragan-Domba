using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Rush/Inventory/Item")]
    public partial class ItemDefinition : IconDefinition
    {
        [SerializeField]
        private RarityDefinition m_Rarity;
        public RarityDefinition Rarity => m_Rarity;

        [SerializeReference, SubclassSelector]
        private ItemBehaviour[] m_CategoryBehaviours;

        public Sprite GetRaritySprite() => m_Rarity.Sprite;
        public bool IsUniqueItem()
        {
            return IsUniqueItemInternal();
        }
        private bool IsUniqueItemInternal()
        {
            bool unique = false;
            foreach (var item in m_CategoryBehaviours)
            {
                if (item is UniqueItemBehaviour match)
                {
                    unique = true;
                }
            }
            return unique;
        }
        public T GetItemBehaviour<T>() where T : ItemBehaviour
        {
            T itemBehaviour = null;
            foreach (var item in m_CategoryBehaviours)
            {
                if (item is T match)
                {
                    itemBehaviour = match;
                }
            }
            if (itemBehaviour != null)
            {
                Debug.Log($"Has Behaviour {typeof(T)} was found on {m_CategoryBehaviours}");
            }
            else
            {
                Debug.LogWarning($"No Behaviour {typeof(T)} was found on {m_CategoryBehaviours}");
            }
            return itemBehaviour;
        }
        public bool HasBehaviour<T>(out T behave) where T : ItemBehaviour
        {
            bool has = HasBehaviourInternal(out T behaviour);
            behave = behaviour;
            return has;
        }
        private bool HasBehaviourInternal<T>(out T behaviour) where T: ItemBehaviour
        {
            T itemBehaviour = null;
            bool has = itemBehaviour != null;
            foreach (var item in m_CategoryBehaviours)
            {
                if (item is T match)
                {
                    itemBehaviour = match;
                }
            }
            behaviour = itemBehaviour;
            return has;
        }

        public string GetSlotId()
        {
            return GetSlotIdInternal();
        }
        private string GetSlotIdInternal()
        {
            if (IsUniqueItemInternal())
            {
                int slotId = Random.Range(-1000, 1000);
                return slotId.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
