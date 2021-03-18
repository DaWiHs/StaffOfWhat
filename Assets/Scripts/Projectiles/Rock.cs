using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{


    List<GameObject> hits = new List<GameObject>();
    bool look = false;

    private void Start()
    {
        StartCoroutine(Expire());
    }

    private void Update()
    {
        if (look && Physics2D.OverlapCircle(transform.position, GetComponent<CircleCollider2D>().radius))
        {
            Debug.Log("COLLISION: " + Physics2D.OverlapCircle(transform.position, GetComponent<CircleCollider2D>().radius).name);
            Hit();
        }

    }

    private void Hit()
    {

        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius + 0.02f);
        foreach (Collider2D collision in collisions)
        {
            if (hits.Contains(collision.gameObject))
            {
                continue;
            }
            hits.Add(collision.gameObject);
            if (collision.CompareTag("Entity"))
            {
                collision.GetComponent<Entity>().StoneHit(GetComponent<Rigidbody2D>().velocity);
            }
            if (collision.CompareTag("Dragon"))
            {
                if (transform.localScale.x > 0.5f)
                {
                    collision.GetComponent<DragonScript>().Lightning();
                    collision.GetComponent<DragonScript>().Lightning();
                }
                collision.GetComponentInParent<DragonScript>().StoneHit(GetComponent<Rigidbody2D>().velocity);
            }
            if (collision.CompareTag("Player"))
            {
                if (transform.localScale.x > 0.5f)
                {
                    collision.GetComponent<PlayerController>().Lightning();
                }
                //collision.GetComponent<PlayerController>().StoneHit(GetComponent<Rigidbody2D>().velocity);
            }
            TODO.Here(); // Others fire functions
        }

    }

    public IEnumerator Expire()
    {
        yield return new WaitForSeconds(0.1f);
        look = true;
        yield return new WaitForSeconds(8);
        Vector3 sizeNow = transform.localScale;
        for (int i = 100; i > 0; i--)
        {
            yield return new WaitForSeconds(0.01f);
            transform.localScale = sizeNow * i / 100f;
        }

        Destroy(gameObject);
    }

}
