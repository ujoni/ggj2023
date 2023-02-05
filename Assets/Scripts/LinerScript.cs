using System.Collections.Generic;
using UnityEngine;

public class LinerScript : MonoBehaviour
{
    public List<GameObject> parents;
    public List<GameObject> children;

    public GameObject ToKids;

    public GameObject Down;
    public GameObject Hor;
    public List<GameObject> ToKidses;

    public int idx;

    // this is for between parents
    LineRenderer lr;
    Color c;

    private void Start()
    {
        Random.State kek = Random.state;
        Random.InitState(parents[0].GetComponent<DudeScript>().id * 10129 + parents[1].GetComponent<DudeScript>().id);
        
        c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        Random.state = kek;

        lr = GetComponent<LineRenderer>();
        ToKidses = new List<GameObject>();
        if (lr == null) print("so bad"); //lr = gameObject.AddComponent<LineRenderer>();
        Down = transform.Find("Down").gameObject;
        Down.GetComponent<LineRenderer>().startColor = c;
        Down.GetComponent<LineRenderer>().endColor = c;
        Hor = transform.Find("Hor").gameObject;
        Hor.GetComponent<LineRenderer>().startColor = c;
        Hor.GetComponent<LineRenderer>().endColor = c;
        lr.startColor = c;
        lr.endColor = c;
    }

    private void Update()
    {
        if (parents.Count > 0 && children.Count > 0)
        {
            Vector3 upping = Vector3.up * idx * 0.3f;
            lr.SetPositions(new Vector3[] { parents[0].transform.position + upping, parents[1].transform.position + upping });

            DudeScript dud = parents[0].GetComponent<DudeScript>();

            //for (int i = 0; i < dud.spouses.Count; i++)
            //{

            /*float left = dud.childtox[dud.childrenbyspouse[parents[1].GetComponent<DudeScript>()][0]];
            float right = dud.childtox[
                dud.childrenbyspouse[parents[1].GetComponent<DudeScript>()][dud.childrenbyspouse[parents[1].GetComponent<DudeScript>()].Count - 1]];
            float mid = (right + left) / 2;*/

            //Down.pos
            //}

            Hor.GetComponent<LineRenderer>().positionCount = children.Count;
            float dd = TreeScript.DOWNADD; // this is how much kids are down from parents

            List<Vector3> poses = new List<Vector3>();
            for (int i = 0; i < children.Count; i++)
            {
                poses.Add(children[i].transform.position + Vector3.up * dd / 2);
            }
            Hor.GetComponent<LineRenderer>().SetPositions(poses.ToArray());

            Vector3 mid = midpoint(poses);

            Down.GetComponent<LineRenderer>().SetPositions(new Vector3[] {
                //(parents[0].transform.position + parents[1].transform.position)/2,
                parents[1].transform.position + (parents[0].transform.position - parents[1].transform.position).normalized * TreeScript.SPOUSERIGHTING + upping,
                mid
                //(parents[0].transform.position + parents[1].transform.position)/2 + Vector3.down*dd/2,
            });

            MakeCorrectNumberKidses();
            for (int i = 0; i < children.Count; i++)
            {
                LineRenderer llr = ToKidses[i].GetComponent<LineRenderer>();
                llr.positionCount = 2;
                llr.SetPositions(new Vector3[] { children[i].transform.position, children[i].transform.position + Vector3.up * dd / 2 });

            }
            //public List<GameObject> ToKidses;
        }
    }

    public void suicide()
    {
        foreach (GameObject go in ToKidses)
            Destroy(go);
    }
    void MakeCorrectNumberKidses()
    {
        while (ToKidses.Count < children.Count)
        {
            ToKidses.Add(GameObject.Instantiate(ToKids));
            ToKidses[ToKidses.Count - 1].GetComponent<LineRenderer>().startColor = c;
            ToKidses[ToKidses.Count - 1].GetComponent<LineRenderer>().endColor = c;
            Hor.GetComponent<LineRenderer>().endColor = c;
        }
        while (ToKidses.Count > children.Count) ToKidses.RemoveAt(ToKidses.Count);
    }

    Vector3 midpoint(List<Vector3> poses)
    {
        if (poses.Count == 1) return poses[0];
        float totallen = Lengthimo(poses);
        float midlen = totallen / 2;

        float len = 0;
        Vector3 curr = poses[0];
        for (int i = 1; i < poses.Count; i++)
        {
            if (len + (poses[i] - curr).magnitude > midlen)
            {
                // go midlen - len much along current
                float left = midlen - len;
                return curr + (poses[i] - curr).normalized * left;
            }
            len += (poses[i] - curr).magnitude;
            curr = poses[i];

        }
        return Vector3.zero;
    }

    float Lengthimo(List<Vector3> poses)
    {
        float len = 0;
        Vector3 curr = poses[0];
        for (int i = 1; i < poses.Count; i++)
        {
            len += (poses[i] - curr).magnitude;
            curr = poses[i];
        }
        return len;
    }

    /*void OnPostRender()
    {
        RenderLines();
    }

    private void RenderLines()
    {
        Matrix4x4 mat = new Matrix4x4();
        mat.SetTRS(transform.position, transform.rotation, transform.localScale);
        GL.PushMatrix();
        GL.MultMatrix(mat);
        GL.Begin(GL.LINES);
        material.SetPass(0);
        RenderLineList(BezierPoints, color);
        GL.End();
        GL.PopMatrix();
    }

    private void RenderLineList(List<List<Vector3>> lineList, Color color)
    {
        GL.Color(color);
        int lineListCount = lineList.Count;

        for (int j = 0; j < lineListCount; j++)
        {
            List<Vector3> line = lineList[j];
            int lineCountMinOne = line.Count - 1;
            for (int i = 0; i < lineCountMinOne; i++)
            {
                GL.Vertex(line[i]);
                GL.Vertex(line[i + 1]);
            }
        }
    }*/
}
