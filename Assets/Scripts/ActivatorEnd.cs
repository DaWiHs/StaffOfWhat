using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatorEnd : MonoBehaviour
{

    public static ActivatorEnd instance;

    public Image fadeImage;
    bool activated;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(Begin());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !activated)
        {
            activated = true;
            StartCoroutine(End());
        }
    }

    public IEnumerator Begin()
    {
        for (int i = 100; i > 0; i--)
        {
            fadeImage.color = new Color(0.094f, 0.094f, 0.094f, 1f * i / 100f);
            Debug.Log("I: " + 1f * i / 100f);
            yield return new WaitForSeconds(0f);
        }
    }

    public IEnumerator End()
    {
        PlayerController.instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        PlayerController.instance.forearmStaffAnim.enabled = false;
        PlayerController.instance.legsAnim.enabled = false;
        for (int i = 0; i < 100; i++)
        {
            fadeImage.color = new Color(0.094f, 0.094f, 0.094f, 1f * i / 100f);
            yield return new WaitForSeconds(0f);
        }

        PlayerPrefs.SetInt("Checkpoint", 0);
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("End");

    }

}
