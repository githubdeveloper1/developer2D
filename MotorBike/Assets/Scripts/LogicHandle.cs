using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicHandle : MonoBehaviour {

    public GameObject bar01Go = null;
    public GameObject bar02Go = null;
    public GameObject bar03Go = null;
    public GameObject circle01Go = null;
    public GameObject circle02Go = null;
    public GameObject circle03Go = null;
    public GameObject circle04Go = null;
    public GameObject circle05Go = null;
    public GameObject circle06Go = null;
    public GameObject dragGo = null;
    public float addForce = .03f;
    public float power = 0;

    private GameObject wheel01 = null;
    private GameObject wheel02 = null;
    private GameObject rensheGo = null;
    private GameObject selectedGo = null;
    private GameObject dragBoxGo = null;
    private GameObject rightGo = null;

    private GameObject winGo = null;
    private GameObject lostGo = null;
    private GameObject blackMask = null;

    private Vector3 leftDragPosition = Vector3.zero;
    private Vector3 rightDragPosition = Vector3.zero;
    private Vector3 selectedPosition = Vector3.zero;
    private Vector3 wheel01Position = Vector3.zero;
    private Vector3 wheel02Position = Vector3.zero;

    private List<GameObject> useComponents = new List<GameObject>();

    private float pressTime = 0;

    private bool isPlay = false;
    private bool isLongPress = false;
    private bool isScroll = false;
    private bool isWin = false;
	// Use this for initialization
	void Start () {
        wheel01 = GameObject.Find("candy1");
        wheel01Position = wheel01.transform.localPosition;
        wheel02 = GameObject.Find("candy2");
        wheel02Position = wheel02.transform.localPosition;
        rensheGo = GameObject.Find("renshe");

        dragBoxGo = GameObject.Find("DragBox");

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

        AddUIListener("bar01");
        AddUIListener("bar02");
        AddUIListener("bar03");
        AddUIListener("circle01");
        AddUIListener("circle02");
        AddUIListener("circle03");
        AddUIListener("circle04");
        AddUIListener("circle05");
        AddUIListener("circle06");
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
        power = 0.002f;
    }

    void OnCollisionExitHandler(Collision2D coll)
    {
        Debug.Log("OnCollisionExitHandler");
        power = 0;
    }

    void OnGestureTapHandler(TapGesture gesture)
    {
        if (gesture.Selection == null)
        {
            ClearSelectedState();
        }
    }

    void OnGestureDragHandler(DragGesture gesture)
    {
        if (gesture.Phase == ContinuousGesturePhase.Ended)
        {
            isLongPress = false;
        }
        else if (gesture.Selection && isLongPress)
        {
            GameObject go = (GameObject)gesture.Selection;
            if (go.name == "bar01(Clone)" || go.name == "bar02(Clone)" || go.name == "bar03(Clone)" || go.name == "circle01(Clone)"
                || go.name == "circle02(Clone)" || go.name == "circle03(Clone)" || go.name == "circle04(Clone)"
                 || go.name == "circle05(Clone)" || go.name == "circle06(Clone)")
            {
                go.transform.localPosition += new Vector3(gesture.DeltaMove.x / 100.0f, gesture.DeltaMove.y / 100.0f, 0);
            }
        }
    }

    void OnGestureLongPressHandler(LongPressGesture gesture)
    {
        isLongPress = true;
    }

    void OnUIClickHandler(GameObject go)
    {
        if (go.name == "PlayBt")
        {
            wheel01.GetComponent<Rigidbody2D>().isKinematic = false;
            wheel02.GetComponent<Rigidbody2D>().isKinematic = false;
            rensheGo.GetComponent<RoleStateMachine>().ChangeMotorState(RoleStateMachine.MotorState.Run);
            GameObject toolBox = GameObject.Find("ToolBox");
            GameObject frame01 = GameObject.Find("frame01");
            TweenPosition.Begin(toolBox, .3f, new Vector3(0, -400, 0));
            TweenPosition.Begin(frame01, .3f, new Vector3(0, -400, 0));
            TweenPosition.Begin(go, .3f, new Vector3(go.transform.localPosition.x, 400, 0));
            isPlay = true;
            isWin = false;
        }
        else if (go.name == "AddPowerBt")
        {
            float radius = GetBikeRadius();
            wheel01.GetComponent<Rigidbody2D>().AddForce(new Vector2(addForce * Mathf.Cos(radius), addForce * Mathf.Sin(radius)));
        }
        else if (go.name == "Win" || go.name == "Lost")
        {
            GameObject toolBox = GameObject.Find("ToolBox");
            toolBox.GetComponent<TweenPosition>().PlayReverse();
            GameObject frame01 = GameObject.Find("frame01");
            frame01.GetComponent<TweenPosition>().PlayReverse();
            GameObject playBt = GameObject.Find("PlayBt");
            playBt.GetComponent<TweenPosition>().PlayReverse();

            NGUITools.SetActive(winGo, false);
            NGUITools.SetActive(lostGo, false);
            NGUITools.SetActive(blackMask, false);

            rensheGo.GetComponent<RoleStateMachine>().ChangeMotorState(RoleStateMachine.MotorState.Idle);

            wheel01.transform.localPosition = wheel01Position;
            wheel02.transform.localPosition = wheel02Position;
            wheel01.GetComponent<Rigidbody2D>().isKinematic = true;
            wheel02.GetComponent<Rigidbody2D>().isKinematic = true;
            ClearSelectedState();
            for (int i = 0; i < useComponents.Count; i++)
            {
                Destroy(useComponents[i]);
            }
            selectedGo = null;
        }
    }

    void OnUIPressHandler(GameObject go, bool isPress)
    {
        if (isPress)
        {
            selectedPosition = go.transform.localPosition;
            pressTime = RealTime.time;
            if (go.GetComponent<UIDragScrollView>())
                go.GetComponent<UIDragScrollView>().enabled = true;
            isScroll = false;
        }
        else
        {
            if (RealTime.time - pressTime < .5f || isScroll)
                return;
            GameObject newBar = null;
            if (go.name == "bar01")
            {
                newBar = GameObject.Instantiate(bar01Go);
                SetSelectedState(go, newBar);
            }
            else if (go.name == "bar02")
            {
                newBar = GameObject.Instantiate(bar02Go);
                SetSelectedState(go, newBar);
            }
            else if (go.name == "bar03")
            {
                newBar = GameObject.Instantiate(bar03Go);
                SetSelectedState(go, newBar);
            }
            else if (go.name == "circle01")
            {
                newBar = GameObject.Instantiate(circle01Go);
                SetSelectedState(go, newBar);
            }
            else if (go.name == "circle02")
            {
                newBar = GameObject.Instantiate(circle02Go);
                SetSelectedState(go, newBar);
            }
            else if (go.name == "circle03")
            {
                newBar = GameObject.Instantiate(circle03Go);
                SetSelectedState(go, newBar);
            }
            else if (go.name == "circle04")
            {
                newBar = GameObject.Instantiate(circle04Go);
                SetSelectedState(go, newBar);
            }
            else if (go.name == "circle05")
            {
                newBar = GameObject.Instantiate(circle05Go);
                SetSelectedState(go, newBar);
            }
            else if (go.name == "circle06")
            {
                newBar = GameObject.Instantiate(circle06Go);
                SetSelectedState(go, newBar);
            }
        }
    }

    void OnUIDragStartHandler(GameObject go)
    {
        if (go.name == "leftDrag")
        {
            leftDragPosition = go.transform.localPosition;
            isLongPress = false;
        }
        else if (go.name == "rightDrag")
        {
            rightDragPosition = go.transform.localPosition;
            isLongPress = false;
        }
    }

    void OnUIDragEndHandler(GameObject go)
    {
        if (go.name == "leftDrag")
        {
            go.transform.localPosition = leftDragPosition;
        }
        else if (go.name == "rightDrag")
        {
            go.transform.localPosition = rightDragPosition;
        }
    }

    void OnUIDragHandler(GameObject go, Vector2 delta)
    {
        if (go.name == "leftDrag")
        {
            go.transform.localPosition += new Vector3(0, delta.y, 0);
            selectedGo.transform.localEulerAngles -= new Vector3(0, 0, delta.y);
        }
        else if (go.name == "rightDrag")
        {
            go.transform.localPosition += new Vector3(0, delta.y, 0);
            selectedGo.transform.localEulerAngles += new Vector3(0, 0, delta.y);
        }
        else
        {
            if (RealTime.time - pressTime > .5f && !isScroll)
            {
                go.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                go.transform.localPosition += new Vector3(delta.x, delta.y, 0);
                if (go.GetComponent<UIDragScrollView>())
                    go.GetComponent<UIDragScrollView>().enabled = false;
            }
            else
            {
                isScroll = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        float radius = GetBikeRadius();
        wheel01.GetComponent<Rigidbody2D>().AddForce(new Vector2(power * Mathf.Cos(radius), power * Mathf.Sin(radius)));

        if (rensheGo.transform.localPosition.y < -17 && isPlay && !isWin)
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

    void SetSelectedState(GameObject go, GameObject newBar)
    {
        ClearSelectedState();
        selectedGo = newBar;
        float parentX = go.transform.parent.localPosition.x;
        newBar.transform.localPosition = (go.transform.localPosition + new Vector3(parentX, -242, 0)) / 100.0f;
        rightGo = NGUITools.AddChild(dragBoxGo, dragGo);
        rightGo.name = "rightDrag";
        rightGo.transform.localPosition = go.transform.localPosition + new Vector3(go.GetComponent<UIWidget>().width / 2 + parentX, -242, 0);
        rightGo.GetComponent<UIEventListener>().onDrag += OnUIDragHandler;
        rightGo.GetComponent<UIEventListener>().onDragStart += OnUIDragStartHandler;
        rightGo.GetComponent<UIEventListener>().onDragEnd += OnUIDragEndHandler;

        selectedGo.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);

        go.transform.localScale = Vector3.one;
        go.transform.localPosition = selectedPosition;

        useComponents.Add(newBar);
    }

    void AddUIListener(string name)
    {
        GameObject go = GameObject.Find(name);
        go.GetComponent<UIEventListener>().onDrag += OnUIDragHandler;
        go.GetComponent<UIEventListener>().onPress += OnUIPressHandler;
    }

    void ClearSelectedState()
    {
        if (selectedGo)
        {
            selectedGo.GetComponent<SpriteRenderer>().color = Color.white;
            for (int i = 0; i < dragBoxGo.transform.childCount; i++)
            {
                Destroy(dragBoxGo.transform.GetChild(i).gameObject);
            }
        }
    }
}
