using Unity.VisualScripting;
using UnityEngine;

namespace Rush
{
    public interface IMotion
    {
        void Move(Vector2 moveInput);
        void Direction(Vector2 dirInput);
    }
    public abstract class Locomotion : MonoBehaviour, IMotion 
    {
        [SerializeField]
        protected LocomotionField m_LocomotionField;

        private void Awake()
        {
            m_LocomotionField.SetMainCamera(Camera.main);
        }
        protected abstract Vector3 GetVelocity();
        public Vector3 GetDashForce()
        {
            return m_LocomotionField.GetDashForce();
        }
        public virtual void Move(Vector2 moveInput)
        {
            SetMoveHorizontalInput(moveInput.x);
            SetMoveVerticalInput(moveInput.y);
        }
        public void Direction(Vector2 dirInput)
        {
            SetAimDirXInternal(dirInput.x);
            SetAimDirYInternal(dirInput.y);
        }
        public void SetAimDirX(float x)
        {
            SetAimDirXInternal(x);
        }
        public void SetAimDirY(float y)
        {
            SetAimDirYInternal(y);
        }
        public Vector3 GetFinaleMoveForce()
        {
            return m_LocomotionField.GetFinalMoveForce();
        }
        protected virtual bool CanMove()
        {
            return m_LocomotionField.CanMove;
        }
        public void SetCanMove(bool canMove)
        {
            m_LocomotionField.SetCanMove(canMove);
        }
        public void SetCanMoveReverse(bool reverse)
        {
            m_LocomotionField.SetCanMove(!reverse);
        }
        protected float GetCurrentSpeed()
        {
            return m_LocomotionField.GetCurrentSpeed();
        }
        private void SetMoveHorizontalInput(float x)
        {
            m_LocomotionField.SetMoveHorizontalInput(x);
        }
        private void SetMoveVerticalInput(float y)
        {
            m_LocomotionField.SetMoveVerticalInput(y);
        }

        private void SetAimDirXInternal(float x)
        {
            m_LocomotionField.SetAimDirX(x);
        }
        private void SetAimDirYInternal(float y)
        {
            m_LocomotionField.SetAimDirY(y);
        }
        public void SetAdditionalForce(Vector3 force)
        {
            SetAdditionalForceInternal(force);
        }
        protected void SetAdditionalForceInternal(Vector3 force)
        {
            m_LocomotionField.SetAdditionalForce(force);
        }

        public void SetSpeed(float val)
        {
            m_LocomotionField.SetSpeed(val);
        }
    }
}
