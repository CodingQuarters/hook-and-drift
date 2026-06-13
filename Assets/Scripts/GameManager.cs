using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject YouDie;
    public GameObject YouWin;
    public GameObject Restart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        YouWin.SetActive(false);
        YouDie.SetActive(false);
        Restart.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerDies()
    {
        YouDie.SetActive(true);
        Restart.SetActive(true);
        Debug.Log("text shown");
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
        Debug.Log("the player won");
        Time.timeScale = 0;
    }
}
