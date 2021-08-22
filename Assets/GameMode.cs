using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private float earthRadius = 20;
    [SerializeField] private Transform disasterPF;
    [SerializeField] private Transform earthParent;
    [SerializeField] private Texture2D landWaterTex;
    private bool do_once = true;

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
        }
    }
}