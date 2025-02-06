using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public class AnimationInteractAgent : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private int m_InteractIndex;
        [SerializeField, ReadOnly]
        private bool m_StayInteracting = false;

        [SerializeField]
        private UnityEvent<int, bool> m_OnStartInteract = new();

        public void SetInteractIndex(int index)
        {
            m_InteractIndex = index;
        }
        public void SetStayInteracting(bool stayInteracting)
        {
            m_StayInteracting = stayInteracting;
        }

        public void StartInteract()
        {
            m_OnStartInteract?.Invoke(m_InteractIndex, m_StayInteracting);
        }
    }
}
