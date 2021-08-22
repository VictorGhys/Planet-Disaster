using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Disaster : MonoBehaviour
{
    private DisasterType disasterType;

    private enum DisasterType
    {
        Flood, Fire, Earthquake, Tornado, Winterstorm
    }

    private DisasterType GetRandomDisasterType()
    {
        Array values = Enum.GetValues(typeof(DisasterType));
        DisasterType randomType = (DisasterType)values.GetValue(Random.Range(0, values.Length));
        return randomType;
    }

    // Start is called before the first frame update
    private void Start()
    {
        disasterType = GetRandomDisasterType();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}