using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    [Serializable]
    public partial class AnimationControllerField
    {
        [SerializeField]
        private Animator m_Animator;
        [SerializeField]
        private string m_SpeedRateName = "SpeedRate";
        [SerializeField]
        private string m_TargetForwardName = "TargetForward";
        [SerializeField]
        private string m_TargetSidewardName = "TargetSideward";
        [SerializeField]
        private string m_TurnName = "Turn";
        [SerializeField]
        private string m_StartInteractName = "StartInteract";
        [SerializeField]
        private string m_InteractingName = "Interacting";
        [SerializeField]
        private string m_InteractIndexName = "InteractIndex";
        


        public void SetSpeedRate(float value)
        {
            m_Animator.SetFloat(m_SpeedRateName, value);
        }
        public void SetTargetForward(float value)
        {
            m_Animator.SetFloat(m_TargetForwardName, value);
        }
        public void SetTargetSideward(float value)
        {
            m_Animator.SetFloat(m_TargetSidewardName, value);
        }

        public void SetAnimationSpeed(float val)
        {
            m_Animator.speed = val;
        }

        public void TriggerStartInteract(int interactIndex, bool loop)
        {
            m_Animator.SetTrigger(m_StartInteractName);
            m_Animator.SetInteger(m_InteractIndexName, interactIndex);

            SetInteractingInternal(loop);
        }
        private void SetInteractingInternal(bool interacting)
        {
            m_Animator.SetBool(m_InteractingName, interacting);
        }
        public void SetInteracting(bool interacting)
        {
            SetInteractingInternal(interacting);
        }
        public void SetTurn(float val)
        {
            m_Animator.SetFloat(m_TurnName, val);
        }

        public void SetFloat(string anim, float val)
        {
            m_Animator.SetFloat(anim, val);
        }

    }
    public partial class AnimationController : MonoBehaviour
    {
        [SerializeField] private AnimationControllerField m_AnimationHandlerField;

        public void SetSpeedRate(float value)
        {
            SetSpeedRateInternal(value);
        }
        private void SetSpeedRateInternal(float value)
        {
            m_AnimationHandlerField.SetSpeedRate(value);
        }
        public void SetSpeedRate(object value)
        {
            if (value is float newValue)
            {
                SetSpeedRateInternal(newValue);
            }
        }
        public void SetTargetForward(float value)
        {
            m_AnimationHandlerField.SetTargetForward(value);
        }
        public void SetTargetSideward(float value)
        {
            m_AnimationHandlerField.SetTargetSideward(value);
        }

        public void SetAnimationSpeed(float val)
        {
            m_AnimationHandlerField.SetAnimationSpeed(val);
        }
        public void TriggerStartInteract(int interactIndex, bool loop)
        {
            m_AnimationHandlerField.TriggerStartInteract(interactIndex, loop);
        }
        public void SetInteracting(bool interacting)
        {
            m_AnimationHandlerField.SetInteracting(interacting);
        }
        public void SetTurn(float val)
        {
            m_AnimationHandlerField.SetTurn(val);
        }
    }
}

