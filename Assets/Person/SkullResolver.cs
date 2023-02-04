using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullResolver : DNAResolver
{
    protected override int SelectComponent(DNA dna, int count)
    {
        return Random.Range(0, count);
    }
}
