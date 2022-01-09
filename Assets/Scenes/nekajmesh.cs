using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nekajmesh : MonoBehaviour
{
    private Vector3 target = new Vector3(5, 0, 0);
    int x = 1; // _
    int y = 1; // |
    int z = 1;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] koti = new Vector3[]{
            new Vector3(-x, y, -z),
            new Vector3(-x, y, z),
            new Vector3(x, y, -z),
            new Vector3(x, y, z),

            new Vector3(0, -y, 0)
    };
        int[] trikotniki = new int[]{
            //kvadrat
            0,1,2,
            2,1,3,
            // trikotniki
            2,3,4,
            3,1,4,//
            1,0,4,
            0,2,4 //
        };

        GetComponent<MeshFilter>().mesh.Clear();
        GetComponent<MeshFilter>().mesh.vertices = koti;
        GetComponent<MeshFilter>().mesh.triangles = trikotniki;
    }
    void Update()
    {
        transform.Rotate(Vector3.up * (30 * Time.deltaTime));
    }

}
