using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicHandle : MonoBehaviour {

    public float addForce = .03f;
    public float power = 0.0015f;

    private float motorPower = 0;

    private GameObject wheel01 = null;
    private GameObject wheel02 = null;
    private GameObject rensheGo = null;

    private GameObject touchPoint = null;

    private GameObject winGo = null;
    private GameObject lostGo = null;
    private GameObject blackMask = null;

    private Vector3 wheel01Position = Vector3.zero;
    private Vector3 wheel02Position = Vector3.zero;

    private bool isPlay = false;
    private bool isWin = false;
	// Use this for initialization
	void Start () {
        wheel01 = GameObject.Find("candy1");
        wheel01Position = wheel01.transform.localPosition;
        wheel02 = GameObject.Find("candy2");
        wheel02Position = wheel02.transform.localPosition;
        rensheGo = GameObject.Find("renshe");

        GameObject.Find("PlayBt").GetComponent<UIEventListener>().onClick += OnUIClickHandler;
        GameObject.Find("AddPowerBt").GetComponent<UIEventListener>().onClick += OnUIClickHandler;
        winGo = GameObject.Find("Win");
        winGo.GetComponent<UIEventListener>().onClick += OnUIClickHandler;
        winGo.SetActive(false);
        lostGo = GameObject.Find("Lost");
        lostGo.GetComponent<UIEventListener>().onClick += OnUIClickHandler;
        lostGo.SetActive(false);
        blackMask = GameObject.Find("blackMask");
        blackMask.SetActive(false);

        GameObject fingerGesture = GameObject.Find("FingerGestures");
        fingerGesture.GetComponent<TouchManager>().onDragHandle += OnGestureDragHandler;
        fingerGesture.GetComponent<TouchManager>().onLongPressHandle += OnGestureLongPressHandler;
        fingerGesture.GetComponent<TouchManager>().onTapHandle += OnGestureTapHandler;

        wheel01.GetComponent<ColliderListener>().onTriggerEnterHandle += OnTriggerEnterHandler;
        wheel01.GetComponent<ColliderListener>().onCollisionEnterHandle += OnCollisionEnterHandler;
        wheel01.GetComponent<ColliderListener>().onCollisionExitHandle += OnCollisionExitHandler;

        touchPoint = GameObject.Find("TouchPen");
	}

    void OnTriggerEnterHandler(Collider2D coll)
    {
        if (coll.gameObject.name == "END")
        {
            NGUITools.SetActive(winGo, true);
            NGUITools.SetActive(blackMask, true);
            winGo.GetComponent<TweenScale>().PlayForward();
            isWin = true;
        }
    }

    void OnCollisionEnterHandler(Collision2D coll)
    {
        motorPower = power;
        //rensheGo.GetComponent<RoleStateMachine>().ChangeMotorState(RoleStateMachine.MotorState.Run);
    }

    void OnCollisionExitHandler(Collision2D coll)
    {
        motorPower = 0;
        //rensheGo.GetComponent<RoleStateMachine>().ChangeMotorState(RoleStateMachine.MotorState.Jump);
    }

    void OnGestureTapHandler(TapGesture gesture)
    {
        
    }

    void OnGestureDragHandler(DragGesture gesture)
    {
        switch (gesture.Phase)
        {
            case ContinuousGesturePhase.Started:
                break;
            case ContinuousGesturePhase.Updated:
                touchPoint.transform.localPosition = (gesture.Position - new Vector2(Screen.width / 2, Screen.height / 2)) / 100;
                break;
            case ContinuousGesturePhase.Ended:
                break;
            case ContinuousGesturePhase.None:
                break;
        }
    }

    void OnGestureLongPressHandler(LongPressGesture gesture)
    {
        
    }

    void OnUIClickHandler(GameObject go)
    {
        if (go.name == "PlayBt")
        {
            wheel01.GetComponent<Rigidbody2D>().isKinematic = false;
            wheel02.GetComponent<Rigidbody2D>().isKinematic = false;
            rensheGo.GetComponent<RoleStateMachine>().ChangeMotorState(RoleStateMachine.MotorState.Run);
            TweenPosition.Begin(go, .3f, new Vector3(go.transform.localPosition.x, 400, 0));
            touchPoint.SetActive(false);
            isPlay = true;
            isWin = false;
        }
        else if (go.name == "AddPowerBt")
        {
            //float radius = GetBikeRadius();
            //wheel01.GetComponent<Rigidbody2D>().AddForce(new Vector2(addForce * Mathf.Cos(radius), addForce * Mathf.Sin(radius)));
            GameObject m_LineBox = GameObject.Find("LineBox");
            for (int i = 0; i < m_LineBox.transform.childCount; i++)
            {
                GameObject mLineGo = m_LineBox.transform.GetChild(i).gameObject;
                DestroyImmediate(mLineGo);
            }
        }
        else if (go.name == "Win" || go.name == "Lost")
        {
            GameObject playBt = GameObject.Find("PlayBt");
            playBt.GetComponent<TweenPosition>().PlayReverse();
            touchPoint.SetActive(true);

            NGUITools.SetActive(winGo, false);
            NGUITools.SetActive(lostGo, false);
            NGUITools.SetActive(blackMask, false);

            rensheGo.GetComponent<RoleStateMachine>().ChangeMotorState(RoleStateMachine.MotorState.Idle);

            wheel01.transform.localPosition = wheel01Position;
            wheel02.transform.localPosition = wheel02Position;
            wheel01.GetComponent<Rigidbody2D>().isKinematic = true;
            wheel02.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    void OnUIPressHandler(GameObject go, bool isPress)
    {
        
    }

    void OnUIDragStartHandler(GameObject go)
    {
        
    }

    void OnUIDragEndHandler(GameObject go)
    {
        
    }

    void OnUIDragHandler(GameObject go, Vector2 delta)
    {
        
    }
	
	// Update is called once per frame
	void Update () {
        float radius = GetBikeRadius();
        wheel01.GetComponent<Rigidbody2D>().AddForce(new Vector2(motorPower * Mathf.Cos(radius), motorPower * Mathf.Sin(radius)));

        if (rensheGo.transform.localPosition.y < -30 && isPlay && !isWin)
        {
            NGUITools.SetActive(lostGo, true);
            NGUITools.SetActive(blackMask, true);
            lostGo.GetComponent<TweenScale>().PlayForward();
            isPlay = false;
        }
	}

    float GetBikeRadius()
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
}
