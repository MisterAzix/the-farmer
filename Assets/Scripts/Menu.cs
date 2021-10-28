using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] private Button startButton;
    //[SerializeField] private GameObject menuCamera;
    //[SerializeField] private GameObject gameCamera;
    [SerializeField] private float camSpeed;
    //[SerializeField] private Transform gameCameraTransform;
    //[SerializeField] private Transform menuCameraTransform;
    //Transform menuCameraTransform;
    [SerializeField] private Transform gameCamPosition;
    [SerializeField] private GameObject Player;

    //private bool gamePause;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        //gameCamera.SetActive(false);
        Time.timeScale = 0f;
        //gamePause = true;
    }

    private void StartGame()
    {
        //gameObject.SetActive(false);
        //menuCamera.SetActive(false);
        //gameCamera.SetActive(true);
        Time.timeScale = 1f;

        //gamePause = false;
    }

    private void CameraTranslation()
    {
        //transform.position = Vector3.Lerp(transform.position, gameCameraTransform.position, Time.deltaTime * camSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
        Player.transform.position = Vector3.Lerp(Player.transform.position, gameCamPosition.position, Time.deltaTime * camSpeed);
        //Debug.Log(gameCameraTransform.position);
        //Debug.Log(menuCameraTransform.position);
    }


}
