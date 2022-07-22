using System.Collections.Generic;
using System;

public static class StatChangesValidator
{
    private const int COMMON_STATS_Min_Index = 50;

    private static Dictionary<BodyPartType, Range> _ranges
        = new Dictionary<BodyPartType, Range>()
        {
            [BodyPartType.Head] = new Range(0, 1),
            [BodyPartType.Corpus] = new Range(2, 4),
            [BodyPartType.Hands] = new Range(5, 6),
            [BodyPartType.Legs] = new Range(7, 9),
        };

    public static void Validate(BodyPartType bodyPart, BodyPartStatType stat)
    {
        if (IsValidPair(bodyPart, stat) == false)
            throw new InvalidPairException();
    }

    public static bool IsValidPair(BodyPartType bodyPart, BodyPartStatType stat)
    {
        Range indexRange = _ranges[bodyPart];
        int statIndex = (int)stat;

        return indexRange.IsValueInRange(statIndex) || statIndex >= COMMON_STATS_Min_Index;
    }

    private struct Range
    {
        public int Min;
        public int Max;

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public bool IsValueInRange(int value) =>
            value <= Max
            && value >= Min;
    }

    private class InvalidPairException : Exception
    {
        private const string MESSAGE = "Change enums idiota >:|";

        public InvalidPairException() : base(MESSAGE) { }
    }
}