using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    [Header("UI Manager")]
    public GameObject YouDie;
    public GameObject YouWin;
    public GameObject Restart;
    public GameObject nextLevel;
    public GameObject player;
    public PlayerLogic playerLogicScript;
    public Vector2 newSpawn;
    [Header("Spawning Currents Arrow")]
    public GameObject arrowPrefab;
    public List<GameObject> currents;
    public int stepX = 3;
    public int stepY = 4;
    public float driftSpeed = 1.5f;
    public float startY;

    public class arrowData
    {
        public float speedf = 5.0f;
        public float directionAngle = 5.0f;
        public float positionEndX;
        public float positionStartX;
        public float positionEndY;
        public float positionStartY;
        public arrowData(float speed, float rotation, float endX, float startX, float startY, float endY)
        {
            speedf = speed;
            directionAngle = rotation;
            positionEndX = endX;
            positionStartX = startX;
            positionEndY = endY;
            positionStartY = startY;
        }
    }
    private Dictionary<GameObject, arrowData> objectRegistry = new Dictionary<GameObject, arrowData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currents = new List<GameObject>(GameObject.FindGameObjectsWithTag("Currents"));
        YouWin.SetActive(false);
        YouDie.SetActive(false);
        Restart.SetActive(false);
        nextLevel.SetActive(false);
        Time.timeScale = 1;
        SpriteRenderer arrowSpriteRenderer = arrowPrefab.GetComponent<SpriteRenderer>();
        float arrowHeight = arrowSpriteRenderer.bounds.size.x;
        float arrowWidth = arrowSpriteRenderer.bounds.size.y;

        foreach (GameObject gameObject in currents)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            float gameObjectWidth = spriteRenderer.bounds.size.x;
            float gameObjectHeight = spriteRenderer.bounds.size.y;
            Vector2 gameObjectPosition = gameObject.transform.position;
            float startX = gameObjectPosition.x - (gameObjectWidth/2f) + arrowWidth;
            float startY = gameObjectPosition.y - (gameObjectHeight/2f) + arrowHeight;
            float finalX = gameObjectPosition.x + (gameObjectWidth/2f) - arrowHeight;
            float finalY = gameObjectPosition.y + (gameObjectHeight/2f) - arrowHeight;
            
            AreaEffector2D areaEffector2D = gameObject.GetComponent<AreaEffector2D>();
            float directionAngle = areaEffector2D.forceAngle;
            float speed = areaEffector2D.forceMagnitude;
            for (float i = 0; i < gameObjectHeight; i += stepY)
            {
                for (float e = 0; e < gameObjectWidth; e += stepX)
                {
                    Vector3 spawnPosition = new Vector3(startX + e, startY + i, 0);
                    GameObject newObject = Instantiate(arrowPrefab, spawnPosition, Quaternion.Euler(0, 0, directionAngle));
                    arrowData customData = new arrowData(speed, directionAngle, finalX, startX, startY, finalY);
                    objectRegistry.Add(newObject, customData);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        newSpawn = playerLogicScript.newSpawn;
        foreach (KeyValuePair<GameObject, arrowData> obj in objectRegistry)
        {
            GameObject gameObject = obj.Key;
            arrowData data = obj.Value;
            

            if (gameObject != null)
            {
                float directionAngle = data.directionAngle;
                Vector3 direction = Quaternion.Euler(0, 0, directionAngle) * Vector3.right;
                gameObject.transform.position += direction * data.speedf * Time.deltaTime;
            }

            if (gameObject.transform.position.x > data.positionEndX)
            {
                float positionY = gameObject.transform.position.y;
                gameObject.transform.position = new Vector3(data.positionStartX, positionY, 0);
            }
            else if (gameObject.transform.position.y > data.positionEndY)
            {
                float positionX = gameObject.transform.position.x;
                gameObject.transform.position = new Vector3(positionX, data.positionStartY, 0);

            }
            else if (gameObject.transform.position.x < data.positionStartX)
            {
                float positionY = gameObject.transform.position.y;
                gameObject.transform.position = new Vector3(data.positionEndX, positionY, 0);
            }
            else if (gameObject.transform.position.y < data.positionStartY)
            {
                float positionX = gameObject.transform.position.x;
                gameObject.transform.position = new Vector3(positionX, data.positionEndY, 0);

            }
        }
    }
    public void PlayerDies()
    {
        player.transform.position = newSpawn;
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void YouWon()
    {
        YouWin.SetActive(true);
        nextLevel.SetActive(true);
        Time.timeScale = 0;
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }
}
