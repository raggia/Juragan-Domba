using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    public partial class CCLocoMotion : Locomotion
    {
        [SerializeField]
        private CCLocomotionField m_CharacterControllerField;

        private void Start()
        {
            m_CharacterControllerField.Initialize(this);
        }

        private void Update()
        {
            m_CharacterControllerField.ApplyForce();
        }

        public CharacterController GetCharacterController() => m_CharacterControllerField.GetCharacterController();
        public override void Move(Vector2 moveInput)
        {
            base.Move(moveInput);

            //Vector3 gravityForce = new Vector3(0f, m_CharacterControllerField.GetGravity(), 0f);
            //SetDashForceInternal(gravityForce);
            SetAdditionalForceInternal(m_CharacterControllerField.GetFinalAdditionalForce());
            m_CharacterControllerField.Move(GetFinaleMoveForce());

            //m_CharacterControllerField.Move(GetMove());
            m_LocomotionField.SetCurrenVelocity(GetVelocity());
        }

        protected override Vector3 GetVelocity()
        {
            return m_CharacterControllerField.GetVelocity();
        }
    }

    [System.Serializable]
    public partial class CCLocomotionField
    {
        [SerializeField]
        private CharacterController m_CharacterController;
        [SerializeReference, SubclassSelector]
        private List<CCForceExt> m_AdditionalForces = new();

        [SerializeField, ReadOnly]
        private Vector3 m_FinalAdditionalForce = Vector3.zero;
        public void Initialize(CCLocoMotion cLocoMotion)
        {
            foreach (var c in m_AdditionalForces)
            {
                c.Initialize(cLocoMotion);
            }
        }
        public void ApplyForce()
        {
            Vector3 force = Vector3.zero;
            foreach(var c in m_AdditionalForces)
            {
                c.ApplyForce();
                force += c.Force;
            }
            m_FinalAdditionalForce = force;
        }

        private CCForceExt GetForce(string id)
        {
            CCForceExt match = m_AdditionalForces.Find(x => x.ForceName == id);
            if (match == null)
            {
                Debug.LogError($"No Force called {id} on the list");
                return default;
            }
            else
            {
                return match;
            }
        }
        public Vector3 GetFinalAdditionalForce()
        {
            return m_FinalAdditionalForce;
        }
        public void AddForce(CCForceExt force)
        {
            if (m_AdditionalForces.Contains(force)) return;
            m_AdditionalForces.Add(force);
        }
        public void RemoveForce(CCForceExt force)
        {
            if (!m_AdditionalForces.Contains(force)) return;
            m_AdditionalForces.Remove(force);
        }
        public CharacterController GetCharacterController() => m_CharacterController;

        public void Move(Vector3 move)
        {
            m_CharacterController.Move(move);
        }
        public Vector3 GetVelocity()
        {
            return m_CharacterController.velocity;
        }
    }
}
