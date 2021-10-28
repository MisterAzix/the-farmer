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
    }

    private void QuitCredits()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
