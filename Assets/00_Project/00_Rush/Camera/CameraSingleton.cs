using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    
    public class CameraSingleton : Singleton<CameraSingleton>
    {
        [SerializeField]
        private Transform m_CameraSpawnRoot;
        [SerializeField, ReadOnly]
        private CameraShot m_ActiveCamera;

        [SerializeField, ReadOnly]
        private List<CameraShot> m_CameraShots = new();
        [SerializeField]
        private UnityEvent<CameraShot> m_OnCameraShotsChanged = new();
        private CameraShot GetCameraShotInternal(CinemachineCamera camera)
        {
            CameraShot target = m_CameraShots.Find(x => x.GetCameraDefinition() == camera);
            if (target == null)
            {
                Debug.LogError($"There is no camera shot name [{camera}] on the list");
                return default;
            }
            return target;
        }
        public void ChangeCamea(CinemachineCamera camera)
        {
            ChangeCameraInternal(camera);
        }
        private void ChangeCameraInternal(CinemachineCamera camera)
        {
            m_ActiveCamera = GetCameraShotInternal(camera);
            foreach(CameraShot target in m_CameraShots)
            {
                target.SetPriority(0);
                target.gameObject.SetActive(false);
            }
            m_ActiveCamera.gameObject.SetActive(true);
            m_ActiveCamera.SetPriority(1);
            OnCameraChangeInvoked(m_ActiveCamera);
        }
        public void AddCameraHandlerShot(CameraShot target)
        {
            if (!m_CameraShots.Contains(target))
            {
                m_CameraShots.Add(target);
                target.SpawnVirtualCamera(m_CameraSpawnRoot);

                if (target.UseThisAsTheFirstCameraOnStartScene)
                {
                    ChangeCameraInternal(target.GetSpawnedCamera());
                }
            }
        }

        public void RemoveCameraHandlerShot(CameraShot target)
        {
            if (m_CameraShots.Contains(target))
            {
                m_CameraShots.Remove(target);
            }
        }

        private void OnCameraChangeInvoked(CameraShot cameraShot)
        {
            m_OnCameraShotsChanged?.Invoke(cameraShot);
        }
    }
}

