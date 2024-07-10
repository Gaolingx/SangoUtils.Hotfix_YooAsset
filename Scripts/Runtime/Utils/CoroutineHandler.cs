using System.Collections;
using UnityEngine.Events;

namespace SangoUtils.Patchs_YooAsset.Utils
{
    internal static class CoroutineExtensions
    {
        internal static CoroutineHandler Start(this IEnumerator enumerator)
        {
            CoroutineHandler handler = new CoroutineHandler(enumerator);
            handler.Start();
            return handler;
        }
    }

    internal class CoroutineHandler
    {
        internal IEnumerator Coroutine { get; private set; } = null;

        internal bool Paused { get; private set; } = false;

        internal bool Running { get; private set; } = false;

        internal bool Stopped { get; private set; } = false;

        internal class FinishedHandler : UnityEvent<bool> { }

        private readonly FinishedHandler OnCompleted = new FinishedHandler();

        internal CoroutineHandler(IEnumerator coroutine)
        {
            Coroutine = coroutine;
        }

        internal void Pause()
        {
            Paused = true;
        }

        internal void Resume()
        {
            Paused = false;
        }

        internal void Start()
        {
            if (null != Coroutine)
            {
                Running = true;
                CoroutineDriver.Run(CallWrapper());
            }
            else
            {
                UnityEngine.Debug.LogError("Coroutine is Null.");
            }
        }

        internal void Stop()
        {
            Stopped = true;
            Running = false;
        }

        private void Complete()
        {
            OnCompleted?.Invoke(Stopped);
            OnCompleted?.RemoveAllListeners();
            Coroutine = null;
        }

        internal CoroutineHandler OnComplete(UnityAction<bool> action)
        {
            OnCompleted.AddListener(action);
            return this;
        }

        private IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator enumerator = Coroutine;
            while (Running)
            {
                if (Paused)
                {
                    yield return null;
                }
                else
                {
                    if (enumerator != null && enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                    else
                    {
                        Running = false;
                    }
                }
            }
            Complete();
        }
    }
}