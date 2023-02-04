using System.Threading;
using Unity.Burst;
using Unity.VisualScripting;
using static GeneData;

public static class BodyFactory
{
    public static int GetBodyIndex(DNA dna, int count)
    {
        var primaryGene = dna.GetGeneByCategory(GeneCategory.R_Physique_5);
        var phenotypeIndex = primaryGene.GetPhenotypeIndex();
        var phenotypeCount = primaryGene.GetPhenotypeCount();

        var baseValue = phenotypeIndex * phenotypeCount;

        var secondaryValue = dna.GetGeneByCategory(GeneCategory.Charisma_2)?.GetPhenotypeIndex() ?? 0;
        var tertiaryValue = dna.GetGeneByCategory(GeneCategory.Piety_5)?.GetPhenotypeIndex() ?? 0;

        return (baseValue + secondaryValue + tertiaryValue) % count;
    }

    public static int GetSkullIndex(DNA dna, int count)
    {
        var primaryGene = dna.GetGeneByCategory(GeneCategory.R_Logic_5);
        var baseValue = primaryGene.GetPhenotypeIndex();
        var secondaryValue = (dna.GetGeneByCategory(GeneCategory.Creativity_4)?.GetPhenotypeIndex() ?? 0) + 1;
        return baseValue * secondaryValue % count;
    }

    public static int GetFaceIndex(DNA dna, int count)
    {
        var primaryGene = dna.GetGeneByCategory(GeneCategory.R_Empathy_2);
        var secondaryGene = dna.GetGeneByCategory(GeneCategory.R_Physique_5);

        var baseValue = (primaryGene.GetPhenotypeIndex() + 1) * (secondaryGene.GetPhenotypeIndex() + 1);


        return (baseValue + (dna.GetGeneByCategory(GeneCategory.Quirks_10)?.GetPhenotypeIndex() ?? 0)) % count;
    }
}