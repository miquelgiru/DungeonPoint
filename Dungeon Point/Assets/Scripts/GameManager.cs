using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region Instance
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    #endregion

    [SerializeField] GridManager gridManager;

    Vector3 PlayerPosition = Vector3.zero;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        gridManager.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MovePlayer(int posX, int posZ)
    {
        PlayerPosition = new Vector3(posX, 0, posZ);
    }
}
