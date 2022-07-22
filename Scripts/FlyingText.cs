using TMPro;
using UnityEngine;

public class FlyingText : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private TMP_Text _text;

    public void Update()
    {
        transform.position = transform.position + Vector3.up * _verticalSpeed * Time.deltaTime;
        _text.alpha = Mathf.Lerp(_text.alpha, 0, _fadeSpeed * Time.deltaTime);

        if (_text.alpha <= 0.2f)
            Destroy(gameObject);
    }

    public void Init(string text)
    {
        _text.text = text;
    }
}
