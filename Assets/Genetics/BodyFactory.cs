using static GeneData;

public static class BodyFactory
{
    public struct Result
    {
        public int index;
        public bool mutated;
    }

    public static Result GetBodyIndex(DNA dna, int count)
    {
        var primaryGene = dna.GetGeneByCategory(GeneCategory.R_Physique_5);
        var phenotypeIndex = primaryGene.GetPhenotypeIndex();
        var phenotypeCount = primaryGene.GetPhenotypeCount();

        var baseValue = phenotypeIndex * phenotypeCount;

        var secondaryGene = dna.GetGeneByCategory(GeneCategory.Charisma_2);
        var secondaryDead = secondaryGene?.IsDead() ?? false;
        var secondaryValue = !secondaryDead ? secondaryGene?.GetPhenotypeIndex() ?? 0 : 0;

        var tertiaryGene = dna.GetGeneByCategory(GeneCategory.Piety_5);
        var tertiaryDead = tertiaryGene?.IsDead() ?? false;
        var tertiaryValue = !tertiaryDead ? tertiaryGene?.GetPhenotypeIndex() ?? 0 : 0;

        return new()
        {
            index = (baseValue + secondaryValue + tertiaryValue) % count,
            mutated = secondaryDead || tertiaryDead,
        };
    }

    public static Result GetSkullIndex(DNA dna, int count)
    {
        var primaryGene = dna.GetGeneByCategory(GeneCategory.R_Logic_5);
        var baseValue = primaryGene.GetPhenotypeIndex();

        var secondaryGene = dna.GetGeneByCategory(GeneCategory.Creativity_4);
        var secondaryDead = secondaryGene?.IsDead() ?? false;
        var secondaryValue = !secondaryDead ? (secondaryGene?.GetPhenotypeIndex() ?? 0) + 1 : 1;
        return new()
        {
            index = baseValue * secondaryValue % count,
            mutated = secondaryDead,
        };
    }

    public static Result GetFaceIndex(DNA dna, int count)
    {
        var primaryGene = dna.GetGeneByCategory(GeneCategory.R_Empathy_2);
        var secondaryGene = dna.GetGeneByCategory(GeneCategory.R_Physique_5);
        var baseValue = (primaryGene.GetPhenotypeIndex() + 1) * (secondaryGene.GetPhenotypeIndex() + 1);

        var tertiaryGene = dna.GetGeneByCategory(GeneCategory.Quirks_10);
        var tertiaryDead = tertiaryGene?.IsDead() ?? false;
        var tertiaryValue = !tertiaryDead ? tertiaryGene?.GetPhenotypeIndex() ?? 0 : 0;


        return new()
        {
            index = (baseValue + tertiaryValue) % count,
            mutated = tertiaryDead,
        };
    }
}