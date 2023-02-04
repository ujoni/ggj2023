using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public abstract class DNAResolver : MonoBehaviour
{
    public void SetDNA(DNA dna)
    {
        var spriteResolver = this.GetComponent<SpriteResolver>();
        if (spriteResolver == null) throw new System.Exception("WHAT THE DUCK, where is SpriteResolver?!");
        var category = spriteResolver.GetCategory();
        var labels = new List<string>(spriteResolver.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(category));
        var index = this.SelectComponent(dna, labels.Count);
        spriteResolver.SetCategoryAndLabel(category, labels[index % labels.Count]);
        spriteResolver.ResolveSpriteToSpriteRenderer();
    }

    // Returns a number from [0, count)
    protected abstract int SelectComponent(DNA dna, int count);
}