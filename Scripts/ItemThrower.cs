using UnityEngine;

public class ItemThrower : MonoBehaviour
{
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _startSpeed;

    [HideInInspector][SerializeField]
    private float _maxAngleInRad, _minAngleInRad;

    private void OnValidate()
    {
        _maxAngleInRad = (90 + _maxAngle) * Mathf.Deg2Rad;
        _minAngleInRad = (90 - _maxAngle) * Mathf.Deg2Rad;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 1);

        Vector2 max = new Vector2(Mathf.Cos(_maxAngleInRad), Mathf.Sin(_maxAngleInRad));
        Vector2 min = new Vector2(Mathf.Cos(_minAngleInRad), Mathf.Sin(_minAngleInRad));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, max);
        Gizmos.DrawLine(transform.position, min);
    }

    public void Throw(Rigidbody2D rigidbody)
    {
        rigidbody.AddForce(GetThrowDirection() * _startSpeed, ForceMode2D.Impulse);
    }

    public void Throw(Rigidbody2D[] rigidbodies)
    {
        foreach (var rb in rigidbodies)
        {
            Throw(rb);
        }
    }

    public void Throw(Rigidbody2D rigidbody, Vector3 direction)
    {
        rigidbody.AddForce(direction.normalized * _startSpeed, ForceMode2D.Impulse);
    }

    private Vector2 GetThrowDirection()
    {
        float angle = Random.Range(_minAngleInRad, _maxAngleInRad);

        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
