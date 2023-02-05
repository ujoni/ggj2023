using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using static GeneData;
public class TraitRequirement
{
    public readonly GeneCategory category;
    public readonly string phenotype;

    public TraitRequirement(GeneCategory category, string phenotype)
    {
        this.category = category;
        this.phenotype = phenotype;
    }

    public bool IsFilled(DNA dna)
    {
        return (dna.GetGeneByCategory(this.category)?.GetPhenotype() ?? "") == this.phenotype;
    }
}

public class Trait
{
    public readonly string name;

    public readonly List<TraitRequirement> requirements;

    public Trait(string name, params TraitRequirement[] requirements)
    {
        this.name = name;
        this.requirements = new(requirements);
    }

    public bool PresentIn(DNA dna)
    {
        return this.requirements
            // convert each requirement to a boolean - either it is filled or it is not
            .Select(req => req.IsFilled(dna))
            // aggregate result
            .Aggregate((final, cur) => final && cur);
    }

    public int GetValue()
    {
        // more requirements, and the less likely the gene of the requirement is to inherit
        // the bigger the value
        return (int)(this.requirements.Count + this.requirements
            .Select(req => 1f - ProbabilityToInherit[req.category])
            .Aggregate((total, cur) => total + cur));
    }
}

public static class Traits
{
    private static bool sorted = false;
    public static readonly List<Trait> traits = new(){
        // if you want to add only one requirement, the type must be specified.
        // with multiple reqs, the type inference works just fine

        // singular requirement traits
        // LOGIC
        new("Slow-witted", new TraitRequirement(GeneCategory.R_Logic_5, "A")),
        new("Smart", new TraitRequirement(GeneCategory.R_Logic_5, "b")),
        // EMPATHY
        new("Caring", new TraitRequirement(GeneCategory.R_Empathy_2, "D")),
        new("Indifferent", new TraitRequirement(GeneCategory.R_Empathy_2, "e")),
        // PHYSIQUE
        new("Weak", new TraitRequirement(GeneCategory.R_Physique_5, "F")),
        new("Strong", new TraitRequirement(GeneCategory.R_Physique_5, "FH")),
        new("Muscular", new TraitRequirement(GeneCategory.R_Physique_5, "g")),
        // CREATIVITY
        new("Creative", new TraitRequirement(GeneCategory.Creativity_4, "I")),
        new("Absent-minded", new TraitRequirement(GeneCategory.Creativity_4, "IJ")),
        new("Sociopath", new TraitRequirement(GeneCategory.Creativity_4, "x")),
        // CHARISMA
        new("Extrovert", new TraitRequirement(GeneCategory.Charisma_2, "K")),
        new("Introvert", new TraitRequirement(GeneCategory.Charisma_2, "l")),
        new("Hideous", new TraitRequirement(GeneCategory.Charisma_2, "x")),
        // PIETY
        new("Single-minded", new TraitRequirement(GeneCategory.Piety_5, "M")),
        new("Fathful", new TraitRequirement(GeneCategory.Piety_5, "n")),
        new("Fervent", new TraitRequirement(GeneCategory.Piety_5, "p")),
        new("Delusional", new TraitRequirement(GeneCategory.Piety_5, "x")),
        // QUIRKS
        new("Fairly average", new TraitRequirement(GeneCategory.Quirks_10, "Q")),
        new("Large-nosed", new TraitRequirement(GeneCategory.Quirks_10, "r")),
        new("Fidgety", new TraitRequirement(GeneCategory.Quirks_10, "rs")),
        new("Leery", new TraitRequirement(GeneCategory.Quirks_10, "rt")),
        new("Very short", new TraitRequirement(GeneCategory.Quirks_10, "sr")),
        new("Large", new TraitRequirement(GeneCategory.Quirks_10, "s")),
        new("Big-boned", new TraitRequirement(GeneCategory.Quirks_10, "st")),
        new("Smelly", new TraitRequirement(GeneCategory.Quirks_10, "tr")),
        new("Long-armed", new TraitRequirement(GeneCategory.Quirks_10, "ts")),
        new("Lost", new TraitRequirement(GeneCategory.Quirks_10, "t")),
        new("Two-headed", new TraitRequirement(GeneCategory.Quirks_10, "x")),
        
        // complex traits

    };

    public static List<Trait> GetTraits(DNA dna)
    {
        return traits.Where(trait => trait.PresentIn(dna)).ToList();
    }

    public static List<Trait> GetRandomGoalTraits(int count = 3)
    {
        var realCount = count < 1 ? 3 : count;
        if (!sorted)
        {
            sorted = true;
            traits.Sort((trait1, trait2) => trait2.GetValue() - trait1.GetValue());
        }
        List<Trait> ret = new();
        for (int i = 0; i < realCount; i++)
        {
            // favors traits closer to the beginning
            var index = (int)(Mathf.Pow(UnityEngine.Random.Range(0f, 1f), 2) * traits.Count);
            ret.Add(traits[index]);
        }
        return ret;
    }
}