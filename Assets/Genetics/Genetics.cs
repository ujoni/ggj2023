using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor;

public enum GeneCategory {
    Smarts,
    Looks,
    Psyche,
    Body,
}

public class GeneAllele {
    // marks a recessive, inhibitting mutation
    public static readonly char MUTATION_MARKER = 'x';
    private static readonly int ALLELE_LEN = 2;
    private readonly char[] alleleChars = new char[GeneAllele.ALLELE_LEN];

    public GeneAllele(GeneAllele other) {
        this.alleleChars[0] = other.alleleChars[0];
        this.alleleChars[1] = other.alleleChars[1];
    }

    public GeneAllele(char a, char b) {
        this.alleleChars[0] = a;
        this.alleleChars[1] = b;
    }

    // Returns a "phenotype" (alleles affecting "visible" factors).
    // Always upper-case
    public string GetPhenotype() {
        string pheno = "";
        for (int i = 0; i < GeneAllele.ALLELE_LEN; i++) {
            char al = this.alleleChars[i];
            if (this.IsDominant(al)) {
                pheno += al;
            }
        }

        // we have only recessive alleles
        if (pheno.Length == 0) {
            // we have aa or bb, which are represented by A or B
            // xx is represented as X 
            if (this.HasTwoOfTheSame()) return ("" + this.alleleChars[0]).ToUpper();
            // ab is represented as AB. if ax, then A (mutation is removed)
            return this.GetAlleleWithoutMutation().ToUpper();
        }

        // we have only one dominant allele
        if (pheno.Length == 1) return pheno;
        
        // we have something like AA or BB
        if (pheno.Length == 2 && this.HasTwoOfTheSame()) {
            return "" + this.alleleChars[0];
        }

        // our dominants format a perfect phenotype
        return pheno;
    }

    public List<GeneAllele> GetCombinations(GeneAllele other) {
        List<GeneAllele> combinations = new();
        for (int a = 0; a < GeneAllele.ALLELE_LEN; a++) {
            for (int b = 0; b < GeneAllele.ALLELE_LEN; b++) {
                combinations.Add(new GeneAllele(this.alleleChars[a], other.alleleChars[b]));
            }
        }
        return combinations;
    }

    // replaces a recessive allele with MUTATION_MARKER
    // if no recessive alleles a are present, replaces a dominant allele
    // with MUTATION_MARKER
    // when the phenoty of the GeneAllele is X (entirely mutated), the gene is "dead"
    public void Mutate() {
        int resIndex = this.GetRecessiveIndex();
        int domIndex = this.GetDominantIndex();
        if (resIndex != -1) {
            this.alleleChars[resIndex] = GeneAllele.MUTATION_MARKER;
        } else if (domIndex != -1) {
            this.alleleChars[domIndex] = GeneAllele.MUTATION_MARKER;
        }
    }

    public bool IsDeadGeneAllele() {
        return this.GetPhenotype() == GeneAllele.MUTATION_MARKER.ToString().ToUpper();
    }

    private int GetRecessiveIndex() {
        for (int i = 0; i < GeneAllele.ALLELE_LEN; i++) {
            char val = this.alleleChars[i];
            if (!this.IsDominant(val) && val != GeneAllele.MUTATION_MARKER) return i;
        }
        return -1;
    }

    private int GetDominantIndex() {
        for (int i = 0; i < GeneAllele.ALLELE_LEN; i++) {
            if (!this.IsDominant(this.alleleChars[i])) return i;
        }
        return -1;
    }

    private bool IsDominant(char al) {
        return Char.IsUpper(al);
    }

    private bool HasTwoOfTheSame() {
        return this.alleleChars[0] == this.alleleChars[1];
    }

    private string GetAlleleWithoutMutation() {
        string allele = "";
        if (this.alleleChars[0] != GeneAllele.MUTATION_MARKER) allele += this.alleleChars[0];
        if (this.alleleChars[1] != GeneAllele.MUTATION_MARKER) allele += this.alleleChars[1];
        return allele;
    }
}

public class Gene
{
    public static readonly Dictionary<GeneCategory, string> alleles = new (){
        {GeneCategory.Smarts, "ABc"},
        {GeneCategory.Looks, "Ab"},
        {GeneCategory.Psyche, "AB"},
        {GeneCategory.Body, "ABcd"},

    };

    public GeneCategory geneCategory;
    public GeneAllele allele;

    public string GetPhenotype() {
        return this.allele.GetPhenotype();
    }

    public Gene CombineWith(Gene other) {
        if (other == null) return new Gene {
            geneCategory = this.geneCategory,
            allele = new GeneAllele(this.allele),
        };

        var combinations = this.allele.GetCombinations(other.allele);
        var allele = combinations[UnityEngine.Random.Range(0, combinations.Count)];
        var gene = new Gene
        {
            geneCategory = this.geneCategory,
            allele = allele
        };
        return gene;
    }
}

public class DNA {
    private readonly List<Gene> genes;

    public DNA(List<Gene> genes) {
        this.genes = genes;
    }

    public static DNA Combine(DNA dna1, DNA dna2) {
        // TODO
        return dna1;
    } 
}