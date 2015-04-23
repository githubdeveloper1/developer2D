using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineVertsInfo : MonoBehaviour {

    private List<Vector2> m_LineVerts = new List<Vector2>();

    public List<Vector2> LineVerts
    {
        set { m_LineVerts = value; }
        get { return m_LineVerts; }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
