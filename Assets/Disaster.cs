using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Disaster : MonoBehaviour
{
    private DisasterType disasterType;
    public float size { get; set; }
    [SerializeField] private float startSize = 1;
    [SerializeField] private float burstSize = 10;
    [SerializeField] private float sizeGrowthSpeed = 10;
    public bool isGrowing { get; set; } = true;
    [SerializeField] private Material floodMat;
    [SerializeField] private Material fireMat;
    [SerializeField] private Material earthquakeMat;
    [SerializeField] private Material tornadoMat;
    [SerializeField] private Material winterstormMat;

    public enum DisasterType
    {
        Flood, Fire, Earthquake, Tornado, Winterstorm
    }

    private DisasterType GetRandomDisasterType()
    {
        Array values = Enum.GetValues(typeof(DisasterType));
        DisasterType randomType = (DisasterType)values.GetValue(Random.Range(0, values.Length));
        return randomType;
    }

    public DisasterType GetDisasterType()
    {
        return disasterType;
    }

    // Start is called before the first frame update
    private void Start()
    {
        size = startSize;
        disasterType = GetRandomDisasterType();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        switch (disasterType)
        {
            case DisasterType.Flood:
                renderer.material = floodMat;
                break;

            case DisasterType.Fire:
                renderer.material = fireMat;

                break;

            case DisasterType.Earthquake:
                renderer.material = earthquakeMat;

                break;

            case DisasterType.Tornado:
                renderer.material = tornadoMat;

                break;

            case DisasterType.Winterstorm:
                renderer.material = winterstormMat;

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isGrowing)
        {
            size += sizeGrowthSpeed * Time.deltaTime;
            transform.localScale = new Vector3(size, size, size);
        }
    }
}