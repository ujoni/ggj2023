using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseListenerScript : MonoBehaviour
{

    public int year;

    public int YearsPerGenerationMin;
    public int YearsPerGenerationMax;
    public int MatingsPerGeneration;
    public float suitorprob;

    TMP_Text yearshower;

    public GameObject ghost;
    public TreeScript tree;

    int running;

    DudeScript chosen;

    GameObject blob;

    public void Awake()
    {
        running = 10;
        tree = GameObject.Find("Tree").GetComponent<TreeScript>();
        ghost = tree.ghost;
        blob = GameObject.Find("Blob");
        //print("set yershow");
        

        yearshower = GameObject.Find("year").GetComponent<TMP_Text> ();
        yearshower.text = year.ToString();

        if (MatingsPerGeneration <= 0) print("matings pls");
        if (YearsPerGenerationMin < 0 || YearsPerGenerationMin > YearsPerGenerationMax || YearsPerGenerationMax == 0) print("yers pls");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TimePass();
        }
    }
    public void WasClicked(DudeScript d, int btn)
    {

        // print(d.id);
        // left button chooses
        if (btn == 0)
        {

            if (d == null)
            {
                if (chosen != null)
                {
                    chosen.UnChoose();
                    SetChosen(null);
                }
                return;
            }
            if (chosen != null && chosen != d)
            {
                chosen.UnChoose();
                SetChosen(null);
                
            }
            if (d.ghost) d = d.real;
            if (d.Choose())
            {
                SetChosen(d);
            }
        }
        if (btn == 1)
        {
            if (chosen == null || d == null) return;
            if (d.ghost) d = d.real;
            if (chosen.ghost) chosen = chosen.real;
            if (chosen == d) return;
            if (!chosen.family && !d.family) return;
            if (!d.IsAlive || !chosen.IsAlive) return;
            if (!d.CanReproduce || !chosen.CanReproduce) return;
            if (MatingsPerGeneration == 1)
            {
                Mate(d);
            }
            else
            {
                print("not implemented");
            }
        }
    }

    string ExplainAge(DudeScript d)
    {
        string ag = d.age.ToString();
        if (!d.IsAlive) ag += " (dead)";
        else if (!d.CanReproduce && d.young) ag += " (young)";
        else if (!d.CanReproduce) ag += " (old)";
        else if (d.CanReproduce) ag += "";
        return ag;
    }

    void SetChosen(DudeScript d)
    {
        if (d != null)
        {
            GameObject.Find("chosen").GetComponent<TMP_Text>().text = d.GetComponent<Person>().GetDNA().ToString();
            GameObject.Find("chosenage").GetComponent<TMP_Text>().text = ExplainAge(d);
            GameObject.Find("attribs").GetComponent<TMP_Text>().text = d.GetComponent<Person>().GetDNA().ToAttributeText();
            //Camera.main.GetComponent<SounderScript>().PlaySound("Sounders/di2");
        }
        else
        {
            GameObject.Find("chosen").GetComponent<TMP_Text>().text = "(nobody in focus)";
            GameObject.Find("chosenage").GetComponent<TMP_Text>().text = "";
            GameObject.Find("attribs").GetComponent<TMP_Text>().text = "";
            Camera.main.GetComponent<SounderScript>().PlaySound("Sounders/di");
        }
        chosen = d;
    }

    void Mate(DudeScript d)
    {
        GameObject gho = GameObject.Instantiate(ghost);
        gho.transform.position = chosen.transform.position;
        gho.GetComponent<DudeScript>().id = running++;
        DudeScript dud = gho.GetComponent<DudeScript>();
        dud.parents = new List<GameObject> { chosen.gameObject, d.gameObject };
        dud.birthday = year;

        chosen.ToLayer(LayerMask.NameToLayer("Default"));
        d.ToLayer(LayerMask.NameToLayer("Default"));


        DNA A = chosen.GetComponent<Person>().GetDNA();
        // print(A);
        // print("kili");
        DNA B = d.GetComponent<Person>().GetDNA();
        /*print(B);
        print("kildi");
        print(dud.GetComponent<Person>());
        print(A.Combine(B));
        print("www");*/
        dud.GetComponent<Person>().SetDNA(A); //.Combine(B));
        //print("ok");
        dud.Initialize();
        dud.InformParents();
        tree.LayOut();

        Camera.main.GetComponent<SounderScript>().PlaySound("Sounders/mate");

        TimePass();
        
    }
    private void OnDrawGizmos()
    {
        if (chosen)
        {
            List<float> reql = chosen.GetComponent<DudeScript>().reql;
            List<float> reqr = chosen.GetComponent<DudeScript>().reqr;
            if (reql.Count != reqr.Count) print("ummm whatsup??");

            for (int i = 0; i < reql.Count; i++)
            {
                Gizmos.DrawCube(chosen.transform.position + Vector3.left * reql[i] + Vector3.down * TreeScript.DOWNADD * i, new Vector3(0.3f, 0.3f, 0.3f));
                Gizmos.DrawCube(chosen.transform.position + Vector3.right * reqr[i] + Vector3.down * TreeScript.DOWNADD*i, new Vector3(0.3f, 0.3f, 0.3f));
            }
        }
    }

    public void BackClick()
    {
        print("baackcli");
    }
    public void TimePass()
    {
        GameObject.Find("SuitorsCamEtc").GetComponent<SuitorMaker>().MakeSuitors(suitorprob);
        SetYear(year + Random.Range(YearsPerGenerationMin, YearsPerGenerationMax + 1));
        
    }

    private void SetYear(int ye)
    {
        year = ye;
        yearshower.text = year.ToString();
        bool someonealive = false;
        foreach (GameObject d in GameObject.FindGameObjectsWithTag("Dude"))
        {
            d.GetComponent<DudeScript>().TimePassed();
            if (d.GetComponent<DudeScript>().family && d.GetComponent<DudeScript>().IsAlive)
                someonealive = true;
        }
        if (!someonealive)
        {
            GameObject.Find("GameOver").GetComponent<TMP_Text>().fontSize = 20;
        }
    }
}
