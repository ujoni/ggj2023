using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : DNAResolver
{
    protected override int SelectComponent(DNA dna, int count)
    {
        var logic = dna.GetGeneByCategory(GeneData.GeneCategory.R_Logic_5);
        var empathy = dna.GetGeneByCategory(GeneData.GeneCategory.R_Empathy_2);
        var physique = dna.GetGeneByCategory(GeneData.GeneCategory.R_Physique_5);
        var total = (logic.GetPhenotypeIndex() + 1)
            * (empathy.GetPhenotypeIndex() + 1)
            * (physique.GetPhenotypeIndex() + 1);
        return total % count;
    }
}
