using System;
using System.Collections;
using UnityEngine;

public class CoroutineMaster : MonoBehaviour
{
    public static CoroutineMaster Instance { get; private set; }

    public void SetInstance()
    {
        Instance = this;
    }

    public Coroutine InvokeWithInterval(Action action, float interval, int count)
    {
        return StartCoroutine(DoWithInterval(action, interval, count));
    }

    public Coroutine InvokeAfterDelay(Action action, float delay)
    {
        return StartCoroutine(DoAfterDelay(action, delay));
    }

    private IEnumerator DoAfterDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    private IEnumerator DoWithInterval(Action action, float interval, int count)
    {
        var waitForInterval = new WaitForSeconds(interval);
        for (int i = 0; i < count; i++)
        {
            yield return waitForInterval;
            action?.Invoke();
        }
    }
}