using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderHit : MonoBehaviour
{
    public Transform lightning;
    public ParticleSystem impact;
    public Vector3[] targets = new Vector3[3];
    public Transform[] indicators = new Transform[3];
    public float scale = 5;
    // Start is called before the first frame update
    void Start()
    {
        targets[0] = Vector3.up * 14f * scale + Vector3.right * Random.Range(-5f, 5f) * scale;
        lightning.localPosition = targets[0];
        targets[1] = Vector3.up * 9f * scale + Vector3.right * Random.Range(-5f, 5f) * scale;
        targets[2] = Vector3.up * 4f * scale + Vector3.right * Random.Range(-5f, 5f) * scale;

        //indicators[0].localPosition = targets[0];
        //indicators[1].localPosition = targets[1];
        //indicators[2].localPosition = targets[2];

        StartCoroutine(Strike());
    }

    IEnumerator Strike()
    {

        yield return new WaitForSeconds(0.02f);

        lightning.localPosition = targets[1];
        yield return new WaitForSeconds(0.02f);

        lightning.localPosition = targets[2];
        yield return new WaitForSeconds(0.02f);

        lightning.localPosition = Vector3.zero;
        impact.Play();
    }
}
