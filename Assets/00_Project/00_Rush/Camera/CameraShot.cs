using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public class ShotField
    {
        [SerializeField]
        private string m_ShotDefinition; // change with scriptable object later
        [SerializeField]
        private Transform m_FollowAt;
        [SerializeField]
        private Transform m_LookAt;
        public string Definition => m_ShotDefinition;
        public Transform FollowAt => m_FollowAt;
        public Transform LookAt => m_LookAt;

        [SerializeField]
        private UnityEvent<ShotField> m_OnShotChange = new();

    }
    public abstract class CameraShot : MonoBehaviour
    {
        [SerializeField]
        private bool m_UseThisAsTheFirstCameraOnStartScene;
        [SerializeField]
        private CinemachineCamera m_VirtualCameraPrefab;
        [SerializeField]
        private List<ShotField> m_ShotFields = new();

        [Header("Rotation Setting")]
        [SerializeField]
        protected bool m_CanRotate;
        [SerializeField]
        private bool m_IsCurrentDeviceMouse = false;

        [SerializeField, Range(1f, 20f)]
        protected float m_RotationSensitifity;

        protected const float m_Threshold = 0.01f;

        protected float m_PitchValue;
        protected float m_YawValue;

        [SerializeField, ReadOnly]
        protected CinemachineCamera m_SpawnedCamera;
        [SerializeField, ReadOnly]
        private CinemachineBasicMultiChannelPerlin m_BasicMultiChannelPerlin;
        public bool UseThisAsTheFirstCameraOnStartScene => m_UseThisAsTheFirstCameraOnStartScene;
        public CinemachineCamera GetCameraPrefab() => m_VirtualCameraPrefab;
        public CinemachineCamera GetSpawnedCamera() => m_SpawnedCamera;
        protected float GetDeltaMultiplier()
        {
            return m_IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
        }
        private CinemachineBasicMultiChannelPerlin GetBasicMultiChannelPerlin()
        {
            if (m_BasicMultiChannelPerlin == null)
            {
                if (m_BasicMultiChannelPerlin.TryGetComponent(out CinemachineBasicMultiChannelPerlin noise))
                {
                    m_BasicMultiChannelPerlin = noise;
                }
            }
            return m_BasicMultiChannelPerlin;
        }
        public void SetPriority(int priority)
        {
            SetPriorityInternal(priority);
        }
        public void ChangeShot(string definition)
        {

        }
        private void SetPriorityInternal(int priority)
        {
            m_SpawnedCamera.Priority.Value = priority;
        }

        public CinemachineCamera GetCameraDefinition()
        {
            return m_SpawnedCamera;
        }
        private ShotField GetShotInternal(string definition)
        {
            ShotField field = m_ShotFields.Find(x => x.Definition == definition);
            if (field == null)
            {
                Debug.LogError($"There is no camera shot name [{definition}] on the list");
                return default;
            }    
            return field;
        }
        public bool IsMainShot(string definition)
        {
            return GetShotInternal(definition) == m_ShotFields[0];
        }
        private ShotField GetMainShotInternal()
        {
            if (m_ShotFields.Count > 0)
            {
                return m_ShotFields[0];
            }
            else
            {
                return default;
            }
        }
        public Transform GetFollowAt(string definition)
        {
            return GetShotInternal(definition).FollowAt;
        }
        public Transform GetLookAt(string definition)
        {
            return GetShotInternal(definition).LookAt;
        }

        // next we will make this addressable instantiate, and make this async or ienumrator
        public virtual void SpawnVirtualCamera(Transform spawnRoot)
        {
            if (m_SpawnedCamera != null) return;
            m_SpawnedCamera = Instantiate(m_VirtualCameraPrefab, spawnRoot, false);

            m_SpawnedCamera.LookAt = GetMainShotInternal().LookAt;
            m_SpawnedCamera.Follow = GetMainShotInternal().FollowAt;
        }

        private void RegisterShot()
        {
            CameraSingleton.Instance.AddCameraHandlerShot(this);
        }
        private void UnRegisterShot()
        {
            CameraSingleton.Instance.RemoveCameraHandlerShot(this);
        }
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => CameraSingleton.HasInstance);
            RegisterShot();
        }
        private void OnDestroy()
        {
            UnRegisterShot();
        }

        protected virtual float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public abstract void Rotate(Vector2 input);


        public abstract void Zoom(float input);
    }
}

