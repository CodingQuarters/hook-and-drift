using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject Text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Text.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerDies()
    {
        Text.SetActive(true);
        Debug.Log("text shown");
        Time.timeScale = 0;
    }
    
}
