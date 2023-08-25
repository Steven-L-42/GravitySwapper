using UnityEngine;

public class SwapTouch_Behaviour : MonoBehaviour
{
    private Movement Movement;

    void Start()
    {
        Movement = FindObjectOfType<Movement>();
    }
    public void Restart()
    {
        Start();
    }
    public void OnJumpPress()
    {
        Movement.isSwapHolding = true;
        Movement.isSwapOnce = true;
    }

    public void OnJumpRelease()
    {
        Movement.isSwapHolding = false;
    }
}
