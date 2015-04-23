using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour {

    private LogicHandle mLogicHandle;
    void Start() 
    {
        mLogicHandle = gameObject.GetComponent<LogicHandle>();
    }

    public float GetBikeRadius(GameObject wheel01,GameObject wheel02)
    {
        float y = wheel01.transform.localPosition.y - wheel02.transform.localPosition.y;
        float x = wheel01.transform.localPosition.x - wheel02.transform.localPosition.x;
        float radius = 0;
        if (x < 0)
        {
            radius = Mathf.Atan(y / x) + 180 * Mathf.Deg2Rad;
        }
        else
        {
            radius = Mathf.Atan(y / x);
        }
        return radius;
    }

    public void setLinePosition(LogicHandle.Direction direct, GameObject lineGo)
    {
        int lineVertsCount = lineGo.GetComponent<LineVertsInfo>().LineVerts.Count;
        LineVertsInfo lineVertInfo = lineGo.GetComponent<LineVertsInfo>();
        LineRenderer lineRenderer = lineGo.GetComponent<LineRenderer>();
        switch (direct)
        {
            case LogicHandle.Direction.Up:
                for (int i = 0; i < lineVertsCount; i++)
                {
                    lineVertInfo.LineVerts[i] += new Vector2(0, mLogicHandle.moveAmount);
                    lineRenderer.SetPosition(i, lineGo.GetComponent<LineVertsInfo>().LineVerts[i]);
                }
                break;
            case LogicHandle.Direction.Down:
                for (int i = 0; i < lineVertsCount; i++)
                {
                    lineVertInfo.LineVerts[i] -= new Vector2(0, mLogicHandle.moveAmount);
                    lineRenderer.SetPosition(i, lineGo.GetComponent<LineVertsInfo>().LineVerts[i]);
                }
                break;
            case LogicHandle.Direction.Left:
                for (int i = 0; i < lineVertsCount; i++)
                {
                    lineVertInfo.LineVerts[i] -= new Vector2(mLogicHandle.moveAmount, 0);
                    lineRenderer.SetPosition(i, lineGo.GetComponent<LineVertsInfo>().LineVerts[i]);
                }
                break;
            case LogicHandle.Direction.Right:
                for (int i = 0; i < lineVertsCount; i++)
                {
                    lineVertInfo.LineVerts[i] += new Vector2(mLogicHandle.moveAmount, 0);
                    lineRenderer.SetPosition(i, lineGo.GetComponent<LineVertsInfo>().LineVerts[i]);
                }
                break;
        }

        lineRenderer.SetPosition(lineVertsCount, lineVertInfo.LineVerts[lineVertsCount - 1]);
        lineRenderer.SetPosition(lineVertsCount + 1, lineVertInfo.LineVerts[lineVertsCount - 1]);
        int pointCount = lineVertInfo.LineVerts.Count;
        Vector2[] verts = new Vector2[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            verts[i] = lineVertInfo.LineVerts[i];
        }
        lineGo.GetComponent<EdgeCollider2D>().points = verts;
    }

    public void GumClear(Collision2D coll, GameObject go)
    {
        Vector2 touchPenPos = go.transform.localPosition;
        LineVertsInfo m_lineVerts = coll.gameObject.GetComponent<LineVertsInfo>();
        LineRender m_lineRender = gameObject.GetComponent<LineRender>();
        bool isStart = true;
        bool isEnd = false;
        int startIndex = 0;
        int endIndex = m_lineVerts.LineVerts.Count;
        List<Vector2> startLine = new List<Vector2>();
        List<Vector2> endLine = new List<Vector2>();
        for (int i = 0; i < m_lineVerts.LineVerts.Count; i++)
        {
            float x = m_lineVerts.LineVerts[i].x - touchPenPos.x;
            float y = m_lineVerts.LineVerts[i].y - touchPenPos.y;
            x = x * x;
            y = y * y;
            if ((x + y) < mLogicHandle.GumAmount)
            {
                if (isStart)
                {
                    isStart = false;
                    startIndex = i;
                }
                else
                {
                    endIndex = i;
                }
            }
            else if (endIndex > 0)
            {
                isEnd = true;
            }
            if (isStart)
            {
                startLine.Add(m_lineVerts.LineVerts[i]);
            }
            else if (isEnd)
            {
                endLine.Add(m_lineVerts.LineVerts[i]);
            }
        }
        if (startLine.Count > 1)
        {
            m_lineRender.NewVerts = startLine;
            m_lineRender.CreateLineGo();
        }
        if (endLine.Count > 1)
        {
            m_lineRender.NewVerts = endLine;
            m_lineRender.CreateLineGo();
        }
        Destroy(coll.gameObject);
    }
}
