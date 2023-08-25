using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountLogin : MonoBehaviour
{
    public TextMeshProUGUI Information;
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField passwordrepeat;

    private async void CreateAccountOnPlayerDataReceived(string note, Color color)
    {
        try
        {
            
            Information.color = color;
            Information.text = note;
            if (note == "Account created!")
            {
                await Task.Delay(1000);
                LoginAccount();
            }
            else
            {
                gameObject.GetComponent<Button>().interactable = true;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void CreateAccount()
    {
        try
        {
            gameObject.GetComponent<Button>().interactable = false;
          
            if (username.text == string.Empty || username.text.Length < 3)
            {
                Information.color = new Color32(255, 218, 69, 255);
                Information.text = "Please enter a username with at least 3 characters.";
                return;
            }
            if (password.text.Length < 3)
            {
                Information.color = new Color32(255, 218, 69, 255);
                Information.text = "Please enter a password with at least 3 characters.";
                return;
            }
            if (password.text != passwordrepeat.text)
            {
                Information.color = new Color32(255, 218, 69, 255);
                Information.text = "Passwords do not match!";
                return;
            }
           
            PlayerPrefs.SetString("username", username.text);
            PlayerPrefs.SetString("password", password.text);
            if (!PlayerPrefs.HasKey("highscore"))
                PlayerPrefs.SetInt("highscore", 0);
            PlayerPrefs.Save();
            PlayerDataFetcher._instanceFetcher.CreatePlayerData(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"), PlayerPrefs.GetInt("highscore"), "No Clan", CreateAccountOnPlayerDataReceived);
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private async void LoginAccountOnPlayerDataReceived(PlayerData playerData, string note, Color color)
    {
        try
        {
            Information.color = color;
            Information.text = note;
            if (note == "Logged in successfully.")
            {
                PlayerPrefs.SetString("username", playerData.username);
                PlayerPrefs.SetString("password", playerData.password);

                if (PlayerPrefs.HasKey("highscore"))
                {
                    if (PlayerPrefs.GetInt("highscore") < playerData.highscore)
                        PlayerPrefs.SetInt("highscore", playerData.highscore);
                }
                else
                    PlayerPrefs.SetInt("highscore", 0);
                PlayerPrefs.Save();

                await Task.Delay(1500);
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                gameObject.GetComponent<Button>().interactable = true;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void LoginAccount()
    {
        try
        {
            gameObject.GetComponent<Button>().interactable = false;
            if (username.text == string.Empty || username.text.Length < 3)
            {
                Information.color = new Color32(255, 218, 69, 255);
                Information.text = "Please enter a username with at least 3 characters.";
                return;
            }
            if (password.text.Length < 3)
            {
                Information.color = new Color32(255, 218, 69, 255);
                Information.text = "Please enter a password with at least 3 characters.";
                return;
            }

            PlayerDataFetcher._instanceFetcher.LoginPlayerData(username.text, password.text, LoginAccountOnPlayerDataReceived);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }
}