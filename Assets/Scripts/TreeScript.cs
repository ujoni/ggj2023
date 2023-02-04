using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public GameObject[] dudes;
    public GameObject root;

    public GameObject ghost;
    int filibuster;

    int running = 10;

    const float FIRSTLEVEWIDADD = 1;
    public const float DOWNADD = 4.5f;
    const float DUDELEFTING = 1;
    public const float SPOUSERIGHTING = 1;
    public const float CHILDSEPA = 1.5f;

    DudeScript chosen;

    private void Start()
    {
        dudes = GameObject.FindGameObjectsWithTag("Dude");
        foreach (GameObject g in dudes)
        {
            g.GetComponent<DudeScript>().Initialize();
        }
        root.GetComponent<DudeScript>().root = true;
        LayOut();
        
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
                    chosen = null;
                }
                return;
            }
            if (chosen != null && chosen != d){
                chosen.UnChoose();
                chosen = null;
            }
            if (d.Choose()) chosen = d;
        }
        if (btn == 1)
        {
            if (chosen == null || d == null) return;
            if (d.ghost) return;
            if (chosen == d) return;
            GameObject gho = GameObject.Instantiate(ghost);
            gho.transform.position = chosen.transform.position;
            gho.GetComponent<DudeScript>().id = running++; 
            DudeScript dud = gho.GetComponent<DudeScript>();
            dud.parents = new List<GameObject> { chosen.gameObject, d.gameObject };

            DNA A = chosen.GetComponent<Person>().GetDNA();
            print(A);
            print("kili");
            DNA B = d.GetComponent<Person>().GetDNA();
            print(B);
            print("kildi");
            print(dud.GetComponent<Person>());
            print(A.Combine(B));
            print("www");
            dud.GetComponent<Person>().SetDNA(A.Combine(B));
            print("ok");
            dud.Initialize();
            dud.InformParents();
            LayOut();
        }
    }

    public void LayOut()
    {
        dudes = GameObject.FindGameObjectsWithTag("Dude");
        foreach (GameObject g in dudes)
        {
            g.GetComponent<DudeScript>().PreInitialize();
        }
        foreach (GameObject g in dudes)
        {
            g.GetComponent<DudeScript>().Initialize();
        }
        foreach (GameObject g in dudes)
        {
            g.GetComponent<DudeScript>().InformParents();
        }

        filibuster = 0;
        CalculateUnder(root);
        PositionDudes(root, transform.position);

        foreach (GameObject d in dudes)
        {
            d.GetComponent<DudeScript>().FixLiners();
        }
    }
    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = GameObject.Find("C2ab");
            print(go);
            DudeScript d = go.GetComponent<DudeScript>();
            if (d.birthday == 7) d.birthday = 9;
            else if (d.birthday == 9) d.birthday = 7;
            print(d.id);
            print(d.birthday);
            LayOut();
        }*/

        float spe = 0.1f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.position = Camera.main.transform.position + Vector3.left*spe;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.position = Camera.main.transform.position + Vector3.right*spe;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.position = Camera.main.transform.position + Vector3.up*spe;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.position = Camera.main.transform.position + Vector3.down*spe;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.orthographicSize *= 1.01f;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            Camera.main.orthographicSize /= 1.01f;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LayOut();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            /*if (chosen == null || d == null) return;
            if (d.ghost) return;
            if (chosen == d) return;
            GameObject gho = GameObject.Instantiate(ghost);
            gho.transform.position = chosen.transform.position;
            gho.GetComponent<DudeScript>().id = running++;
            DudeScript dud = gho.GetComponent<DudeScript>();
            dud.parents = new List<GameObject> { chosen.gameObject, d.gameObject };

            DNA A = chosen.GetComponent<Person>().GetDNA();
            DNA B = d.GetComponent<Person>().GetDNA();
            dud.GetComponent<Person>().SetDNA(A.Combine(B));

            dud.Initialize();
            dud.InformParents();
            LayOut();*/
        }
    }

    public void PositionDudes(GameObject dude, Vector2 pos)
    {
        filibuster += 1;
        if (filibuster > 100)
        {
            print("recur?");
            return;
        }
        DudeScript dud = dude.GetComponent<DudeScript>();
        //print(dud.id);
        dud.SetWantPos(pos + new Vector2(dud.wantx, 0));
        if (dud.wantx != 0) print("wrong");

        for (int i = 0; i < dud.spouses.Count; i++)
        {
            if (dud.spouses[i].IsOrphan() && !dud.spouses[i].HasOtherActiveSpouse(dud))
            { // printed 1 0 then 1 1
                print("hem with " + dud.id.ToString() + " and " + i.ToString());
                dud.spouses[i].InformActiveSpouse(dud);
                dud.spouses[i].SetWantPos(pos + new Vector2(dud.spousetox[dud.spouses[i]], 0));

                // spouse is not an orphan (anymore?? not actually possible) so destroy ghost
                if (dud.spousetoghost.ContainsKey(dud.spouses[i]))
                {
                    Destroy(dud.spousetoghost[dud.spouses[i]].gameObject);
                    dud.spousetoghost.Remove(dud.spouses[i]);
                }
            }
            else
            {
                print("hullll with " + dud.id.ToString() + " and " + i.ToString());
                if (!dud.spousetoghost.ContainsKey(dud.spouses[i]))
                {
                    print("!ontian");
                    GameObject gho = GameObject.Instantiate(ghost);
                    dud.spousetoghost[dud.spouses[i]] = gho.GetComponent<DudeScript>();
                    gho.GetComponent<DudeScript>().GhostCopy(dud.spouses[i], true, "spouse");
                    gho.GetComponent<DudeScript>().SetWantPos(pos + new Vector2(dud.spousetox[dud.spouses[i]], 0));
                }
                else
                {
                    print("yep ontian");
                    GameObject gho = dud.spousetoghost[dud.spouses[i]].gameObject;
                    //dud.spousetoghost[dud.spouses[i]] = gho.GetComponent<DudeScript>();
                    gho.GetComponent<DudeScript>().GhostCopy(dud.spouses[i], false, "spouse");
                    gho.GetComponent<DudeScript>().SetWantPos(pos + new Vector2(dud.spousetox[dud.spouses[i]], 0));
                }
            }
        }

        for (int i = 0; i < dud.children.Count; i++)
        {
            if (dud.children[i].GetComponent<DudeScript>().IsPrimaryParent(dud))
            {
                //dud.children[i].GetComponent<DudeScript>().wantpos = pos +
                //    new Vector2(dud.childtox[dud.children[i].GetComponent<DudeScript>()], -DOWNADD);
                PositionDudes(dud.children[i], pos + new Vector2(dud.childtox[dud.children[i].GetComponent<DudeScript>()], -DOWNADD));

                if (dud.childtoghost.ContainsKey(dud.children[i].GetComponent<DudeScript>()))
                {
                    Destroy(dud.childtoghost[dud.children[i].GetComponent<DudeScript>()].gameObject);
                    dud.childtoghost.Remove(dud.children[i].GetComponent<DudeScript>());
                }
            }
            else
            {
                print(dud.id.ToString() + " not primary of " + dud.children[i].GetComponent<DudeScript>().id);
                if (!dud.childtoghost.ContainsKey(dud.children[i].GetComponent<DudeScript>()))
                {
                    GameObject gho = GameObject.Instantiate(ghost);
                    dud.childtoghost[dud.children[i].GetComponent<DudeScript>()] = gho.GetComponent<DudeScript>();
                    gho.GetComponent<DudeScript>().GhostCopy(dud.children[i].GetComponent<DudeScript>(), true, "child");
                    gho.GetComponent<DudeScript>().SetWantPos(pos + new Vector2(dud.childtox[dud.children[i].GetComponent<DudeScript>()], -DOWNADD));
                }
                else
                {
                    GameObject gho = dud.childtoghost[dud.children[i].GetComponent<DudeScript>()].gameObject;
                    gho.GetComponent<DudeScript>().GhostCopy(dud.children[i].GetComponent<DudeScript>(), false, "child");
                    gho.GetComponent<DudeScript>().SetWantPos(pos + new Vector2(dud.childtox[dud.children[i].GetComponent<DudeScript>()], -DOWNADD));
                }
            }
        }
       
    }

    // we 
    /*public List<float> CalculateUnderOLD(GameObject dude)
    {
        filibuster += 1;
        if (filibuster > 100)
        {
            print("recur?");
            return new List<float>();
        }

        DudeScript dud = dude.GetComponent<DudeScript>();
        List<GameObject> children = dud.children;
        if (children.Count == 0)
        {
            dud.req = new List<float> { 1 };
            return dud.req;
        }
        dud.OrderChildren();
        List<List<float>> childreqs = new List<List<float>>();
        for (int l = 0; l < dud.children.Count; l++)
        {
            childreqs.Add(CalculateUnder(dud.children[l]));
        }
              
        // find uniform separtion s that works, it's
        // s >= max sep(i, j) /| i - j | over all pairs
        // where sep(i, j) is max on all levels the sum of widths
        float s = 0;
        if (dud.children.Count >= 1)
        {
            for (int k = 0; k < dud.children.Count; k++)
            {
                for (int j = k + 1; j < dud.children.Count; j++)
                {
                    s = Mathf.Max(s, MinSep(childreqs[k], childreqs[j]) / (j - k));
                }
            }
        }
        // position of leftmost child ACTUAL HEAD
        float leftmostchildx = - s * dud.children.Count / 2;

        dud.childtox = new Dictionary<DudeScript, float>();
        for (int c = 0; c < dud.children.Count; c++)
        {
            dud.childtox[dud.children[c].GetComponent<DudeScript>()] = leftmostchildx + c * s;
        }
        //float righmostchildx = s * dud.children.Count / 2;

        // where should the upgoing lines go exactly.
        // is just avg position of children
        dud.spousetoup = new Dictionary<DudeScript, float>();

        int curri = 0;
        foreach (DudeScript spouses in dud.childrenbyspouse.Keys)
        {
            float summa = 0; // sum of children's horizontal positions
            foreach (DudeScript child in dud.childrenbyspouse[spouses]) {
                summa += leftmostchildx + s*curri;
                curri += 1;
            }
            dud.spousetoup[spouses] = summa / dud.childrenbyspouse[spouses].Count;
        }

        float kek = dud.spousetoup[dud.spouses[0]] - DUDELEFTING;
        dud.wantx = 0; //kek;
        // move everything by kek to right
        
        dud.spousetox = new Dictionary<DudeScript, float>();
        for (int k = 0; k < dud.spouses.Count; k++)
        {
            dud.spousetox[dud.spouses[k]] = dud.spousetoup[dud.spouses[k]] + SPOUSERIGHTING + kek;
        }

        //float firstlevwid = Mathf.Max(Mathf.Abs(dud.wantx), Mathf.Abs(dud.spousetox[dud.spouses[dud.spouses.Count - 1]])) + FIRSTLEVEWIDADD;
        float firstlevwid = Mathf.Abs(dud.spousetox[dud.spouses[dud.spouses.Count - 1]]) + FIRSTLEVEWIDADD;

        dud.req = new List<float> { firstlevwid };
        int i = 0;
        int checke = 0;
        while (true)
        {
            // print("kimmo");
            float max = 0;
            for (int t = 0; t < dud.children.Count; t++)
            {
                if (i < childreqs[t].Count)
                {
                    float w = childreqs[t][i]; // width of this child
                    float left = leftmostchildx + s*t - w;
                    float right = leftmostchildx + s*t + w;
                    max = Mathf.Max(max, Mathf.Abs(left) + 1);
                    max = Mathf.Max(max, Mathf.Abs(right) + 1);
                }
            }
            if (max == 0) break;
            dud.req.Add(max);
            checke += 1;
            if (checke > 100)
            {
                print("debug bd");
                break;
            }
            i += 1;
        }

        /*if (dud.id == 0)
        {
            print(dud.req.Count);
            for (int l = 0; l < dud.req.Count; l++)
            {
                print(dud.req[l]);
            }
        }*

        return dud.req;
    }*/

    public void CalculateUnder(GameObject dude)
    {
        filibuster += 1;
        if (filibuster > 100)
        {
            print("recur?");
            return; // new List<float>();
        }

        DudeScript dud = dude.GetComponent<DudeScript>();
        List<GameObject> children = dud.children;
        if (children.Count == 0)
        {
            dud.reql = new List<float> { 0.5f };
            dud.reqr = new List<float> { 0.5f };
            return;
        }
        dud.OrderChildren();
        List<List<float>> childreqls = new List<List<float>>();
        List<List<float>> childreqrs = new List<List<float>>();
        for (int l = 0; l < dud.children.Count; l++)
        {
            //dud.children[i].GetComponent<DudeScript>().FixPrimary();
            if (dud.children[l].GetComponent<DudeScript>().IsPrimaryParent(dud))
            {
                CalculateUnder(dud.children[l]);
                childreqls.Add(dud.children[l].GetComponent<DudeScript>().reql);
                childreqrs.Add(dud.children[l].GetComponent<DudeScript>().reqr);
            }
            else
            {
                childreqls.Add(new List<float> { 0.5f });
                childreqrs.Add(new List<float> { 0.5f });
            }
        }

        // find leftmost possible positions of children instead of just s
        List<float> childpositions = new List<float> { 0 };

        if (dud.children.Count >= 1)
        {
            for (int k = 1; k < dud.children.Count; k++)
            {
                float minpos = 0;
                for (int j = 0; j < k; j++)
                {
                    minpos = Mathf.Max(minpos, childpositions[j] + MinSep(childreqrs[j], childreqls[k]));
                    // s = Mathf.Max(s, MinSep(childreqrs[k], childreqls[j]) / (j - k));
                }
                childpositions.Add(minpos);
            }
        }
        if (childpositions.Count != children.Count) print("not my thing!");

        // the dude is now always 0
        dud.wantx = 0;

        /*// position of leftmost child ACTUAL HEAD
        // should put it dudelefting right since that's where we want midpoint
        float leftmostchildx = -s * dud.children.Count / 2 + DUDELEFTING;

        dud.childtox = new Dictionary<DudeScript, float>();
        for (int c = 0; c < dud.children.Count; c++)
        {
            dud.childtox[dud.children[c].GetComponent<DudeScript>()] = leftmostchildx + c * s;
        }
        //float righmostchildx = s * dud.children.Count / 2;*/



        // where should the upgoing lines go exactly.
        // is just avg position of children
        dud.spousetoup = new Dictionary<DudeScript, float>();

        int curri = 0;
        // leftmostsup is where we would put the leftmost up going
        // if we put the leftmost child at 0
        // we will actually move leftmostsup to DUDELEFTING though!
        // ^^^^^ what is this about??
        float trans = 10000;
        foreach (DudeScript spouses in dud.childrenbyspouse.Keys)
        {
            float summa = 0; // sum of children's horizontal positions
            /*foreach (DudeScript child in dud.childrenbyspouse[spouses])
            {
                summa += childpositions[curri]; //s * curri;
                curri += 1;
            }    */
            int num = dud.childrenbyspouse[spouses].Count;
            //summa += dud.childrenbyspouse[spouses][0] + dud.childrenbyspouse[spouses][ - 1];
            summa = childpositions[curri] + childpositions[curri + num-1];
            curri += num;
            dud.spousetoup[spouses] = summa / 2; //dud.childrenbyspouse[spouses].Count;
            if (trans == 10000) trans = DUDELEFTING - dud.spousetoup[spouses]; // at first upping, we set trans
            dud.spousetoup[spouses] = dud.spousetoup[spouses] + trans; //leftmostsup + DUDELEFTING;
        }
        if (Mathf.Abs(dud.spousetoup[dud.spouses[0]] - DUDELEFTING) > 0.0001f) print("lbeeeed");

        for (int c = 0; c < dud.children.Count; c++)
        {
            childpositions[c] += trans;
            dud.childtox[dud.children[c].GetComponent<DudeScript>()] = childpositions[c]; //c * s;
        }

        // now we know child positions and up positions...

        //float kek = dud.spousetoup[dud.spouses[0]] - DUDELEFTING;
        //dud.wantx = 0; //kek;
        // move everything by kek to right

        dud.spousetox = new Dictionary<DudeScript, float>();
        for (int k = 0; k < dud.spouses.Count; k++)
        {
            dud.spousetox[dud.spouses[k]] = dud.spousetoup[dud.spouses[k]] + SPOUSERIGHTING;
        }

        //float firstlevwid = Mathf.Max(Mathf.Abs(dud.wantx), Mathf.Abs(dud.spousetox[dud.spouses[dud.spouses.Count - 1]])) + FIRSTLEVEWIDADD;
        //print("killi" + dud.spouses.Count.ToString());
        //print("billi" + dud.children.Count.ToString());
        float firstlevwid = Mathf.Abs(dud.spousetox[dud.spouses[dud.spouses.Count - 1]]) + FIRSTLEVEWIDADD;

        dud.reql = new List<float> { FIRSTLEVEWIDADD };
        dud.reqr = new List<float> { firstlevwid };
        int i = 0;
        int checke = 0;
        while (true)
        {
            // print("kimmo");
            float maxl = 0;
            float maxr = 0;
            bool did = false;
            for (int t = 0; t < dud.children.Count; t++)
            {
                if (i < childreqls[t].Count)
                {
                    did = true;
                    float wl = childreqls[t][i]; // width of this child
                    float wr = childreqrs[t][i]; // width of this child
                    float left = childpositions[t] - wl;
                    float right = childpositions[t] - wr;
                    maxl = Mathf.Max(maxl, -left - 0.5f);
                    maxr = Mathf.Max(maxr, right + 0.5f);
                }
            }
            if (!did) break;
            dud.reql.Add(maxl);
            dud.reqr.Add(maxr);
            checke += 1;
            if (checke > 100)
            {
                print("debug bd");
                break;
            }
            i += 1;
        }

        /*if (dud.id == 0)
        {
            print(dud.req.Count);
            for (int l = 0; l < dud.req.Count; l++)
            {
                print(dud.req[l]);
            }
        }*/

            return; // dud.req;
    }

    float MinSep(List<float> a, List<float> b)
    {
        float max = 0; // max over levels
        for (int i = 0; i < Mathf.Min(a.Count, b.Count); i++)
        {
            max = Mathf.Max(max, a[i] + b[i] + CHILDSEPA);
        }
        return max;
    }

    // mark dudes for our use
    /*public void ComputeDescendants()
    {
        for (int i = 0; i < dudes.Length; i++)
        {
            dudes[i].GetComponent<DudeScript>().desc = false;
        }
        
    }*/
}
