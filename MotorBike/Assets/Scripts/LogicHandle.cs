using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicHandle : MonoBehaviour {

    public float addForce = .03f;
    public float power = 0.0015f;
    public float moveAmount = .1f;

    private float motorPower = 0;

    private GameObject wheel01 = null;
    private GameObject wheel02 = null;
    private GameObject rensheGo = null;

    private GameObject touchPoint = null;

    private GameObject winGo = null;
    private GameObject lostGo = null;
    private GameObject blackMask = null;
    private GameObject directBts = null;
    private GameObject selectedLineGo = null;

    private Vector3 wheel01Position = Vector3.zero;
    private Vector3 wheel02Position = Vector3.zero;

    private Utils mUtils = null;

    private bool isPlay = false;
    private bool isWin = false;
    private bool isGum = false;
    private bool isTap = false;
    private bool isTouchDirectUI = false;

    private float ratio = 0;
    private float gumAmount = .03f;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    public bool IsGum{
        get{return isGum;}
    }
    public float GumAmount
    {
        get { return gumAmount; }
    }
	// Use this for initialization
	void Start () {
        mUtils = gameObject.GetComponent<Utils>();

        wheel01 = GameObject.Find("candy1");
        wheel01Position = wheel01.transform.localPosition;
        wheel02 = GameObject.Find("candy2");
        wheel02Position = wheel02.transform.localPosition;
        rensheGo = GameObject.Find("renshe");

        winGo = GameObject.Find("Win");
        lostGo = GameObject.Find("Lost");

        AddUIClickListener(GameObject.Find("PlayBt"));
        AddUIClickListener(GameObject.Find("AddPowerBt"));
        AddUIClickListener(GameObject.Find("GumBt"));
        AddUIClickListener(GameObject.Find("LeftBt"));
        AddUIClickListener(GameObject.Find("RightBt"));
        AddUIClickListener(GameObject.Find("UpBt"));
        AddUIClickListener(GameObject.Find("DownBt"));
        AddUIClickListener(GameObject.Find("DeleteBt"));
        AddUIClickListener(winGo, false);
        AddUIClickListener(lostGo, false);
        blackMask = GameObject.Find("blackMask");
        blackMask.SetActive(false);
        directBts = GameObject.Find("DirectButtons");
        directBts.SetActive(false);

        GameObject fingerGesture = GameObject.Find("FingerGestures");
        fingerGesture.GetComponent<TouchManager>().onDragHandle += OnGestureDragHandler;
        fingerGesture.GetComponent<TouchManager>().onLongPressHandle += OnGestureLongPressHandler;
        fingerGesture.GetComponent<TouchManager>().onTapHandle += OnGestureTapHandler;

        wheel01.GetComponent<ColliderListener>().onTriggerEnterHandle += OnTriggerEnterHandler;
        wheel01.GetComponent<ColliderListener>().onCollisionEnterHandle += OnCollisionEnterHandler;
        wheel01.GetComponent<ColliderListener>().onCollisionExitHandle += OnCollisionExitHandler;

        touchPoint = GameObject.Find("TouchPen");
        touchPoint.GetComponent<ColliderListener>().onCollisionEnterHandle += OnCollisionEnterHandler;

        ratio = 640f / Screen.height;
	}

    void OnTriggerEnterHandler(Collider2D coll,GameObject go)
    {
        if (go.name == "candy1")
        {
            if (coll.gameObject.name == "END")
            {
                NGUITools.SetActive(winGo, true);
                NGUITools.SetActive(blackMask, true);
                winGo.GetComponent<TweenScale>().PlayForward();
                isWin = true;
            }
        }
    }

    void OnCollisionEnterHandler(Collision2D coll, GameObject go)
    {
        if (go.name == "candy1")
        {
            motorPower = power;
        }
        else if (go.name == "TouchPen")
        {
            if (coll.gameObject.name == "line" && isGum)
            {
                mUtils.GumClear(coll, go);
            }
            else if (coll.gameObject.name == "line" && !isGum && isTap)
            {
                selectedLineGo = coll.gameObject;
                isTap = false;
                directBts.SetActive(true);
            }
        }
    }

    void OnCollisionExitHandler(Collision2D coll, GameObject go)
    {
        motorPower = 0;
    }

    void OnGestureTapHandler(TapGesture gesture)
    {
        if (isTouchDirectUI)
        {
            isTouchDirectUI = false;
        }
        else
        {
            isTap = true;
            touchPoint.transform.localPosition = (gesture.Position - new Vector2(Screen.width / 2, Screen.height / 2)) / 100 * ratio;
            touchPoint.transform.GetChild(0).gameObject.SetActive(false);
            touchPoint.transform.GetChild(1).gameObject.SetActive(false);
            directBts.SetActive(false);
        }
    }

    void OnGestureDragHandler(DragGesture gesture)
    {
        switch (gesture.Phase)
        {
            case ContinuousGesturePhase.Started:
                touchPoint.transform.GetChild(0).gameObject.SetActive(true);
                touchPoint.transform.GetChild(1).gameObject.SetActive(false);
                directBts.SetActive(false);
                touchPoint.transform.localEulerAngles = Vector3.zero;
                break;
            case ContinuousGesturePhase.Updated:
                touchPoint.transform.localPosition = (gesture.Position - new Vector2(Screen.width / 2, Screen.height / 2)) / 100 * ratio;
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
        else if (go.name == "GumBt")
        {
            touchPoint.transform.localEulerAngles = Vector3.zero;
            if (isGum)
            {
                isGum = false;
                GameObject.Find("GumBt").GetComponent<UISprite>().color = Color.white;
                touchPoint.transform.GetChild(0).gameObject.SetActive(true);
                touchPoint.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                isGum = true;
                GameObject.Find("GumBt").GetComponent<UISprite>().color = Color.yellow;
                touchPoint.transform.GetChild(0).gameObject.SetActive(false);
                touchPoint.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else if (go.name == "LeftBt")
        {
            mUtils.setLinePosition(Direction.Left, selectedLineGo);
            isTouchDirectUI = true;
        }
        else if (go.name == "RightBt")
        {
            mUtils.setLinePosition(Direction.Right, selectedLineGo);
            isTouchDirectUI = true;
        }
        else if (go.name == "UpBt")
        {
            mUtils.setLinePosition(Direction.Up, selectedLineGo);
            isTouchDirectUI = true;
        }
        else if (go.name == "DownBt")
        {
            mUtils.setLinePosition(Direction.Down, selectedLineGo);
            isTouchDirectUI = true;
        }
        else if (go.name == "DeleteBt")
        {
            Destroy(selectedLineGo);
            directBts.SetActive(false);
            isTouchDirectUI = true;
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
        float radius = mUtils.GetBikeRadius(wheel01,wheel02);
        float powerPercent = (100 + radius * Mathf.Rad2Deg) / 100f;
        powerPercent = Mathf.Clamp(powerPercent, 0, 1.5f);
        wheel01.GetComponent<Rigidbody2D>().AddForce(new Vector2(motorPower * Mathf.Cos(radius) * powerPercent, motorPower * Mathf.Sin(radius) * powerPercent));

        if (rensheGo.transform.localPosition.y < -30 && isPlay && !isWin)
        {
            NGUITools.SetActive(lostGo, true);
            NGUITools.SetActive(blackMask, true);
            lostGo.GetComponent<TweenScale>().PlayForward();
            isPlay = false;
        }
	}

    void AddUIClickListener(GameObject go, bool isActive = true)
    {
        go.GetComponent<UIEventListener>().onClick += OnUIClickHandler;
        go.SetActive(isActive);
    }

}
