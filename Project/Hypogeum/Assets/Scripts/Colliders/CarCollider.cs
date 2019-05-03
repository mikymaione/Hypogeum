using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollider : MonoBehaviour
{
    private Mesh sharedMesh;

    public void SetMesh()
    {
        sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
    }
}
