using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static WishesManager WishesManager { get; private set; }
    
    public static MainCharacter MainCharacter { get; private set; }
    public float CurrentLevel { get; private set; } = 1;
    public bool Paused { get; private set; } = false;

    private void Awake()
    {
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

    private void Start()
    {
        UIManager = gameObject.GetComponent<UIManager>();
        WishesManager = gameObject.GetComponent<WishesManager>();
    }

    public void RegisterMainCharacter(MainCharacter mainCharacter)
    {
        MainCharacter = mainCharacter;
        mainCharacter.GetComponent<Damageable>().onDeath.AddListener(() => { UIManager.OpenMenu(MenuType.GameOver); });
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

    public void Reset()
    {
        StatSO[] stats = Resources.LoadAll<StatSO>("");
        foreach (var stat in stats)
        {
            stat.Reset();
        }
        WishesManager.Reset();
    }

    public void TeleportCharacterBy(Vector3 characterOffset, Vector3 cameraOffset)
    {
        Vector3 characterPos = MainCharacter.transform.position + characterOffset;
        Vector3 cameraPos = Camera.main.transform.position + cameraOffset;
        MainCharacter.TeleportTo(characterPos, cameraPos);
    }

    public void TeleportCharacterTo(Vector3 playerPosition, Vector3 cameraPosition)
    {
        MainCharacter.TeleportTo(playerPosition, cameraPosition);
    }
}