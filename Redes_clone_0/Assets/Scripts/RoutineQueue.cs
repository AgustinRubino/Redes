using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineQueue
{
    private Func<IEnumerator, Coroutine> StartCoroutine;
    private Queue<IEnumerator> _queue;
    private Coroutine _routine;

    public RoutineQueue(Func<IEnumerator, Coroutine> startCoroutine)
    {
        _queue = new Queue<IEnumerator>();
        StartCoroutine = startCoroutine;
    }

    public void Enqueue(IEnumerator routine)
    {
        _queue.Enqueue(routine);
        if (_routine == null)
            _routine = StartCoroutine(Dequeue());
    }
    
    public IEnumerator Dequeue()
    {
        while(_queue.Count > 0) 
        {
            yield return StartCoroutine(_queue.Dequeue());    
        }
        _routine = null;
    }

    public void Clear() {
        if (_routine != null)
        {
            GameManager.Instance.StopCoroutine(_routine);
            _routine = null;
        }
        _queue.Clear();
    }
}