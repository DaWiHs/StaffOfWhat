using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regen : MonoBehaviour
{
    List<GameObject> hits;
    bool look = true;
    public LayerMask collideWith;

    private void Update()
    {
        if (look && Physics2D.OverlapCircle(transform.position, 0.5f, collideWith))
        {
            look = false;
            Debug.Log("COLLISION: " + Physics2D.OverlapCircle(transform.position, 0.5f, collideWith).name);
            Hit();
        }

        StartCoroutine(ExpireTimeout());
    }

    private void Hit()
    {
        List<GameObject> received = new List<GameObject>();

        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius / 2 + 0.1f);
        foreach (Collider2D collision in collisions)
        {
            if (received.Contains(collision.gameObject))
            {
                continue;
            }
            received.Add(collision.gameObject);
            if (collision.CompareTag("Entity"))
            {
                collision.GetComponent<Entity>().Regen();
            }
            if (collision.CompareTag("Dragon"))
            {
                collision.GetComponentInParent<DragonScript>().leftRegenIterations += 5;
            }
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().Regen();
            }
            TODO.Here(); // Others fire functions
        }
        StartCoroutine(Expire());

    }

    IEnumerator ExpireTimeout()
    {
        yield return new WaitForSeconds(8);
        StartCoroutine(Expire());

    }

    public IEnumerator Expire()
    {
        Vector3 sizeNow = transform.localScale;
        for (int i = 100; i > 0; i--)
        {
            yield return new WaitForSeconds(0.01f);
            transform.localScale = sizeNow * i / 100f;
        }

        Destroy(gameObject);
    }

}
