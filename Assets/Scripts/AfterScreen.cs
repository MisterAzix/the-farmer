using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AfterScreen : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    // Start is called before the first frame update
    void Start()
    {
        menuButton.onClick.AddListener(QuitLoseScreen);
    }

    private void QuitLoseScreen()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
    }

}
