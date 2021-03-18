using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [Header("Crosshair")]
    public float defaultDistance;
    public float maxDistance;
    public float distanceBump;
    public float distanceRetract;
    public Transform[] segments = new Transform[4];
    public float animationSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * Camera.main.transform.position.z, 0.2f);

        if (Input.GetMouseButtonDown(1))
        {
            BumpSegments();
        }


        if (segments[0].transform.localPosition.y > defaultDistance)
        {
            segments[0].transform.localPosition -= new Vector3(0, 1) * distanceRetract;
            segments[1].transform.localPosition -= new Vector3(1, 0) * distanceRetract;
            segments[2].transform.localPosition -= new Vector3(0, -1) * distanceRetract;
            segments[3].transform.localPosition -= new Vector3(-1, 0) * distanceRetract;
        }
    }

    private void FixedUpdate()
    {
        GetComponentInChildren<Animator>().speed = animationSpeed;
    }

    void BumpSegments()
    {
        segments[0].transform.localPosition += new Vector3( 0,  1) * distanceBump;
        segments[1].transform.localPosition += new Vector3( 1,  0) * distanceBump;
        segments[2].transform.localPosition += new Vector3( 0, -1) * distanceBump;
        segments[3].transform.localPosition += new Vector3(-1,  0) * distanceBump;
    }
}
