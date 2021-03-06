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
    [SerializeField] private GameObject questUI;

    public AudioClip menuSong;
    public AudioClip playSong;
    public AudioSource audioSourceMenu;
    public AudioSource audioSourcePlay;

    private bool startGame = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSourceMenu.clip = menuSong;
        audioSourceMenu.Play();
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        creditButton.onClick.AddListener(Credit);
        Time.timeScale = 0f;
    }

    private void StartGame()
    {
        questUI.SetActive(true);
        audioSourceMenu.Stop();
        audioSourcePlay.clip = playSong;
        audioSourcePlay.Play();
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

    public bool GetStartGame()
    {
        return startGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame)
        {
            if (Mathf.Floor(Player.transform.position.x) != Mathf.Floor(gameCamPosition.position.x) && Mathf.Floor(Player.transform.position.y) != Mathf.Floor(gameCamPosition.position.y) || Mathf.Floor(Player.transform.position.z) != Mathf.Floor(gameCamPosition.position.z))
                CameraTranslation();
            else
                gameObject.SetActive(false);
        }
    }
}
