using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

public class Shaking : MonoBehaviour
{
    public ShakeData myShake;

    public void Shake()
    {
        CameraShakerHandler.Shake(myShake);
    }

}
