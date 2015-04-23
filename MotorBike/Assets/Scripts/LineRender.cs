using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineRender : MonoBehaviour {

    public Material m_material = null;
    public GameObject go = null;

    private LineRenderer m_lineRenderer = null;
    private LineVertsInfo m_lineVertsInfo = null;
    private GameObject m_LineGo = null;
    private GameObject m_LineBox = null;
    private EdgeCollider2D m_EdgeCollider = null;
    private List<Vector2> m_LineVerts = new List<Vector2>();
    private int penHeight = 30;
    private int count = 0;
    private float ratio = 0;

    private List<Vector2> m_newVerts = new List<Vector2>();
    public List<Vector2> NewVerts
    {
        set { m_newVerts = value; }
        get { return m_newVerts; }
    }
	// Use this for initialization
	void Start () {
        m_LineBox = GameObject.Find("LineBox");
        GameObject fingerGesture = GameObject.Find("FingerGestures");
        fingerGesture.GetComponent<TouchManager>().onDragHandle += OnDrag;
        ratio = 640f / Screen.height;
	}

    void OnDrag(DragGesture gesture)
    {
        if (gameObject.GetComponent<LogicHandle>().IsGum)
            return;

        switch (gesture.Phase)
        {
            case ContinuousGesturePhase.Updated:
                if (count < 1000)
                {
                    count++;
                    m_lineRenderer.SetVertexCount(count + 2);
                    m_lineRenderer.SetPosition(count - 1, (gesture.Position - new Vector2(Screen.width / 2, Screen.height / 2 - penHeight)) / 100 * ratio);
                    m_lineRenderer.SetPosition(count, (gesture.Position - new Vector2(Screen.width / 2, Screen.height / 2 - penHeight)) / 100 * ratio);
                    m_lineRenderer.SetPosition(count + 1, (gesture.Position - new Vector2(Screen.width / 2, Screen.height / 2 - penHeight)) / 100 * ratio);
                    m_LineVerts.Add((gesture.Position - new Vector2(Screen.width / 2, Screen.height / 2 - penHeight)) / 100 * ratio);
                }
                break;
            case ContinuousGesturePhase.Ended:
                int pointCount = m_LineVerts.Count;
                Vector2[] verts = new Vector2[pointCount];
                for (int i = 0; i < pointCount; i++)
                {
                    verts[i] = m_LineVerts[i];
                    m_lineVertsInfo.LineVerts.Add(m_LineVerts[i]);
                }
                m_EdgeCollider.points = verts;
                m_LineVerts.Clear();
                count = 0;
                break;
            case ContinuousGesturePhase.Started:
                CreateLineGameObject();
                break;
        }
    }

    void CreateLineGameObject()
    {
        m_LineGo = (GameObject)GameObject.Instantiate(go);
        m_LineGo.name = "line";
        m_LineGo.transform.parent = m_LineBox.transform;
        m_lineRenderer = m_LineGo.AddComponent<LineRenderer>();
        m_lineVertsInfo = m_LineGo.AddComponent<LineVertsInfo>();
        m_lineRenderer.SetWidth(.2f, .2f);
        m_lineRenderer.SetColors(Color.white, Color.white);
        m_lineRenderer.material = m_material;
        m_lineRenderer.material.color = new Color(1, 1, 0, .25f);

        m_EdgeCollider = m_LineGo.AddComponent<EdgeCollider2D>();
    }

    public void CreateLineGo()
    {
        int pointCount = NewVerts.Count;
        if (pointCount == 2 && NewVerts[0].Equals(NewVerts[1]))
        {
            return;
        }

        CreateLineGameObject();

        Vector2[] verts = new Vector2[pointCount];
        m_lineRenderer.SetVertexCount(pointCount);
        for (int i = 0; i < pointCount; i++)
        {
            verts[i] = NewVerts[i];
            m_lineVertsInfo.LineVerts.Add(NewVerts[i]);
            m_lineRenderer.SetPosition(i, NewVerts[i]);
        }
        m_EdgeCollider.points = verts;
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
