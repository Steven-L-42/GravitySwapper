using System.Collections;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public bool gameStopped = false;
    private int maxSize = 45;
    private int minSize = 40;
    private GenerateFloor generateFloor;
    public int distancePoints = 0;
    public int distanceMultiplikator = 1;
    public TextMeshProUGUI score_Text;
    public GameObject Head_Field;
    public GameObject gameover_Screen;
    private AudioSource audioSource;
    public AudioSource audioSourceCanvas;
    public AudioClip end_explosion;
   


    private void Start()
    {
    
        audioSource = GetComponent<AudioSource>();
        generateFloor = FindObjectOfType<GenerateFloor>();
    }
    public void pauseGame()
    {
        audioSourceCanvas.Pause();
        Time.timeScale = 0;
    }
    public void resumeGame()
    {
        audioSourceCanvas.Play();
        Time.timeScale = 1;
    }
    public void addPoints(int point = 1)
    {
        distancePoints += point * distanceMultiplikator;
        score_Text.text = (distancePoints + " METERS").ToString();
    }
    public float explSound = 0.6f;

    private void CreateAccountOnPlayerDataReceived(string note, Color color)
    {
        try
        {
            Debug.Log(note);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void gameOver()
    {
        try
        {
            string newHighscore;
            if (PlayerPrefs.HasKey("highscore"))
            {
                Debug.Log(distancePoints);
                Debug.Log(PlayerPrefs.GetInt("highscore"));
                if (distancePoints > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", distancePoints);
                    PlayerPrefs.Save();
                    Debug.Log("test1");
                    if (PlayerPrefs.HasKey("username"))
                        PlayerDataFetcher._instanceFetcher.UpdatePlayerData(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"), PlayerPrefs.GetInt("highscore"), PlayerDataFetcher._instanceFetcher.playerData.clan, CreateAccountOnPlayerDataReceived);
                    Debug.Log("test2");
                    newHighscore = "NEW HIGHSCORE " + PlayerPrefs.GetInt("highscore") + " METERS";
                    Debug.Log(newHighscore);
                }
                else
                    newHighscore = "you reached " + score_Text.text;
            }
            else
            {
                PlayerPrefs.SetInt("highscore", distancePoints);
                PlayerPrefs.Save();
                if (PlayerPrefs.HasKey("username"))
                    PlayerDataFetcher._InstanceFetcher.UpdatePlayerData(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"), PlayerPrefs.GetInt("highscore"), PlayerDataFetcher._InstanceFetcher.playerData.clan, CreateAccountOnPlayerDataReceived);
                newHighscore = "you reached " + PlayerPrefs.GetInt("highscore") + " METERS";
            }

            gameover_Screen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "HIGHSCORE " + PlayerPrefs.GetInt("highscore") + " METERS";

            audioSourceCanvas.Stop();
            audioSource.PlayOneShot(end_explosion, explSound);
            gameStopped = true;
            generateFloor.StopAllCoroutines();
            Time.timeScale = 0;

            gameover_Screen.SetActive(true);
            Head_Field.SetActive(false);
      
            gameover_Screen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = newHighscore;
            StartCoroutine(RestartButtonAnimation());
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void restartFromPause()
    {
        try
        {
            GameObject[] oldPlayer = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < oldPlayer.Length; i++)
            {
                Destroy(oldPlayer[i]);
            }

            if (PlayerPrefs.HasKey("highscore"))
            {
                if (distancePoints > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", distancePoints);
                    PlayerPrefs.Save();

                    if (PlayerPrefs.HasKey("username"))
                        PlayerDataFetcher._InstanceFetcher.UpdatePlayerData(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"), PlayerPrefs.GetInt("highscore"), "No Clan", CreateAccountOnPlayerDataReceived);
                }
            }
            else
            {
                PlayerPrefs.SetInt("highscore", distancePoints);
                PlayerPrefs.Save();
                if (PlayerPrefs.HasKey("username"))
                    PlayerDataFetcher._InstanceFetcher.UpdatePlayerData(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"), PlayerPrefs.GetInt("highscore"), "No Clan", CreateAccountOnPlayerDataReceived);
            }

            gameStopped = true;
            generateFloor.StopAllCoroutines();
            audioSourceCanvas.Stop();
            audioSourceCanvas.Play();
            gameStopped = false;
            distancePoints = 0;
            score_Text.text = (distancePoints + " METERS").ToString();
            Head_Field.SetActive(true);
         
            generateFloor.RemoveAndRestartAll();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void restart()
    {
        GameObject[] oldPlayer = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < oldPlayer.Length; i++)
        {
            Destroy(oldPlayer[i]);
        }
        audioSourceCanvas.Play();
        gameStopped = false;
        distancePoints = 0;
        score_Text.text = (distancePoints + " METERS").ToString();
        Head_Field.SetActive(true);
        generateFloor.RemoveAndRestartAll();
    }

    IEnumerator RestartButtonAnimation()
    {
        TextMeshProUGUI restartButtonText = gameover_Screen.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        while (gameStopped)
        {
            float startSize = minSize;
            float endSize = maxSize;

            float timeElapsed = 0f;
            float lerpDuration = 1f; // Time to complete one cycle (from small to big and back)

            while (timeElapsed < lerpDuration)
            {
                float t = Mathf.PingPong(timeElapsed, lerpDuration) / lerpDuration;
                restartButtonText.fontSize = Mathf.Lerp(startSize, endSize, t);

                timeElapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            timeElapsed = 0f;

            while (timeElapsed < lerpDuration)
            {
                float t = Mathf.PingPong(timeElapsed, lerpDuration) / lerpDuration;
                restartButtonText.fontSize = Mathf.Lerp(endSize, startSize, t);

                timeElapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            yield return null;
        }

    }

}