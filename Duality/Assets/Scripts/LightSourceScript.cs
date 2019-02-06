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

    public float litAreaEdgeRadius;
	// Use this for initialization
	void Start () {
		litArea = new GameObject("Light", typeof(MeshFilter), typeof(MeshRenderer), typeof(EdgeCollider2D));
        //litArea.transform.parent = transform;
        litArea.transform.position = Vector2.zero;
        litArea.GetComponent<MeshRenderer>().material = litMaterial;
        litArea.GetComponent<EdgeCollider2D>().edgeRadius = litAreaEdgeRadius;
        litArea.layer = LayerMask.NameToLayer("LightArea");
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
        LayerMask raycastMask = int.MaxValue - LayerMask.GetMask("LightArea");

        for (int i = 0; i <= amountOfRayCasts; ++i) {
            float angle = Mathf.Deg2Rad * (startAngle + (i * angleDifference) / (float)amountOfRayCasts);
           
            Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
           
            //RaycastHit2D raycastHitData2D = Physics2D.Raycast(raycastStart, dir, raycastRange);
            RaycastHit2D[] raycastData = Physics2D.RaycastAll(raycastStart, dir, raycastRange, raycastMask);
            
            bool hitPlayer = false;
            foreach(RaycastHit2D raycast in raycastData){
                if(raycast.collider.tag == "Player"){
                    PlayerScript script = raycast.collider.GetComponent<PlayerScript>();
                    //script.contactLight();
                    hitPlayer = true;
                }
                else{
                    break;
                }
            }
            float distance = raycastData.Length > 0 ? raycastData[0].distance : raycastRange;
            if(hitPlayer){
                LayerMask maskWithoutPlayer = raycastMask - LayerMask.GetMask("Character");
                
                RaycastHit2D raycast = Physics2D.Raycast(raycastStart, dir, raycastRange, maskWithoutPlayer);
                distance = raycast.collider != null ? raycast.distance : raycastRange;
            }
            
            float drawRange = distance;
            Debug.DrawRay(raycastStart, dir * drawRange, Color.white, 0.0f, true);
            vertices[i + 1] = new Vector2(raycastStart.x, raycastStart.y) + drawRange * dir;
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
        litArea.GetComponent<EdgeCollider2D>().points = convertVector3ArrayToVector2Array(vertices); 
        
    }

    private Vector2[] convertVector3ArrayToVector2Array(Vector3[] input){
        Vector2[] output = new Vector2[input.Length + 1];
        for(int i = 0; i < input.Length; i++){
            output[i] = new Vector2(input[i].x, input[i].y);
            
        }
        output[input.Length] = output[0];
        return output;
    }

}
