using System.Collections.Generic;

public static class GeneData
{
    public enum GeneCategory
    {
        Intelligence,
        Emotions,
        Body,
        Musculature,
        Empathy,
        Attractiveness,
        Quirks,
    }

    public static readonly Dictionary<GeneCategory, string> Alleles = new(){
        {GeneCategory.Intelligence, "Ab"},
        {GeneCategory.Emotions, "AB"},
        {GeneCategory.Body, "ABc"},
        {GeneCategory.Empathy, "ABcd"},
        {GeneCategory.Musculature, "A"},
        {GeneCategory.Attractiveness, "A"},
        {GeneCategory.Quirks, "A"},
    };

    // value of 1 means that this gene is necessary for being a human, or something
    public static readonly Dictionary<GeneCategory, float> ProbabilityToInherit = new(){
        // required genes
        {GeneCategory.Intelligence, 1f},
        {GeneCategory.Emotions, 1f},
        {GeneCategory.Body, 1f},
        // additional genes
        {GeneCategory.Empathy, 0.75f},
        {GeneCategory.Musculature, 0.75f},
        {GeneCategory.Attractiveness, 0.5f},
        {GeneCategory.Quirks, 0.25f},
    };

}