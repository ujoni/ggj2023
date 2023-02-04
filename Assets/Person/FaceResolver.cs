using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceResolver : DNAResolver
{
    protected override int SelectComponent(DNA dna, int count)
    {
        return Random.Range(0, count);
    }
}
