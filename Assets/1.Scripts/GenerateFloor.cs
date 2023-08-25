using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenerateFloor : MonoBehaviour
{
    public GameObject controller;
    public GameObject player;
    public GameObject[] ground;
    public GameObject[] objectsPrefabs;
    public float speed = 4.0f; // Geschwindigkeit der Bewegung
    public int MetersCount = 1;
    private Scoring scoring;
    private List<Transform> groundsList = new();
    private List<Transform> objectsTopList = new();
    private List<Transform> objectsBottomList = new();
    private System.Random randomGeneratorTopObjects;
    private System.Random randomGeneratorBottomObjects;
    public int targetDistanceSpeed = 100;
    private int distance = 0;
    public float multiplyDistanceSpeed = 1.0f;
    void Start()
    {
        scoring = FindObjectOfType<Scoring>();
        randomGeneratorTopObjects = new System.Random();
        randomGeneratorBottomObjects = new System.Random();
        Instantiate(player, new Vector2(-5, 0), Quaternion.identity);
        controller.SetActive(true);
        SwapTouch_Behaviour attachedScript = controller.GetComponent<SwapTouch_Behaviour>();

        if (attachedScript != null)
        {
            attachedScript.Restart();
        }
        CreateInitialGrounds();
        StartCoroutine(MoveGrounds());
        StartCoroutine(CheckGrounds());
        StartCoroutine(GenerateTopObjects());
        StartCoroutine(GenerateBottomObjects());
        StartCoroutine(CountDistance());
        StartCoroutine(RemoveObjects());
    }
    
    private void Update()
    {
        if (scoring.distancePoints >= distance + targetDistanceSpeed / 2)
        {

            targetDistanceSpeed += targetDistanceSpeed;
            distance = scoring.distancePoints;
            multiplyDistanceSpeed += 0.2f;
            speed = 4f * multiplyDistanceSpeed;
        }
    }

    public void RemoveAndRestartAll()
    { 
        targetDistanceSpeed = 100;
        distance = 0;
        multiplyDistanceSpeed = 1.0f;
        speed = 4; 
        Time.timeScale = 1;

        if (objectsTopList.Count > 0)
            for (int i = objectsTopList.Count - 1; i >= 0; i--)
            {
                Destroy(objectsTopList[i].gameObject);
                objectsTopList.RemoveAt(i);
            }
        if (objectsBottomList.Count > 0)
            for (int i = objectsBottomList.Count - 1; i >= 0; i--)
            {
                Destroy(objectsBottomList[i].gameObject);
                objectsBottomList.RemoveAt(i);
            }
        if (groundsList.Count > 0)
        {
            for (int i = groundsList.Count - 1; i >= 0; i--)
            {
                Destroy(groundsList[i].gameObject);
                groundsList.RemoveAt(i);
            }
        }

        controller.SetActive(false);
        Instantiate(player, new Vector2(-5, 0), Quaternion.identity);
        controller.SetActive(true);
        SwapTouch_Behaviour attachedScript = controller.GetComponent<SwapTouch_Behaviour>();

        if (attachedScript != null)
        {
            attachedScript.Restart();
        }
        
        CreateInitialGrounds();
        StartCoroutine(MoveGrounds());
        StartCoroutine(CheckGrounds());
        StartCoroutine(GenerateTopObjects());
        StartCoroutine(GenerateBottomObjects());
        StartCoroutine(CountDistance());
        StartCoroutine(RemoveObjects());
    }

    void CreateInitialGrounds()
    {
        Vector3 initialPosition = Vector3.zero;

        for (int i = 0; i < 2; i++)
        {
            groundsList.Add(Instantiate(ground[0], initialPosition, Quaternion.identity).transform);
            initialPosition.x += 30;
        }
    }

    IEnumerator GenerateTopObjects()
    {   int seed;
        while (!scoring.gameStopped)
        {
            float randomWaitTime = Random.Range(1f, 3f);
            float randomFraction = (float)randomGeneratorTopObjects.NextDouble(); // Eigenen Random-Generator für TopSpikes verwenden

            float totalWaitTime = randomWaitTime + randomFraction;
            yield return new WaitForSeconds(totalWaitTime);
            seed = (int)System.DateTime.Now.Ticks;
            Random.InitState(seed); // Set the random seed
            float posY = 0;
            int nextObject = Random.Range(0, 6);
            switch (nextObject)
            {
                case 0:
                    posY = 4f;
                    break;
                case 1:
                    posY = 4.65f;
                    break;
                case 2:
                    posY = 4.65f;
                    break;
                case 3:
                    posY = 3.775f;
                    break;
                case 4:
                    posY = 4.65f;
                    break;
                case 5:
                    posY = 3.15f;
                    break;
            }
            objectsTopList.Add(Instantiate(objectsPrefabs[nextObject], new Vector2(30, posY), Quaternion.Euler(0, 0, 180)).transform);
        }
    }

    IEnumerator GenerateBottomObjects()
    {
        int seed;
        while (!scoring.gameStopped)
        {
            float randomWaitTime = Random.Range(1f, 3f);
            float randomFraction = (float)randomGeneratorBottomObjects.NextDouble(); // Eigenen Random-Generator für BottomSpikes verwenden

            float totalWaitTime = randomWaitTime + randomFraction;
            yield return new WaitForSeconds(totalWaitTime);
            seed = (int)System.DateTime.Now.Ticks;
            Random.InitState(seed); // Set the random seed
            float posY = 0;
            int nextObject = Random.Range(0, 7);
            switch (nextObject)
            {
                case 0 :
                    posY = -4f;
                    break;
                case 1:
                    posY = -4.65f;
                    break;
                case 2:
                    posY = -4.65f;
                    break;
                case 3:
                    posY = -3.775f;
                    break;
                case 4:
                    posY = -4.65f;
                    break;
                case 5:
                    posY = -3.15f;
                    break;
                case 6:
                    posY = 0f;
                    break;
            }
            objectsBottomList.Add(Instantiate(objectsPrefabs[nextObject], new Vector2(30, posY), objectsPrefabs[nextObject].transform.rotation).transform);
        }
    }

    IEnumerator RemoveObjects()
    {
        while (!scoring.gameStopped)
        {
            if (objectsTopList.Count > 0)
                if (objectsTopList[0].position.x < -30)
                {
                    Destroy(objectsTopList[0].gameObject);
                    objectsTopList.RemoveAt(0);
                }
            if (objectsBottomList.Count > 0)
                if (objectsBottomList[0].position.x < -30)
                {
                    Destroy(objectsBottomList[0].gameObject);
                    objectsBottomList.RemoveAt(0);
                }
            yield return null;
        }
    }

    IEnumerator CheckGrounds()
    {
        int seed;
        while (!scoring.gameStopped)
        {
            if (groundsList.Count > 0)
                if (groundsList[0].position.x < -30)
                {
                    Destroy(groundsList[0].gameObject);
                    groundsList.RemoveAt(0);
                    seed = (int)System.DateTime.Now.Ticks;
                    Random.InitState(seed); // Set the random seed
                    Vector3 newPosition = groundsList[groundsList.Count - 1].position + new Vector3(30, 0, 0);
                    groundsList.Add(Instantiate(ground[0], newPosition, Quaternion.identity).transform);
                }

            yield return null;
        }
    }

    IEnumerator CountDistance()
    {
        float posOld = 0;

        while (!scoring.gameStopped)
        {
            if (groundsList.Count > 0)
            {
                if (posOld == 0)
                    posOld = groundsList[0].position.x;
                else
                if (player.transform.position.x >= -5 && groundsList[0].position.x - posOld >= MetersCount || player.transform.position.x >= -5 && groundsList[0].position.x - posOld <= -MetersCount)
                {
                    posOld = groundsList[0].position.x;
                    scoring.addPoints();
                }
            }
            yield return null;
        }
    }

    IEnumerator MoveGrounds()
    {
        while (!scoring.gameStopped)
        {
            if (groundsList.Count > 0)
                foreach (Transform item in groundsList)
                {   if (item)
                        item.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                }
            if (objectsTopList.Count > 0)
                foreach (Transform item in objectsTopList)
                {
                    if (item)
                        item.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                }
            if (objectsBottomList.Count > 0)
                foreach (Transform item in objectsBottomList)
                {
                    if (item)
                        item.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                }
            yield return null;
        }
    }
}