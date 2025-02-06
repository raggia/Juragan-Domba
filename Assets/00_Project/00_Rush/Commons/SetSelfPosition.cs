using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public partial class SetSelfPositionField
    {

    }
    public partial class SetSelfPosition : MonoBehaviour
    {
        private Vector2 m_OriginalPosition;
        private void Start()
        {
            m_OriginalPosition = transform.localPosition;
        }
        public void SetPositionVector2(Vector2 position)
        {
            transform.position = position;
        }

        public void ResetPosition()
        {
            transform.localPosition = m_OriginalPosition;
        }
    }
}
