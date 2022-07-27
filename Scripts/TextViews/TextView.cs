using System;
using TMPro;
using UnityEngine;

public class TextView : MonoBehaviour
{
    [SerializeField] protected TMP_Text Text;

    protected Action Kill;

    public void Init(string text, Action kill)
    {
        Text.text = text;
        Init(kill);
    }

    public void Init(Action kill)
    {
        Kill = kill;
    }
}
