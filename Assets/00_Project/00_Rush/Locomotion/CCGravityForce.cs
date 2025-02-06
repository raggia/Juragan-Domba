using UnityEngine;

namespace Rush
{
    [System.Serializable]
    public class CCGravityForce : CCForceExt
    {
        [SerializeField]
        private bool m_UseGravity = true;
        [SerializeField]
        private float m_GravityForce = 9f;
        [SerializeField]
        private float m_GravityMultiplier = 1f;
        [SerializeField]
        private float m_GroundedOffsite = 0.14f;
        [SerializeField]
        private float m_GroundedRadius = 0.5f;
        [SerializeField]
        private LayerMask m_GroundLayers;

        [SerializeField, ReadOnly]
        private float m_VerticalVelocity;
        [SerializeField, ReadOnly]
        private bool m_IsGrounded;
        public CharacterController GetCharacterController() => m_Locomotion.GetCharacterController();

        private float GetGravity()
        {
            if (!m_UseGravity) return 0f;
            if (IsGrounded())
            {

                m_VerticalVelocity = -1f;
            }
            else
            {
                m_VerticalVelocity -= m_GravityForce * m_GravityMultiplier * Time.deltaTime;
            }

            return m_VerticalVelocity;
        }

        private bool IsGrounded()
        {
            float x = GetCharacterController().transform.position.x;
            float y = GetCharacterController().transform.position.y;
            float z = GetCharacterController().transform.position.z;
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(x, y - m_GroundedOffsite, z);
            return m_IsGrounded = Physics.CheckSphere(spherePosition, m_GroundedRadius, m_GroundLayers, QueryTriggerInteraction.Ignore);
        }
        protected override void OnApplyForceInvoked()
        {
            m_Force = new Vector3(0f, GetGravity(), 0f);
            base.OnApplyForceInvoked();

        }
    }
}
