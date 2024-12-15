using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int enemySpawnerRank = 100;
    private GridData grid;

    [SerializeField] private EnemyDatabase enemyDatabase;
    [SerializeField] private ItemDatabase itemDatabase;

    private EnemyData[] enemyData;
    private ItemStats[] itemStats;

    public int currSpawnerRank = 0;
    public int currItemRank = 0;

    private List<EnemyData> spawnedEnemies = new List<EnemyData>();

    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject itemPrefab;

    private List<Node> WalkableNodes = new List<Node>();

    [SerializeField] private NumPadScript numPadScript;

    public float minimumEnemySpawnDistance = 10f;

    [SerializeField] private Transform TeleporterObject;
    [SerializeField] private Transform NoteObject;

    [SerializeField] private DeathScreen deathScreen;
    [SerializeField] private TextMeshProUGUI daysScreen;


    public int currDay;

    private InventorySystem inventorySystem;
    private PlayerWeaponScript playerWeaponScript;
    private PlayerStatsScript playerStatsScript;    
    private string filePath;



    private void Start()
    {
        
        grid = GridData.Instance;
        inventorySystem = InventorySystem.Instance;
        playerWeaponScript = PlayerWeaponScript.Instance;
        playerStatsScript = PlayerStatsScript.Instance;
        enemyData = enemyDatabase.enemyDatabase.ToArray();
        itemStats = itemDatabase.itemDatabase.ToArray();
        filePath = Application.persistentDataPath + "/inventoryData.json";
        StartCoroutine(GameSetUp());
    }


    private IEnumerator GameSetUp()
    {
        StartCoroutine(LoadGameData());

        yield return null;

        for (int x = 0; x < grid.nodes.GetLength(0); x++)
        {
            for (int y = 0; y < grid.nodes.GetLength(1); y++)
            {
                if (grid.nodes[x, y] != null && grid.nodes[x, y].IsWalkable)
                {
                    WalkableNodes.Add(grid.nodes[x, y]);
                }
            }
        }

        SetRandomCode();

        yield return null;

        StartCoroutine(SpawnPlayer());

        yield return null;

        StartCoroutine(SpawnAllEnemies());

        yield return null;

        while (currItemRank < enemySpawnerRank) {

            StartCoroutine(SpawnLoot());
            yield return null;
        }

        yield return null;

        StartCoroutine(SpawnObjectives());

        yield return null;

        foreach (Node node in WalkableNodes)
        {
            if (node.hasObject) node.hasObject = false;
        }

        yield return null;


        if (currDay > 0)
        {
            StartCoroutine(numPadScript.StartGameFromWhite());
        }
        

        yield return null;
    }

    private IEnumerator LoadGameData()
    {
        currDay = PlayerPrefs.GetInt("Day");

        StartCoroutine(DayText(2.5f));

        if (currDay > 1)
        {
            numPadScript.SetWhite();

            enemySpawnerRank = PlayerPrefs.GetInt("SpawnerRank");

            string weapons = PlayerPrefs.GetString("CurrMagazine");
            playerWeaponScript.FillWeaponsMag(weapons);

            playerStatsScript.SetPlayerHealth(PlayerPrefs.GetFloat("Health"));


            List<Item> loadInventoryData = new List<Item>();
            if (File.Exists(filePath))
            {
                loadInventoryData = JsonUtility.FromJson<List<Item>>(File.ReadAllText(filePath));

            }

            inventorySystem.ItemsInInventory = loadInventoryData;

            Debug.Log("Inventory Data Loaded");
        }

        yield return null;
    }

    private IEnumerator DayText(float speed)
    {

        yield return new WaitForSeconds(1.5f);

        daysScreen.text = "Day " + currDay.ToString();
        daysScreen.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        float elapsedTime = 0f;
        float currentValue = 0f;

        while (elapsedTime < speed)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Clamp01(elapsedTime / speed);

            Color color = daysScreen.color;
            color.a = 1 - currentValue;
            daysScreen.color = color;
            yield return null;
        }

    }

    private void SetRandomCode()
    {
        string code = "";

        for (int i = 0; i < 7; i++)
        {
            code += Random.Range(0, 10).ToString();
        }
        Debug.Log(code);
        numPadScript.SetCode(code);
    }

    private IEnumerator SpawnPlayer()
    {
        int rand = Random.Range(0, WalkableNodes.Count);

        playerTransform.position = (Vector3Int)WalkableNodes[rand].position;

        WalkableNodes[rand].hasObject = true;

        yield return null;
    }

    private IEnumerator SpawnLoot()
    {

        int randomLoot = Random.Range(0, 100);

        if (randomLoot <= 60)
        {
            //ammo
            int weight = Random.Range(0, 100);
            int startingWeight = 0;
            for (int i = 0; i < 3; i++)
            {
                if (i == 0) startingWeight = 0;
                else startingWeight += (int)itemStats[i - 1].SpawnChance;

                if (weight >= startingWeight && weight < startingWeight + itemStats[i].SpawnChance)
                {
                    SpawnItem(itemStats[i]);
                    break;
                }
            }
        }
        else
        {
            //health
            int weight = Random.Range(0, 100);

            if (weight <= itemStats[3].SpawnChance)
            {
                SpawnItem(itemStats[3]);
            }
            else
            {
                SpawnItem(itemStats[4]);
            }

        }


        yield return null;
    }

    private void SpawnItem(ItemStats stats)
    {
        float randAmount = Random.Range(0.25f, 1f);

        if (stats.OnHealth)
        {
            currItemRank += (int)(stats.rank * randAmount);
        }
        else
        {
            currItemRank += stats.rank / stats.maxValue;
        }


        Vector2 pos = Vector2.zero;

        while (true)
        {
            int rand = Random.Range(0, WalkableNodes.Count);

            Node n = WalkableNodes[rand];
            if (n.hasObject == false)
            {
                n.hasObject = true;
                pos = n.position + new Vector2(0.5f, 0.5f);
                break;
            }
        }

        GameObject obj = Instantiate(itemPrefab, (Vector3)pos, itemPrefab.transform.rotation);
        ItemContainer itemContainer = obj.GetComponent<ItemContainer>();
        if (itemContainer != null)
        {

            itemContainer.containedItem.type = stats.type;
            if (stats.OnHealth) itemContainer.containedItem.value = 1;
            else itemContainer.containedItem.value = (int)(stats.maxValue * randAmount);
            itemContainer.containedItem.useOnHealth = stats.OnHealth;

        }
        else Debug.Log("Bug with Container");
    }

    private IEnumerator SpawnAllEnemies()
    {

        while (currSpawnerRank < enemySpawnerRank)
        {
            RandomEnemySpawn();
            yield return null;
        }

        yield return null;
    }

    private void RandomEnemySpawn()
    {
        int weight = Random.Range(0, 100);
        int startingWeight = 0;
        for (int i = 0; i < enemyData.Length; i++)
        {
            if (i == 0) startingWeight = 0;
            else startingWeight += (int)enemyData[i - 1].spawnChance;

            if (weight >= startingWeight && weight < startingWeight + enemyData[i].spawnChance)
            {

                FindEnemySpace(enemyData[i]);
                currSpawnerRank += enemyData[i].rank;
                break;
            }
        }
    }

    private void FindEnemySpace(EnemyData enemyData)
    {
        Debug.Log("Spawning " + enemyData.name);

        List<Vector2Int> spawnPoints;

        while (true)
        {
            int randomNode = Random.Range(0, WalkableNodes.Count);

            if (Vector2.Distance(WalkableNodes[randomNode].position, playerTransform.position) >= minimumEnemySpawnDistance)
            {
                List<Vector2Int> nodes = CheckSurroundingOnSize(enemyData.size, WalkableNodes[randomNode].position);

                if (nodes != null)
                {
                    spawnPoints = nodes;
                    break;
                }
            }
        }

        SpawnEnemyOnLocation(enemyData, spawnPoints);

        spawnedEnemies.Add(enemyData);

    }

    private List<Vector2Int> CheckSurroundingOnSize(Vector2 size, Vector2Int location)
    {
        List<Vector2Int> chosenNodes = new List<Vector2Int>();

        location += grid.offset;

        for (int x = 0; x < (int)size.x; x++) {

            for (int y = 0; y < (int)size.y; y++)
            {
                Node node = grid.nodes[location.x + x, location.y + y];

                if (node != null && node.IsWalkable && node.hasObject == false)
                {
                    chosenNodes.Add(new Vector2Int(location.x + x, location.y + y));
                }
            }
        }

        if (chosenNodes.Count >= (size.x * size.y)) {

            foreach (Vector2Int vi in chosenNodes)
            {
                grid.nodes[vi.x, vi.y].hasObject = true;
            }

            return chosenNodes;
        }
        else
        {
            return null;
        }

    }

    private void SpawnEnemyOnLocation(EnemyData enemy, List<Vector2Int> spawnPoints)
    {

        float x = 0;
        float y = 0;

        foreach (Vector2Int v in spawnPoints)
        {
            x += v.x + 0.5f;
            y += v.y + 0.5f;
        }

        x /= spawnPoints.Count;
        y /= spawnPoints.Count;

        Vector2 position = new Vector2(x, y) - grid.offset;
        Instantiate(enemy.prefab, position, enemy.prefab.transform.rotation);

    }

    private IEnumerator SpawnObjectives()
    {
        while (true)
        {
            int rand = Random.Range(0, WalkableNodes.Count);
            if (WalkableNodes[rand].hasObject == false && Vector2.Distance(WalkableNodes[rand].position, playerTransform.position) >= minimumEnemySpawnDistance)
            {
                TeleporterObject.position = (Vector3Int)WalkableNodes[rand].position + new Vector3(0.5f,0.5f);

                break;
            }
        }

        while (true)
        {
            int rand = Random.Range(0, WalkableNodes.Count);
            if (WalkableNodes[rand].hasObject == false && Vector2.Distance(WalkableNodes[rand].position, playerTransform.position) >= minimumEnemySpawnDistance)
            {
                NoteObject.position = (Vector3Int)WalkableNodes[rand].position + new Vector3(0.5f, 0.5f); ;

                break;
            }
        }

        yield return null;

    }



    public void NextLevel()
    {
        Debug.Log("Next Level");

        PlayerPrefs.SetInt("Day", currDay + 1);
        PlayerPrefs.SetInt("SpawnerRank", enemySpawnerRank >= 350 ? enemySpawnerRank : enemySpawnerRank + 50);

        string weaponsMag = playerWeaponScript.GenerateWeaponInMag();

        PlayerPrefs.SetString("CurrMagazine", weaponsMag);

        PlayerPrefs.SetFloat("Health", playerStatsScript.GetCurrentPlayerHealth());

        List<Item> saveInventoryData = inventorySystem.ItemsInInventory;
        string jsonString = JsonUtility.ToJson(saveInventoryData, true);
        File.WriteAllText(filePath, jsonString);

        Debug.Log("Inventory Data Saved");



        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void EndGame()
    {
        ResetSave();
        StartCoroutine(deathScreen.StartDeathScreen());

    }

    private void ResetSave()
    {
        PlayerPrefs.SetInt("Day", 0);
        PlayerPrefs.SetInt("SpawnerRank", 100);

        PlayerPrefs.SetString("CurrMagazine", "");

        PlayerPrefs.SetFloat("Health", 100);

        List<Item> resetInventoryData = new List<Item>();
        string jsonString = JsonUtility.ToJson(resetInventoryData, true);
        File.WriteAllText(filePath, jsonString);

    }
    

    public void PauseGame()
    {

    }


}

public enum EnemyState
{
    Active, Idle, Dead
}