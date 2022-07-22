using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Image _notAvailablePanel;
    
    private float _cooldown;
    private float _secondsLeft;

    public event Action Closed;

    private void Update()
    {
        _timerText.text = _secondsLeft.ToString("0.0");
        _secondsLeft -= Time.deltaTime;
        _notAvailablePanel.fillAmount = _secondsLeft / _cooldown;

        if(_secondsLeft <= 0)
        {
            Close();
        }
    }

    public void Open(float cooldown)
    {
        gameObject.SetActive(true);
        _cooldown = cooldown;
        _secondsLeft = cooldown;

        _notAvailablePanel.gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);

        _notAvailablePanel.fillAmount = 1;
        _notAvailablePanel.gameObject.SetActive(false);

        Closed?.Invoke();
    }
}
