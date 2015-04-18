using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

    public delegate void OnDragHandler(DragGesture _gesture);
    public OnDragHandler onDragHandle;
    public delegate void OnLongPressHandler(LongPressGesture _gesture);
    public OnLongPressHandler onLongPressHandle;
    public delegate void OnTapHandler(TapGesture _gesture);
    public OnTapHandler onTapHandle;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnDrag(DragGesture gesture)
    {
        //Debug.Log(gesture.Selection ? gesture.Selection.name : "NULL");
        //Debug.Log(gesture.Position);
        if (onDragHandle != null)
        {
            onDragHandle(gesture);
        }
    }

    void OnLongPress(LongPressGesture gesture)
    {
        if (onLongPressHandle != null)
        {
            onLongPressHandle(gesture);
        }
    }

    void OnTwist(TwistGesture gesture)
    {
        //Debug.Log(gesture.TotalRotation);
    }

    void OnTap(TapGesture gesture)
    {
        //wheel01.GetComponent<Rigidbody2D>().AddForce(new Vector2(.1f, 0));
        if (onTapHandle != null)
        {
            onTapHandle(gesture);
        }
    }
}
