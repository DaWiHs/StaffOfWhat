using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public Image fade;

    public GameObject note;
    public GameObject menu;

    public Button playB;
    public Button noteB;
    public Button exitB;
    public Button noteBack;

    public bool toNote;

    public float fadeSize = 1000;
    public float fadeFreq = 0.01f;

    public void StartGame()
    {
        StartCoroutine(FadeOut());
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Notes()
    {
        if (note.activeSelf)
        {
            toNote = false;
        } else
        {
            toNote = true;
        }
        StartCoroutine(FadeInOut());

    }

    IEnumerator FadeInOut()
    {
        noteB.interactable = false;
        noteBack.interactable = false;
        playB.interactable = false;
        exitB.interactable = false;

        for (int i = 0; i < fadeSize; i++)
        {
            fade.color = new Color(0.095f, 0.095f, 0.095f, i / fadeSize);
            yield return new WaitForSeconds(fadeFreq);
        }

        yield return new WaitForSeconds(fadeFreq);

        if (note.activeSelf)
        {
            note.SetActive(false);
            menu.SetActive(true);
        } else
        {
            note.SetActive(true);
            menu.SetActive(false);
        }

        yield return new WaitForSeconds(fadeFreq);

        for (int i = (int)fadeSize; i > 0; i--)
        {
            fade.color = new Color(0.095f, 0.095f, 0.095f, i / fadeSize);
            yield return new WaitForSeconds(fadeFreq);
        }

        if (!toNote)
        {
            noteB.interactable = true;
            playB.interactable = true;
            exitB.interactable = true;
        } else
        {
            noteBack.interactable = true;
        }

    }

    IEnumerator FadeOut()
    {
        noteB.interactable = false;
        playB.interactable = false;
        exitB.interactable = false;


        for (int i = 0; i < fadeSize; i++)
        {
            fade.color = new Color(0.095f, 0.095f, 0.095f, i / fadeSize);
            yield return new WaitForSeconds(fadeFreq);
        }

        yield return new WaitForSeconds(fadeFreq);

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
