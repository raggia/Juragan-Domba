using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public enum ERotateToward
    {
        Movement = 0,
        Aim = 1,
    }
    [Serializable]
    public class AdditionalForce
    {
        public string Id;
        public Vector3 Force;

        public AdditionalForce(string id, Vector3 force)
        {
            Id = id;
            Force = force;
        }
    }
    [Serializable]
    public partial struct LocoMotionUpdateInfo
    {
        [Header("Motion")]
        public bool IsMove;
        public float CurrentSpeed;
        public float CurrentSpeedRate;

        public Vector2 MoveInput;
        public Vector3 CurrentVelocity;

        [Header("Force")]
        public Vector3 MoveForce;
        public Vector3 AdditionalForceTowardMovement;

        [Header("Direction")]
        public float TargetAngleY;
        public float SpinVelocity;
        public Vector2 AimInput;
        public Vector3 TowardInput;
        public Vector3 TowardAim;
        public Vector3 TowardMovement;
    }
    [Serializable]
    public partial class LocomotionField
    {
        [Header("Motion")]
        [SerializeField]
        private float m_Speed = 5f;
        [SerializeField]
        private float m_Acceleration = 5f;
        [SerializeField]
        private float m_Deceleration = 5f;
        [SerializeField]
        private float m_SpeedRate = 1f;
        [SerializeField]
        private bool m_CanMove = false;
        [SerializeField]
        private UnityEvent<float> m_OnCurrentSpeedRateChange;

        [Header("Direction")]
        [SerializeField]
        private Transform m_UnitRotateDirection;
        [SerializeField]
        private ERotateToward m_RotateToward = ERotateToward.Aim;
        [SerializeField, Range(m_MinRotationSensitivity, m_MaxRotationSensitivity)]
        private float m_RotationSensitivity = 0.12f;
        [SerializeField]
        private UnityEvent<float> m_OnTowardInputXChange;
        [SerializeField]
        private UnityEvent<float> m_OnTowardInputZChange;

        [SerializeField, ReadOnly]
        private LocoMotionUpdateInfo m_Info;
        
        private const float m_MinRotationSensitivity = 0.01f;
        private const float m_MaxRotationSensitivity = 10f;

        private static Camera m_MainCamera;
        public static Camera MainCamera => m_MainCamera;
        
        public void SetMainCamera(Camera mainCamera)
        {
            m_MainCamera = mainCamera;
        }
        public bool CanMove => m_CanMove;
        public float GetFinalSpeed()
        {
            return m_Speed; // any additional modifier here
        }
        public float GetFinalAcceleration()
        {
            return m_Acceleration; // any additional modifier here
        }
        private float GetFinalSpeedRate()
        {
            return m_SpeedRate; // any additional modifier here
        }
        private Vector2 GetMoveInput()
        {
            return m_Info.MoveInput;
        }
        private Vector2 GetAimInput()
        {
            return m_Info.AimInput;
        }
        private float GetAimInputMagnitude()
        {
            return GetAimInput().magnitude;
        }
        private float GetMoveInputMagnitude()
        {
            return m_Info.MoveInput.magnitude;
        }
        private float GetRotationSensitivity()
        {
            return m_MaxRotationSensitivity - m_RotationSensitivity;
        }
        public Vector3 GetCurrentVelocity()
        {
            return m_Info.CurrentVelocity;
        }
        public Vector3 GetDashForce()
        {
            return m_Info.AdditionalForceTowardMovement;
        }

        public Vector3 GetFinalMoveForce()
        {
            float speed = GetCurrentSpeedInternal();
            if (!m_CanMove) return Vector3.zero;

            Quaternion targetRotation = m_UnitRotateDirection.rotation;
            switch (m_RotateToward)
            {
                case ERotateToward.Movement:
                    targetRotation = Quaternion.LookRotation(GetFacingTowardMovement(), Vector3.up);
                    break;
                case ERotateToward.Aim:
                    targetRotation = Quaternion.LookRotation(GetFacingTowardAim(), Vector3.up);
                    break;
            }

            m_UnitRotateDirection.rotation = Quaternion.RotateTowards(m_UnitRotateDirection.rotation, targetRotation, m_RotationSensitivity);
            Vector3 moveForce = Vector3.zero;
            if (GetMoveInputMagnitude() < 0.1f)
            {
                moveForce = new Vector3(0f, m_Info.AdditionalForceTowardMovement.y, 0f) * Time.deltaTime;
            }
            else
            {
                moveForce = (GetFacingTowardMovement() + m_Info.AdditionalForceTowardMovement) * speed * Time.deltaTime;
            }
            AdjustTowardInput(); 
            m_Info.MoveForce = moveForce;
            return m_Info.MoveForce;
        }
        private static Quaternion GetCameraFacingInternal()
        {
            var cameraRotation = Quaternion.Euler(0f, m_MainCamera.transform.eulerAngles.y, 0f);
            return cameraRotation;
        }
        public static Quaternion GetCameFacing()
        {
            return GetCameraFacingInternal();
        }
        private Vector3 GetFacingTowardAim()
        {
            if (GetAimInputMagnitude() < 0.1f) return m_Info.TowardAim;
            
            m_Info.TowardAim = GetCameraFacingInternal() * new Vector3(GetAimInput().x, 0f, GetAimInput().y);

            SetTargetAngleY(m_Info.AimInput);

            return m_Info.TowardAim;
        }
        private Vector3 GetFacingTowardMovement()
        {
            if (GetMoveInputMagnitude() < 0.1f) return m_Info.TowardMovement;
            m_Info.TowardMovement = GetCameraFacingInternal() * new Vector3(GetMoveInput().x, 0f, GetMoveInput().y);

            SetTargetAngleY(m_Info.MoveInput);

            return m_Info.TowardMovement;
        }

        public float GetCurrentSpeed()
        {
            return GetCurrentSpeedInternal();
        }
        private float GetCurrentSpeedInternal()
        {
            float currentVelocitySpeed = new Vector3(GetMoveInput().x, 0f, GetMoveInput().y).magnitude;
            float currentSpeed = Mathf.Lerp(0f, GetFinalSpeed() * GetFinalSpeedRate() * currentVelocitySpeed, GetFinalAcceleration());
            m_Info.CurrentSpeed = currentSpeed;

            //UpdateInfo.CurrentSpeed += Acceleration * Time.deltaTime;
            m_Info.IsMove = GetMoveInput() != Vector2.zero;
            if (!m_Info.IsMove)
            {
                m_Info.CurrentSpeed -= m_Deceleration * Time.deltaTime;
            }
            float currentSpeedClamped = Mathf.Clamp(m_Info.CurrentSpeed, 0f, GetFinalSpeed() * GetFinalSpeedRate());
            m_Info.CurrentSpeed = currentSpeedClamped;

            float currentSpeedRate = m_Info.CurrentSpeed / GetFinalSpeed() * GetFinalSpeedRate();
            m_Info.CurrentSpeedRate = currentSpeedRate;

            m_OnCurrentSpeedRateChange?.Invoke(currentSpeedRate);
            return m_Info.CurrentSpeed;
        }

        private void AdjustTowardInput()
        {
            Vector3 forwardDirection = m_UnitRotateDirection.transform.forward;
            Vector3 rightDirection = m_UnitRotateDirection.transform.right;

            float xTowardDir = Mathf.Lerp(0f, Vector3.Dot(rightDirection, m_Info.CurrentVelocity) / m_Speed, m_Acceleration);
            float zTowardDir = Mathf.Lerp(0f, Vector3.Dot(forwardDirection, m_Info.CurrentVelocity) / m_Speed, m_Acceleration);

            float moveThreshold = 0.1f;
            if (GetMoveInputMagnitude() < moveThreshold)
            {
                xTowardDir = 0f;
                zTowardDir = 0f;
            }
            Vector3 towardDir = new(xTowardDir, 0f, zTowardDir);
            m_Info.TowardInput = towardDir;

            m_OnTowardInputXChange?.Invoke(towardDir.x);
            m_OnTowardInputZChange?.Invoke(towardDir.z);
        }

        public void SetAdditionalForce(Vector3 force)
        {
            m_Info.AdditionalForceTowardMovement = force;
        }

        private void SetTargetAngleY(Vector2 input)
        {
            var aimDir = new Vector3(input.x, 0f, input.y).normalized;
            m_Info.TargetAngleY = Mathf.Atan2(aimDir.x, aimDir.z) * Mathf.Rad2Deg + m_MainCamera.transform.eulerAngles.y;
        }
        public void SetCurrenVelocity(Vector3 val)
        {
            m_Info.CurrentVelocity = val;
        }
        public void SetCanMove(bool val)
        {
            m_CanMove = val;
        }
        public void SetMoveVerticalInput(float val)
        {
            m_Info.MoveInput.y = val;
        }
        public void SetMoveHorizontalInput(float val)
        {
            m_Info.MoveInput.x = val;
        }
        public void SetSpeed(float set)
        {
            m_Speed = set;
        }
        public void SetAccelaration(float set)
        {
            m_Acceleration = set;
        }
        public void SetSpeedRate(float set)
        {
            m_SpeedRate = set;
        }

        public void SetSpinVelocity(float val)
        {
            m_Info.SpinVelocity = val;
        }
        public void SetTowardMode(ERotateToward val)
        {
            m_RotateToward = val;
            AdjustAngleY();
        }
        public void SetTowardMode(int change)
        {
            m_RotateToward = (ERotateToward)change;
            AdjustAngleY();
        }

        private void AdjustAngleY()
        {
            float xOri = m_UnitRotateDirection.localRotation.x;
            float zOri = m_UnitRotateDirection.localRotation.z;
            m_UnitRotateDirection.localRotation = Quaternion.Euler(xOri, m_Info.TargetAngleY, zOri);
        }
        public void SetAimDirX(float set)
        {
            m_Info.AimInput.x = set;
        }
        public void SetAimDirY(float set)
        {
            m_Info.AimInput.y = set;
        }
        public void SetMoveInput(Vector2 set)
        {
            m_Info.MoveInput = set;
        }
    }
}

