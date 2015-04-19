using UnityEngine;
using System.Collections;

public class RoleAutoRotate : MonoBehaviour {

    private GameObject wheel01 = null;
    private GameObject wheel02 = null;
    // Use this for initialization
    void Start()
    {
        wheel01 = GameObject.Find("candy1");
        wheel02 = GameObject.Find("candy2");
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = (wheel01.transform.localPosition + wheel02.transform.localPosition) / 2.0f - new Vector3(0,.5f,0);
        float y = wheel01.transform.localPosition.y - wheel02.transform.localPosition.y;
        float x = wheel01.transform.localPosition.x - wheel02.transform.localPosition.x;
        //Debug.Log(Mathf.Atan(y / x) * Mathf.Rad2Deg);
        if (x < 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan(y / x) * Mathf.Rad2Deg + 180);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan(y / x) * Mathf.Rad2Deg);
        }
    }
}
