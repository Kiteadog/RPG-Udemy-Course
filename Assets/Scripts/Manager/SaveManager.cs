using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler("H:\\Unity\\RPG - Udemy Course\\SL", fileName, encryptData);
        dataHandler.Delete();
    }

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler("H:\\Unity\\RPG - Udemy Course\\SL", fileName, encryptData);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }


    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        //��data handler��ȡ��Ϸ����
        gameData = dataHandler.Load();

        if(this.gameData == null)//�޴洢����
        {
            NewGame();
        }

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //ͨ��data handler�洢��Ϸ����
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSaveData()
    {
        if(dataHandler.Load() != null)
        {
            return true;
        }
        return false;
    }
}
