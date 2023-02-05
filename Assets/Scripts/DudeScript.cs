using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DudeScript : MonoBehaviour
{
    public int id;
    public int birthday;
    public TreeScript tree;

    public bool root;
    public bool family;


    public bool ghost;
    public DudeScript real;
    string ghosttype;

    public int age;

    public const int THRESHOLD1 = 15;
    public const int THRESHOLD2 = 55;
    public const int THRESHOLD3 = 70;


    // the actual parents if exist
    public List<GameObject> parents;
    // automatically computed
    public List<GameObject> children;

    Color facecolor;

    Vector2 wantpos;

    public bool CanReproduce;
    public bool IsAlive;
    public bool young;

    DudeScript activespouse;

    // technical stuff
    public List<DudeScript> spouses;
    public Dictionary<DudeScript, List<DudeScript>> childrenbyspouse;
    public Dictionary<DudeScript, float> spousetoup = new Dictionary<DudeScript, float>();
    public Dictionary<DudeScript, float> spousetox = new Dictionary<DudeScript, float>();
    public Dictionary<DudeScript, DudeScript> spousetoghost;
    public Dictionary<DudeScript, float> childtox = new Dictionary<DudeScript, float>();
    public Dictionary<DudeScript, DudeScript> childtoghost;

    public GameObject liner;
    public Dictionary<DudeScript, LinerScript> spousetoliner;

    // when dups, we make ghost versions of wives and kids
    GameObject[] ghostwives;
    GameObject[] ghostkids;

    public float wantx; // position where we horizontally want based on KIDS

    // this is just something we use when putting in the tree
    public bool desc;

    // public int primaryparent; now just first is always primary
    // space requirements, what distance on each side do we need
    public List<float> reql;
    public List<float> reqr;

    bool initialized;
    MouseListenerScript listener;

    public void Awake()
    {

        Initialize();
        
    }

    public void PreInitialize()
    {
        children = new List<GameObject>();
    }

    public void TimePassed()
    {
        
        age = listener.year - birthday;
        //if (id == 12 && ghost) print("lipi" + age.ToString());
        //if (id == 12 && ghost)  transform.Find("age").GetComponent<TextMeshPro>().text = "limerick";
        //Debug.Break();

        // if (ghost) age = real.age; // somehow disappears so yeah
        string ag = age.ToString();
        if (!IsAlive) ag = "";
        transform.Find("age").GetComponent<TextMeshPro>().text = ag;
        young = false;
        if (age >= THRESHOLD1) CanReproduce = true;
        else young = true;

        if (age >= THRESHOLD2) CanReproduce = false;
        if (age >= THRESHOLD3) IsAlive = false;
        //print(GetComponent<Person>());
        //print(GetComponent<Person>().GetDNA());
        if (GetComponent<Person>().GetDNA() != null && !GetComponent<Person>().GetDNA().IsViable()) IsAlive = false;
        if (!IsAlive)
        {
            transform.Find("Frame").GetComponent<SpriteRenderer>().color = Color.white;
            transform.Find("Background").GetComponent<SpriteRenderer>().color = Color.black;
        }
        else if (CanReproduce)
        {
            transform.Find("Frame").GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            transform.Find("Frame").GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public bool TooOldToBeSuitor()
    {
        return !IsAlive || (!CanReproduce && !young);
    }

    public void Initialize()
    {
        if (initialized) return;
        
        IsAlive = true;
        
        CanReproduce = false;

        family = false;
        listener = GameObject.Find("Cadence").GetComponent<MouseListenerScript>();
        if (birthday == 0) birthday = listener.year;

        transform.Find("age").GetComponent<TextMeshPro>().text = (listener.year - birthday).ToString();
        // print("don");
        initialized = true;

        // facecolor = transform.Find("Face").GetComponent<SpriteRenderer>().color;
        SetName("-"); // GetComponent<Person>().GetDNA().ToString());
        if (parents == null) parents = new List<GameObject>();
        FixPrimary();
        
        ghost = false;
        if (parents == null)
        {
            parents = new List<GameObject>();
        }
        wantpos = transform.position;
        // primaryparent = 0;
        tree = GameObject.Find("Tree").GetComponent<TreeScript>();
        InformParents();

        spouses = new List<DudeScript>();
        childrenbyspouse = new Dictionary<DudeScript, List<DudeScript>>();
        spousetoup = new Dictionary<DudeScript, float>();
        spousetox = new Dictionary<DudeScript, float>();
        spousetoghost = new Dictionary<DudeScript, DudeScript>();
        childtox = new Dictionary<DudeScript, float>();
        childtoghost = new Dictionary<DudeScript, DudeScript>();
        spousetoliner = new Dictionary<DudeScript, LinerScript>();

        TimePassed();
    }

    public void InformParents()
    {
        for (int i = 0; i < parents.Count; i++)
        {
            parents[i].GetComponent<DudeScript>().AddChild(gameObject);
        }
    }

    public void FixPrimary()
    {
        //print("fixing " + id.ToString());
        if (parents.Count > 0)
        {
            //if (id == 33) print("now fix");
            // parent is not orphan
            bool pphas = !parents[0].GetComponent<DudeScript>().IsOrphan(); // parents[0].GetComponent<DudeScript>().parents.Count > 0;
            //if (id == 33) print(pphas);
            //if (id == 5) print(pphas);
            //if (id == 5) print(parents[1 - primaryparent].GetComponent<DudeScript>().parents.Count > 0);
            //if (id == 5) print(parents[primaryparent].GetComponent<DudeScript>().id);
            // if parent is orphan and other parent is not orphan, then other parent is better primary
            // if (id == 33) print(!parents[0].GetComponent<DudeScript>().IsOrphan());
            if (!pphas && !parents[1].GetComponent<DudeScript>().IsOrphan())
            {
                //primaryparent = 1 - primaryparent;
                parents = new List<GameObject>() { parents[1], parents[0] };
            }
            //if (id == 5) print(parents[primaryparent].GetComponent<DudeScript>().id);
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (ghost && ghosttype == "child")
            {
                if (!real.parents[1].GetComponent<DudeScript>().IsOrphan())
                {
                    real.parents = new List<GameObject> { real.parents[1], real.parents[0] };
                    
                }
            }
            tree.LayOut();
            listener.WasClicked(this, 0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            listener.WasClicked(this, 1);
        }
    }


    public void UnChoose()
    {
        if (ghost || !IsAlive) return;
        transform.Find("Background").GetComponent<SpriteRenderer>().color = Color.white;
    }
    public bool Choose()
    {
        if (/*ghost || */ !IsAlive) return false;
        if (ghost) real.Choose();
        else transform.Find("Background").GetComponent<SpriteRenderer>().color = Color.green;
        return true;
    }

    void SetName(string nam)
    {
        transform.name = nam;
        transform.Find("textimo").GetComponent<TextMeshPro>().text = nam;
    }

    // order children by first spouse then age, and also group them by spouse
    public void OrderChildren()
    {
        if (ghost)
        {
            print("ordering ghost children");
            return;
        }
        List<DudeScript> oldspouses = spouses;
        List<GameObject> oldchildren = children;
        childrenbyspouse = new Dictionary<DudeScript, List<DudeScript>>();
        spouses = new List<DudeScript>();
        List<GameObject> newchildren = new List<GameObject>();
        //print("NOW");
        /*if (id == 1) print(children.Count);
        if (id == 1) print(children[0].GetComponent<DudeScript>().id);
        if (id == 1) print(children[1].GetComponent<DudeScript>().id);
        if (id == 1) print(children[2].GetComponent<DudeScript>().id);*/
        children.Sort(delegate (GameObject x, GameObject y)
        {
            // return -1 if x < y, 0 if same
            float xb = x.GetComponent<DudeScript>().birthday;
            float yb = y.GetComponent<DudeScript>().birthday;
            if (xb < yb) return -1; // x born before, so put first
            if (xb == yb) return 0;
            return 1;
        });
        //if (id == 1) print(children[0].GetComponent<DudeScript>().id);
        //if (id == 1) print(children[1].GetComponent<DudeScript>().id);
        //if (id == 1) print(children[2].GetComponent<DudeScript>().id);
        /*if (id == 1) print(children[0].GetComponent<DudeScript>().id);
        if (id == 1) print(children[0].GetComponent<DudeScript>().birthday);
        if (id == 1) print(children[1].GetComponent<DudeScript>().id);
        if (id == 1) print(children[1].GetComponent<DudeScript>().birthday);*/
        for (int i = 0; i < children.Count; i++)
        {
            GameObject currchild = children[i];
            //if (id == 1) print(i.ToString() + "lip" + currchild.GetComponent<DudeScript>().id.ToString());
            if (newchildren.Contains(currchild)) continue;
            //print("is new");
            DudeScript otherparent = currchild.GetComponent<DudeScript>().OtherParent(this);
            //print("other " + otherparent.id.ToString());
            if (otherparent == null) print("llllle"); 
            if (childrenbyspouse.ContainsKey(otherparent)) print("cannot hap");
           
            spouses.Add(otherparent);
            //print("spo count" + spouses.Count.ToString());
            childrenbyspouse[otherparent] = new List<DudeScript>();
            childrenbyspouse[otherparent].Add(currchild.GetComponent<DudeScript>());
            newchildren.Add(currchild);

            for (int j = i + 1; j < children.Count; j++)
            {
                if (children[j].GetComponent<DudeScript>().OtherParent(this) == otherparent)
                {
                    //print("addimo");
                    newchildren.Add(children[j]);
                    childrenbyspouse[otherparent].Add(children[j].GetComponent<DudeScript>());
                    // print("adding" + )
                }
            }
        }
        /*
        if (id == 1)
        {
            print(spouses.Count == 1);
            foreach (DudeScript g in childrenbyspouse[spouses[0]])
            {
                print(g.id);
            }
            foreach (GameObject gh in newchildren)
            {
                print(gh.GetComponent<DudeScript>().id);
            }
        }*/
        //if (id == 1) print(children.Count);
        //if (id == 1) print(newchildren.Count);
        // if (id == 1) print(childrenbyspouse[spouses[0]].Count);
        if (children.Count != newchildren.Count)
        {
            print("bad");
        }
        int othercount = 0; // just sanity check
        foreach (DudeScript d in childrenbyspouse.Keys)
        {
            othercount += childrenbyspouse[d].Count;
        }
        if (othercount != children.Count) print("whatchama");
        children = newchildren;


 


        // maybe do smth more later
        for (int s = 0; s < oldspouses.Count; s++)
        {
            if (!spouses.Contains(oldspouses[s]))
            {
                print("vme");
                // this spouse no longer exists, maybe we want to do smth?
                // if has ghost then we destroy
                if (spousetoghost.ContainsKey(oldspouses[s])) {
                    Destroy(spousetoghost[oldspouses[s]].gameObject);
                    spousetoghost.Remove(oldspouses[s]);
                }
                else 
                {
                    oldspouses[s].InformNotActiveSpouse(this);
                }
            }
        }
        for (int s = 0; s < oldchildren.Count; s++)
        {
            if (!children.Contains(oldchildren[s]))
            {
                print("his ahe");
                if (childtoghost.ContainsKey(oldchildren[s].GetComponent<DudeScript>()))
                {
                    Destroy(childtoghost[oldchildren[s].GetComponent<DudeScript>()].gameObject);
                    childtoghost.Remove(oldchildren[s].GetComponent<DudeScript>());
                }
                else
                {
                    // oldspouses[s].InformNotActiveSpouse(this);
                }
            }
        }


    }

    public void FixLiners()
    {
        // print("corr linesr");
        // correctness of liners
        List<DudeScript> remove = new List<DudeScript>();
        foreach (DudeScript s in spousetoliner.Keys)
        {
            if (!spouses.Contains(s))
            {
                spousetoliner[s].GetComponent<LinerScript>().suicide();
                Destroy(spousetoliner[s].gameObject);
                //spousetoliner.Remove(s);
                remove.Add(s);
            }
        }
        foreach (DudeScript s in remove)
        {
            spousetoliner.Remove(s);
        }
        int idx = 0;
        foreach (DudeScript s in spouses)
        {
            // print("construct linerd");
            if (!spousetoliner.ContainsKey(s))
            {
                //print("construct liner");
                spousetoliner[s] = GameObject.Instantiate(liner).GetComponent<LinerScript>();
            }

            LinerScript ls = spousetoliner[s];
            ls.idx = idx;
            idx += 1;
            ls.parents = new List<GameObject>();
            ls.parents.Add(gameObject);
            if (spousetoghost.ContainsKey(s))
                ls.parents.Add(spousetoghost[s].gameObject);
            else
                ls.parents.Add(s.gameObject);
            ls.children = new List<GameObject>();
            foreach (DudeScript c in childrenbyspouse[s])
            {
                if (childtoghost.ContainsKey(c))
                    ls.children.Add(childtoghost[c].gameObject);
                else
                {
                    //print(c.id);
                    ls.children.Add(c.gameObject);
                }
            }
        }
    }

    public void SetWantPos(Vector2 wantp)
    {
        if (id == 6) print(wantp);
        wantpos = wantp;
    }

    public Vector2 GetWantPos(Vector2 wantp)
    {
        return wantpos;
    }

    public bool IsPrimaryParent(DudeScript d)
    {
        FixPrimary();
        return parents[0].GetComponent<DudeScript>() == d;
    }

    public void GhostCopy(DudeScript d, bool copypos, string type)
    {
        if (type != "child" && type != "spouse") print("bad stuffll");
        // do!! copy values of d
        ghosttype = type;
        ghost = true;
        real = d;
        SetName(d.transform.name);
        if (copypos)
            transform.position = d.transform.position;
        id = d.id;
        birthday = d.birthday;
        // transform.Find("Face").GetComponent<SpriteRenderer>().sortingOrder = -1;
        //transform.Find("Face").GetComponent<SpriteRenderer>().color = Color.gray;
        //transform.Find("Face").GetComponent<SpriteRenderer>().color = Color.gray;
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            SpriteRenderer sr = t.gameObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.25f);
            }
        }

        GetComponent<Person>().Copy(d.GetComponent<Person>());
    }


    // inform that d is no longer the active spouse
    public void InformNotActiveSpouse(DudeScript d)
    {
        if (activespouse == d) activespouse = d;
    }

    public void InformActiveSpouse(DudeScript d)
    {
        activespouse = d;
    }

    public bool HasOtherActiveSpouse(DudeScript d)
    {
        return activespouse != null && activespouse != d;
    }

    public bool IsOrphan()
    {
        return !root && parents.Count == 0;
    }

    public DudeScript OtherParent(DudeScript oneparent)
    {
        for (int i = 0; i < parents.Count; i++)
        {
            if (parents[i].GetComponent<DudeScript>() != oneparent)
                return parents[i].GetComponent<DudeScript>();
        }
        return null;
    }

    public void ToLayer(int layer)
    {
        gameObject.layer = layer; // LayerMask.NameToLayer("Default");
        foreach (Transform t in GetComponentsInChildren<Transform>() )
        {
            t.gameObject.layer = layer; //LayerMask.NameToLayer("Default");
        }
    }
    public void AddChild(GameObject child)
    {
        //print("adding " + child.GetComponent<DudeScript>().id.ToString() + " to " + id.ToString());
        if (!children.Contains(child))
            children.Add(child);
    }

    public void FixedUpdate()
    {
        float speed = 0.2f;
        /*if (transform.position.x < wantpos.x) transform.position = transform.position + new Vector3(speed, 0, 0);
        if (transform.position.x > wantpos.x) transform.position = transform.position + new Vector3(-speed, 0, 0);
        if (transform.position.y < wantpos.y) transform.position = transform.position + new Vector3(0, speed, 0);
        if (transform.position.y > wantpos.y) transform.position = transform.position + new Vector3(0, -speed, 0);*/
        Vector3 dir = (Vector3)wantpos - transform.position;
        float derp = Mathf.Min(10, dir.magnitude * dir.magnitude);
        transform.position = transform.position + dir.normalized*speed*derp;

    }

    private void Update()
    {
        TimePassed(); // I used to call this just when needed, but there was a ghost problem.

        if (ghost)
        {
            Vector3 pos1 = transform.position;
            Vector3 pos2 = real.transform.position;
            int nu = (int)Mathf.Ceil((pos2 - pos1).magnitude) + 2;
            GetComponent<LineRenderer>().positionCount = nu + 1;
            Vector3 di = (pos2 - pos1) / nu;
            List<Vector3> poses = new List<Vector3>();
            Vector3 side = new Vector3((pos2 - pos1).y, -(pos2 - pos1).x) * 0.1f;
            for (int i = 0; i < nu+1; i++)
            {
                poses.Add(pos1 + di * i + side * Mathf.Sin((float)(i)/nu*Mathf.PI));
            }
            GetComponent<LineRenderer>().SetPositions(poses.ToArray());
        }
    }

    /*
    float scale(float a)
    {
        a = Mathf.Abs(a);
        if (a > 10) a = 10;
        return a;
    }

    public void FixedUpdate()
    {
        Vector2 move = new Vector2(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
        float drop = tree.transform.position.y - transform.position.y;
        if (drop < birthday) move += Vector2.down * scale(drop - birthday);
        if (drop > birthday) move += Vector2.up;

        for (int i = 0; i < parents.Length; i++)
        {
            float diff = parents[i].transform.position.x - transform.position.x;
            if (diff > 0) move -= Vector2.left * scale(diff);
            if (diff < 0) move -= Vector2.right * scale(diff);
        }

        for (int i = 0; i < tree.dudes.Length; i++)
        {
            Vector2 diff = transform.position - tree.dudes[i].transform.position;
            if (diff.magnitude < 2.5f)
                move += diff.normalized;
        }

        GetComponent<Rigidbody2D>().AddForce(move);
    }
    */
}
