using System;
using System.Collections.Generic;
using System.Linq;
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
        return GeneAllele.GetPhenotypeFromPair(this.alleleChars[0], this.alleleChars[1]);
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
        return this.GetPhenotype() == GeneAllele.MUTATION_MARKER.ToString();
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

    public static List<String> GetAllPhenotypes(string alleles)
    {
        var arr = alleles.ToCharArray();
        List<string> phenotypes = new();
        foreach (char a in arr)
        {
            foreach (char b in arr)
            {
                phenotypes.Add(GeneAllele.GetPhenotypeFromPair(a, b));
            }
        }
        return phenotypes;
    }

    private static string GetPhenotypeFromPair(char a, char b)
    {
        string pheno = "";
        if (Char.ToUpper(a) == a) pheno += a;
        if (Char.ToUpper(b) == b) pheno += b;

        // we have only recessive alleles
        if (pheno.Length == 0)
        {
            // we have aa or bb, which are represented by A or B
            // xx is represented as X 
            if (a == b) return "" + a;
            // ab is represented as AB. if ax, then A (mutation is removed)
            string ret = "";
            if (a != GeneAllele.MUTATION_MARKER) ret += a;
            if (b != GeneAllele.MUTATION_MARKER) ret += b;
            return ret;
        }

        // we have only one dominant allele
        if (pheno.Length == 1) return pheno;

        // we have something like AA or BB
        if (pheno.Length == 2 && a == b)
        {
            return "" + a;
        }

        // our dominants format a perfect phenotype
        return pheno;
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

    public bool IsDead()
    {
        return this.allele.IsDeadGeneAllele();
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

    public int GetPhenotypeIndex()
    {
        var phenotype = this.allele.GetPhenotype();
        var allPhenos = GeneAllele.GetAllPhenotypes(Alleles[this.geneCategory]);
        return allPhenos.FindIndex(val => val == phenotype);
    }

    public int GetPhenotypeCount()
    {
        return GeneAllele.GetAllPhenotypes(Alleles[this.geneCategory]).Count;
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
    // how many generations the act of inheriting a dna is remembered
    // if a child inherits dna from both parent,
    private static readonly int GENERATIONAL_MEMORY = 4;

    // The minimum accumulation of GENERATIONAL_MEMORY per ancestor
    // in order for a mutation roll to be taken.
    // mutation chance is 
    //      count^2/(GENERATIONAL_MEMORY + count)^2
    //      
    private static readonly int TOO_MANY_COUNTS_OF_PARENT = 5;

    public readonly string id = System.Guid.NewGuid().ToString();
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

    public bool IsViable()
    {
        return !this.GetGeneByCategory(GeneCategory.R_Logic_5).IsDead()
            && !this.GetGeneByCategory(GeneCategory.R_Empathy_2).IsDead()
            && !this.GetGeneByCategory(GeneCategory.R_Physique_5).IsDead();
    }

    public DNA Combine(DNA other)
    {
        if (other == this) throw new Exception("Bad boi! This is me!");
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
        foreach (var entry in this.inbreedingChart)
        {
            var count = entry.Value;
            if (count >= DNA.TOO_MANY_COUNTS_OF_PARENT)
            {
                var mutationProbability = Mathf.Pow(count, 2) / Mathf.Pow(count + DNA.GENERATIONAL_MEMORY, 2);
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
        // combine old values
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

        // reduce existing values by 1
        foreach (var key in newDict.Keys) newDict[key] -= 1;
        var toRemove = newDict.Where(kvp => kvp.Value == 0).ToList();
        foreach (var entry in toRemove) newDict.Remove(entry.Key);

        if (newDict.ContainsKey(dna1.id)) newDict[dna1.id] += DNA.GENERATIONAL_MEMORY;
        else newDict.Add(dna1.id, DNA.GENERATIONAL_MEMORY);
        if (newDict.ContainsKey(dna2.id)) newDict[dna2.id] += DNA.GENERATIONAL_MEMORY;
        else newDict.Add(dna2.id, DNA.GENERATIONAL_MEMORY);
        return newDict;
    }

    public string GetPhenotypeByCategory(GeneCategory category)
    {
        var gene = this.GetGeneByCategory(category);
        if (gene == null) return null;
        return gene.GetPhenotype();
    }

    public override string ToString()
    {
        return this.genes.Select(gene => gene.GetPhenotype()).Aggregate((whole, val) => whole + "-" + val);
    }

    // or should this interpretation be done elsewhere?
    public string ToAttributeText()
    {
        return "I do not have a clue, but the dna is " + ToString();
    }
}