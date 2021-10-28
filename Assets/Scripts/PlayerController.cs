using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stealthSpeed = 3f;
    [SerializeField] private float runningSpeed = 8f;
    [SerializeField] private float mouseSensitivity = 8f;

    [SerializeField] private Animator animator;

    public bool isCrouching;
    public bool isRunning;

    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
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

        if (isCrouching)
        {
            velocity = (moveHorizontal + moveVertical).normalized * stealthSpeed;
            animator.SetTrigger("isCrouching");
            animator.SetFloat("VelocityX", xMov * stealthSpeed);
            animator.SetFloat("VelocityY", zMov * stealthSpeed);
        } 
        else if (isRunning && !isCrouching)
        {
            velocity = (moveHorizontal + moveVertical).normalized * runningSpeed;
            animator.ResetTrigger("isCrouching");
            animator.SetFloat("VelocityX", xMov * runningSpeed);
            animator.SetFloat("VelocityY", zMov * runningSpeed);
        }
        else
        {
            velocity = (moveHorizontal + moveVertical).normalized * speed;
            animator.ResetTrigger("isCrouching");
            animator.SetFloat("VelocityX", xMov * speed);
            animator.SetFloat("VelocityY", zMov * speed);
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
