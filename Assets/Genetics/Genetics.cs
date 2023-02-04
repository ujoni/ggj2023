using System;
using System.Collections.Generic;
using UnityEngine;
using static GeneData;

public class GeneAllele
{
    // marks a recessive, inhibitting mutation
    public static readonly char MUTATION_MARKER = 'x';
    private static readonly int ALLELE_LEN = 2;
    private readonly char[] alleleChars = new char[GeneAllele.ALLELE_LEN];

    public GeneAllele(GeneAllele other)
    {
        this.alleleChars[0] = other.alleleChars[0];
        this.alleleChars[1] = other.alleleChars[1];
    }

    public GeneAllele(char a, char b)
    {
        this.alleleChars[0] = a;
        this.alleleChars[1] = b;
    }

    // Returns a "phenotype" (alleles affecting "visible" factors).
    // Always upper-case
    public string GetPhenotype()
    {
        string pheno = "";
        for (int i = 0; i < GeneAllele.ALLELE_LEN; i++)
        {
            char al = this.alleleChars[i];
            if (this.IsDominant(al))
            {
                pheno += al;
            }
        }

        // we have only recessive alleles
        if (pheno.Length == 0)
        {
            // we have aa or bb, which are represented by A or B
            // xx is represented as X 
            if (this.HasTwoOfTheSame()) return ("" + this.alleleChars[0]).ToUpper();
            // ab is represented as AB. if ax, then A (mutation is removed)
            return this.GetAlleleWithoutMutation().ToUpper();
        }

        // we have only one dominant allele
        if (pheno.Length == 1) return pheno;

        // we have something like AA or BB
        if (pheno.Length == 2 && this.HasTwoOfTheSame())
        {
            return "" + this.alleleChars[0];
        }

        // our dominants format a perfect phenotype
        return pheno;
    }

    public List<GeneAllele> GetCombinations(GeneAllele other)
    {
        List<GeneAllele> combinations = new();
        for (int a = 0; a < GeneAllele.ALLELE_LEN; a++)
        {
            for (int b = 0; b < GeneAllele.ALLELE_LEN; b++)
            {
                combinations.Add(new GeneAllele(this.alleleChars[a], other.alleleChars[b]));
            }
        }
        return combinations;
    }

    // replaces a recessive allele with MUTATION_MARKER
    // if no recessive alleles a are present, replaces a dominant allele
    // with MUTATION_MARKER
    // when the phenoty of the GeneAllele is X (entirely mutated), the gene is "dead"
    public void Mutate()
    {
        int resIndex = this.GetRecessiveIndex();
        int domIndex = this.GetDominantIndex();
        if (resIndex != -1)
        {
            this.alleleChars[resIndex] = GeneAllele.MUTATION_MARKER;
        }
        else if (domIndex != -1)
        {
            this.alleleChars[domIndex] = GeneAllele.MUTATION_MARKER;
        }
    }

    public bool IsDeadGeneAllele()
    {
        return this.GetPhenotype() == GeneAllele.MUTATION_MARKER.ToString().ToUpper();
    }

    private int GetRecessiveIndex()
    {
        for (int i = 0; i < GeneAllele.ALLELE_LEN; i++)
        {
            char val = this.alleleChars[i];
            if (!this.IsDominant(val) && val != GeneAllele.MUTATION_MARKER) return i;
        }
        return -1;
    }

    private int GetDominantIndex()
    {
        for (int i = 0; i < GeneAllele.ALLELE_LEN; i++)
        {
            if (!this.IsDominant(this.alleleChars[i])) return i;
        }
        return -1;
    }

    private bool IsDominant(char al)
    {
        return Char.IsUpper(al);
    }

    private bool HasTwoOfTheSame()
    {
        return this.alleleChars[0] == this.alleleChars[1];
    }

    private string GetAlleleWithoutMutation()
    {
        string allele = "";
        if (this.alleleChars[0] != GeneAllele.MUTATION_MARKER) allele += this.alleleChars[0];
        if (this.alleleChars[1] != GeneAllele.MUTATION_MARKER) allele += this.alleleChars[1];
        return allele;
    }
}

public class Gene
{
    public readonly GeneCategory geneCategory;
    public readonly GeneAllele allele;

    public Gene(GeneCategory category, GeneAllele allele)
    {
        this.geneCategory = category;
        this.allele = allele;
    }

