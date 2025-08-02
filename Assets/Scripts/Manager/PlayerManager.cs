using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager Instance;
    public Player player;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        //Instantiate(player);
    }
    public void LoadData(GameData gameData)
    {
        PlayerSaveData LoadData = gameData.PlayerData;
        player.levels.SetLevel(LoadData.level, LoadData.exp);
    }

    public void SaveData(ref GameData gameData)
    {
        PlayerSaveData saveData = new PlayerSaveData(player.levels.Level, player.levels.Exp);
        gameData.PlayerData = saveData;
    }
}
