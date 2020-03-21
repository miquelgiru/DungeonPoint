using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyConfig
{
    public Vector3 CustomEnemyPos;
    public Transform PrefabEnemy;
}

[System.Serializable]
public class ItemConfig
{
    public Vector3 CustomItemPos;
    public Transform PrefabItem;
}

[CreateAssetMenu(fileName = "GridElementsConfig", menuName = "MapConfig/GridElementsConfig", order = 1)]
public class GridElementsConfig : ScriptableObject
{
    [Header("Map Size")]
    [SerializeField] public int SizeX;
    [SerializeField] public int SizeY;
    [SerializeField] public int SizeZ;

    [Header("Entry Point")]
    public bool DefaultEntryPos = true;
    public Vector3 CustomEntryPos;
    public Transform EntryPrefrab;


    [Header("Exit Point")]
    public bool RandomExitPos = true;
    public Vector3 CustomExitPos;
    public Transform ExitPrefab;

    [Header("Enemies Config")]
    public bool RandomEnemyPos = true;
    public List<EnemyConfig> enemies;

    [Header("Items Config")]
    public bool RandomItemPos = true;
    public List<ItemConfig> items;
}