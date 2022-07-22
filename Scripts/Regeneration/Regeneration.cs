using System;
using System.Collections;
using UnityEngine;

public abstract class Regeneration : IBodyChangesHandler
{
    private Coroutine _task;

    public int PointsPerSecond { get; protected set; }

    public event Action<int> Changed;

    public Regeneration(int pointsPerSecond)
    {
        PointsPerSecond = pointsPerSecond;
    }

    public virtual void OnBodyChanged(Body body)
    {
        Changed?.Invoke(PointsPerSecond);
    }

    public void StopRegeneration()
    {
        if (_task != null)
            CoroutineMaster.Instance.StopCoroutine(_task);
    }

    public void StartRegeneration()
    {
        _task = CoroutineMaster.Instance.StartCoroutine(RegenerateEverySecond());
    }

    private IEnumerator RegenerateEverySecond()
    {
        var waitForSecond = new WaitForSeconds(1);
        while (true)
        {
            yield return waitForSecond;
            Regenerate();
        }
    }

    protected abstract void Regenerate();
}
