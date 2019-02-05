using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSourceScript : MonoBehaviour {
    public float startAngle;
    public float endAngle;
    public int amountOfRayCasts;

    public float raycastRange;

    public bool isOn;
    GameObject litArea;
    public Material litMaterial;
	// Use this for initialization
	void Start () {
		litArea = new GameObject("Light", typeof(MeshFilter), typeof(MeshRenderer));
        //litArea.transform.parent = transform;
        litArea.transform.position = Vector2.zero;
        litArea.GetComponent<MeshRenderer>().material = litMaterial;
	}
	
	// Update is called once per frame
	void Update () {
        if (isOn) {
            GenerateLightArea();
        }
	}

    void GenerateLightArea() {
        Vector3 raycastStart = transform.position;
        Vector3[] vertices = new Vector3[amountOfRayCasts + 2];
        vertices[0] = raycastStart;
        Vector2[] uvs = new Vector2[amountOfRayCasts + 2];
        uvs[0] = Vector2.zero;
        int[] triangles = new int[amountOfRayCasts * 3];

        float angleDifference = endAngle - startAngle;
        
        for (int i = 0; i <= amountOfRayCasts; ++i) {
            float angle = Mathf.Deg2Rad * (startAngle + i * angleDifference / (float)amountOfRayCasts);
            Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            
            RaycastHit2D raycastHitData2D = Physics2D.Raycast(raycastStart, dir, raycastRange);
            Debug.DrawRay(raycastStart, dir * raycastHitData2D.distance, Color.white, 0.0f, true);

            if(raycastHitData2D.collider != null){
                if(raycastHitData2D.collider.tag == "PlayerBlack"){
                    print("player in area");
                }
            }
          
            vertices[i + 1] = new Vector2(raycastStart.x, raycastStart.y) + raycastHitData2D.distance * dir;
            uvs[i + 1] = Vector2.zero;

            if(i != amountOfRayCasts){
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        litArea.GetComponent<MeshFilter>().mesh = mesh;

    }
}
