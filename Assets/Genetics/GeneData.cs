using System.Collections.Generic;

public static class GeneData
{
    // name stats with R, if it is a required gene
    // names contain phenotype permutation count
    public enum GeneCategory
    {
        R_Logic_5,
        R_Empathy_2,
        R_Physique_5,
        Creativity_4,
        Charisma_2,
        Piety_5,
        Quirks_10,
        // this is a boolean gene for adding 1 for some calculations xD
        Singularity_1
    }

    public static readonly Dictionary<GeneCategory, string> Alleles = new(){
        // phenotypes: 5
        {GeneCategory.R_Logic_5, "Abc"},
        // phenotypes: 2
        {GeneCategory.R_Empathy_2, "De"},
        // phenotypes: 5
        {GeneCategory.R_Physique_5, "FgH"},
        // phenotypes: 4
        {GeneCategory.Creativity_4, "IJ"},
        // phenotypes: 2
        {GeneCategory.Charisma_2, "Kl"},
        // phenotypes: 5
        {GeneCategory.Piety_5, "Mnp"},
        // phenotypes: 10
        {GeneCategory.Quirks_10, "Qrst"},
        // phenotypes: 1
        {GeneCategory.Singularity_1, "U"},
    };

    // value of 1 means that this gene is necessary for being a human, or something
    public static readonly Dictionary<GeneCategory, float> ProbabilityToInherit = new(){
        // required genes
        {GeneCategory.R_Logic_5, 1f},
        {GeneCategory.R_Empathy_2, 1f},
        {GeneCategory.R_Physique_5, 1f},
        // additional genes
        {GeneCategory.Charisma_2, 0.75f},
        {GeneCategory.Creativity_4, 0.75f},
        {GeneCategory.Piety_5, 0.5f},
        {GeneCategory.Quirks_10, 0.25f},
        {GeneCategory.Singularity_1, 0.25f},
    };

}