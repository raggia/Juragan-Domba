using UnityEngine;

namespace Rush
{

    public partial interface IIdentifier
    {
        string Id { get; }
    }
    public partial interface ISetIdentifier
    {
        void SetId(string id);
    }

    public partial interface IBusy
    {
        bool IsBusy { get; }
        void SetBusy (bool isBusy);
    }
}
