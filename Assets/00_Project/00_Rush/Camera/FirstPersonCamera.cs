using UnityEngine;

namespace Rush
{
    public class FirstPersonCamera : CameraShot
    {
        [SerializeField]
        private Transform m_UpDownLookTarget;
        [SerializeField]
        private Transform m_LeftRightLookTarget;
        

        

        [SerializeField]
        private float m_TopClamp = 90f;
        [SerializeField]
        private float m_BottomClamp = -90f;

        
        public override void Rotate(Vector2 input)
        {
            if (!m_CanRotate) return;
            // if there is an input
            if (input.sqrMagnitude >= m_Threshold)
            {
                m_PitchValue += -input.y * m_RotationSensitifity * GetDeltaMultiplier();
                m_YawValue = input.x * m_RotationSensitifity * GetDeltaMultiplier();

                // clamp our pitch rotation
                m_PitchValue = ClampAngle(m_PitchValue, m_BottomClamp, m_TopClamp);

                // Update Cinemachine camera target pitch
                m_UpDownLookTarget.localRotation = Quaternion.Euler(m_PitchValue, 0.0f, 0.0f);

                // rotate the player left and right
                m_LeftRightLookTarget.Rotate(Vector3.up * m_YawValue);
            }
        }

        public override void Zoom(float input)
        {

        }
        
    }
}
