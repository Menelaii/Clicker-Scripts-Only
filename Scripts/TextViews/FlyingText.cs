using UnityEngine;

public class FlyingText : TextView
{
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private float _verticalSpeed;

    public void Update()
    {
        transform.position = transform.position + Vector3.up * _verticalSpeed * Time.deltaTime;
        Text.alpha = Mathf.Lerp(Text.alpha, 0, _fadeSpeed * Time.deltaTime);

        if (Text.alpha <= 0.2f)
            Kill();
    }
}
