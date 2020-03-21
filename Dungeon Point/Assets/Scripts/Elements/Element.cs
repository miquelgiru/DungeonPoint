using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    private GridNode node;
    public GridNode Node
    {
        get { return node; }
        set { node = value; }
    }

    protected void Start()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

        GameManager.Instance.RegisterElement(this);
        gameObject.SetActive(false);
    }
}
