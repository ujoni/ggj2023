using System.Collections.Generic;

public static class GeneData
{
    public enum GeneCategory
    {
        Logic,
        Empathy,
        Physique,
        Creativity,
        Charisma,
        Piety,
        Quirks,
    }

    public static readonly Dictionary<GeneCategory, string> Alleles = new(){
        {GeneCategory.Logic, "Ab"},
        {GeneCategory.Empathy, "ABcd"},
        {GeneCategory.Physique, "ABc"},
        {GeneCategory.Creativity, "AB"},
        {GeneCategory.Charisma, "A"},
        {GeneCategory.Piety, "A"},
        {GeneCategory.Quirks, "A"},
    };

    // value of 1 means that this gene is necessary for being a human, or something
    public static readonly Dictionary<GeneCategory, float> ProbabilityToInherit = new(){
        // required genes
        {GeneCategory.Logic, 1f},
        {GeneCategory.Empathy, 1f},
        {GeneCategory.Physique, 1f},
        // additional genes
        {GeneCategory.Charisma, 0.75f},
        {GeneCategory.Creativity, 0.75f},
        {GeneCategory.Piety, 0.5f},
        {GeneCategory.Quirks, 0.25f},
    };

}