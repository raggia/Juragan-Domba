using NaughtyAttributes;
using UnityEngine;

namespace Rush
{
    public partial class AdventureLevel : MonoBehaviour, ILevel
    {
        [SerializeField]
        private bool m_UseDummyExpData;
        [SerializeField, ShowIf(nameof(m_UseDummyExpData)), AllowNesting]
        private float m_DummyExp;
        [SerializeField]
        private LevelField m_LevelField;

        private void Start()
        {
            if (m_UseDummyExpData)
            {
                m_LevelField.Init(m_DummyExp);
            }
            else
            {
                SetExperience(m_LevelField.GetStartExperience());
            }
        }
        private void OnValidate()
        {
            m_LevelField.Validate();
        }
        public void AddExperience(float add)
        {
            m_LevelField.AddExperience(add);
        }

        public void Init(float loadExp)
        {
            m_LevelField.Init(loadExp);
        }

        public void SetExperience(float val)
        {
            m_LevelField.SetExperience(val);
        }
    }
}
