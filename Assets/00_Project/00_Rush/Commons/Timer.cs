using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    [System.Serializable]
    public class TimerField
    {
        public bool IsPlay;
        public float TimeTarget;

        [Rush.ReadOnly]
        public float CurrentTime;
        public UnityEvent<float> OnCurrentTimeChange = new();
        public UnityEvent OnTimeTriggered = new();
    }
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TimerField _timerControl;

        private void Start()
        {
            Init();
        }
        private void Update()
        {
            UpdateTimer();
        }
        public void Init()
        {
            _timerControl.CurrentTime = 0f;
        }
        public void SetTimeTarget(float set) => _timerControl.TimeTarget = set;
        public void UpdateTimer()
        {
            if (_timerControl.IsPlay)
            {
                _timerControl.CurrentTime += Time.deltaTime;
                _timerControl.OnCurrentTimeChange?.Invoke(_timerControl.CurrentTime);

                if (_timerControl.CurrentTime >= _timerControl.TimeTarget)
                {
                    _timerControl.OnTimeTriggered?.Invoke();
                    ResetTimer();
                }
            }
        }
        public void PlayTimer()
        {
            _timerControl.CurrentTime = 0f;
            _timerControl.IsPlay = true;
        }
        public void ResumeTimer()
        {
            _timerControl.IsPlay = true;
        }
        public void PauseTimer()
        {
            _timerControl.IsPlay = false;
        }
        public void ResetTimer()
        {
            _timerControl.CurrentTime = 0f;
        }
    }
}

