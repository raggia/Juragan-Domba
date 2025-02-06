using System.Collections.Generic;
using UnityEngine;

namespace Rush
{
    [CreateAssetMenu(fileName = "New Level Name", menuName = "Rush/Level/Level Name")]
    public partial class LevelNameDefinition : IconDefinition
    {
        [SerializeField]
        private float m_StartExperience = 100f;
        [SerializeField]
        private float m_ExponentialGrowth = 1.3f;
        [SerializeField]
        private int m_MaxLevel = 50;

        public float StartExperience => m_StartExperience;
        public float ExponentialGrowth => m_ExponentialGrowth;
        public float MaxLevel => m_MaxLevel;
    }
}
