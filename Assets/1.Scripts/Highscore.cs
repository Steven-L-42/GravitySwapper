using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Highscore : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    private PlayerDataFetcher playerDataFetcher;
    public GameObject username;
    public GameObject[] loginControls;

    void Start()
    {
        playerDataFetcher = FindObjectOfType<PlayerDataFetcher>();
        GetHighscore();
    }

    public void LogoutAccount()
    {
        try
        {
            loginControls[0].SetActive(true);
            loginControls[1].SetActive(true);
            loginControls[2].SetActive(false);
            username.GetComponent<TextMeshProUGUI>().text = "User: not logged in!";

            highScoreText.text = "NO HIGHSCORE";

            PlayerPrefs.DeleteKey("highscore");
            PlayerPrefs.DeleteKey("username");
            PlayerPrefs.DeleteKey("password");
            PlayerPrefs.Save();
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    private void OnPlayerDataReceived(PlayerData playerData, string note, Color color)
    {
        try
        {
            if (playerData != null) // User Found
            {
                loginControls[0].SetActive(false);
                loginControls[1].SetActive(false);
                loginControls[2].SetActive(true);
                username.GetComponent<TextMeshProUGUI>().text = "User: " + playerData.username;
                highScoreText.text = "HIGHSCORE " + playerData.highscore + " METERS";
            }
            else // User  not Found
            {
                loginControls[0].SetActive(true);
                loginControls[1].SetActive(true);
                loginControls[2].SetActive(false);
                username.GetComponent<TextMeshProUGUI>().text = "User: not found!";
                if (PlayerPrefs.HasKey("highscore"))
                    highScoreText.text = "HIGHSCORE " + PlayerPrefs.GetInt("highscore") + " METERS";
                else
                    highScoreText.text = "NO HIGHSCORE";
            }

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }



    }

    private void GetHighscore()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            playerDataFetcher.GetPlayerData(OnPlayerDataReceived);
            return;
        }
        else if (PlayerPrefs.HasKey("highscore"))
            highScoreText.text = "HIGHSCORE " + PlayerPrefs.GetInt("highscore") + " METERS";
        else
            highScoreText.text = "NO HIGHSCORE";
        loginControls[0].SetActive(true);
        loginControls[1].SetActive(true);
        loginControls[2].SetActive(false);
    }

}
