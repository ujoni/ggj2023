
public class FaceResolver : DNAResolver
{
    protected override int SelectComponent(DNA dna, int count)
    {
        var result = BodyFactory.GetFaceIndex(dna, count);
        // TODO: handle mutation
        return result.index;
    }
}
