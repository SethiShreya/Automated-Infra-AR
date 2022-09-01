using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHolder : MonoBehaviour
{
    public void DeleteAllChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
            Debug.Log("All child deleted");
        }
    }
}
