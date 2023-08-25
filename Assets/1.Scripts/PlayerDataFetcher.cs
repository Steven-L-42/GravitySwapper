using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDataFetcher : MonoBehaviour
{
    private void Awake()
    {
        if (_instanceFetcher != null && _instanceFetcher != this)
        {
            Destroy(gameObject); // Destroy this instance if another already exists
            return;
        }

        _instanceFetcher = this; // Set the singleton instance
        DontDestroyOnLoad(gameObject); // Preserve this GameObject across scene changes
    }
    public static PlayerDataFetcher _instanceFetcher;
    // Static singleton property
    public static PlayerDataFetcher _InstanceFetcher
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get { return _instanceFetcher ?? (_instanceFetcher = new GameObject("PlayerDataFetcher").AddComponent<PlayerDataFetcher>()); }
    }

    public PlayerData playerData = new();

    public void GetPlayerData(System.Action<PlayerData, string, Color> onDataReceived)
    {
        StartCoroutine(GetAccountCoroutine(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"), onDataReceived));
    }

    public void GetLeaderboard(System.Action<List<PlayerData>, string, Color> onDataReceived)
    {
        StartCoroutine(GetLeaderboardCoroutine(onDataReceived));
    }

    public void LoginPlayerData(string username, string password, System.Action<PlayerData, string, Color> onDataReceived)
    {
        StartCoroutine(GetAccountCoroutine(username, password, onDataReceived));
    }

    public void CreatePlayerData(string username, string password, int highscore, string clan, System.Action<string, Color> onDataReceived)
    {
        playerData = new PlayerData
        {
            username = username,
            password = password,
            highscore = highscore,
            clan = clan
        };
        StartCoroutine(CreateOrUpdateAccountCoroutine(true, onDataReceived));
    }

    public void UpdatePlayerData(string username, string password, int highscore, string clan, System.Action<string, Color> onDataReceived)
    {
        playerData = new PlayerData
        {
            username = username,
            password = password,
            highscore = highscore,
            clan = clan
        };
        StartCoroutine(CreateOrUpdateAccountCoroutine(false, onDataReceived));
    }

    private IEnumerator GetLeaderboardCoroutine(System.Action<List<PlayerData>, string, Color> onDataReceived)
    {
        string apiUrl = "https://about-steven.de/game/gravityswapper/get/leaderboard";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.ConnectionError && webRequest.result != UnityWebRequest.Result.ProtocolError)
            {
                string jsonString = webRequest.downloadHandler.text;

                List<PlayerData> receivedDataList = JsonConvert.DeserializeObject<List<PlayerData>>(jsonString);

                if (receivedDataList != null && receivedDataList.Count > 0)
                {
                    onDataReceived?.Invoke(receivedDataList, "Leaderboard loaded successfully.", new Color32(70, 255, 114, 255)); // Callback aufrufen
                }
                else
                {
                    playerData = null;
                    onDataReceived?.Invoke(null, "Leaderboard cannot be loaded.", new Color32(255, 218, 69, 255)); // Callback aufrufen
                }
            }
            else
            {
                Debug.LogError("Fehler beim Abrufen der Spielerdaten: " + webRequest.error);
            }
        }
    }


    private IEnumerator GetAccountCoroutine(string username, string password, System.Action<PlayerData, string, Color> onDataReceived)
    {
        string apiUrl = "https://about-steven.de/game/gravityswapper/get/" + username + "/" + password;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.ConnectionError && webRequest.result != UnityWebRequest.Result.ProtocolError)
            {
                string jsonString = webRequest.downloadHandler.text;

                List<PlayerData> receivedDataList = JsonConvert.DeserializeObject<List<PlayerData>>(jsonString);

                if (receivedDataList != null && receivedDataList.Count > 0)
                {
                    PlayerData receivedData = receivedDataList[0];
                    playerData = receivedData;
                    onDataReceived?.Invoke(receivedData, "Logged in successfully.", new Color32(70, 255, 114, 255)); // Callback aufrufen
                }
                else
                {
                    playerData = null;
                    onDataReceived?.Invoke(null, "Username does not exist!", new Color32(255, 218, 69, 255)); // Callback aufrufen

                }
            }
            else
            {
                Debug.LogError("Fehler beim Abrufen der Spielerdaten: " + webRequest.error);
            }
        }
    }

    private IEnumerator CreateOrUpdateAccountCoroutine(bool create, System.Action<string, Color> onDataReceived)
    {
        string postUrl;
        if (create)
            postUrl = "https://about-steven.de/game/gravityswapper/create";
        else
            postUrl = "https://about-steven.de/game/gravityswapper/update";

        string json = JsonConvert.SerializeObject(playerData);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest webRequest = new UnityWebRequest(postUrl, UnityWebRequest.kHttpVerbPOST))
        {
            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(data);
            uploadHandler.contentType = "application/json";
            webRequest.uploadHandler = uploadHandler;
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.ConnectionError && webRequest.result != UnityWebRequest.Result.ProtocolError)
            {
                string responseText = webRequest.downloadHandler.text;
                if (responseText.Equals("User already exists."))
                {
                    onDataReceived?.Invoke("Username already taken!", new Color32(255, 218, 69, 255));
                }
                else
                {
                    onDataReceived?.Invoke("Account created!", new Color32(70, 255, 114, 255));
                }
            }
            else
            {
                onDataReceived?.Invoke("Error: " + webRequest.error, new Color32(255, 218, 69, 255));
            }
        }
    }

}

[System.Serializable]
public class PlayerData
{
    public int id;
    public string username;
    public string password;
    public int highscore;
    public string clan;
    public string created_at;
    public string updated_at;

    public PlayerData()
    {
    }
}
