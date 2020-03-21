using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Instance
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }
    #endregion

    public delegate void TileSelected(GridTile tile);
    public TileSelected OnTileSelected;

    public delegate void EnemySelected(Enemy enemy);
    public EnemySelected OnEnemySelected;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    OnEnemySelected?.Invoke(enemy);
                }

                else
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Tile"))
                    {
                        GridTile tile = hit.collider.GetComponent<GridTile>();
                        if (tile.Node.IsExplorable || tile.Node.IsExplored)
                            if (!tile.Node.IsBlocked && !tile.Node.HasEnemy)
                                OnTileSelected?.Invoke(tile);
                    }
                }
            }
        }
    }
}
