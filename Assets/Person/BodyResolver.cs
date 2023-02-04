using UnityEngine;

public class BodyResolver : DNAResolver
{
    protected override int SelectComponent(DNA dna, int count)
    {
        return BodyFactory.GetBodyIndex(dna, count);
    }
}
