using MoreMountains.Tools;
using Unity.Cinemachine;
using UnityEngine;

namespace Rush
{
    public class ThirdPersonCamera : CameraShot
    {
        [SerializeField, ReadOnly]
        private CinemachineOrbitalFollow m_OrbitalCamera;

        [SerializeField]
        private float m_StartPitchValue = 60f;
        [SerializeField]
        private float m_MaxZoomOut = 10f;

        private bool m_ReadyToUse;
        private void SetOrbitalCamera(CinemachineOrbitalFollow set)
        {
            m_OrbitalCamera = set;
            m_OrbitalCamera.VerticalAxis.Value = m_StartPitchValue;
            m_OrbitalCamera.Radius = m_MaxZoomOut;
            m_ReadyToUse = true;
        }
        public override void Rotate(Vector2 input)
        {
            if (!m_ReadyToUse || !m_CanRotate) return;
            //if (!m_CanRotate) return;
            m_PitchValue += input.y * m_RotationSensitifity * GetDeltaMultiplier();
            m_YawValue += input.x * m_RotationSensitifity * GetDeltaMultiplier();

            // clamp our pitch & yaw rotation
            m_YawValue = ClampAngle(m_YawValue, m_OrbitalCamera.HorizontalAxis.Range.x, m_OrbitalCamera.HorizontalAxis.Range.y);
            m_PitchValue = ClampAngle(m_PitchValue, m_OrbitalCamera.VerticalAxis.Range.x, m_OrbitalCamera.VerticalAxis.Range.y);

            m_OrbitalCamera.HorizontalAxis.Value = m_YawValue;
            m_OrbitalCamera.VerticalAxis.Value = m_PitchValue;
        }

        public override void Zoom(float input)
        {
            m_OrbitalCamera.Radius = m_MaxZoomOut * input;

        }
        public override void SpawnVirtualCamera(Transform spawnRoot)
        {
            base.SpawnVirtualCamera(spawnRoot);
            CinemachineOrbitalFollow orbit = m_SpawnedCamera.GetComponent<CinemachineOrbitalFollow>();

            SetOrbitalCamera(orbit);
        }

    }
}
