using System.ComponentModel;
using UnityEngine;

public class SkullResolver : DNAResolver
{
    protected override int SelectComponent(DNA dna, int count)
    {
        return BodyFactory.GetSkullIndex(dna, count);
    }
}
