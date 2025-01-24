using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridsParent;
    [SerializeField] private Grid girdObject;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private float gapDistance;


    private void Awake()
    {
        SpawnGrids();
    }


    private void SpawnGrids()
    {
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                Grid grid = Instantiate(girdObject);
                grid.transform.parent = gridsParent.transform;
                grid.transform.localPosition = new Vector3((i-1f) * gapDistance, (j-1f) * gapDistance, 0);
                
            }
        }
    }

}
