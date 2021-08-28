using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnClose : MonoBehaviour
{
    public void OnClose()
    {
        Destroy(gameObject);
    }
}