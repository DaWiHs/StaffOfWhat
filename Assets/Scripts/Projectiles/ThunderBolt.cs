using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : MonoBehaviour
{
    public float explodeRadius;
    public GameObject lightningObject;
    bool look = true;

    public LayerMask collideWith;

    private void Update()
    {
        if (look && Physics2D.OverlapCircle(transform.position, 0.5f, collideWith))
        {
            look = false;
            Debug.Log("COLLISION: " + Physics2D.OverlapCircle(transform.position, 0.5f).name);
            Explode();
        }
    }

    void Explode()
    {
        lightningObject.SetActive(true);

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<Collider2D>().enabled = false;

        List<GameObject> received = new List<GameObject>();

        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, explodeRadius / 2);
        foreach (Collider2D collision in collisions)
        {
            if (received.Contains(collision.gameObject))
            {
                continue;
            }
            received.Add(collision.gameObject);
            if (collision.CompareTag("Entity"))
            {
                collision.GetComponent<Entity>().Lightning();
            }
            if (collision.CompareTag("Dragon"))
            {
                collision.GetComponentInParent<DragonScript>().Lightning();
            }
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().Lightning();
            }
            TODO.Here(); // Others fire functions
        }

        StartCoroutine(DecayParticles());

    }

    IEnumerator DecayParticles()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < GetComponentsInChildren<ParticleSystem>().Length; i++)
        {
            var main = GetComponentsInChildren<ParticleSystem>()[i].main;
            main.startSize = 0;
        }
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}