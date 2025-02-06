using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public class AnimationEventState : StateMachineBehaviour
    {
        [SerializeField]
        private string m_EventName;

        [Range(0f, 1f)]
        [SerializeField]
        private float m_TriggerTime;

        [SerializeField, ReadOnly]
        private bool m_HasTriggered;
        [SerializeField, ReadOnly]
        private float m_CurrentTime;

        public float GetTriggerTime()
        {
            return m_TriggerTime;
        }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            m_HasTriggered = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            m_CurrentTime = stateInfo.normalizedTime % 1f;
            if (!m_HasTriggered && m_CurrentTime >= m_TriggerTime)
            {
                NotifyReceiver(animator);
                m_HasTriggered = true;
            }
            if (m_CurrentTime < 0.01f)
            {
                m_HasTriggered = false;
            }
        }

        private void NotifyReceiver(Animator animator)
        {
            if (animator.TryGetComponent(out AnimationEventReceiver receiver))
            {
                receiver.OnAnimationEventInvoked(m_EventName);
            }
        }
    }
}

