using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Disaster : MonoBehaviour
{
    public float size { get; set; }
    public bool isGrowing { get; set; } = true;
    public Slider slider { get; set; }

    [SerializeField] private float startSize = 1;
    [SerializeField] private float burstSize = 10;
    [SerializeField] private float sizeGrowthSpeed = 10;
    [SerializeField] private Material floodMat;
    [SerializeField] private Material fireMat;
    [SerializeField] private Material earthquakeMat;
    [SerializeField] private Material tornadoMat;
    [SerializeField] private Material winterstormMat;

    [SerializeField] private Transform floodModel;
    [SerializeField] private Transform fireModel;
    [SerializeField] private Transform earthquakeModel;
    [SerializeField] private Transform tornadoModel;
    [SerializeField] private Transform winterstormModel;
    [SerializeField] private float populationDrainRate;
    private DisasterType disasterType;
    private Transform model;
    private Vector3 modelSize;
    private float iconYScale;

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
        iconYScale = transform.localScale.y;
        disasterType = GetRandomDisasterType();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        switch (disasterType)
        {
            case DisasterType.Flood:
                renderer.material = floodMat;
                break;

            case DisasterType.Fire:
                renderer.material = fireMat;
                modelSize = fireModel.localScale;
                model = Instantiate(fireModel, transform.position, transform.rotation, transform.parent);
                break;

            case DisasterType.Earthquake:
                renderer.material = earthquakeMat;

                break;

            case DisasterType.Tornado:
                renderer.material = tornadoMat;
                modelSize = tornadoModel.localScale;
                model = Instantiate(tornadoModel, transform.position, transform.rotation, transform.parent);
                break;

            case DisasterType.Winterstorm:
                renderer.material = winterstormMat;
                modelSize = winterstormModel.localScale;
                model = Instantiate(winterstormModel, transform.position, transform.rotation, transform.parent);
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
            if (size < burstSize)
            {
                size += sizeGrowthSpeed * Time.deltaTime;
                //transform.localScale = new Vector3(size, transform.localScale.y, size);
                transform.localScale = new Vector3(size, iconYScale * size / 2, size);
                if (model)
                {
                    model.transform.localScale = modelSize * size;
                }
            }
            else
            {
                //burst
            }
        }
        //drain population
        slider.value -= populationDrainRate * size * Time.deltaTime;
    }

    public void ResetSize()
    {
        size = startSize;
    }

    public void Relocate(Transform toTransform)
    {
        transform.position = toTransform.position;
        transform.rotation = toTransform.rotation;
        if (model)
        {
            model.transform.position = toTransform.position;
            model.rotation = toTransform.rotation;
        }
    }

    public void Destroy()
    {
        if (model)
        {
            Destroy(model.gameObject);
        }
        Destroy(transform.gameObject);
    }
}