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
    [SerializeField] private float earthquakeShakeSpeed = 1;
    [SerializeField] private float earthquakeShakeStrength = 1;
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
    [SerializeField] private Outline outline;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip floodSFX;
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private AudioClip earthquakeSFX;
    [SerializeField] private AudioClip tornadoSFX;
    [SerializeField] private AudioClip winterstormSFX;
    [SerializeField] private float volumeLow = 0.4f;
    [SerializeField] private float volumeHigh = 0.7f;

    private DisasterType disasterType;
    private Transform model;
    private Vector3 modelSize;
    private float iconYScale;

    public enum DisasterType
    {
        None, Flood, Fire, Earthquake, Tornado, Winterstorm
    }

    public DisasterType GetDisasterType()
    {
        return disasterType;
    }

    public void SetDisasterType(DisasterType type)
    {
        disasterType = type;
    }

    // Start is called before the first frame update
    private void Start()
    {
        DisableOutline();
        size = startSize;
        iconYScale = transform.localScale.y;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        switch (disasterType)
        {
            case DisasterType.Flood:
                renderer.material = floodMat;
                modelSize = floodModel.localScale;
                model = Instantiate(floodModel, transform.position, transform.rotation, transform.parent);
                audioSource.clip = floodSFX;
                break;

            case DisasterType.Fire:
                renderer.material = fireMat;
                modelSize = fireModel.localScale;
                model = Instantiate(fireModel, transform.position, transform.rotation, transform.parent);
                audioSource.clip = fireSFX;
                break;

            case DisasterType.Earthquake:
                renderer.material = earthquakeMat;
                modelSize = earthquakeModel.localScale;
                model = Instantiate(earthquakeModel, transform.position, transform.rotation, transform.parent);
                audioSource.clip = earthquakeSFX;
                break;

            case DisasterType.Tornado:
                renderer.material = tornadoMat;
                modelSize = tornadoModel.localScale;
                model = Instantiate(tornadoModel, transform.position, transform.rotation, transform.parent);
                audioSource.clip = tornadoSFX;
                break;

            case DisasterType.Winterstorm:
                renderer.material = winterstormMat;
                modelSize = winterstormModel.localScale;
                model = Instantiate(winterstormModel, transform.position, transform.rotation, transform.parent);
                audioSource.clip = winterstormSFX;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        audioSource.Play();
    }

    // Update is called once per frame
    private void Update()
    {
        //Grow
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
        if (slider)
        {
            slider.value -= populationDrainRate * size * Time.deltaTime;
        }
        //Earthquake shake animation
        if (disasterType == DisasterType.Earthquake)
        {
            float sin = Mathf.Sin(Time.time * earthquakeShakeSpeed) * earthquakeShakeStrength;
            //model.transform.position += model.transform.right * sin;
            //transform.position += transform.right * sin;
            model.GetChild(0).transform.rotation = transform.rotation;
            model.GetChild(0).transform.Rotate(transform.up, sin);
            //transform.Rotate(transform.up, sin);
        }
        //volume adjustment (the bigger the louder)
        float t = (size - startSize) / (burstSize - startSize);
        audioSource.volume = Mathf.Lerp(volumeLow, volumeHigh, t);
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
        audioSource.Stop();
        if (model)
        {
            Destroy(model.gameObject);
        }
        Destroy(transform.gameObject);
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }
}