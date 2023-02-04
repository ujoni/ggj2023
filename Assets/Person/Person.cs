using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

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
            this.SetDNA(new DNA(new()));
        }
    }

    public DNA GetDNA()
    {
        return this.dna;
    }

    public void SetDNA(DNA dna)
    {
        this.dna = dna;
        foreach (var resolver in this.dnaResolvers)
        {
            resolver.SetDNA(dna);
        }
    }
}
