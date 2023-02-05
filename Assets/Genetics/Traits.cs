using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Mono.Cecil;
using TMPro.SpriteAssetUtilities;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UIElements;
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

    public override string ToString()
    {
        return name;
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

    public bool SharesGeneCategory(Trait withAnother)
    {
        var otherCategories = withAnother.requirements.Select(req => req.category).ToList();
        return requirements.Select(req => otherCategories.Contains(req.category)).Any(val => val);
    }
}

public static class Traits
{
    public static readonly List<Trait> traits = new(){
        // if you want to add only one requirement, the type must be specified.
        // with multiple reqs, the type inference works just fine

        // singular requirement traits (tied to dominant alleles only)
        // LOGIC
        new("Slow-witted", new TraitRequirement(GeneCategory.R_Logic_5, "A")),
        new("Smart", new TraitRequirement(GeneCategory.R_Logic_5, "b")),
        new("Quick-witted", new TraitRequirement(GeneCategory.R_Logic_5, "bc")),
        new("Logical", new TraitRequirement(GeneCategory.R_Logic_5, "cb")),
        new("Irrational", new TraitRequirement(GeneCategory.R_Logic_5, "c")),
        // EMPATHY
        new("Caring", new TraitRequirement(GeneCategory.R_Empathy_2, "D")),
        new("Indifferent", new TraitRequirement(GeneCategory.R_Empathy_2, "e")),
        // PHYSIQUE
        new("Weak", new TraitRequirement(GeneCategory.R_Physique_5, "F")),
        new("Strong", new TraitRequirement(GeneCategory.R_Physique_5, "FH")),
        new("Agile", new TraitRequirement(GeneCategory.R_Physique_5, "g")),
        new("Fast", new TraitRequirement(GeneCategory.R_Physique_5, "H")),
        new("Slow", new TraitRequirement(GeneCategory.R_Physique_5, "HF")),
        // CREATIVITY + x
        new("Creative", new TraitRequirement(GeneCategory.Creativity_4, "I")),
        new("Absent-minded", new TraitRequirement(GeneCategory.Creativity_4, "IJ")),
        new("Industrious", new TraitRequirement(GeneCategory.Creativity_4, "JI")),
        new("Lazy", new TraitRequirement(GeneCategory.Creativity_4, "J")),
        new("Sociopath", new TraitRequirement(GeneCategory.Creativity_4, "x")),
        // CHARISMA + x
        new("Extrovert", new TraitRequirement(GeneCategory.Charisma_2, "K")),
        new("Introvert", new TraitRequirement(GeneCategory.Charisma_2, "l")),
        new("Hideous", new TraitRequirement(GeneCategory.Charisma_2, "x")),
        // PIETY + x
        new("Single-minded", new TraitRequirement(GeneCategory.Piety_5, "M")),
        new("Fathful", new TraitRequirement(GeneCategory.Piety_5, "n")),
        new("Fervent", new TraitRequirement(GeneCategory.Piety_5, "np")),
        new("Untrusting", new TraitRequirement(GeneCategory.Piety_5, "pn")),
        new("Loyal", new TraitRequirement(GeneCategory.Piety_5, "p")),
        new("Delusional", new TraitRequirement(GeneCategory.Piety_5, "x")),
        // QUIRKS + x 
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
        // Singularity + x
        new("Unique", new TraitRequirement(GeneCategory.Singularity_1, "u")),
        new("Outlandish", new TraitRequirement(GeneCategory.Singularity_1, "x")),

        
        // complex traits between vital traits (logic, empathy, physique)
        new("Manipulative", new(GeneCategory.R_Logic_5, "b"), new(GeneCategory.R_Empathy_2, "D")),
        new("People-person", new(GeneCategory.R_Logic_5, "bc"), new(GeneCategory.R_Empathy_2, "e")),
        new("Competitive", new(GeneCategory.R_Logic_5, "cb"), new(GeneCategory.R_Physique_5, "FH")),
        new("Ferocious", new(GeneCategory.R_Logic_5, "c"), new(GeneCategory.R_Physique_5, "H")),
        new("Approachable", new(GeneCategory.R_Empathy_2, ""), new(GeneCategory.R_Physique_5, "")),
        new("Forgettable", new(GeneCategory.R_Empathy_2, ""), new(GeneCategory.R_Physique_5, "")),

    };

    public static List<Trait> GetTraits(DNA dna)
    {
        return traits.Where(trait => trait.PresentIn(dna)).ToList();
    }

    public static List<Trait> GetRandomGoalTraits(int count = 2)
    {
        var realCount = count < 1 ? 2 : count;
        var traitsWithTwoOrMoreReqs = traits.Where(trait => trait.requirements.Count > 1).ToList();
        traitsWithTwoOrMoreReqs.Sort((trait1, trait2) => trait2.GetValue() - trait1.GetValue());

        List<Trait> ret = new();
        while (ret.Count < realCount)
        {
            // favors traits closer to the beginning
            var index = (int)(Mathf.Pow(UnityEngine.Random.Range(0f, 1f), 2) * traitsWithTwoOrMoreReqs.Count);
            var selected = traitsWithTwoOrMoreReqs[index];
            if (ret.Any(trait => trait.SharesGeneCategory(selected))) continue;
            ret.Add(selected);
        }
        return ret;
    }
    public static string ToAttributeText(string end, List<Trait> traits)
    {
        string s = "";
        foreach (Trait t in traits)
        {
            s += t.ToString() + "\n";
        }
        if (end == "") return s;
        return s + "\n(" + end + ")";
    }
}