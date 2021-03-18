using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragonScript : MonoBehaviour
{
    public static DragonScript instance;

    public Animator anim;

    public UnityEngine.UI.Image healthbar;

    public int health = 150;
    public int leftBurnIterations = 0;
    public int leftRegenIterations = 0;

    public GameObject[] projectiles;
    public Transform rocksSpawn;
    public Transform fireballsSpawn;
    public Transform minionsSpawn;

    public Transform endBlock;
    public bool dead;

    public AudioPlayer dragonFire;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TakeEffects", 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.fillAmount = health / 300f;
        if (!dead && health <= 0)
        {
            dead = true;
            HideAll();
        }
    }

    public IEnumerator Begin()
    {
        GameObject proj = Instantiate(projectiles[1], fireballsSpawn.position, Quaternion.identity, PlayerController.instance.objectZero);
        proj.GetComponent<Rigidbody2D>().velocity = ((PlayerController.instance.transform.position + Vector3.up * 6) - fireballsSpawn.position).normalized * 50f;
        proj.layer = LayerMask.NameToLayer("BossProjectiles");
        yield return new WaitForSeconds(0.1f);
        proj = Instantiate(projectiles[1], fireballsSpawn.position, Quaternion.identity, PlayerController.instance.objectZero);
        proj.GetComponent<Rigidbody2D>().velocity = ((PlayerController.instance.transform.position + Vector3.up) - fireballsSpawn.position).normalized * 50f;
        proj.layer = LayerMask.NameToLayer("BossProjectiles");
        yield return new WaitForSeconds(0.1f);
        proj = Instantiate(projectiles[1], fireballsSpawn.position, Quaternion.identity, PlayerController.instance.objectZero);
        proj.GetComponent<Rigidbody2D>().velocity = ((PlayerController.instance.transform.position + Vector3.up * 12) - fireballsSpawn.position).normalized * 50f;
        proj.layer = LayerMask.NameToLayer("BossProjectiles");
        yield return new WaitForSeconds(5f);
        
        StartCoroutine(Attack());
    }

    public IEnumerator Attack()
    {
        Debug.Log("Dragon Attack");
        float roll = Random.Range(0f, 100f);

        if (roll < 40f)
        {
            // Fireballs
            anim.Play("Fireballs");
            yield return new WaitForSeconds(0.6f);
            for (int i = 0; i < 10; i++)
            {
                GameObject proj = Instantiate(projectiles[0], fireballsSpawn.position, Quaternion.identity, PlayerController.instance.objectZero);
                proj.GetComponent<Rigidbody2D>().velocity = (PlayerController.instance.transform.position - fireballsSpawn.position).normalized * 30f;
                proj.layer = LayerMask.NameToLayer("BossProjectiles");
                dragonFire.PlaySound(0);
                yield return new WaitForSeconds(0.4f);
            }

        }
        else if (roll < 80f)
        {
            // Rocks
            anim.Play("Rocks");
            yield return new WaitForSeconds(0.1f);

            dragonFire.PlaySound(1);

            for (int i = 0; i < 20; i++)
            {
                GameObject proj = Instantiate(projectiles[2], rocksSpawn.position + Vector3.right * Random.Range(-20, 30), Quaternion.identity, PlayerController.instance.objectZero);
                proj.GetComponent<Rigidbody2D>().velocity = Vector3.down * 4f;
                proj.layer = LayerMask.NameToLayer("BossProjectiles");
                yield return new WaitForSeconds(0.6f);
                Destroy(proj, 10);
            }
        }
        else if (roll < 100f)
        {
            // Spawn Minions

            anim.Play("Minions");
            yield return new WaitForSeconds(0.1f);
            dragonFire.PlaySound(2);

            for (int i = 0; i < 10; i++)
            {
                GameObject proj = Instantiate(projectiles[3], minionsSpawn.position + Vector3.right * Random.Range(-20, 30), Quaternion.identity, PlayerController.instance.objectZero);
                proj.transform.localScale = Vector3.zero;
                proj.GetComponent<Entity>().Spawned();
                
                yield return new WaitForSeconds(0.5f);
            }


        }



        yield return new WaitForSeconds(4f);
        StartCoroutine(Attack());
    }

    public void Morph()
    {
        if (Random.Range(0f, 1000f) < 1f)
        {
            Destroy( Instantiate(PlayerController.instance.morphExplosion, transform.position, Quaternion.identity, PlayerController.instance.objectZero), 5);
            Destroy( Instantiate(PlayerController.instance.morphExplosion, transform.position, Quaternion.identity, PlayerController.instance.objectZero), 5);

            HideAll();

            if (Random.Range(0f, 10f) > 5f)
            {
                Instantiate(PlayerController.instance.phoque, transform.position, Quaternion.identity, PlayerController.instance.objectZero);
            }
            else
            {
                Instantiate(PlayerController.instance.rabbit, transform.position, Quaternion.identity, PlayerController.instance.objectZero);
            }

        }
    }

    public void Burn()
    {
        leftBurnIterations += 3;
    }

    public void Lightning()
    {
        health -= 3;
    }

    public void Explosion(Vector3 originPos)
    {
        health -= 2;
    }

    public void StoneHit(Vector3 velocity)
    {
        if (velocity.magnitude > 8f)
        {
            Debug.Log("velocity: " + velocity.magnitude);
            health -= 2;
        }
    }

    void TakeEffects()
    {
        if (leftBurnIterations > 0)
        {
            leftBurnIterations--;
            health--;
        }
        if (leftRegenIterations > 0)
        {
            leftRegenIterations--;
            health++;
        }

    }

    public void HideAll()
    {
        StopAllCoroutines();

        anim.Play("Dead");
        dragonFire.PlaySound(2);
        
        foreach (Collider2D coll in GetComponentsInChildren<Collider2D>())
        {
            coll.enabled = false;
        }
        health = 0;

        StartCoroutine(AfterKill());

    }

    IEnumerator AfterKill()
    {
        yield return new WaitForSeconds(3f);

        anim.enabled = false;

        for (int i = 0; i < 100; i++)
        {
            transform.position += Vector3.down * 0.15f;
            endBlock.position += Vector3.up * 0.05f;
            yield return new WaitForSeconds(0f);
        }
        foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>())
        {
            rend.enabled = false;
        }

    }

}
