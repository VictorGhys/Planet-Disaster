using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    [SerializeField] private Transform earthExplosion;
    [SerializeField] private Transform earthModel;

    public void DestroyEarth()
    {
        Instantiate(earthExplosion);
        GetComponent<Animator>().SetTrigger("GoAway");
        earthModel.gameObject.SetActive(false);
    }

    public void SetEarth(Transform earth)
    {
        earthModel = earth;
    }
}