using UnityEngine;
using TMPro;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void OnGoldChanged(int gold)
    {
        _text.text = $"{gold}";
    }
}
