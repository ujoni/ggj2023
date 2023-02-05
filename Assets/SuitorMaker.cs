using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitorMaker : MonoBehaviour
{
    public List<GameObject> positions;
    public List<GameObject> suitors;
    public GameObject dude;
    public void MakeSuitors(float p)
    {
        for (int i = 0; i < suitors.Count; i++)
        {
            //print(i.ToString()+ "suito");
            //print(suitors[i].GetComponent<DudeScript>().TooOldToBeSuitor());
            // no longer here...
            if (suitors[i] != null && (suitors[i].layer == LayerMask.NameToLayer("Default") ||
                suitors[i].GetComponent<DudeScript>().TooOldToBeSuitor()))
            {
                if (suitors[i].GetComponent<DudeScript>().TooOldToBeSuitor())
                {
                    Destroy(suitors[i]);
                }
                suitors[i] = null;
            }
        }
        for (int i = 0; i < suitors.Count; i++)
        {
            if (suitors[i] == null)
            {
                if (Random.Range(0f, 1f) < p)
                {
                    suitors[i] = GameObject.Instantiate(dude);
                    suitors[i].transform.position = positions[i].transform.position;
                    suitors[i].GetComponent<DudeScript>().ToLayer(LayerMask.NameToLayer("Suitor"));
                    suitors[i].GetComponent<DudeScript>().SetWantPos(positions[i].transform.position);
                    suitors[i].GetComponent<Person>().SetDNA(DNAFactory.CreateDNA(0, true));
                    suitors[i].GetComponent<DudeScript>().birthday =
                        GameObject.Find("Cadence").GetComponent<MouseListenerScript>().year - Random.Range(5, 20);
                }
            }
        }
    }
}
