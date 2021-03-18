using UnityEngine;

public class Activator : MonoBehaviour
{
    bool triggered = false;
    public GameObject healthbar;

    public AudioPlayer bossAlarm;
    public AudioPlayer music;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            triggered = true;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(PlayerController.instance.BossLair());
            StartCoroutine(DragonScript.instance.Begin());
            healthbar.SetActive(true);
            for (int i = 0; i < 10000; i++)
            {
                healthbar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = i / 10000f;
            }
            bossAlarm.PlaySound(0);
            music.PlaySound(1);
            PlayerPrefs.SetInt("Checkpoint", 1);
            PlayerPrefs.Save();
        }

    }
}
