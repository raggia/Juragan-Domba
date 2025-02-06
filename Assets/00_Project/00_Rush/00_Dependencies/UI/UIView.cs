using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public class UIView : View, IIdentifier
    {
        [SerializeField]
        protected string m_Id;

        public virtual string Id => m_Id;
    }
}
