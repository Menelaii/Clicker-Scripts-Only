using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private int _value;
    [SerializeField] private float _lifeTimeOnGround;

    public event Action<Coin> Destroyed;

    public int Value => _value;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent<Ground>(out Ground ground))
        {
            StartCoroutine(DestroyWithDelay());
        }
    }

    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(_lifeTimeOnGround);

        Destroyed?.Invoke(this);

        Destroy(gameObject);
    }
}
