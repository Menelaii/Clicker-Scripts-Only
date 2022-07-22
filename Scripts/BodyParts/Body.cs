using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Body : ISerializationCallbackReceiver
{
    private static readonly int HEAD_INDEX = 0;
    private static readonly int CORPUS_INDEX = 1;
    private static readonly int HANDS_INDEX = 2;
    private static readonly int LEGS_INDEX = 3;

    [HideInInspector] [SerializeField]
    private List<SerializedData> _serializedBodyParts;
    [HideInInspector] [SerializeField] 
    private SetId _enabledBonusId;

    private List<BodyPart> _bodyParts;

    public event Action<Body> BodyChanged;

    public Body(Head head, Corpus corpus, Hands hands, Legs legs)
    {
        _bodyParts = new List<BodyPart>()
        {
            head,
            corpus,
            hands,
            legs
        };

        CheckForBonuses();
    }

    public Head Head => (Head)_bodyParts[HEAD_INDEX];
    public Corpus Corpus => (Corpus)_bodyParts[CORPUS_INDEX];
    public Hands Hands => (Hands)_bodyParts[HANDS_INDEX];
    public Legs Legs => (Legs)_bodyParts[LEGS_INDEX];
    public BodyPart LastReplacedPart { get; private set; }

    public BodyPart Replace(BodyPart bodyPart)
    {
        int replacedIndex = _bodyParts.FindIndex(x => x.GetType() == bodyPart.GetType());
        LastReplacedPart = _bodyParts[replacedIndex];
        _bodyParts[replacedIndex] = bodyPart;

        OnBodyChanged();
        BodyChanged?.Invoke(this);

        return LastReplacedPart;
    }

    public void OnBodyPartUpgraded()
    {
        BodyChanged?.Invoke(this);
    }

    public BodyPart GetBodyPart(BodyPartType type)
    {
        return _bodyParts.Find(x => x.GetType().Name == type.ToString());
    }

    private void OnBodyChanged()
    {
        CheckForCreatedSpellEnities();
        CheckForBonuses();
    }

    private void CheckForCreatedSpellEnities()
    {
        (LastReplacedPart as ISpellCollection)?.DestroyCreatedSpellEntities();
    }

    private void CheckForBonuses()
    {
        SetId bonusId = GetBonusId();
        if (bonusId == _enabledBonusId)
            return;

        DisableCurrentBonus();

        EnableBonus(bonusId);
    }

    private SetId GetBonusId()
    {
        SetId setId = _bodyParts[0].SetId;
        foreach (var bodypart in _bodyParts)
        {
            if (bodypart.SetId != setId)
                return SetId.None;
        }

        return setId;
    }

    private void EnableBonus(SetId setId)
    {
        if (setId == SetId.None)
            return;

        SetBonus bonus = SetBonusLibrary.Instance.GetElement(setId);
        if (bonus != null)
        {
            bonus.EnableOn(this);
            _enabledBonusId = setId;
        }
        else
        {
            _enabledBonusId = SetId.None;
        }
    }

    private void DisableCurrentBonus()
    {
        if (_enabledBonusId != SetId.None)
        {
            SetBonusLibrary
                .Instance
                .GetElement(_enabledBonusId)
                .DisableOn(this);

            _enabledBonusId = SetId.None;
        }
    }

    public void OnBeforeSerialize()
    {
        _serializedBodyParts = AbstractObjectsSerializer.Serialize(_bodyParts);
    }

    public void OnAfterDeserialize()
    {
        _bodyParts = AbstractObjectsSerializer.Deserialize<BodyPart>(_serializedBodyParts);
    }
}
