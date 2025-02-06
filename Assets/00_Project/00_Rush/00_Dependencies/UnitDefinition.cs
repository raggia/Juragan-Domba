using UnityEngine;

namespace Rush
{
    public interface IHasDefinition<T> where T : UnitDefinition
    {
        T Definition { get; }
        void SetDefinition(T val);

    }
    [System.Serializable]
    public abstract class UnitDefinition : ScriptableObject
    {
        [SerializeField]
        private string m_Id;
        [SerializeField]
        private string m_Label;
        [SerializeField, TextArea]
        private string m_Description;

        public string Id => $"{m_Id}/{m_Label}";
        public string Label => m_Label;
        public string Description => m_Description;
    }
}
