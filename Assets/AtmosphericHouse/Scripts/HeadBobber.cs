using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    public SimplePlayerController controller;
    public float walkingBobbingSpeed = 14f;
    public float runningBobbingSpeed = 20f;
    public float bobbingAmount = 0.05f;

    float defaultPosY = 0;
    float timer = 0;
    Vector3 playerMoveDirection;

    void Start()
    {
        defaultPosY = transform.localPosition.y;
    }

    void Update()
    {
        playerMoveDirection = controller.MoveDirection;

        if(controller.IsMoving && !controller.IsRunning)
        {
            timer += Time.deltaTime * walkingBobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
        }
        else if (controller.IsMoving && controller.IsRunning)
        {
            timer += Time.deltaTime * runningBobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
        }
        else
        {
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform.localPosition.z);
        }
    }
}
