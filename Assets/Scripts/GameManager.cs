using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using System;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour
{
    [Header("UI Manager")]
    public GameObject YouDie;
    public GameObject YouWin;
    public GameObject Restart;
    [Header("Spawning Currents Arrow")]
    public GameObject arrowPrefab;
    public List<GameObject> currents;
    public int stepX = 3;
    public int stepY = 4;
    public float driftSpeed = 1.5f;
    public float startY;

    public class objectData
    {
        public float vitesse = 5.0f;
        public float angle = 5.0f;
        public float positionEndX;
        public float positionStartX;
        public float positionEndY;
        public float positionStartY;
        public objectData(float speed, float rotation, float endX, float startX, float startY, float endY)
        {
            vitesse = speed;
            angle = rotation;
            positionEndX = endX;
            positionStartX = startX;
            positionEndY = endY;
            positionStartY = startY;
        }
    }
    private Dictionary<GameObject, objectData> objectRegistry = new Dictionary<GameObject, objectData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        YouWin.SetActive(false);
        YouDie.SetActive(false);
        Restart.SetActive(false);
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
            float angle = areaEffector2D.forceAngle;
            float speed = areaEffector2D.forceMagnitude;
            for (int i = 0; i < gameObjectHeight; i += stepY)
            {
                for (int e = 0; e < gameObjectWidth; e += stepX)
                {
                    Vector3 spawnPosition = new Vector3(startX + e, startY + i, 0);
                    GameObject newObject = Instantiate(arrowPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
                    objectData customData = new objectData(speed, angle, finalX, startX, startY, finalY);
                    objectRegistry.Add(newObject, customData);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<GameObject, objectData> obj in objectRegistry)
        {
            GameObject gameObject = obj.Key;
            objectData data = obj.Value;
            

            if (gameObject != null)
            {
                float angle = data.angle;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
                gameObject.transform.position += direction * data.vitesse * Time.deltaTime;
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
        }
    }
    public void PlayerDies()
    {
        YouDie.SetActive(true);
        Restart.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void YouWon()
    {
        YouWin.SetActive(true);
        Restart.SetActive(true);
        Time.timeScale = 0;
    }
}
