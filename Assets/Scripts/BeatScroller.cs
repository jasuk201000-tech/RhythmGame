using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float BeatTempo;

    public bool HasStarted;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BeatTempo = BeatTempo / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!HasStarted)
        {
            /*if (Input.anyKeyDown)
            {
                HasStarted = true;
            }
            */
        }
        else
        {
            transform.position -= new Vector3(0f, BeatTempo * Time.deltaTime, 0f);
        }
    }
}
