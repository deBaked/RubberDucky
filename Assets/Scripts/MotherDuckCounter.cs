using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherDuckCounter : MonoBehaviour
{
    public List<Transform> Ducks;

    public int ducklingCount;
    
    public Vector3 ducklingDisUnit;
    
    private GameSingleton gameSingleton;


    private void Start()
    {
        Ducks.Add(transform);
        gameSingleton = GameSingleton.GetInstance();
    }

    public void UpdateDucklingCount(Transform duck)
    {
        ducklingCount = ducklingCount + 1;
        Ducks.Add(duck);
        gameSingleton.AddDuckling();
        //ducklingDisUnit = new Vector3(ducklingDisUnit.x, ducklingDisUnit.y, ducklingDisUnit.z * ducklingCount);
    }

}
