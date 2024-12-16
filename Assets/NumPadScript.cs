using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumPadScript : MonoBehaviour
{
    public string noteValue;
    [SerializeField] private TextMeshProUGUI inputValueText;
    [SerializeField] private Image inputBackground;
    public bool hasNote = false;
    [SerializeField] private GameObject notFoundText;
    [SerializeField] private GameObject foundTextBg;
    [SerializeField] private TextMeshProUGUI foundText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image whiteImage;
    [SerializeField] private float whiteSpeed;
    [SerializeField] private GameObject numpad;

    private PlayerStatsScript playerStatsScript;   
    private AudioManager audioManager;

    private Color originalColor;

    private void Start()
    {
        originalColor = inputBackground.color;
        playerStatsScript = PlayerStatsScript.Instance;
        audioManager = AudioManager.Instance;
        inputValueText.text = "";
    }

    public void FoundNote()
    {
        hasNote = true;
        notFoundText.SetActive(false);
        foundTextBg.SetActive(true);
        foundText.text = noteValue;
    }

    public void SetCode(string code)
    {
        noteValue = code;
    }

    public void InsertValue(string value)
    {
        if (inputValueText.text.Length < 7)
        {
            audioManager.Play("NumpadClick");
            inputValueText.text += value;
        }
        
    }

    public void ClearValue()
    {
        inputBackground.color = originalColor;
        audioManager.Play("NumpadClick");
        inputValueText.text = "";
    }

    public void EnterValue()
    {
        //Validation
        if (inputValueText.text.Equals(noteValue))
        {
            inputBackground.color = Color.green;
            gameManager.PlayEnterValueSound();
            //RestartGame
            StartCoroutine(goWhite());
            numpad.SetActive(false);
        }
        else
        {
            inputBackground.color = Color.red;
            audioManager.Play("NumpadError");
        }
    }


    private IEnumerator goWhite()
    {
        playerStatsScript.SetPlayerInvulnerability(true);

        

        float elapsedTime = 0f; 
        float currentValue = 0f;

        while (elapsedTime < whiteSpeed)
        {
            elapsedTime += Time.deltaTime; 
            currentValue = Mathf.Clamp01(elapsedTime / whiteSpeed); 

            Color color = whiteImage.color;
            color.a = currentValue;
            whiteImage.color = color;
        }

        gameManager.NextLevel();

        yield return null;
    }

    public IEnumerator StartGameFromWhite()
    {

        float elapsedTime = 0f; 
        float currentValue = 0f;

        while (elapsedTime < whiteSpeed)
        {
            elapsedTime += Time.deltaTime; 
            currentValue = Mathf.Clamp01(elapsedTime / whiteSpeed);

            Color color = whiteImage.color;
            color.a = 1 - currentValue;
            whiteImage.color = color;
            yield return null;
        }
    }

    public void SetWhite()
    {
        whiteImage.color = Color.white;
    }
}
