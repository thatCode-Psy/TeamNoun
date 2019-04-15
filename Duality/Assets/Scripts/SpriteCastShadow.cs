using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCastShadow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().receiveShadows = true;
        gameObject.GetComponent<SpriteRenderer>().castShadows = true;
        gameObject.GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
