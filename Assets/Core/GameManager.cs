using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static WishesManager WishesManager { get; private set; }
    public float CurrentLevel { get; private set; } = 1;
    public bool Paused { get; private set; } = false;

    private void Awake()
    {
        UIManager = gameObject.GetComponent<UIManager>();
        WishesManager = gameObject.GetComponent<WishesManager>();
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        CurrentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame(bool paused)
    {
        Paused = paused;
        Time.timeScale = paused ? 0 : 1;
    }
}