using System.Collections;
using UnityEngine;

namespace SangoUtils.Patchs_YooAsset.Utils
{
    internal class CoroutineDriver : Singleton<CoroutineDriver>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        internal static Coroutine Run(IEnumerator target)
        {
            return CoroutineDriver.Instance.StartCoroutine(target);
        }
    }
}