using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameObject player;
    private int playerHealth;
    public Slider healthSlider;
    public Slider motherShipSlider;
    public GameObject activeUI;
    public GameObject deathUI;
    public GameObject winUI;
    public GameObject motherShipHealthBar;
    void Start()
    {
        Time.timeScale = 1f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = player.GetComponent<PlayerController>().currentHealth;
        healthSlider.value = playerHealth;
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;
        activeUI.SetActive(false);
        deathUI.SetActive(true);
        winUI.SetActive(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Main");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Victory()
    {
        Time.timeScale = 0f;
        activeUI.SetActive(false);
        deathUI.SetActive(false);
        winUI.SetActive(true);

    }
}
