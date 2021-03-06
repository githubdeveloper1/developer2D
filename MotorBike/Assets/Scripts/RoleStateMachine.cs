﻿using UnityEngine;
using System.Collections;

public class RoleStateMachine : MonoBehaviour {

    public enum MotorState
    {
        Idle,
        Run,
        Jump,
        Fall,
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
                gameObject.GetComponent<SkeletonAnimation>().AnimationName = "run";
                break;
            case MotorState.Jump:
                gameObject.GetComponent<SkeletonAnimation>().AnimationName = "jump";
                break;
            case MotorState.Fall:
                gameObject.GetComponent<SkeletonAnimation>().AnimationName = "fall";
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (onTriggerEnterHandle != null)
        {
            onTriggerEnterHandle(coll);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (onCollisionEnterHandle != null)
        {
            onCollisionEnterHandle(coll);
        }
    }
}
