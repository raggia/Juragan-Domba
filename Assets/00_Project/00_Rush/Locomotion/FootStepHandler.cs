using UnityEngine;

namespace Rush
{
    public partial class FootStepHandler : MonoBehaviour
    {
        [SerializeField]
        private Animator m_Animator;
        [SerializeField]
        private string m_SpeedRateName = "SpeedRate";
        [SerializeField, ReadOnly]
        private float m_FootstepTimer;
        [SerializeField]
        private float m_FootstepInterval = 1f; // Default interval (adjust based on speed)

        [SerializeField]
        private Transform m_StepSpawn;
        [SerializeField]
        private AudioClip m_StepClip;
        private void Update()
        {
            Handle();
        }

        private void Handle()
        {
            // Get the current speed parameter from the Animator
            float speed = m_Animator.GetFloat(m_SpeedRateName);
            if (speed < 0.1f)
            {
                m_FootstepTimer = 0f;
                return;
            }

                
            // Adjust interval based on speed (example thresholds)
            //m_FootstepInterval = Mathf.Lerp(0.9f, 0.5f, speed);

            // Play footstep sound at intervals
            m_FootstepTimer += Time.deltaTime * speed;
            if (m_FootstepTimer >= m_FootstepInterval)
            {
                //PlayFootstep();
                AudioSingleton.Instance.PlaySFX(m_StepClip, m_StepSpawn);
                m_FootstepTimer = 0f;
            }
        }
    }
}
