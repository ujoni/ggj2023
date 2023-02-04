using System.Collections.Generic;

public static class GeneData
{
    public enum GeneCategory
    {
        Smarts,
        Looks,
        Psyche,
        Body,
    }

    public static readonly Dictionary<GeneCategory, string> Alleles = new(){
        {GeneCategory.Smarts, "ABc"},
        {GeneCategory.Looks, "Ab"},
        {GeneCategory.Psyche, "AB"},
        {GeneCategory.Body, "ABcd"},

    };

    // value of 1 means that this gene is necessary for being a human, or something
    public static readonly Dictionary<GeneCategory, float> ProbabilityToInherit = new(){
        {GeneCategory.Smarts, 1f},
        {GeneCategory.Looks, 1f},
        {GeneCategory.Psyche, 1f},
        {GeneCategory.Body, 1f},
    };

}