using System.Collections.Generic;
using System.Linq;
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

    public bool IsFilled(DNA dna) {
        return (dna.GetGeneByCategory(this.category)?.GetPhenotype() ?? "") == this.phenotype;
    }
}

public class Trait
{
    public readonly string name;

    public readonly List<TraitRequirement> requirements;

    public Trait(string name, IEnumerable<TraitRequirement> requirements)
    {
        this.name = name;
        this.requirements = new(requirements);
    }

    public bool FillsRequirements(DNA dna)
    {
        return this.requirements
            // convert each requirement to a boolean - either it is filled or it is not
            .Select(req => req.IsFilled(dna))
            // aggregate result
            .Aggregate((final, cur) => final && cur);
    }
}