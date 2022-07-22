using UnityEngine;

[CreateAssetMenu(fileName = "New Corpus", menuName = "Custom/BodyParts/Corpus")]
public class CorpusContainer : BodyPartContainer
{
    public Corpus Corpus;

    public override BodyPart Item => Corpus;
}
