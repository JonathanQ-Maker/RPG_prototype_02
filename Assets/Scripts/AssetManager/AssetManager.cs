using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance { get; private set; }
    public List<PrefabType> types = new List<PrefabType>();
    public List<GameObject> prefabs = new List<GameObject>();

    public List<MaterialType> materialTypes = new List<MaterialType>();
    public List<Material> materials = new List<Material>();

    public readonly Dictionary<PrefabType, GameObject> Prefabs = new Dictionary<PrefabType, GameObject>();
    public readonly Dictionary<MaterialType, Material> Materials = new Dictionary<MaterialType, Material>();

    private void Awake()
    {
        CheckExistance();
        DontDestroyOnLoad(this);
        for (int i = 0; i < types.Count; i++)
        {
            Prefabs[types[i]] = prefabs[i];
        }

        for (int i = 0; i < materialTypes.Count; i++)
        {
            Materials[materialTypes[i]] = materials[i];
        }
    }

    private void CheckExistance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogWarning("An instance of AssetManager already exists!");
            Destroy(this);
            return;
        }
    }

    public static Material GetMaterial(MaterialType materialAsset)
    {
        Material material;
        if (!Instance.Materials.TryGetValue(materialAsset, out material))
        {
            Debug.LogWarning($"AssetManager: Tried to get {materialAsset} material but was not found");
        }
        return material;
    }

    public static GameObject GetPrefab(PrefabType type)
    {
        GameObject gameObject;
        if (!Instance.Prefabs.TryGetValue(type, out gameObject))
        {
            Debug.LogWarning($"AssetManager: Tried to get {type} prefab but was not found");
            return null;
        }
        return gameObject;
    }

    public static T GetPrefab<T>(PrefabType type)
    {
        GameObject gameObject = GetPrefab(type);
        if (gameObject == null) return default(T);
        return gameObject.GetComponent<T>();
    }
}
