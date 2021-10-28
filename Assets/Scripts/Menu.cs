using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject startButtonObject;
    [SerializeField] private GameObject creditButtonObject;
    [SerializeField] private GameObject quitButtonObject;
    [SerializeField] private float camSpeed;
    [SerializeField] private Transform gameCamPosition;
    [SerializeField] private GameObject Player;

    private bool startGame;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        creditButton.onClick.AddListener(Credit);
        Time.timeScale = 0f;
    }

    private void StartGame()
    {
        startGame = true;
        Time.timeScale = 1f;
        startButtonObject.SetActive(false);
        creditButtonObject.SetActive(false);
        quitButtonObject.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void Credit()
    {
        SceneManager.LoadScene("Credits");
    }

    private void CameraTranslation()
    {
        Player.transform.position = Vector3.Lerp(Player.transform.position, gameCamPosition.position, Time.time * camSpeed);
    }

    public bool getStartGame()
    {
        return startGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position != gameCamPosition.position)
            CameraTranslation();
        if (Player.transform.position == gameCamPosition.position)
            gameObject.SetActive(false);
    }
}
