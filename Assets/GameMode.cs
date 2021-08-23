﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using static Disaster.DisasterType;

public class GameMode : MonoBehaviour
{
    [SerializeField] private float earthRadius = 20;
    [SerializeField] private Transform disasterPF;
    [SerializeField] private Transform earthParent;
    [SerializeField] private Texture2D landWaterTex;
    [SerializeField] private Camera camera;
    private bool do_once = true;
    private Disaster selectedDisaster = null;

    private Dictionary<Disaster.DisasterType, List<Disaster.DisasterType>> disasterMatches =
        new Dictionary<Disaster.DisasterType, List<Disaster.DisasterType>> {
            { Flood, new List<Disaster.DisasterType>{Fire}},
            { Fire, new List<Disaster.DisasterType>{Earthquake, Winterstorm}},
            { Earthquake, new List<Disaster.DisasterType>{Flood, Tornado}},
            { Tornado, new List<Disaster.DisasterType>{Fire, Winterstorm}},
            { Winterstorm, new List<Disaster.DisasterType>{Flood, Fire}},
        };

    private Dictionary<Disaster.DisasterType, List<Disaster.DisasterType>> disasterWorseners =
        new Dictionary<Disaster.DisasterType, List<Disaster.DisasterType>> {
            { Flood, new List<Disaster.DisasterType>{Flood, Tornado}},
            { Fire, new List<Disaster.DisasterType>{Fire, Tornado}},
            { Earthquake, new List<Disaster.DisasterType>{Earthquake, Winterstorm}},
            { Tornado, new List<Disaster.DisasterType>{Tornado, Earthquake}},
            { Winterstorm, new List<Disaster.DisasterType>{Winterstorm, Tornado}},
        };

    private void CreateDisaster()
    {
        Vector3 pos = GetRandomDisasterSpawnPos();
        Instantiate(disasterPF, pos, Quaternion.identity, earthParent);
    }

    private Vector3 GetRandomDisasterSpawnPos()
    {
        Vector3 rand;
        Vector3 randPos;
        RaycastHit hit;
        int n = 0;
        while (n < 100)
        {
            rand = Random.onUnitSphere;
            randPos = rand * earthRadius;
            if (Physics.Raycast(randPos + rand, -rand, out hit, earthRadius))
            {
                Vector2 texCoord = hit.textureCoord;
                float sample = landWaterTex.GetPixelBilinear(texCoord.x, texCoord.y).a;
                if (sample < 0.5f)
                {
                    return randPos;
                }
            }
            n++;
        }
        Debug.Log("failed!");
        return Vector3.back;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (do_once)
        {
            do_once = false;
            CreateDisaster();
            CreateDisaster();
        }
        //click disasters to select them if an other is selected match them
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                //Debug.Log(hit.transform.gameObject.name);
                Disaster hitDisaster = hit.transform.gameObject.GetComponent<Disaster>();
                if (hitDisaster != null)
                {
                    if (selectedDisaster != null)
                    {
                        //match
                        Debug.Log("match " + selectedDisaster.GetDisasterType() + " with " + hitDisaster.GetDisasterType());
                    }
                    else
                    {
                        //select
                        selectedDisaster = hit.transform.gameObject.GetComponent<Disaster>();
                        Debug.Log("selected " + selectedDisaster.GetDisasterType());
                    }
                }
                else
                {
                    //unselect
                    selectedDisaster = null;
                }
            }
        }
    }
}