    public string GetPhenotype()
    {
        return this.allele.GetPhenotype();
    }

    // combines two genes and returns a combined gene
    public static Gene Combine(Gene one, Gene two)
    {
        if (one != null && two != null)
        {
            var combinations = one.allele.GetCombinations(two.allele);
            var allele = combinations[UnityEngine.Random.Range(0, combinations.Count)];
            var gene = new Gene(one.geneCategory, allele);


            return gene;
        }
        if (one != null)
        {
            return new Gene(one.geneCategory, new GeneAllele(one.allele));
        }
        if (two != null)
        {
            return new Gene(two.geneCategory, new GeneAllele(two.allele));
        }
        return null;
    }

    public static bool DoesInherit(GeneCategory category)
    {
        return UnityEngine.Random.Range(0f, 1f) < ProbabilityToInherit[category];
    }

    public void Mutate()
    {
        this.allele.Mutate();
    }
}

public class DNA
{
    // The minimum number of times person has had to provide their genes 
    // in order for a mutation roll to be taken.
    // mutation chance is 
    //      count^2/sum_countributor_counts^2
    // the more people partake in the DNA, the less is the chance. 
    // the "roll" is performed for each participant that meets the limit.
    // if the roll is a success, a random Gene is chosen to be mutated.
    private static readonly int TOO_MANY_COUNTS_OF_PARENT = 3;

    private readonly string id = System.Guid.NewGuid().ToString();
    private readonly List<Gene> genes;
    // contains unique identifiers of family members and how many times
    // those have appeared within the history of this DNA.
    // artifical tracker for figuting out whether bad mutations should occure
    private readonly Dictionary<string, int> inbreedingChart;

    public DNA(IEnumerable<Gene> genes) : this(genes, null) { }

    public DNA(IEnumerable<Gene> genes, Dictionary<string, int> inbreeding)
    {
        this.genes = new(genes);
        if (inbreeding != null)
        {
            this.inbreedingChart = inbreeding;
        }
        else this.inbreedingChart = new();
    }

    public Gene GetGeneByCategory(GeneCategory cat)
    {
        return this.genes.Find(gene => gene.geneCategory == cat);
    }

    public DNA Combine(DNA other)
    {
        List<Gene> genes = new();

        foreach (GeneCategory cat in Enum.GetValues(typeof(GeneCategory)))
        {
            if (Gene.DoesInherit(cat))
            {
                var thisGene = this.GetGeneByCategory(cat);
                var otherGene = other.GetGeneByCategory(cat);
                var newGene = Gene.Combine(thisGene, otherGene);
                if (newGene != null) genes.Add(newGene);
            }
        }

        var dna = new DNA(genes, DNA.CombineInBreedingCharts(this, other));
        dna.Mutate();
        return dna;
    }

    private void Mutate()
    {
        if (this.ShouldMutate())
        {
            this.genes[UnityEngine.Random.Range(0, this.genes.Count)].Mutate();
        }
    }

    private bool ShouldMutate()
    {
        int total = 0;
        foreach (var entry in this.inbreedingChart)
        {
            total += entry.Value;
        }
        int totalSquared = total * total;

        foreach (var entry in this.inbreedingChart)
        {
            var count = entry.Value;
            if (entry.Value >= DNA.TOO_MANY_COUNTS_OF_PARENT)
            {
                var mutationProbability = (count * count) / totalSquared;
                var chance = UnityEngine.Random.Range(0, 1);
                Debug.Log("Mutation probability is " + mutationProbability + ", roll is " + chance);
                if (chance < mutationProbability) return true;
            }
        }
        return false;
    }

    private static Dictionary<string, int> CombineInBreedingCharts(DNA dna1, DNA dna2)
    {
        Dictionary<string, int> newDict = new();
        foreach (var entry in dna1.inbreedingChart)
        {
            newDict.Add(entry.Key, entry.Value);
        }
        foreach (var entry in dna2.inbreedingChart)
        {
            if (newDict.ContainsKey(entry.Key))
            {
                newDict[entry.Key] += entry.Value;
            }
            else
            {
                newDict.Add(entry.Key, entry.Value);
            }
        }
        newDict.Add(dna1.id, 1);
        newDict.Add(dna2.id, 1);
        return newDict;
    }
}