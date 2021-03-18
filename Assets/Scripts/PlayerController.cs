using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Targeting")]
    public Animator forearmStaffAnim;
    public Transform forearmLeft;
    public Transform foreaarLeftLookAt;
    public Transform head;
    public Transform mousePosition;
    public float rotZAddition;
    public LayerMask projectileMask;

    [Header("Movement")]
    public CharacterController2D controller;
    public Transform legsParent;
    public Animator legsAnim;
    public float speed;
    public bool inControl;

    [Header("UI")]
    public Image damageTint;
    public float tintDecrease;
    public Transform heartsParent;


    [Header("Player")]
    public Transform stomach;
    private int scaleX = 1;
    private Vector3 scaleReference;
    public int health = 10;

    public GameObject burn;
    public GameObject regen;
    public int leftBurnIterations = 0;
    public int leftRegenIterations = 0;

    public GameObject segmented;
    public GameObject player;
    public bool dead;

    public bool checkpoint;

    [Space]
    [Header("Casting")]
    public Transform objectZero;
    public Transform castOrigin;
    public GameObject castProjectile;
    public GameObject[] castProjectiles;
    public float timeOutCast;
    public float castForce;
    public float castCooldown;
    public bool castable;
    public GameObject phoque, rabbit;
    public GameObject morphExplosion;

    [Header("Audio")]
    public AudioPlayer staffCast;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Debug.Log("Instance created, destroying!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scaleReference = transform.localScale;

        InvokeRepeating("TakeEffects", 0, 0.5f);

        castCooldown = 0.75f / 1.5f;
        castable = true;

        if (PlayerPrefs.HasKey("Checkpoint"))
        {
            if (PlayerPrefs.GetInt("Checkpoint") == 1)
            {
                transform.position = new Vector3(36, 2, 0);
            }
        }

        //StartCoroutine(BossLair());
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !dead)
        {
            dead = true;
            StopAllCoroutines();
            StartCoroutine(Dead());
        }
        if (!dead)
        {
            if (Input.GetMouseButtonDown(1) && castable)
            {
                castable = false;
                StartCoroutine(Cast());
                StartCoroutine(CastCooldown());
            }


            if (inControl)
            {
                float inH = Input.GetAxis("Horizontal");
                if (inH > 0)
                {
                    legsAnim.SetBool("Walking", true);
                    legsAnim.SetFloat("SpeedModifier", 1 * scaleX);

                }
                else if (inH < 0)
                {
                    legsAnim.SetBool("Walking", true);
                    legsAnim.SetFloat("SpeedModifier", -1 * scaleX);
                }
                else
                {
                    legsAnim.SetBool("Walking", false);
                }
                controller.Move(inH * speed, Input.GetKeyDown(KeyCode.Space));
            }
            else
            {
                legsAnim.SetBool("Walking", false);
            }

        
            SetRotations();
            UIControl();
        }
    }

    private void SetRotations()
    {
        // Get Angle in Radians
        float AngleRad;
        if (mousePosition.localPosition.x < 0)
        {
            scaleX = -1;
            AngleRad = Mathf.Atan2(foreaarLeftLookAt.position.y - mousePosition.position.y, foreaarLeftLookAt.position.x - mousePosition.position.x);
        }
        else
        {
            scaleX = 1;
            AngleRad = Mathf.Atan2(mousePosition.position.y - foreaarLeftLookAt.position.y, mousePosition.position.x - foreaarLeftLookAt.position.x);
        }
        transform.localScale = new Vector3(scaleReference.x * scaleX, scaleReference.y, scaleReference.z);

        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        foreaarLeftLookAt.rotation = Quaternion.Euler(0, 0, AngleDeg + rotZAddition * scaleX);
        head.rotation = Quaternion.Euler(0, 0, AngleDeg);

        forearmLeft.rotation = Quaternion.Lerp(forearmLeft.rotation, foreaarLeftLookAt.rotation, 0.2f);

    }

    private void UIControl()
    {
        if (damageTint.color.a > 0f)
        {
            damageTint.color = new Color(damageTint.color.r, damageTint.color.g, damageTint.color.b, damageTint.color.a - 1f / 20f * tintDecrease);
        }
        int remHealth = 0;
        for (int i = 0; i < 5; i++)
        {
            if (i*2 + 2 <= health) {
                heartsParent.GetChild(i).GetChild(0).gameObject.SetActive(false);
                heartsParent.GetChild(i).GetChild(1).gameObject.SetActive(true);
                remHealth += 2;
            } else if (i*2 + 1 <= health)
            {
                heartsParent.GetChild(i).GetChild(0).gameObject.SetActive(true);
                heartsParent.GetChild(i).GetChild(1).gameObject.SetActive(false);
            } else
            {
                heartsParent.GetChild(i).GetChild(0).gameObject.SetActive(false);
                heartsParent.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    private void DamageTint(int str)
    {
        if (damageTint.color.a + 1f / 7f * str >= 1f)
        {
            damageTint.color = new Color(damageTint.color.r, damageTint.color.g, damageTint.color.b, 1f);
        }
        else
        {
            damageTint.color = new Color(damageTint.color.r, damageTint.color.g, damageTint.color.b, damageTint.color.a + 1f / 7f * str);
        }
    }

    private IEnumerator Cast()
    {
        forearmStaffAnim.SetTrigger("Cast");

        float roll = Random.Range(0f, 100f);

        yield return new WaitForSeconds(timeOutCast);

        // RNG GODS
        if (roll < 95f)
        {
            GameObject toCast = null;

            // Explosion | Lightning | Fire | Morph | Stone | Water | Time | Clone | Heal | 
            //     3           1        0      5       2                              4

            if (roll < 20f)                     //20
            {
                toCast = castProjectiles[0];
            }
            else if (roll < 40f)                //20
            {
                toCast = castProjectiles[1];
            }
            else if (roll < 67f)                //27
            {
                toCast = castProjectiles[2];
            }
            else if (roll < 82f)                //15
            {
                toCast = castProjectiles[3];
            }
            else if (roll < 94f)                //12
            {
                toCast = castProjectiles[4];
            }
            else if (roll < 95f)                //1
            {
                toCast = castProjectiles[5];
            }

            staffCast.PlaySound();

            Vector3 castDirection = (mousePosition.position - castOrigin.position).normalized;

            GameObject casted = Instantiate(toCast, castOrigin.position, Quaternion.identity, objectZero);
            Rigidbody2D rb = casted.GetComponent<Rigidbody2D>();
            rb.velocity = castDirection * castForce;


            Destroy(casted, 5);
        } else
        {
            // No cast
            castOrigin.GetComponent<ParticleSystem>().Play();
        }
    }

    private IEnumerator CastCooldown()
    {
        yield return new WaitForSeconds(castCooldown);
        if (Input.GetMouseButton(1))
        {
            castable = false;
            StartCoroutine(Cast());
            StartCoroutine(CastCooldown());
        } else
        {
            castable = true;
        }
    }

    public void Burn()
    {
        leftBurnIterations += 2;
    }

    public void Regen()
    {
        leftRegenIterations += 5;
    }

    public void Lightning()
    {
        health -= 2;
        DamageTint(3);
    }

    public void Explosion(Vector3 originPos)
    {
        inControl = false;
        transform.position += Vector3.up;
        controller.Explode(40f, (transform.position - originPos).normalized);
        health -= 1;
        DamageTint(1);
        //controller.Explode(100f, Vector3.up + Vector3.right);
    }

    public void Regaincontrol()
    {
        inControl = true;
        controller.m_AirControl = true;
    }

    public void StoneHit(Vector3 velocity)
    {
        if (velocity.magnitude > 8)
        {
            DamageTint(1);
            health--;
        }
        if (velocity.magnitude > 16)
        {
            DamageTint(1);
            health--;
        }
    }

    public void TakeDamage()
    {
        health -= 2;
        DamageTint(3);
    }

    void TakeEffects()
    {
        if (leftBurnIterations > 0)
        {
            burn.SetActive(true);
            DamageTint(1);
            leftBurnIterations--;
            health--;
        } else
        {
            burn.SetActive(false);
        }
        if (leftRegenIterations > 0)
        {
            regen.SetActive(true);
            DamageTint(-1);
            leftRegenIterations--;
            if (health++ > 10)
            {
                health = 10;
            }
        } else
        {
            regen.SetActive(false);
        }

    }

    public IEnumerator BossLair()
    {
        for (int i = 0; i < 100; i++)
        {
            Camera.main.orthographicSize = Mathf.Lerp(5f, 15f, i / 100f);
            BackgroundController.instance.bg2.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2.6f, i / 100f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public IEnumerator Dead()
    {
        player.SetActive(false);
        segmented.SetActive(true);
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < 100; i++)
        {
            ActivatorEnd.instance.fadeImage.color = new Color(0.094f, 0.094f, 0.094f, 1f * i / 100f);
            yield return new WaitForSeconds(0.01f);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");

    }

}
