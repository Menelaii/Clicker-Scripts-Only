using UnityEngine;

[RequireComponent(typeof(PlayerAnimator), typeof(Combatant))]
public class PlayerView : MonoBehaviour, IBodyChangesHandler
{
    public void OnBodyChanged(Body body)
    {
        //foreach (Transform child in transform)
        //{
        //    Destroy(child.gameObject);
        //}

        //BodyViewer.CreateViews(body, transform);
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
}
