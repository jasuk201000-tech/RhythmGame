using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool obtained = false;
    public bool canBePressed;
    public KeyCode keyToPress;


    public GameObject goodEffect, greatEffect, perfectEffect, missEffect;

    // Hit line and thresholds
    public float hitLineY = -3.4f;
    public float perfectThreshold = 0.08f;  // most defined threshold
    public float greatThreshold = 0.15f;  // further outside
    public float goodThreshold = 0.25f;  // last boundary until not counted

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                obtained = true;
                canBePressed = false;

                float distance = Mathf.Abs(transform.position.y - hitLineY);

                if (distance < perfectThreshold)
                {
                    GameManager.instance.PerfectHit();
                    Debug.Log("Perfect Hit");
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation); 
                }
                else if (distance < greatThreshold)
                {
                    GameManager.instance.GreatHit();
                    Debug.Log("Great Hit");
                    Instantiate(greatEffect, transform.position, greatEffect.transform.rotation);
                }
                else if (distance < goodThreshold)
                {
                    GameManager.instance.GoodHit();
                    Debug.Log("Good Hit");
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                }
                else
                {
                    Debug.Log("Missed Note");
                    GameManager.instance.NoteMissed();
                    Instantiate(missEffect, transform.position, missEffect.transform.rotation);
                }
                // outside goodThreshold: no hit registered

                gameObject.SetActive(false);   
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Activator") return;
        canBePressed = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Activator") return;
        canBePressed = false;
        if (!obtained)
            GameManager.instance.NoteMissed();
    }
}