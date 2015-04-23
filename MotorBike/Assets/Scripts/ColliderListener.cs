using UnityEngine;
using System.Collections;

public class ColliderListener : MonoBehaviour {

    public delegate void OnTriggerEnterHandler(Collider2D coll,GameObject go);
    public OnTriggerEnterHandler onTriggerEnterHandle;
    public delegate void OnCollisionEnterHandler(Collision2D coll, GameObject go);
    public OnCollisionEnterHandler onCollisionEnterHandle;
    public delegate void OnCollisionExitHandler(Collision2D coll, GameObject go);
    public OnCollisionExitHandler onCollisionExitHandle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (onTriggerEnterHandle != null)
        {
            onTriggerEnterHandle(coll,gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (onCollisionEnterHandle != null)
        {
            onCollisionEnterHandle(coll,gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (onCollisionExitHandle != null)
        {
            onCollisionExitHandle(coll,gameObject);
        }
    }
}
