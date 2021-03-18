using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScript : MonoBehaviour
{
    public string[] texts;
    public int textIndex = 0;
    public string[] lastTexts;
    public Text text;
    public Image fadeImage;
    public Image textImage;
    public float charFreq;
    public bool clickable;
    public GameObject clickHint;
    public Transform staff;
    public AudioPlayer tickPlayer;
    public SpriteRenderer blinder;
    public GameObject blinder2;

    // Start is called before the first frame update
    void Start()
    {
        staff.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-3f, 3f));   
        staff.transform.rotation = Quaternion.Euler(0,0,Random.Range(-60f, 60f));
        text.text = "";
        StartCoroutine(Begin());
    }

    // Update is called once per frame
    void Update()
    {
        if (clickable && Input.anyKeyDown)
        {

            clickable = false;
            clickHint.SetActive(false);
            if (++textIndex >= texts.Length)
            {
                StartCoroutine(CloseUp());
            } else
            {
                StartCoroutine(Typing());
            }
        }
    }

    public IEnumerator Typing()
    {
        text.text = "";
        for (int i = 0; i < texts[textIndex].Length; i++)
        {
            text.text = "";
            for (int k = 0; k <= i; k++) {
                text.text += texts[textIndex][k];
            }
            if (texts[textIndex][i] != ' ')
            {
                tickPlayer.PlaySound(0);
            }
            yield return new WaitForSeconds(charFreq);
        }

        clickHint.SetActive(true);
        clickable = true;
    }

    public IEnumerator CloseUp()
    {
        text.text = "";

        //textImage.gameObject.SetActive(false);

        for (int i = 0; i < 150; i++) {
            Camera.main.orthographicSize = Mathf.Lerp(5f, 1f, i / 150f);
            Camera.main.transform.position = Vector3.Lerp(Vector3.zero + Vector3.back * 10f, staff.GetChild(0).position + Vector3.back * 10f, i / 150f);
            yield return new WaitForSeconds(0.01f);
        }
        text.text = "";
        //textImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);


        //textImage.gameObject.SetActive(true);

        for (int i = 0; i < lastTexts[0].Length; i++)
        {
            text.text = "";
            for (int k = 0; k <= i; k++)
            {
                text.text += lastTexts[0][k];
            }
            if (lastTexts[0][i] != ' ')
            {
                tickPlayer.PlaySound(0);
            }
            yield return new WaitForSeconds(charFreq * 4f);
        }

        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 100; i++) {
            blinder.color = new Color(0, 0, 0, 1f * i / 100f);
            yield return new WaitForSeconds(0f);
        }
        blinder2.SetActive(true);
        yield return new WaitForSeconds(1f);

        StartCoroutine(End());

    }

    public IEnumerator Begin()
    {
        for (int i = 100; i > 0; i--)
        {
            fadeImage.color = new Color(0.094f, 0.094f, 0.094f, 1f * i / 100f);
            Debug.Log("I: " + 1f * i / 100f);
            yield return new WaitForSeconds(0f);
        }

        StartCoroutine(Typing());
    }

    public IEnumerator End()
    {
        for (int i = 0; i < 100; i++)
        {
            fadeImage.color = new Color(0.094f, 0.094f, 0.094f, 1f * i / 100f);
            yield return new WaitForSeconds(0f);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");

    }
}
