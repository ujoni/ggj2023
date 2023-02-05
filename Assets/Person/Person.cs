using System;
using UnityEngine;

public class Person : MonoBehaviour
{
    public bool generateStartingDNA = false;
    private DNA dna;
    private DNAResolver[] dnaResolvers;

    public string dnatext; // debug

    // Start is called before the first frame update
    void Awake()
    {
        //print(gameObject.name);
        this.dnaResolvers = this.GetComponentsInChildren<DNAResolver>();
        //print("a");
        if (this.generateStartingDNA)
        {
           //print("b");
            DNA dn = DNAFactory.CreateDNA(0, true);
            //print("c");
            this.SetDNA(dn);
            //print("d");
        }

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyUp(KeyCode.Return))
        {
            this.SetDNA(DNAFactory.CreateDNA(0));
        }
        */

        SetDNA(GetDNA());
        dnatext = GetDNA().ToString();
        //print("fvi");
        //print("set" + dnatext);
        
    }

    public DNA GetDNA()
    {
        return this.dna;
    }

    public void SetDNA(DNA dna)
    {
        //print("mallu!");
        // Debug.Log("SetDNA: " + dna.id + " " + dna);
        if (!dna.IsViable())
        {
            throw new System.Exception("This fellow is dead, yo!");
        }
        //print("killu");
        this.dna = dna;
        foreach (var resolver in this.dnaResolvers)
        {
            //print("viulu" + resolver.ToString());
            resolver.SetDNA(dna);
        }
        //print("paklu");
    }

    public void Copy(Person other)
    {
        dna = other.dna;
    }

    public void FixedUpdate()
    {
       
    }
}
