using UnityEngine;
using System.Collections;

public class RoleStateMachine : MonoBehaviour {

    public enum MotorState
    {
        Idle,
        Run,
    }

    public delegate void OnTriggerEnterHandler(Collider2D coll);
    public OnTriggerEnterHandler onTriggerEnterHandle;
    public delegate void OnCollisionEnterHandler(Collision2D coll);
    public OnCollisionEnterHandler onCollisionEnterHandle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeMotorState(MotorState state)
    {
        switch (state)
        {
            case MotorState.Idle:
                gameObject.GetComponent<SkeletonAnimation>().AnimationName = null;
                break;
            case MotorState.Run:
                gameObject.GetComponent<SkeletonAnimation>().AnimationName = "animation";
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("OnTriggerEnterHandler");
        if (onTriggerEnterHandle != null)
        {
            onTriggerEnterHandle(coll);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("OnCollisionEnter2D");
        if (onCollisionEnterHandle != null)
        {
            onCollisionEnterHandle(coll);
        }
    }
}
