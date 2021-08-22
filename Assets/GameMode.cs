using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private float earthRadius = 20;
    [SerializeField] private Transform disasterPF;

    private void CreateDisaster()
    {
        Vector3 pos = GetRandomDisasterSpawnPos();
        Instantiate(disasterPF, pos, Quaternion.identity);
    }

    private Vector3 GetRandomDisasterSpawnPos()
    {
        Vector3 rand = Random.onUnitSphere;
        Vector3 randPos = rand * earthRadius;
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