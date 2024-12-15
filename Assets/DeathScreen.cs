using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Image deathScreen;
    [SerializeField] private GameObject deathText;
    [SerializeField] private Image quitButton;
    [SerializeField] private Image menuButton;
    [SerializeField] private Color orgColor;

    private AudioManager audioManager;


    private void Start()
    {
        audioManager = AudioManager.Instance;
        deathScreen.enabled = false;
        deathText.SetActive(false);
        quitButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
    }

    public IEnumerator StartDeathScreen()
    {
        deathScreen.enabled = true;

        yield return new WaitForSeconds(4f);

        deathText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        quitButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);

        yield return null;

    }

    public void BackToMenu()
    {
        Time.timeScale = 1;

    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }



}
