using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour, ISaveManager
{

    public static MapManager Instance { get; private set; }
    public Dictionary<string, MapData> MapsData { get; private set; }
    public CurrentMap currentMap { get; private set; }
    [SerializeField] private MapInfo respawnMap;

    private void Awake()
    {
        if (Instance==null)
            Instance = this;
        else
            Destroy(gameObject);


    }

    private void Start()
    {
        
    }
    public void InitMapsData()
    {

    }    

    public void Respawn()
    {
        GoToMapByTeleportTower(respawnMap);
    }
    public void GoToMapByTeleportTower(MapInfo destinationInfo)
    {
        Map destinationMap = CreateMap(destinationInfo);

        destinationMap.JoinMapByTeleportTower();

        currentMap = new CurrentMap(destinationInfo, destinationMap);

        if (!MapsData[destinationInfo.Id].IsUnlocked())
            MapsData[destinationInfo.Id].Unlock();

        if (destinationInfo.IsSafetyZone)
        {
            respawnMap = destinationInfo;
        }    
    }

    public void GoToMapByPortal(MapInfo destinationInfo)
    {
        Map destinationMap = CreateMap(destinationInfo);

        destinationMap.JoinMapByPortal(currentMap.MapInfo);

        currentMap = new CurrentMap(destinationInfo, destinationMap);

        if (!MapsData[destinationInfo.Id].IsUnlocked())
            MapsData[destinationInfo.Id].Unlock();

        if (destinationInfo.IsSafetyZone)
        {
            respawnMap = destinationInfo;
        }
    }
    
    public Map CreateMap(MapInfo mapInfo)
    {
        Map destinationMap = Instantiate(mapInfo.MapPrefab).GetComponent<Map>();
        if (destinationMap == null)
        {
            Debug.LogError($"Cannot create map name: \"{mapInfo.Id}\"");
        }

        //Destroy old map
        if (currentMap != null)
        {
            Destroy(currentMap.Map.gameObject);
        }

        //if (!MapsData[mapInfo.Id].IsUnlocked())
        //    MapsData[mapInfo.Id].Unlock();
        return destinationMap;
    }

      


    public void SaveData(ref GameData gameData)
    {
        List<MapData> mapsData = MapsData.Values.ToList();
        MapSaveData saveData = new MapSaveData(mapsData);
        gameData.MapData = saveData;
    }

    public void LoadData(GameData gameData)
    {
        MapSaveData loadData = gameData.MapData;

        string path = "Map";

        List<MapInfo> mapsInfo = Resources.LoadAll<MapInfo>(path).ToList();
        MapsData = new();

        foreach(var mapData in loadData.MapsData)
        {
            MapsData[mapData.Id] = new MapData( mapsInfo.Find(o => o.Id == mapData.Id) , mapData.unlocked);
        }

        foreach(var mapInfo in mapsInfo)
        {
            if(!MapsData.ContainsKey(mapInfo.Id))
            {
                MapsData[mapInfo.Id] = new MapData(mapInfo, false);
            }
        }
        Respawn();
    }
}
