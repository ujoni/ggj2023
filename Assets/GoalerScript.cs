using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalerScript : MonoBehaviour
{

    public GameObject nam;
    //public TMP_Text times;
    public GameObject attribs;
    public GameObject CheckMark;

    public List<int> goalcomplexities;

    public List<AbstractGoal> gos;
    public List<bool> done;
    public List<GameObject> nams;
    public List<GameObject> attribses;

    private void Awake()
    {
        int running = 0;
        gos = new List<AbstractGoal>();
        done = new List<bool>();

        nams = new List<GameObject>();
        attribses = new List<GameObject>();
        foreach (int q in goalcomplexities)
        {
            gos.Add(new AbstractGoal(q));
            done.Add(false);
            print("go" + gos.Count.ToString());
            GameObject namcopy = GameObject.Instantiate(nam, GameObject.Find("Canvas").transform);
            namcopy.transform.position = transform.position + running*Vector3.down;
            running += 160;
            namcopy.GetComponent<TextMeshProUGUI>().text = gos[gos.Count-1].name;
            nams.Add(namcopy);

            GameObject attribscopy = GameObject.Instantiate(attribs, GameObject.Find("Canvas").transform);
            attribscopy.transform.position = transform.position + running * Vector3.down;
            running += 25 * gos[gos.Count - 1].traits.Count + 1;
            attribscopy.GetComponent<TextMeshProUGUI>().text = Traits.ToAttributeText("", gos[gos.Count - 1].traits);
            attribses.Add(attribscopy);
        }
    }

    private void Update()
    {
        if (Random.Range(0, 1000) == 0)
        {
            GameObject[] dudes = GameObject.FindGameObjectsWithTag("Dude");
            foreach (GameObject g in dudes)
            {
                if (g.GetComponent<DudeScript>().ghost || g.GetComponent<DudeScript>().usedforpost || !g.GetComponent<DudeScript>().family) continue;
                List<Trait> ts = Traits.GetTraits(g.GetComponent<Person>().GetDNA());
                for (int j = 0; j < gos.Count; j++)
                {
                    if (done[j]) continue;
                    if (Traits.HasAllTraits(g.GetComponent<Person>().GetDNA(), gos[j].traits) /* traits good */)
                    {
                        g.GetComponent<DudeScript>().usedforpost = true;
                        done[j] = true;
                        GameObject cm = GameObject.Instantiate(CheckMark, nams[j].transform.position, Quaternion.identity,
                            GameObject.Find("Canvas").transform);
                        
                        break;
                    }
                }
            }
        }
    } 
}

public class AbstractGoal
{
    public string name;
    public List<Trait> traits;
    public AbstractGoal(int num)
    {
        string[] choices = new string[0];
        if (num == 1)
        {
             choices = new string[] { "Some peasant" };
        }
        else if(num == 2)
        {
            choices = new string[] { "Vesa-Matti Loiri" };
        }
        else if (num == 3)
        {
            choices = new string[] { "President of US" };
        }
        else if (num == 4)
        {
            choices = new string[] { "Jesus in the flesh", "Zimbabwe Minister of State for Housing and Local Government" };
        }
        name = choices[Random.Range(0, choices.Length)];
        traits = Traits.GetRandomGoalTraits(num);
    }
}