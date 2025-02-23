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
    public int CurrentLevel { get; private set; }
    public bool Paused { get; private set; } = false;

    public FloatValue timeScale;

    private StatSO[] _stats;
    private BoolSO[] _bools;
    private MaterialSO[] _materials;

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
        _stats = Resources.LoadAll<StatSO>("");
        _bools = Resources.LoadAll<BoolSO>("");
        _materials = Resources.LoadAll<MaterialSO>("");
        
        Time.timeScale = timeScale.Get();
        timeScale.stat.OnValueChanged += newValue => Time.timeScale = newValue;
    }

    public void RegisterMainCharacter(MainCharacter mainCharacter)
    {
        MainCharacter = mainCharacter;
        MainCharacter.GetComponent<Damageable>().onDeath.AddListener(() => { UIManager.OpenMenu(MenuType.GameOver); });
    }

    public void NextLevel()
    {
        GoToLevel(CurrentLevel+1);
    }

    public void GoToLevel(int newLevel)
    {
        CurrentLevel = newLevel;
        ResetEvents();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        WishesManager.ResetEvent();
    }

    public void PauseGame(bool paused)
    {
        Paused = paused;
        Time.timeScale = paused ? 0 : timeScale.Get();
    }

    public void Reset()
    {
        CurrentLevel = 0;
        WishesManager.Reset();
        ResetEvents();
        WishesManager.ResetEvent();
        ResetStats();
    }

    private void ResetEvents()
    {
        foreach (var statRef in _stats)
            statRef.ResetEvent();
        foreach (var boolRef in _bools)
            boolRef.ResetEvent();
        foreach (var materialRef in _materials)
            materialRef.ResetEvent();
        
        timeScale.stat.OnValueChanged += newValue => Time.timeScale = newValue;
    }

    public void ResetStats()
    {
        foreach (var statRef in _stats)
            statRef.Reset();
        foreach (var boolRef in _bools)
            boolRef.Reset();
        foreach (var materialRef in _materials)
            materialRef.Reset();
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