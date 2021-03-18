using UnityEngine;

public class Blockade : MonoBehaviour
{
    public GameObject sliced;
    public void Explode()
    {
        Instantiate(sliced, transform.position, transform.rotation, PlayerController.instance.objectZero).transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}
