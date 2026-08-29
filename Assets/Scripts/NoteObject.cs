using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool obtained = false;
    public bool canBePressed;
    public KeyCode keyToPress;

    public GameObject goodEffect, greatEffect, perfectEffect, missEffect;

    // Hit line and thresholds
    public float hitLineY = -3.4f;
    public float perfectThreshold = 0.15f;  // most defined threshold
    public float greatThreshold = 0.25f;    // further outside
    public float goodThreshold = 0.35f;     // last boundary until not counted

    // fixed area for timing
    public float effectX = 0f;              
    public float effectY = -3.9f;           

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                obtained = true;
                canBePressed = false;

                float distance = Mathf.Abs(transform.position.y - hitLineY);

                // one shared, centred position for every lane's effect
                Vector3 effectPos = new Vector3(effectX, effectY, 0f);

                if (distance < perfectThreshold)
                {
                    GameManager.instance.PerfectHit();
                    Debug.Log("Perfect Hit");
                    Instantiate(perfectEffect, effectPos, perfectEffect.transform.rotation);
                }
                else if (distance < greatThreshold)
                {
                    GameManager.instance.GreatHit();
                    Debug.Log("Great Hit");
                    Instantiate(greatEffect, effectPos, greatEffect.transform.rotation);
                }
                else if (distance < goodThreshold)
                {
                    GameManager.instance.GoodHit();
                    Debug.Log("Good Hit");
                    Instantiate(goodEffect, effectPos, goodEffect.transform.rotation);
                }
                else
                {
                    Debug.Log("Missed Note");
                    GameManager.instance.NoteMissed();
                    Instantiate(missEffect, effectPos, missEffect.transform.rotation);
                }
                

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