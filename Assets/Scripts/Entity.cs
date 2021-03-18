using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public float seekRadius;
    public float damageRedius;
    public bool targetPlayer;
    public bool aggresive;

    public int health = 6;
    public int leftBurnIterations = 0;
    public int leftRegenIterations = 0;
    public GameObject burn;
    public GameObject regen;

    public float toMove;
    public bool moveRight;
    public bool attacking;
    public bool hasControl;
    public float speed;
    public Transform attackOrigin;
    public Vector3 attackBoxScale;
    Vector3 posBefore;
    Animator anim;
    float dist;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        posBefore = transform.position;

        StartCoroutine(TakeDecision());
        InvokeRepeating("TakeEffects", 0, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasControl)
        {
            if (Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), 1 << LayerMask.NameToLayer("Ground")))
            {
                hasControl = true;
            }
            return;
        }
        if (health <= 0)
        {
            transform.localScale -= Vector3.one * 0.01f;
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
            return;
        }
        dist = Vector2.Distance(transform.position, PlayerController.instance.transform.position);
        if (dist < seekRadius)
        {
            targetPlayer = true;

            if (dist < damageRedius && !attacking)
            {
                attacking = true;
                StartCoroutine(Attack());
                anim.SetTrigger("Attack");
                anim.SetBool("Walking", false);
            }
            else if (!attacking)
            {
                GetComponent<Rigidbody2D>().velocity = 
                    new Vector3(PlayerController.instance.transform.position.x - transform.position.x, GetComponent<Rigidbody2D>().velocity.y, 0).normalized * speed;

                anim.SetBool("Walking", true);
            }
        }
        else
        {
            if (toMove > 0 && !attacking)
            {
                if (moveRight)
                {
                    GetComponent<Rigidbody2D>().velocity = Vector3.right * speed + Vector3.up * GetComponent<Rigidbody2D>().velocity.y;
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = Vector3.left * speed + Vector3.up * GetComponent<Rigidbody2D>().velocity.y;
                }
                anim.SetBool("Walking", true);

                toMove -= (posBefore - transform.position).magnitude;

                posBefore = transform.position;
            }
            else
            {
                anim.SetBool("Walking", false);
            }
        }

        if (dist > seekRadius)
        {
            targetPlayer = false;
            attacking = false;
        }

        if (GetComponent<Rigidbody2D>().velocity.x < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        

    }

    IEnumerator TakeDecision()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));

        if (!targetPlayer)
        {
            toMove = Random.Range(3f, 8f);
            if (Random.Range(0f,4f) > 2f)
            {
                moveRight = true;
            } else
            {
                moveRight = false;
            }
        }

        yield return new WaitForSeconds(Random.Range(2f, 4f));

        StartCoroutine(TakeDecision());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(55f / 60f);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackOrigin.position, attackBoxScale, 0, 1 << LayerMask.NameToLayer("Player"));

        List<GameObject> received = new List<GameObject>();

        foreach (Collider2D collider in colliders)
        {
            if (received.Contains(collider.gameObject))
            {
                continue;
            }
            received.Add(collider.gameObject);
            if (collider.GetComponent<PlayerController>())
            {
                PlayerController.instance.TakeDamage();
            }
        }
        yield return new WaitForSeconds((125f - 55f) / 60f);
        attacking = false;
    }

    public void Morph()
    {
        Destroy(Instantiate(PlayerController.instance.morphExplosion, transform.position, Quaternion.identity, PlayerController.instance.objectZero), 5);

        if (Random.Range(0f, 10f) > 5f)
        {
            Instantiate(PlayerController.instance.phoque, transform.position, Quaternion.identity, PlayerController.instance.objectZero);
        } else
        {
            Instantiate(PlayerController.instance.rabbit, transform.position, Quaternion.identity, PlayerController.instance.objectZero);
        }
        Destroy(gameObject);
    }

    public void Burn()
    {
        leftBurnIterations += 3;
    }

    public void Regen()
    {
        leftRegenIterations += 3;
    }

    public void Lightning()
    {
        health -= 3;
    }

    public void Explosion(Vector3 originPos)
    {
        health -= 2;
        hasControl = false;
        GetComponent<Rigidbody2D>().velocity = ((transform.position - originPos + Vector3.up).normalized) * 40f;
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
            burn.SetActive(true);
            leftBurnIterations--;
            health--;
        } else
        {
            burn.SetActive(false);
        }
        if (leftRegenIterations > 0)
        {
            regen.SetActive(true);
            leftRegenIterations--;
            health++;
        } else
        {
            regen.SetActive(false);
        }

    }

    public void Spawned()
    {
        StartCoroutine(SizeUp());
        transform.localScale = Vector3.right;
    }

    IEnumerator SizeUp()
    {
        for (int i = 0; i < 100; i++)
        {
            transform.localScale += Vector3.up / 100f;
            yield return new WaitForSeconds(0.04f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, seekRadius);
        Gizmos.DrawWireCube(attackOrigin.position, attackBoxScale);
    }

}
