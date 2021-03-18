using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackground : MonoBehaviour
{
    public RectTransform bg1;
    public RectTransform bg2;
    public RectTransform bg3;
    public float bg1Mod;
    public float bg2Mod;
    public float bg3Mod;

    public Vector3 distanceToMove;
    public Vector3 positionBefore;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        bg1.position = new Vector3(Screen.width, Screen.height) / 2f + Input.mousePosition / bg1Mod;
        bg2.position = new Vector3(Screen.width, Screen.height) / 2f + Input.mousePosition / bg2Mod;
        bg3.position = new Vector3(Screen.width, Screen.height) / 2f + Input.mousePosition / bg3Mod;
    }
}
