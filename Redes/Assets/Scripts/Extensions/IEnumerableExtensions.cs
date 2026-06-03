using System;
using System.Collections;
using UnityEngine;

namespace RoloExtensions
{
    public static class IEnumerableExtensions
    {
        public static Coroutine StartCoroutine(this IEnumerator e, MonoBehaviour go = null)
        {
            if (go == null) go = new MonoBehaviour();

            return go.StartCoroutine(e);
        }
    }
}
