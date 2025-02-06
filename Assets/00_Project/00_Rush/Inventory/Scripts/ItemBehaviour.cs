using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public abstract class ItemBehaviour
    {
        /*[SerializeField] // nanti ganti dengan SO
        private ItemCategoryDefinition m_CategoryDefinition;

        public ItemCategoryDefinition CategoryDefinition => m_CategoryDefinition;*/
        [SerializeField]
        private UnityEvent m_OnUse = new();

        public virtual void Init(ItemView itemView)
        {
            itemView.Init();
            itemView.AddAction(() => Use());
        }
        protected virtual void Use()
        {
            OnUseInvoke();
        }

        protected virtual void OnUseInvoke() 
        { 
            m_OnUse?.Invoke();
        }
    
    }
}
