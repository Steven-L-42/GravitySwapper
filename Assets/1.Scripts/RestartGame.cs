using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private int currentSceneIndex;
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartWholeGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(currentSceneIndex);
       
    }
}
