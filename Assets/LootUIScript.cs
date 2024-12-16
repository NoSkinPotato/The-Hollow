using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootUIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private RectTransform itemRectTransform;
    [SerializeField] private float animationSpeed = 5f;
    [SerializeField] private float hangTime = 2f;

    [HideInInspector]
    public bool startingLoot = false;
    public List<Item> LootQueue = new List<Item>();

    private float Ypos;


   

    private void Start()
    {
        Ypos = Mathf.Abs(itemRectTransform.position.y);
    }

    private void Update()
    {
        if (LootQueue.Count > 0 && startingLoot == false)
        {
            StartCoroutine(UpAndDownAnimation(LootQueue[0].name, LootQueue[0].value));
        }
    }

    private IEnumerator UpAndDownAnimation(string itemName, int itemAmount)
    {
        startingLoot = true;
        itemNameText.text = itemName;
        itemAmountText.text = itemAmount.ToString();

        Vector2 pos = itemRectTransform.position;
        pos.y = -Ypos;
        itemRectTransform.position = pos;

        while (itemRectTransform.position.y < Ypos)
        {        
            itemRectTransform.position += Vector3.up * Time.deltaTime * animationSpeed;

            yield return null;
        }

        yield return new WaitForSeconds(hangTime);

        while (itemRectTransform.position.y > -Ypos)
        {
            itemRectTransform.position += Vector3.down * Time.deltaTime * animationSpeed;

            yield return null;
        }

        startingLoot = false;
        LootQueue.RemoveAt(0);
        yield return null;
    }


}
