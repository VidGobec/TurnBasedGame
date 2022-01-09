using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildmesh : MonoBehaviour
{
    public Vector3 vertleftbottomfront = new Vector3(-1, -1, 1);
    public Vector3 vertrightbottomfront = new Vector3(1, -1, 1);
    public Vector3 vertleftbottomback = new Vector3(-1, -1, -1);
    public Vector3 vertrightbottomnack = new Vector3(1, -1, -1);
    
    void start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;
        Vector3[] vertecies = new Vector3[]
        {
            //front face
            new Vector3(-1,1,1), //left-top front
            new Vector3(1,1,1),//right-top front
            vertleftbottomfront,//left-bottom front 
            vertrightbottomfront,//right-bottom front
            //back face
            new Vector3(1,1,-1), //right-top back
            new Vector3(-1,1,-1),//left-top back
            vertrightbottomnack,//right-bottom back
            vertleftbottomback,//left-bottom back
            //left face
            new Vector3(-1,1,-1), //left-top back
            new Vector3(-1,1,1),//left-top front
            vertleftbottomback,//left-bottom back 
            vertleftbottomfront,//left-bottom front 
            //right face
            new Vector3(1,1,1), //right-top front
            new Vector3(1,1,-1),//right-top back
            vertrightbottomfront,//right-bottom front
            vertrightbottomnack,//right-bottom back
            //top
            new Vector3(-1,1,-1), //left-top back
            new Vector3(1,1,-1),//right-top back
            new Vector3(-1,1,1),//left-top front
            new Vector3(1,1,1),//right-top front
            //bottom
            vertleftbottomfront, //left-bottom front 
            vertrightbottomfront,//right-bottom front
            vertleftbottomback,//left-bottom back
            vertrightbottomnack//right-bottom back
        };

        int[] triangles= new int[]{
            //front
            0,2,3,
            3,1,0,
            //back
            4,6,7,
            7,5,4,
            //left
            8,10,11,
            11,9,8,
            //right
            12,14,15,
            15,13,12,
            //top
            16,18,19,
            19,17,16,
            //bottom
            20,22,23,
            23,21,20,
        };

        Vector2[] uv = new Vector2[] {
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0)
        };

        mesh.Clear();
        mesh.vertices = vertecies;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform != null && hit.transform.gameObject.tag == "enemy")
            {
                Vector3 newpos = new Vector3(hit.transform.gameObject.transform.position.x, hit.transform.gameObject.transform.position.y, hit.transform.gameObject.transform.position.z);
                this.transform.position = newpos;
            }
        }
        



        //transform.Rotate(Vector3.up, 10 * Time.deltaTime);
        
    }
}
