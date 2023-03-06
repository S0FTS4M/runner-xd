using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCube : MonoBehaviour
{
    public bool IsBad;


    private Vector3 rotation = new Vector3(0, 0, 1);
    public void FixedUpdate()
    {
        if (IsBad==false) return;

        transform.Rotate(rotation);
    }
}
