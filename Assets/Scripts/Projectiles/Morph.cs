using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morph : MonoBehaviour
{

    public float explodeRadius;
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

    private void Explode()
    {

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<Collider2D>().enabled = false;

        List<GameObject> received = new List<GameObject>();


        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, explodeRadius / 2, PlayerController.instance.projectileMask);
        foreach (Collider2D collision in collisions)
        {
            if (received.Contains(collision.gameObject))
            {
                continue;
            }
            received.Add(collision.gameObject);
            if (collision.CompareTag("Entity"))
            {
                collision.GetComponent<Entity>().Morph();
                Destroy(gameObject);
            }
            if (collision.CompareTag("Dragon"))
            {
                collision.GetComponentInParent<DragonScript>().Morph();
                Destroy(gameObject);
            }
            TODO.Here(); // Others fire functions
        }

        StartCoroutine(DecayParticles());

    }

    IEnumerator DecayParticles()
    {
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
