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
        pickObjectText.SetActive(false);
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
        if(countObjectPicked < 2)
        {
            questText = " / " + numberOfObjects + " partie de l'amulette récupérées";
        } else
        {
            questText = " / " + numberOfObjects +  " parties de l'amulette récupérées";
        }
        Debug.DrawRay(cam.transform.position, cam.transform.forward * raycastDistance, Color.yellow);
        pickObjectText.SetActive(false);
        PerformHitDetection();
        toggleCrouch();
        PerformRun();
        Crouch();
        questTextUI.text = countObjectPicked + questText;
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
        cam.transform.Rotate(-cameraRotation);
    }

    private void PerformHitDetection()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("Pickable Object")))
        {
            pickObjectText.SetActive(true);
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
            playerCol.height = 1;
        else
            playerCol.height = 2;
    }
}
