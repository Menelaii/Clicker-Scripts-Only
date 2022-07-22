using System;

[Serializable]
public class BodyContainer
{
    public HeadContainer Head;
    public CorpusContainer Corpus;
    public HandsContainer Hands;
    public LegsContainer Legs;

    public Body Body => new Body((Head)Head.GetClone(), (Corpus)Corpus.GetClone(), (Hands)Hands.GetClone(), (Legs)Legs.GetClone());
}
