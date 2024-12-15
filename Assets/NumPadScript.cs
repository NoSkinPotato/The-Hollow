using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumPadScript : MonoBehaviour
{
    private string noteValue;
    [SerializeField] private TextMeshProUGUI inputValueText;
    public bool hasNote = false;
    [SerializeField] private GameObject notFoundText;
    [SerializeField] private GameObject foundTextBg;
    [SerializeField] private TextMeshProUGUI foundText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image whiteImage;
    [SerializeField] private float whiteSpeed;
    [SerializeField] private GameObject numpad;

    private PlayerStatsScript playerStatsScript;    

    private void Start()
    {
        playerStatsScript = PlayerStatsScript.Instance;
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
            inputValueText.text += value;
        }
        
    }

    public void ClearValue()
    {
        inputValueText.text = "";
    }

    public void EnterValue()
    {
        //Validation
        if (inputValueText.text.Equals(noteValue))
        {
            //RestartGame
            StartCoroutine(goWhite());
            numpad.SetActive(false);
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
            Debug.Log(currentValue);
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
