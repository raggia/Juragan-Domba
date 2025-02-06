using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rush
{
#if UNITY_EDITOR
    [CustomEditor(typeof(AnimationEventState))]
    public class AnimationEventStateEditor : Editor
    {
        private AnimationClip m_PreviewClip;
        private float m_PreviewTime;

        private bool m_IsPreviewing;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AnimationEventState eventState = (AnimationEventState)target;
            if (Validate(eventState, out string errorMessage))
            {
                GUILayout.Space(10);

                if (m_IsPreviewing)
                {
                    if (GUILayout.Button("Stop Preview"))
                    {
                        m_IsPreviewing = false;
                    }
                    else
                    {
                        PreviewAnimationClip(eventState);
                    }
                }
                else if (GUILayout.Button("Preview"))
                {
                    m_IsPreviewing = true;
                }

                GUILayout.Label($"Previewing at {m_PreviewTime:F2}s", EditorStyles.helpBox);
            }
            else
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
            }
        }
        private void PreviewAnimationClip(AnimationEventState eventState)
        {
            if (m_PreviewClip == null)
            {
                return;
            }

            m_PreviewTime = eventState.GetTriggerTime() * m_PreviewClip.length;

            AnimationMode.StartAnimationMode();
            AnimationMode.SampleAnimationClip(Selection.activeGameObject, m_PreviewClip, m_PreviewTime);
            AnimationMode.StopAnimationMode();
        }

        private bool Validate(AnimationEventState eventState, out string errorMessage)
        {
            AnimatorController controller = GetValidAnimatorController(out errorMessage);
            if (controller == null)
            {
                return false;
            }

            ChildAnimatorState targetState = controller.layers.SelectMany(layer => layer.stateMachine.states).FirstOrDefault(x => x.state.behaviours.Contains(eventState));

            m_PreviewClip = targetState.state?.motion as AnimationClip;
            if (m_PreviewClip == null)
            {
                errorMessage = "No Valid AnimationClip found for the current state";
                return false;
            }
            return true;
        }

        private AnimatorController GetValidAnimatorController(out string errorMessage)
        {
            errorMessage = string.Empty;

            GameObject targetGameObject = Selection.activeGameObject;
            if (targetGameObject == null)
            {
                errorMessage = "Please select a GameObject with an Animator to Preview";
                return null;
            }

            Animator animator = targetGameObject.GetComponent<Animator>();
            if (animator == null)
            {
                errorMessage = "The Selected GameObject does not have a Animator component";
                return null;
            }

            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
            if (controller == null)
            {
                errorMessage = "The selected Animator does not have a valid AnimatorController";
                return null;
            }

            return controller;
        }
    }
#endif
}

