using System.ComponentModel;
using UnityEngine;

public class SkullResolver : DNAResolver
{
    protected override int SelectComponent(DNA dna, int count)
    {
        var result = BodyFactory.GetSkullIndex(dna, count);
        // TODO: handle mutation
        return result.index;
    }
}
