using System;

public class AttackButtonListener
{
    private int _maxClicksPerSecond;

    public event Action Clicked;

    public AttackButtonListener(int maxClicksPerSecond)
    {
        _maxClicksPerSecond = maxClicksPerSecond;
    }

    public int ClicksPerSecond { get; private set; }

    public void OnClick()
    {
        if (ClicksPerSecond + 1 > _maxClicksPerSecond)
            return;

        ClicksPerSecond++;
        CoroutineMaster.Instance.InvokeAfterDelay(() => ClicksPerSecond--, 1);

        Clicked?.Invoke();
    }
}
