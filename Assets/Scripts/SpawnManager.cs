using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] blocks;
    public GameObject blocksHolder;
    void Start()
    {
        SpawnBlock();   
    }

    

    public void SpawnBlock()
    {
        var number = Random.Range(0, blocks.Length-1);

        var insObject =Instantiate(blocks[number], transform.position, Quaternion.identity);
        insObject.transform.parent = blocksHolder.transform;
    }
}
