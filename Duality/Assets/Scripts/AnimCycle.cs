using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCycle : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite idle;
    public Sprite[] frames = new Sprite[8];
    public float bwFrames = 0.1f;
    int fCount = 0;
    int fCap;
    public bool transition = false;

    void Start()
    {
        fCap = frames.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!transition)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                transition = true;
                Invoke("IncrementStep", bwFrames);
            }
            else
            {
                transition = true;
                Invoke("ToIdle", bwFrames);
            }
        }
    }

    void IncrementStep()
    {
        GetComponent<SpriteRenderer>().sprite = frames[fCount];
        if (fCount < fCap)
            fCount++;
        else
            fCount = 0;
        transition = false;
    }

    void ToIdle()
    {
        GetComponent<SpriteRenderer>().sprite = idle;
        fCount = 0;
        transition = false;
    }
}
