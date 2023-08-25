using UnityEngine;

public class ResetCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }
}
