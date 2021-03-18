using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform mouse;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posBetween = (mouse.position + player.position) / 2;
        if (Vector3.Distance(transform.position, posBetween + Vector3.forward * transform.position.z) > 0.2f)
        {
            transform.position = Vector3.Lerp(transform.position, posBetween + Vector3.forward * transform.position.z, 0.1f);
        }
    }
}
