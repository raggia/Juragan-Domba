using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
namespace Rush
{
    [System.Serializable]
    public class CoroutineField
    {
        [SerializeField, TextArea]
        private string m_Id;
        private IEnumerator m_IEnumerator;
        private Coroutine m_Coroutinery;
        public CoroutineField(string id, IEnumerator ienumerator)
        {
            m_Id = id;
            m_IEnumerator = ienumerator;
        }
        public string Id => m_Id;

        public IEnumerator IEnumeratory => m_IEnumerator;
        public Coroutine Coroutinery => m_Coroutinery;

        public void BeginCoroutine(MonoBehaviour parent, bool busy)
        {
            m_Coroutinery = parent.StartCoroutine(BeginCoroutining(busy));
        }
        public Coroutine BeginCoroutineReturn(MonoBehaviour parent, bool busy)
        {
            m_Coroutinery = parent.StartCoroutine(BeginCoroutining(busy));
            return m_Coroutinery;
        }

        private IEnumerator BeginCoroutining(bool busy)
        {
            yield return m_IEnumerator;
            //yield return new WaitForEndOfFrame();
            CoroutineUtility.EndCoroutine(m_Id, busy);
        }
    }
    public static class CoroutineUtility 
    {
        public static void BeginCoroutine(string coroutineId, IEnumerator enumerator, bool busy = false)
        {
            CoroutineSingleton.Instance.BeginCoroutine(coroutineId, enumerator, busy);
        }
        public static Coroutine BeginCoroutineReturn(string coroutineId, IEnumerator enumerator, bool busy = false)
        {
            return CoroutineSingleton.Instance.BeginCoroutineReturn(coroutineId, enumerator, busy);
        }

        public static void EndCoroutine(string coroutineId, bool busy = false)
        {
            CoroutineSingleton.Instance.EndCoroutine(coroutineId, busy);
        }
    }
    public class CoroutineSingleton : Singleton<CoroutineSingleton>
    {
        [SerializeField, ReadOnly]
        private bool m_Busy = false;

        [Tooltip("Use to see certain coroutine that still active")]
        [SerializeField, ReadOnly]
        private List<CoroutineField> m_OrdinaryCoroutineFields = new();

        [Tooltip("If Busy list is stored, maybe you can play some loading and close it when list is empty")]
        [SerializeField, ReadOnly]
        private List<CoroutineField> m_BusyCoroutineFields = new();

        [SerializeField]
        private UnityEvent m_OnBusyCoroutineProgressStart = new();
        [SerializeField]
        private UnityEvent<int> m_OnBusyCoroutineProgressCount = new();

        [SerializeField]
        private UnityEvent m_OnBusyCoroutineProgressDone = new();

        private CoroutineField GetCoroutineInfo(string coroutineId, bool busy)
        {
            CoroutineField c = null;
            if (busy)
            {
                c = GetBusyCoroutineInfo(coroutineId);
            }
            else
            {
                c = GetOrdinaryCoroutineInfo(coroutineId);
            }
            return c;
        }
        private CoroutineField GetOrdinaryCoroutineInfo(string coroutineId)
        {
            CoroutineField c = m_OrdinaryCoroutineFields.Find(x => x.Id == coroutineId);
            return c;
        }
        private CoroutineField GetBusyCoroutineInfo(string coroutineId)
        {
            CoroutineField c = m_BusyCoroutineFields.Find(x => x.Id == coroutineId);
            return c;
        }
        private void BusyActiveCoroutineCount()
        {
            Debug.Log($"Active Coroutine already {m_BusyCoroutineFields.Count} progressing");
            
            if (m_BusyCoroutineFields.Count > 0 )
            {
                if (m_Busy == false)
                {
                    OnBusyCoroutineProgressStart();
                }
                OnBusyCoroutineProgressCountInvoked(m_BusyCoroutineFields.Count);
            }

            if ( m_BusyCoroutineFields.Count <= 0 )
            {
                OnBusyCoroutineProgressDoneInvoked();
            }
        }
        public void BeginCoroutine(string coroutineId, IEnumerator enumerator, bool busy = false)
        {
            if (HasCoroutine(coroutineId, busy))
            {
                EndCoroutineInternal(coroutineId, busy);
            }
            else
            {
                
                CoroutineField newCoroutineField = new(coroutineId, enumerator);
                AddCoroutine(newCoroutineField, busy);
                GetCoroutineInfo(coroutineId, busy).BeginCoroutine(this, busy);
            }
        }
        public Coroutine BeginCoroutineReturn(string coroutineId, IEnumerator enumerator, bool busy = false)
        {
            Coroutine coroutine = null;
            if (HasCoroutine(coroutineId, busy))
            {
                EndCoroutineInternal(coroutineId, busy);
            }
            else
            {

                CoroutineField newCoroutineField = new(coroutineId, enumerator);
                AddCoroutine(newCoroutineField, busy);
                coroutine = GetCoroutineInfo(coroutineId, busy).BeginCoroutineReturn(this, busy);
            }
            return coroutine;
        }
        public void EndCoroutine(string coroutineId, bool busy)
        {
            EndCoroutineInternal(coroutineId, busy);
        }
        private void EndCoroutineInternal(string coroutineId, bool busy)
        {
            if (HasCoroutine(coroutineId, busy))
            {
                StopCoroutine(GetCoroutineInfo(coroutineId, busy).Coroutinery);
                RemoveCoroutine(coroutineId, busy);
            }
        }

        private bool HasCoroutine(string coroutineId, bool busy)
        {
            if (busy)
            {
                return m_BusyCoroutineFields.Contains(GetBusyCoroutineInfo(coroutineId));
            }
            else
            {
                return m_OrdinaryCoroutineFields.Contains(GetOrdinaryCoroutineInfo(coroutineId));
            }
        }
        private void AddCoroutine(CoroutineField coroutine, bool busy)
        {
            if (busy)
            {
                m_BusyCoroutineFields.Add(coroutine);
            }
            else
            {
                m_OrdinaryCoroutineFields.Add(coroutine);
            }
            Debug.Log($"Add {coroutine.Id} to Progress");
            BusyActiveCoroutineCount();
        }
        private void RemoveCoroutine(string coroutineId, bool busy)
        {
            if (busy)
            {
                m_BusyCoroutineFields.Remove(GetBusyCoroutineInfo(coroutineId));
            }
            else
            {
                m_OrdinaryCoroutineFields.Remove(GetOrdinaryCoroutineInfo(coroutineId));
            }
            Debug.Log($"Remove {coroutineId} from Progress");
            BusyActiveCoroutineCount();
        }
        private void OnBusyCoroutineProgressStart()
        {
            m_OnBusyCoroutineProgressStart?.Invoke();
            m_Busy = true;
        }
        private void OnBusyCoroutineProgressCountInvoked(int count)
        {
            m_OnBusyCoroutineProgressCount?.Invoke(count);
        }
        private void OnBusyCoroutineProgressDoneInvoked()
        {
            m_OnBusyCoroutineProgressDone?.Invoke();
            m_Busy = false;
        }
    }
}

