using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] private PlayerMotor player;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && player.countObjectPicked >= player.numberOfObjects)
            SceneManager.LoadScene("Win");

    }
}
