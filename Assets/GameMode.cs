using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private float earthRadius = 20;
    [SerializeField] private Transform disasterPF;
    [SerializeField] private Transform earthParent;
    [SerializeField] private Texture2D landWaterTex;

    private void CreateDisaster()
    {
        Vector3 pos = GetRandomDisasterSpawnPos();
        Instantiate(disasterPF, pos, Quaternion.identity, earthParent);
    }

    private Vector3 GetRandomDisasterSpawnPos()
    {
        Vector3 rand = Random.onUnitSphere;
        Vector3 randPos = rand * earthRadius;
        RaycastHit hit;
        int n = 0;
        while (n < 100)
        {
            if (Physics.Raycast(randPos, -rand, out hit, earthRadius))
            {
                Vector2 texCoord = hit.textureCoord;
                if (landWaterTex.GetPixelBilinear(texCoord.x, texCoord.y).grayscale < 0.5f)
                {
                    return randPos;
                }
            }
            n++;
        }
        Debug.Log("failed!");
        return randPos;
    }

    // Start is called before the first frame update
    private void Start()
    {
        CreateDisaster();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}