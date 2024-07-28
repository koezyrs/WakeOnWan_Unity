using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : SingletonObject<PrefabManager>
{
    [SerializeField] private List<GameObject> listPrefab;
    private Dictionary<string, GameObject> dictionaryPrefab = new Dictionary<string, GameObject>();

    public override void Awake()
    {
        base.Awake();
        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        dictionaryPrefab.Clear(); // Clear existing entries if reloading
        foreach (var prefab in listPrefab)
        {
            if (!dictionaryPrefab.ContainsKey(prefab.name))
            {
                dictionaryPrefab.Add(prefab.name, prefab);
            }
            else
            {
                Debug.LogWarning($"Prefab with name {prefab.name} already exists in the dictionary.");
            }
        }
    }

    public GameObject GetPrefab(string name)
    {
        if (dictionaryPrefab.TryGetValue(name, out var prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogError($"Prefab with name {name} not found.");
            return null;
        }
    }
}