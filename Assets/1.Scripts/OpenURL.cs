using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/steven.lippert_official/");
    }
}
