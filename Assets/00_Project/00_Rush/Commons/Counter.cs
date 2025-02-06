using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rush
{
    public enum StartMode
    {
        StartFromMin = 0,
        StartFromMax = 1,
        StartFromCustom = 2,
    }
    public enum CounterRuleMode
    {
        NoBoundaries = 0,
        BreakMinBackToMax = 1,
        BreakMaxBackToMin = 2,
        BreakMinBackToCustom = 3,
        BreakMaxBackToCustom = 4,
        MinClamp = 5,
        MaxClamp = 6,
        MinMaxClamp = 7,
        BreakMinMaxLoop = 8,
    }
    public enum ThresholdExecute
    {
        Equal = 0,
        NotEqual = 1,
        LessThan = 2,
        GreaterThan = 3,
        LessThanEqual = 4,
        GreaterThanEqual = 5,
    }
    [Serializable]
    public struct MinMax
    {
        public int Min;
        public int Max;
    }
    [Serializable]
    public class CounterEventField
    {
        public ThresholdExecute CounterExecuteType = ThresholdExecute.Equal;
        public int CounterTarget;
        public UnityEvent<int> OnCounterIntEvent = new UnityEvent<int>();

        public void Execute(int result)
        {
            switch (CounterExecuteType)
            {
                case ThresholdExecute.Equal:
                    OnEqual(result);
                    break;
                case ThresholdExecute.NotEqual:
                    OnNotEqual(result);
                    break;
                case ThresholdExecute.LessThan:
                    OnLessThan(result);
                    break;
                case ThresholdExecute.GreaterThan:
                    OnGreaterThan(result);
                    break;
            }
        }
        private void OnEqual(int result)
        {
            if (CounterTarget == result)
            {
                OnCounterIntEvent?.Invoke(result);
            }
        }
        private void OnNotEqual(int result)
        {
            if (CounterTarget != result)
            {
                OnCounterIntEvent?.Invoke(result);
            }
        }
        private void OnLessThan(int result)
        {
            if (CounterTarget > result)
            {
                OnCounterIntEvent?.Invoke(result);
            }
        }
        private void OnGreaterThan(int result)
        {
            if (CounterTarget < result)
            {
                OnCounterIntEvent?.Invoke(result);
            }
        }
    }
    [Serializable]
    public class CounterField
    {
        public StartMode StartCountFrom = StartMode.StartFromMin;
        public CounterRuleMode CounterRule = CounterRuleMode.NoBoundaries;

        [HideIf("CounterRule", CounterRuleMode.NoBoundaries)]
        [AllowNesting]
        public MinMax MinMaxCount;

        [ShowIf("StartCountFrom", StartMode.StartFromCustom)]
        [AllowNesting]
        public int CustomStartCount = 0;

        [ReadOnly] 
        public int CurrentCount;
        public List<CounterEventField> OnCustomCountEventFields = new List<CounterEventField>();

        public void Initialize()
        {
            switch (StartCountFrom)
            {
                case StartMode.StartFromMin:
                    CurrentCount = MinMaxCount.Min;
                    break;
                case StartMode.StartFromMax:
                    CurrentCount = MinMaxCount.Max;
                    break;
                case StartMode.StartFromCustom:
                    CurrentCount = CustomStartCount;
                    break;
            }
        }

        public void Executes(int result) 
        {
            foreach (var ce in OnCustomCountEventFields)
            {
                ce.Execute(result);
            }
        }
    }
    public class Counter : MonoBehaviour
    {
        [SerializeField] 
        private CounterField m_CounterField;

        protected void Start()
        {
            m_CounterField.Initialize();
        }
        public StartMode GetMode() => m_CounterField.StartCountFrom;
        public CounterRuleMode GetCounterRule() => m_CounterField.CounterRule;
        public int GetCustomStartCount() => m_CounterField.CustomStartCount;
        public int GetCurrentCount() => m_CounterField.CurrentCount;
        public int GetMinCount() => m_CounterField.MinMaxCount.Min;
        public int GetMaxCount() => m_CounterField.MinMaxCount.Max;
        public List<CounterEventField> GetOnCounterEventFields() => m_CounterField.OnCustomCountEventFields;
        public void AddCurrentCounter(int add)
        {
            int currentV = GetCurrentCount();
            int result = currentV + add;
            SetCurrentCounter(result);
        }

        private int MaxClamp(int resultCount)
        {
            int maxV = m_CounterField.MinMaxCount.Max;
            if (resultCount > maxV)
            {
                resultCount = maxV;
            }
            return resultCount;
        }
        private int MinClamp(int resultCount)
        {
            int minV = m_CounterField.MinMaxCount.Min;
            if (resultCount < minV)
            {
                resultCount = minV;
            }
            return resultCount;
        }
        private int MinMaxClamp(int resultCount)
        {
            int maxV = m_CounterField.MinMaxCount.Max;
            int minV = m_CounterField.MinMaxCount.Min;
            resultCount = Mathf.Clamp(resultCount, minV, maxV);
            return resultCount;
        }

        private int MaxBackToMin(int resultCount)
        {
            int maxV = m_CounterField.MinMaxCount.Max;
            int minV = m_CounterField.MinMaxCount.Min;
            if (resultCount > maxV)
            {
                resultCount = minV;
            }
            return resultCount;
        }
        private int MinBackToCustom(int resultCount)
        {
            int custom = GetCustomStartCount();
            int minV = m_CounterField.MinMaxCount.Min;
            if (resultCount < minV)
            {
                resultCount = custom;
            }
            return resultCount;
        }
        private int MaxBackToCustom(int resultCount)
        {
            int custom = GetCustomStartCount();
            int maxV = m_CounterField.MinMaxCount.Max;
            if (resultCount > maxV)
            {
                resultCount = custom;
            }
            return resultCount;
        }
        private int MinBackToMax(int resultCount)
        {
            int maxV = m_CounterField.MinMaxCount.Max;
            int minV = m_CounterField.MinMaxCount.Min;
            if (resultCount < minV)
            {
                resultCount = maxV;
            }
            return resultCount;
        }

        private int MinMaxLoop(int resultCount)
        {
            int maxV = m_CounterField.MinMaxCount.Max;
            int minV = m_CounterField.MinMaxCount.Min;
            if (resultCount < minV)
            {
                resultCount = maxV;
            }
            if (resultCount > maxV)
            {
                resultCount = minV;
            }
            return resultCount;
        }

        public void SetCurrentCounter(int set)
        {
            int result = set;
            switch (GetCounterRule())
            {
                case CounterRuleMode.NoBoundaries:
                    break;
                case CounterRuleMode.BreakMinBackToMax:
                    result = MinBackToMax(result);
                    break;
                case CounterRuleMode.BreakMaxBackToMin:
                    result = MaxBackToMin(result);
                    break;
                case CounterRuleMode.BreakMinBackToCustom:
                    result = MinBackToCustom(result);
                    break;
                case CounterRuleMode.BreakMaxBackToCustom:
                    result = MaxBackToCustom(result);
                    break;
                case CounterRuleMode.MinClamp:
                    result = MinClamp(result);
                    break;
                case CounterRuleMode.MaxClamp:
                    result = MaxClamp(result);
                    break;
                case CounterRuleMode.MinMaxClamp:
                    result = MinMaxClamp(result);
                    break;
                case CounterRuleMode.BreakMinMaxLoop:
                    result = MinMaxLoop(result);
                    break;
            }
            m_CounterField.CurrentCount = result;
            m_CounterField.Executes(result);

        }
    }
}

