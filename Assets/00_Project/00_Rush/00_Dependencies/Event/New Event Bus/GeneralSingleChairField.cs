using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public class StringChairField : EventSingleChairField<string>
    {
        
    }
    [System.Serializable]
    public class FloatChairField : EventSingleChairField<float>
    {

    }
    [System.Serializable]
    public class BoolChairField : EventSingleChairField<bool>
    {

    }
    [System.Serializable]
    public class Vector2ChairField : EventSingleChairField<Vector2>
    {

    }
    [System.Serializable]
    public class TransformChair : EventSingleChairField<Transform>
    {

    }
    [System.Serializable]
    public class AudiClipChair : EventSingleChairField<AudioClip>
    {

    }
}
