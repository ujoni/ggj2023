using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SounderScript : MonoBehaviour
{
    public void PlaySound(string name)
    {
        GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load(name));
    }
}
