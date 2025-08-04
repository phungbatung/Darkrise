using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance { get; private set; }
    [SerializeField] private List<ImageData> backgroundImages;
    [SerializeField] private List<ImageData> coinImages;
    [SerializeField] private List<ColorData> itemRarityColors;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);
    }


    public Sprite GetItemSlotBackGroundImageByKey(string key)
    {
        //Debug.Log($"KeyBackground:{key}");
        foreach (var item in backgroundImages)
        {
            if (item.Key.Equals(key))
            {
                return item.Value;
            }
        }
        return null;
    }



    public Sprite GetCoinImageByKey(string key)
    {
        foreach (var item in coinImages)
        {
            if (item.Key.Equals(key))
            {
                return item.Value;
            }
        }
        return null;
    }

    public Color GetColorRarityByKey(string key)
    {
        foreach (var item in itemRarityColors)
        {
            if (item.Key.Equals(key))
            {
                return item.Value;
            }
        }
        Debug.LogError($"Cannot find the rarity color for key: {key}");
        return default;
    }    

    [Serializable]
    public class ImageData
    {
        public string Key;
        public Sprite Value;
    }

    [Serializable]
    public class ColorData
    {
        public string Key;
        public Color Value;
    }
}
