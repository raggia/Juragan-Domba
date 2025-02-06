using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Rush
{
    // Chat GPT source for speed up development
    public enum AIState { Idle, Patrol, Roaming, FollowTarget }

    public class AILocomotion : MonoBehaviour
    {
        [SerializeField] private AIState m_CurrentState;

        [SerializeField] private Transform[] m_PatrolPoints;
        [SerializeField] private float m_InteractionRange = 2f;
        [SerializeField] private float m_FollowSpeed = 3.5f;
        [SerializeField] private float m_PatrolSpeed = 2f;
        [SerializeField] private float m_RoamingSpeed = 2.5f;
        [SerializeField] private float m_RoamingRadius = 10f;
        [SerializeField] private float m_DelayDuration = 2f;

        [SerializeField]
        private UnityEvent m_OnStartIdle = new();
        [SerializeField]
        private UnityEvent m_OnStartRoaming = new();
        [SerializeField]
        private UnityEvent m_OnStartPatrol = new();
        [SerializeField]
        private UnityEvent m_OnStartFollow = new();
        [SerializeField]
        private UnityEvent<bool> m_OnRestChange = new();
        [SerializeField]
        private UnityEvent<float> m_OnTurnChange = new();
        [SerializeField, ReadOnly]
        private bool m_StayInRest;

        private NavMeshAgent m_Agent;
        private int m_CurrentPatrolIndex;
        private Transform m_Target;
        private bool m_IsInteracted = false;
        private bool m_IsRest = false; // Used only for Patrol and Roaming
        private float m_RestTimer = 0f; // Timer for resting during Patrol or Roaming



        private void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_CurrentState = AIState.Roaming; // Default state
            SetRandomRoamingDestination(); // Start with a random position for roaming
        }

        private void Update()
        {
            // State management
            switch (m_CurrentState)
            {
                case AIState.Idle:
                    HandleIdleState();
                    break;
                case AIState.Patrol:
                    HandlePatrolState();
                    break;
                case AIState.Roaming:
                    HandleRoamingState();
                    break;
                case AIState.FollowTarget:
                    HandleFollowTargetState();
                    break;
            }
            CheckTurnDirection();
        }
        private void SetIsRest(bool set)
        {
            m_IsRest = set;
            m_Agent.isStopped = set;
        }
        private void HandleIdleState()
        {
            m_Agent.isStopped = true; // No movement during Idle
        }

        private void HandlePatrolState()
        {
            m_Agent.isStopped = false;
            m_Agent.speed = m_PatrolSpeed;

            if (m_PatrolPoints.Length == 0) return;

            if (m_IsRest && !m_StayInRest)
            {
                m_RestTimer += Time.deltaTime;
                if (m_RestTimer >= m_DelayDuration)
                {
                    m_RestTimer = 0f;
                    m_IsRest = false; // End the rest period and resume movement
                    MoveToNextPatrolPoint();
                }
            }
            else if (!m_Agent.pathPending && m_Agent.remainingDistance < 0.5f)
            {
                m_IsRest = true; // Start resting before moving to the next waypoint
            }

            if (m_IsInteracted)
            {
                m_CurrentState = AIState.FollowTarget;
            }
        }

        private void HandleRoamingState()
        {
            m_Agent.isStopped = false;
            m_Agent.speed = m_RoamingSpeed;

            if (m_IsRest && !m_StayInRest)
            {
                m_RestTimer += Time.deltaTime;
                if (m_RestTimer >= m_DelayDuration)
                {
                    m_RestTimer = 0f;
                    m_IsRest = false; // End the rest period and resume movement
                    SetRandomRoamingDestination();
                }
            }
            else if (!m_Agent.pathPending && m_Agent.remainingDistance < 0.5f)
            {
                m_IsRest = true; // Start resting before moving to the next random roaming position
            }

            if (m_IsInteracted)
            {
                m_CurrentState = AIState.FollowTarget;
            }
        }

        private void HandleFollowTargetState()
        {
            SetIsRest(m_Target == null);
            if (m_Target == null) return;

            m_Agent.speed = m_FollowSpeed;
            m_Agent.SetDestination(m_Target.position);
        }

        private void MoveToNextPatrolPoint()
        {
            m_CurrentPatrolIndex = (m_CurrentPatrolIndex + 1) % m_PatrolPoints.Length;
            m_Agent.SetDestination(m_PatrolPoints[m_CurrentPatrolIndex].position);
        }

        private void SetRandomRoamingDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * m_RoamingRadius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, m_RoamingRadius, NavMesh.AllAreas))
            {
                m_Agent.SetDestination(hit.position);
            }
        }

        public void Interact(Transform interactor)
        {
            m_Target = interactor;
            m_IsInteracted = true;
            m_CurrentState = AIState.FollowTarget;
        }

        public void SetState(AIState newState)
        {
            m_CurrentState = newState;

            SetIsRest(newState == AIState.Idle);

            switch (m_CurrentState)
            {
                case AIState.Idle:
                    OnStartIdleInvoke();
                    break;
                case AIState.Roaming:
                    OnStartRoamingInvoke();
                    break;
                case AIState.Patrol:
                    OnStartPatrolInvoke();
                    break;
                case AIState.FollowTarget:
                    OnStartFollowInvoke();
                    break;
            }
            
        }

        public void SetStayInRest(bool set)
        {
            m_StayInRest = set;
            if (m_StayInRest)
            {
                SetIsRest(true);
            }
        }
        private void CheckTurnDirection()
        {
            if (m_Agent.hasPath)
            {
                // Normalize the forward direction and the direction to the destination
                Vector3 currentForward = transform.forward.normalized;
                Vector3 toDestination = (m_Agent.steeringTarget - transform.position).normalized;

                // Calculate the cross product
                Vector3 cross = Vector3.Cross(currentForward, toDestination);

                // The y-component of the cross product tells us the turn direction
                float normalizedCrossY = cross.y; // This is now in the range [-1, 1]

                // Determine the turning direction
                if (normalizedCrossY > 0)
                {
                    Debug.Log("Turning Left");
                }
                else if (normalizedCrossY < 0)
                {
                    Debug.Log("Turning Right");
                }
                else
                {
                    Debug.Log("Moving Straight");
                }
                OnTurnChangedInvoke(normalizedCrossY);
            }
        }

        private void OnSetRestInvoke(bool set)
        {
            m_OnRestChange?.Invoke(set);
        }
        private void OnStartIdleInvoke()
        {
            m_OnStartIdle?.Invoke();
        }
        private void OnStartRoamingInvoke()
        {
            SetRandomRoamingDestination();
            m_OnStartRoaming?.Invoke();
        }
        private void OnStartPatrolInvoke()
        {
            m_OnStartPatrol?.Invoke();
        }
        private void OnStartFollowInvoke()
        {
            m_OnStartFollow?.Invoke();
        }

        private void OnTurnChangedInvoke(float val)
        {
            m_OnTurnChange?.Invoke(val);
        }
    }
}
