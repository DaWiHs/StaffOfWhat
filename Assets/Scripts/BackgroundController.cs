using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController instance;

    public Transform bg1;
    public Transform bg2;
    public Transform bg3;

    public Vector3 bg1Mod;
    public Vector3 bg2Mod;
    public Vector3 bg3Mod;

    public SpriteRenderer[] bg1Borders;
    float x = 0;
    float sin = 0;

    public Vector3 distanceToMove;
    public Vector3 positionBefore;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        positionBefore = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToMove = Camera.main.transform.position - positionBefore;
        bg1.position += new Vector3(distanceToMove.x * bg1Mod.x, distanceToMove.y * bg1Mod.y, distanceToMove.z * bg1Mod.z);
        bg2.position += new Vector3(distanceToMove.x * bg2Mod.x, distanceToMove.y * bg2Mod.y, distanceToMove.z * bg2Mod.z);
        positionBefore = Camera.main.transform.position;

        foreach(SpriteRenderer ren in bg1Borders)
        {
            ren.color = new Color(Mathf.Sin(Mathf.PI / 1000f * x ) / 2f, 0, 0);
            sin = Mathf.Sin(Mathf.PI / 1000f * x ) / 2f;
        }
        if (x++ > 2000)
        {
            x = 0;
        }
    }
}
