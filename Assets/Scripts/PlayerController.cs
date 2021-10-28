using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stealthSpeed = 3f;
    [SerializeField] private float runningSpeed = 8f;
    [SerializeField] private float mouseSensitivity = 8f;

    public bool isCrouching;
    public bool isRunning;

    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        isCrouching = motor.GetIsCrouching();
        isRunning = motor.GetIsRunning();

        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity;

        if(isCrouching == true)
        {
            velocity = (moveHorizontal + moveVertical).normalized * stealthSpeed;
        } 
        else if (isRunning && isCrouching)
        {
            velocity = (moveHorizontal + moveVertical).normalized * runningSpeed;
        }
        else
        {
            velocity = (moveHorizontal + moveVertical).normalized * speed;
        }


        motor.Move(velocity);

        float yRot = Input.GetAxisRaw("Mouse X");
        float xRot = Input.GetAxisRaw("Mouse Y");


        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivity;
        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * mouseSensitivity;


        motor.RotateCamera(cameraRotation); 
        motor.Rotate(rotation); 
    }
}
