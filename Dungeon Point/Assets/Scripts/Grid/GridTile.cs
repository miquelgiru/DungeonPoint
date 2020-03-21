using UnityEngine;
using System.Collections;

public class GridTile : MonoBehaviour
{
    public SpriteRenderer SpriteOverlay;
    public MeshRenderer TileMaterial;
    public GridNode Node;

    public void SetMaterial(Material mat)
    {
        TileMaterial.material = mat;
    }

    public void BlockTile()
    {
        SpriteOverlay.gameObject.SetActive(true);
        Node.IsBlocked = true;
    }
}
