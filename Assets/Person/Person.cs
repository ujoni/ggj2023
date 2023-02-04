using UnityEngine;

public class Person : MonoBehaviour
{
    private DNA dna;
    private DNAResolver[] dnaResolvers;
    // Start is called before the first frame update
    void Start()
    {
        this.dnaResolvers = this.GetComponentsInChildren<DNAResolver>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            this.SetDNA(DNAFactory.CreateDNA(0));
        }
    }

    public DNA GetDNA()
    {
        return this.dna;
    }

    public void SetDNA(DNA dna)
    {
        Debug.Log(dna);
        if (!dna.IsViable()) {
            throw new System.Exception("This fellow is dead, yo!");
        }
        this.dna = dna;
        foreach (var resolver in this.dnaResolvers)
        {
            resolver.SetDNA(dna);
        }
    }
}
