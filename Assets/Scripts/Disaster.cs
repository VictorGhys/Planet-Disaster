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
    [SerializeField] private float burstPenalty = 0.1f;
    [SerializeField] private float sizeGrowthSpeed = 10;
    [SerializeField] private float earthquakeShakeSpeed = 1;
    [SerializeField] private float earthquakeShakeStrength = 1;
    
    [SerializeField] private Transform disasterModel;
    [SerializeField] private float populationDrainRate;
    [SerializeField] private Outline outline;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float volumeLow = 0.4f;
    [SerializeField] private float volumeHigh = 0.7f;
    [SerializeField] private Transform burstModel;

    [SerializeField] private Sprite disasterIcon;

    [SerializeField] private DisasterType disasterType;
    private Transform model;
    private Vector3 modelSize;
    private float iconYScale;

    public enum DisasterType
    {
        None = 0, Flood = 1, Fire = 2, Earthquake = 3, Tornado = 4, Winterstorm = 5
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
        modelSize = disasterModel.localScale;
        model = Instantiate(disasterModel, transform.position, transform.rotation, transform.parent);
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
                slider.value -= burstPenalty;
                Instantiate(burstModel, transform.position + transform.up, transform.rotation);
                Destroy();
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
            model.GetChild(0).transform.rotation = transform.rotation;
            model.GetChild(0).transform.Rotate(transform.up, sin);
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
        FindObjectOfType<GameMode>().RemoveDisaster(this);
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

    public float GetBurstSize()
    {
        return burstSize;
    }

    public Sprite GetDisasterIcon()
    {
        return disasterIcon;
    }
}