using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedLightSourceScript : MonoBehaviour
{
    protected float startAngle;
    protected float endAngle;

    public float baseAngle;

    public float startAngleOffset;
    public float endAngleOffset;
    public int amountOfRayCasts;

    float raycastRange;

    
    public bool isOn;
    GameObject litArea;

    public float litAreaEdgeRadius;

    Light spotLight;
    static GameObject lightParent;
	// Use this for initialization
	protected void Start () {
        if(lightParent == null){
            lightParent = new GameObject("LightParent", typeof(CompositeCollider2D));
            Rigidbody2D body = lightParent.GetComponent<Rigidbody2D>();
            body.constraints = RigidbodyConstraints2D.FreezeAll;
            body.mass = float.MaxValue;
            //gravityScale = 0;
    //        lightParent.GetCompoentn
            lightParent.GetComponent<CompositeCollider2D>().edgeRadius = litAreaEdgeRadius;
            lightParent.layer = LayerMask.NameToLayer("LightArea");
            lightParent.transform.position = new Vector3(-10000, 0,0);
        }
		litArea = new GameObject("Light", typeof(PolygonCollider2D));
        litArea.transform.parent = lightParent.transform;
        litArea.transform.position = Vector2.zero;
        
        //litArea.GetComponent<PolygonCollider2D>().edgeRadius = litAreaEdgeRadius;
        litArea.GetComponent<PolygonCollider2D>().usedByComposite = true;
        // litArea.GetComponent<PolygonCollider2D>().isTrigger = true;
        litArea.layer = LayerMask.NameToLayer("LightArea");
        spotLight = GetComponent<Light>();
        raycastRange = spotLight.range;
        startAngle = 180f - spotLight.spotAngle/2 + baseAngle;
        endAngle = startAngle + spotLight.spotAngle;
        startAngle += startAngleOffset;
        endAngle -= endAngleOffset;
        isOn = spotLight.enabled;
	}
	
	// Update is called once per frame
	protected void FixedUpdate () {
        spotLight.enabled = isOn;
        if (isOn) {
            GenerateLightArea();
        }
        else{
            litArea.GetComponent<PolygonCollider2D>().enabled = false;
        }
        
	}

    void GenerateLightArea() {
        Vector3 raycastStart = transform.position;
        Vector3[] vertices = new Vector3[amountOfRayCasts + 2];
        vertices[0] = raycastStart;

        float angleDifference = endAngle - startAngle;
        LayerMask raycastMask = int.MaxValue - LayerMask.GetMask("LightArea") - LayerMask.GetMask("Ignore Raycast") - LayerMask.GetMask("Glass");

        for (int i = 0; i <= amountOfRayCasts; ++i) {
            float angle = Mathf.Deg2Rad * (startAngle + (i * angleDifference) / (float)amountOfRayCasts);
           
            Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
           
            //RaycastHit2D raycastHitData2D = Physics2D.Raycast(raycastStart, dir, raycastRange);
            RaycastHit2D[] raycastData = Physics2D.RaycastAll(raycastStart, dir, raycastRange, raycastMask);
            
            bool hitPlayer = false;
            
                foreach(RaycastHit2D raycast in raycastData){
                    if(raycast.collider.tag == "Player"){
                        
                        if(i != 0 && i != amountOfRayCasts){
                            PlayerScript script = raycast.collider.GetComponent<PlayerScript>();
                            script.contactLight();
                        }
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
            vertices[i + 1].z = transform.position.z;
        }
        
        litArea.GetComponent<PolygonCollider2D>().points = convertVector3ArrayToVector2Array(vertices); 
        litArea.GetComponent<PolygonCollider2D>().enabled = true;
        
        
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
