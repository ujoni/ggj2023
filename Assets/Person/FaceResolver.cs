using UnityEngine;

public class FaceResolver : DNAResolver
{
    protected override int SelectComponent(DNA dna, int count)
    {
        return BodyFactory.GetFaceIndex(dna, count);
    }
}
