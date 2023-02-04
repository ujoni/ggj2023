using System.Threading;
using Unity.Burst;
using static GeneData;

public static class BodyFactory
{
    public static int GetBodyIndex(DNA dna, int count)
    {
        string phenotype = dna.GetPhenotypeByCategory(GeneCategory.Physique);
        return 0;
    }
}