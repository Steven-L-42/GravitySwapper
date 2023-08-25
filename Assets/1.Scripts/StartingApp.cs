using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingApp : MonoBehaviour
{
    private Image image;
    public float speed = 0.5f;
    void Awake()
    {
        Application.targetFrameRate = 60;
        image = GetComponent<Image>();
        StartCoroutine(BlurOutScreen());
    }
   
    IEnumerator BlurOutScreen()
    {
        float start = 0;
        yield return new WaitForSeconds(0.25f);
        while (start < 10)
        {
            start += Time.deltaTime;
            image.color = Color.Lerp(image.color, new Color(0, 0, 0, 0), speed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(0, 0, 0, 0);
    }
}
