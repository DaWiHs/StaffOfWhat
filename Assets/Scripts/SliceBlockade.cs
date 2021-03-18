
using UnityEngine;

public class SliceBlockade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D>();
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].velocity = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            rbs[i].AddTorque(Random.Range(-90f, 90f));
            Destroy(rbs[i].gameObject, 8f);
        }
        Destroy(gameObject, 9f);
    }

}
