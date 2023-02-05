using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public abstract class DNAResolver : MonoBehaviour
{
    public void SetDNA(DNA dna)
    {
        //print("kipi");
        var spriteResolver = this.GetComponent<SpriteResolver>();
        //print("kipi");
        if (spriteResolver == null)
        {
            //print("no sr");
            throw new System.Exception("WHAT THE DUCK, where is SpriteResolver?!");
        }
        var category = spriteResolver.GetCategory();
        //print("cate");
        var labels = new List<string>(spriteResolver.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(category));
        //print("alb");
        //print(category);
        //print(labels.Count);
        //print(dna);
        var index = this.SelectComponent(dna, labels.Count);
        //print("ind");
        spriteResolver.SetCategoryAndLabel(category, labels[index % labels.Count]);
        //print("catelab");
        spriteResolver.ResolveSpriteToSpriteRenderer();
        //print("srhin");
    }

    // Returns a number from [0, count)
    protected abstract int SelectComponent(DNA dna, int count);
}