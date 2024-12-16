using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    bool accessMenu = false;

    [SerializeField] private float timeToStart = 2.5f;
    [SerializeField] private CanvasGroup canvas;

    float timer = 0;

    private void Awake()
    {
        StartCoroutine(StartMenu());
    }

    private IEnumerator StartMenu()
    {
        canvas.blocksRaycasts = false;
        while (timer < timeToStart)
        {
            timer += Time.deltaTime;    

            float x = Mathf.Clamp01(timer/timeToStart);

            canvas.alpha = x;
            yield return null;
        }

        canvas.blocksRaycasts = true;

    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);

    }

}
