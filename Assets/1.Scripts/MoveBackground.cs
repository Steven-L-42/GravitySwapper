using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public GameObject[] backgroundObjects;
    private List<Transform> cloudsList = new();
    private List<Transform> grassesList = new();
    // Start is called before the first frame update
    public float speedCloud = 0.5f;
    public float speedGrass = 1f;
    void Start()
    {
        CreateInitialBackground();
    }


    void CreateInitialBackground()
    {
        Vector2 initialPosition = new(-10, -3.5f);
     
        for (int i = 0; i < 3; i++)
        {
            cloudsList.Add(Instantiate(backgroundObjects[0], initialPosition, Quaternion.identity, transform).transform);
            initialPosition.x += 20f;
        }

        initialPosition = new Vector2(0, -1);

        for (int i = 0; i < 3; i++)
        {
            grassesList.Add(Instantiate(backgroundObjects[1], initialPosition, Quaternion.identity, transform).transform);
            initialPosition.x += 20f;
        }

        StartCoroutine(MoveBackgrounds());
        StartCoroutine(CheckBackgrounds());
    }

    IEnumerator CheckBackgrounds()
    {
        while (true)
        {
            if (cloudsList.Count > 0)
                if (cloudsList[0].position.x < -30)
                {
                    Destroy(cloudsList[0].gameObject);
                    cloudsList.RemoveAt(0);
                   
                    Vector3 newPosition = cloudsList[cloudsList.Count - 1].position + new Vector3(20, 0, 0);
                    cloudsList.Add(Instantiate(backgroundObjects[0], newPosition, Quaternion.identity, transform).transform);
                }
            if (grassesList.Count > 0)
                if (grassesList[0].position.x < -30)
                {
                    Destroy(grassesList[0].gameObject);
                    grassesList.RemoveAt(0);

                    Vector3 newPosition = grassesList[grassesList.Count - 1].position + new Vector3(20, 0, 0);
                    grassesList.Add(Instantiate(backgroundObjects[1], newPosition, Quaternion.identity, transform).transform);
                }

            yield return null;
        }
    }
    IEnumerator MoveBackgrounds()
    {
        while (true)
        {
            if (cloudsList.Count > 0)
                foreach (Transform item in cloudsList)
                {
                    if (item)
                        item.position -= new Vector3(speedCloud * Time.deltaTime, 0, 0);
                }

            if (grassesList.Count > 0)
                foreach (Transform item in grassesList)
                {
                    if (item)
                        item.position -= new Vector3(speedGrass * Time.deltaTime, 0, 0);
                }

            yield return null;
        }
       
    }
    
}
