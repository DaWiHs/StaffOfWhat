using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public LayerMask collideWith;
    public float explodeRadius = 8;
    public GameObject explosionPrefab;
    bool look = true;

    private void Update()
    {
        if (look && Physics2D.OverlapCircle(transform.position, 0.5f, collideWith))
        {
            look = false;
            Debug.Log("COLLISION: " + Physics2D.OverlapCircle(transform.position, 0.5f).name);
            Explode();
        }    
    }

    private void Explode()
    {
        Destroy(Instantiate(explosionPrefab, transform.position, Quaternion.identity, PlayerController.instance.objectZero), 2f);

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<Collider2D>().enabled = false;

        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, explodeRadius / 2f);

        List<GameObject> received = new List<GameObject>();

        foreach (Collider2D collision in collisions)
        {
            if (received.Contains(collision.gameObject))
            {
                continue;
            }
            received.Add(collision.gameObject);
            if (collision.CompareTag("Entity"))
            {
                collision.GetComponent<Entity>().Explosion(transform.position);
                continue;
            }
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().Explosion(transform.position);
                continue;
            }
            if (collision.CompareTag("Dragon"))
            {
                collision.GetComponentInParent<DragonScript>().Explosion(transform.position);
            }
            if (collision.GetComponent<Rigidbody2D>())
            {
                collision.GetComponent<Rigidbody2D>().velocity = ((collision.transform.position - transform.position + Vector3.up).normalized) * 40f;
            }
            if (collision.GetComponent<Blockade>())
            {
                collision.GetComponent<Blockade>().Explode();
            }
            TODO.Here(); // Others fire functions
        }

        StartCoroutine(DecayParticles());

    }

    IEnumerator DecayParticles()
    {
        Debug.Log("DECAY");
        yield return new WaitForSeconds(0f);

        for (int i = 0; i < GetComponentsInChildren<ParticleSystem>().Length; i++)
        {
            var main = GetComponentsInChildren<ParticleSystem>()[i].main;
            main.startSize = 0;
        }
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
