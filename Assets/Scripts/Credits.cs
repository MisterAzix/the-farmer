using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    // Start is called before the first frame update
    void Start()
    { 
        quitButton.onClick.AddListener(QuitCredits);
        Time.timeScale = 1f;
    }

    private void QuitCredits()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
