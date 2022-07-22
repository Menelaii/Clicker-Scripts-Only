using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class DamageView : MonoBehaviour
{
    [SerializeField] private float _lifeTimeOnGround;
    [SerializeField] private TMP_Text _text;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<Ground>(out Ground ground))
        {
            StartCoroutine(DestroyWithDelay());
        }
    }

    public void Init(int damage, DamageType type)
    {
        _text.text = damage.ToString();
        _text.color = type.Color;
    }

    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(_lifeTimeOnGround);

        Destroy(gameObject);
    }
}
