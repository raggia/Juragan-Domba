using UnityEngine;

namespace Rush
{
    public partial interface ICurrency : IItem
    {

    }
    [System.Serializable]
    public partial class Currency : Item, ICurrency
    {
        public Currency(ItemDefinition definition, string slotId, int amount) : base(definition, slotId, amount)
        {

        }
    }
}
