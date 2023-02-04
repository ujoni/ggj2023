using System;
using System.Collections.Generic;
using System.Linq;
using static GeneData;

public static class DNAFactory
{

    // rareGeneThreshold
    // 0: includes all genes
    // 1: includes only required genes
    public static DNA CreateDNA(float rareGeneThreshold, bool randomize = false)
    {
        List<GeneCategory> selectedGenes = new();
        foreach (var entry in ProbabilityToInherit)
        {
            if (entry.Value >= rareGeneThreshold)
            {
                // if randomize is true, we check whether our customer lucks out and gets the gene.
                // since required genes have probability of 1, they are always selected
                if (!randomize || UnityEngine.Random.Range(0f, 1f) <= entry.Value)
                    selectedGenes.Add(entry.Key);
            }
        }

        return new(selectedGenes.Select(cat => DNAFactory.CreateGeneFromCategory(cat)));
    }

    private static Gene CreateGeneFromCategory(GeneCategory category)
    {
        var allele = DNAFactory.CreateAlleleFromCategory(category);
        return new Gene(category, allele);
    }

    private static GeneAllele CreateAlleleFromCategory(GeneCategory category)
    {
        var alleleChars = Alleles[category].ToCharArray();
        var len = alleleChars.Length;
        return new GeneAllele(alleleChars[UnityEngine.Random.Range(0, len)], alleleChars[UnityEngine.Random.Range(0, len)]);
    }
}