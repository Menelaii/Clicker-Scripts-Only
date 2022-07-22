using UnityEngine;

public static class BodyViewer
{
    public static void CreateViews(Body body, Transform parent)
    {
        var root = InstantiateBodyPartView(body.Corpus.Label, parent.position, parent) as BodyPartRootView;

        InstantiateBodyPartView(body.Head.Label, root.Head.position, root.transform);
        InstantiateBodyPartView(body.Hands.Label, root.Hands.position, root.transform);
        InstantiateBodyPartView(body.Legs.Label, root.Legs.position, root.transform);
    }

    private static BodyPartView InstantiateBodyPartView(string label, Vector3 position, Transform parent)
    {
        var gameObj = Instantiate("BodyParts/" + label, position);
        gameObj.transform.SetParent(parent);

        return gameObj.GetComponent<BodyPartView>();
    }

    private static GameObject Instantiate(string path, Vector3 position)
    {
        var prefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab, position, Quaternion.identity);
    }
}
