using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FillLeaderboard : MonoBehaviour
{
    public Transform leaderboard_content;
    public GameObject item_Prefab;
    public GameObject noAccountRegistred;
    public GameObject yourRank_Text;
    private List<PlayerData> playerData;
    void Start()
    {
        PlayerDataFetcher._InstanceFetcher.GetLeaderboard(GetLeaderboardOnPlayerDataReceived);
    }

    private void GetLeaderboardOnPlayerDataReceived(List<PlayerData> playerData, string note, Color color)
    {
        try
        {
            this.playerData = playerData;
            this.playerData.Sort((item1, item2) => item1.highscore.CompareTo(item2.highscore));
            this.playerData.Reverse();
            FillLeaderboard_with_Content();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private List<TextMeshProUGUI[]> itemTextComponentsList = new List<TextMeshProUGUI[]>();
    private List<Image[]> itemImageComponentsList = new List<Image[]>();
    void FillLeaderboard_with_Content()
    {

        // Alle bestehenden Items unter scrollViewContent zerstören
        for (int i = 0; i < leaderboard_content.childCount; i++)
        {
            Destroy(leaderboard_content.GetChild(i).gameObject);
        }

        // Die Höhe des Inhalts basierend auf der Anzahl der Items und ihrer Höhe berechnen
        float contentHeight = CalculateContentHeight(playerData.Count, item_Prefab.GetComponent<RectTransform>().rect.height);

        // Die Größe des Inhalts anpassen
        RectTransform contentRectTransform = leaderboard_content.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, contentHeight - 780);

        int j = 0;
        int yourRank = 0;
        bool youAreRanked = false;
        string tempUsername = string.Empty;

        if (PlayerPrefs.HasKey("username"))
            tempUsername = PlayerPrefs.GetString("username");

        foreach (var item in playerData)
        {
            int currentIndex = j;
            // Ein neues Item aus dem Prefab erstellen
            GameObject newItem = Instantiate(item_Prefab, leaderboard_content);

            // Die TMP Text-Komponenten im Prefab finden und zur Liste hinzufügen
            TextMeshProUGUI[] tmpTextComponents = newItem.GetComponentsInChildren<TextMeshProUGUI>();
            itemTextComponentsList.Add(tmpTextComponents);
            Image[] tmpImageComponents = newItem.GetComponentsInChildren<Image>();
            itemImageComponentsList.Add(tmpImageComponents);

            switch (currentIndex + 1)
            {
                case 1:
                    tmpImageComponents[1].color = new Color32(255, 230, 93, 255);
                    break;
                case 2:
                    tmpImageComponents[1].color = new Color32(142, 136, 136, 255);
                    break;
                case 3:
                    tmpImageComponents[1].color = new Color32(178, 178, 178, 255);
                    break;
               default:
                    tmpImageComponents[1].color = new Color32(141, 190, 255, 255);
                    break;
            }
           
            tmpTextComponents[0].text = currentIndex + 1 + ".";
            tmpTextComponents[1].text = item.username;
            tmpTextComponents[2].text = item.clan;
            tmpTextComponents[3].text = item.highscore + " M";

            if (tempUsername != string.Empty)
                if (item.username == tempUsername)
                {
                    yourRank = currentIndex + 1;
                    youAreRanked = true;
                }

                //itemButton[1].onClick.AddListener(() => BuyIncrement(currentIndex));

                //CenterTextAndImageBasedOnTextSize(tmpTextComponents[5], itemImages[5]);

                // Die Position des neuen Items setzen, um sie untereinander anzuordnen
                RectTransform itemTransform = newItem.GetComponent<RectTransform>();
            float itemHeight = itemTransform.rect.height;
            itemTransform.anchoredPosition = new Vector2(0, (-j * itemHeight) - 61);
            j++;
        }

        if (youAreRanked)
        {
            noAccountRegistred.SetActive(false);
            yourRank_Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Your Leaderboard Rank: " + yourRank;
            yourRank_Text.SetActive(true);
        }
        else
        {
            yourRank_Text.SetActive(false);
            noAccountRegistred.SetActive(true);
        }
    }

    // Eine Funktion, um die Höhe des Inhalts basierend auf der Anzahl der Items und ihrer Höhe zu berechnen
    private float CalculateContentHeight(int itemCount, float itemHeight)
    {
        return itemCount * itemHeight;
    }
    //private void CenterTextAndImageBasedOnTextSize(TextMeshProUGUI coinText, Image coinImage)
    //{
    //    // Breite des TMP-Textes erhalten
    //    float textWidth = coinText.preferredWidth;

    //    // Breite des Bildes erhalten (angenommen, es ist ein RectTransform)
    //    float imageWidth = coinImage.GetComponent<RectTransform>().rect.width;

    //    // Neue Position für TMP-Text berechnen, um ihn horizontal zu zentrieren
    //    Vector2 newTextPosition = new Vector2(-imageWidth / 2, coinText.GetComponent<RectTransform>().anchoredPosition.y);

    //    // Neue Position für das Bild berechnen, um es rechts neben dem TMP-Text zu platzieren
    //    Vector2 newImagePosition = newTextPosition + new Vector2(textWidth / 2, 0f) + new Vector2(imageWidth / 2, 0f);
    //    if (newImagePosition.x > 55)
    //        newImagePosition = new Vector2(55, coinText.GetComponent<RectTransform>().anchoredPosition.y);
    //    // Die neuen Positionen auf TMP-Text und Bild anwenden
    //    coinText.GetComponent<RectTransform>().anchoredPosition = newTextPosition;
    //    coinImage.GetComponent<RectTransform>().anchoredPosition = newImagePosition;
    //}
}
