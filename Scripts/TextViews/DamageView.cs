using System.Collections;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class DamageView : TextView
{
    [SerializeField] private float _lifeTimeOnGround;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<Ground>(out Ground ground))
        {
            StartCoroutine(DestroyWithDelay());
        }
    }

    public void Init(string text, DamageType type, Action kill)
    {
        Init(text, kill);
        Text.color = type.Color;
    }

    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(_lifeTimeOnGround);

        Kill();
    }
}
