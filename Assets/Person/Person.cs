using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Person : MonoBehaviour
{
    private DNA dna;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public DNA GetDNA()
    {
        return this.dna;
    }

    public void SetDNA(DNA dna)
    {
        this.dna = dna;
        this.CreateCharacter();
    }

    private void CreateCharacter()
    {
        this.CreateFrame();
        this.CreateBody();
        this.CreateHead();
        this.CreateStats();
    }

    private void CreateStats()
    {
        throw new NotImplementedException();
    }

    private void CreateHead()
    {
        throw new NotImplementedException();
    }

    private void CreateBody()
    {
        throw new NotImplementedException();
    }

    private void CreateFrame()
    {
        throw new NotImplementedException();
    }
}
