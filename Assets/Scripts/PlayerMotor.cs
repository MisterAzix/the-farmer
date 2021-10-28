using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private int raycastDistance;
    [SerializeField] private GameObject pickObjectText;
    [SerializeField] private Text questTextUI;
    [SerializeField] private int numberOfObjects;

    private bool isCrouched = false;
    private bool isRunning = false;
    private string questText;
    private float originalHeight;

    public int countObjectPicked = 0;
    public float reducedHeight;

    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 cameraRotation;

    private RaycastHit hit;

    private Rigidbody rb;

    private CapsuleCollider playerCol;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCol = GetComponent<CapsuleCollider>();
        originalHeight = playerCol.height;
    }

    private void Awake()
    {
        if (pickObjectText) pickObjectText.SetActive(false);
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    } 
    
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    public bool GetIsCrouching()
    {
        return isCrouched;
    }

    public bool GetIsRunning()
    {
        return isRunning;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void Update()
    {
        //ATH en haut à droite
        if(countObjectPicked < 2)
        {
            questText = " / " + numberOfObjects + " partie de l'amulette récupérées";
        } else
        {
            questText = " / " + numberOfObjects +  " parties de l'amulette récupérées";
        }
        questTextUI.text = countObjectPicked + questText;
        if (pickObjectText) pickObjectText.SetActive(false);

        //Raycast dans la scène
        Debug.DrawRay(cam.transform.position, cam.transform.forward * raycastDistance, Color.yellow);

        //Actions du joueur
        PerformHitDetection();
        toggleCrouch();
        PerformRun();
        Crouch();
    }

    private void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        Vector3 targetCamRotation = cam.transform.eulerAngles + -cameraRotation;
        if(targetCamRotation.x > 360)
            targetCamRotation.x -= 360;
        else if (targetCamRotation.x < 0)
            targetCamRotation.x += 360;

        if(targetCamRotation.x > 0 && targetCamRotation.x < 70)
        {
            cam.transform.eulerAngles = targetCamRotation;
        }
        else if(targetCamRotation.x > (360 - 70) && targetCamRotation.x < 360)
        {
            cam.transform.eulerAngles = targetCamRotation;
        }
    }

    private void PerformHitDetection()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("Pickable Object")))
        {
            if(pickObjectText) pickObjectText.SetActive(true);
            if(Input.GetKeyDown("e"))
            {
                countObjectPicked++;
                Destroy(hit.transform.gameObject);
            }
        }
    }

    private void PerformRun()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
    }

    private void toggleCrouch()
    {
        if (Input.GetKeyDown("c")) isCrouched = !isCrouched;
    }

    private void Crouch()
    {
        if(isCrouched)
        {
            playerCol.height = 2;
            //playerCol.center.y = 0;
        }
            
        else
        {
            playerCol.height = 4;
            //playerCol.center.y = 2;
        }
            
    }
}
