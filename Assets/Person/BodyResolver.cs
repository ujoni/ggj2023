using UnityEngine;

public class BodyResolver : DNAResolver
{
    protected override int SelectComponent(DNA dna, int count)
    {
        var result = BodyFactory.GetBodyIndex(dna, count);
        // TODO: handle mutation
        return result.index;
    }
}
