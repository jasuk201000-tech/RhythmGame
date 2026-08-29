using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool obtained = false;
    public bool canBePressed;
    public KeyCode keyToPress;

    public GameObject goodEffect, greatEffect, perfectEffect, missEffect;

    // hitline thresholds
    public float hitLineY = -3.4f;
    public float perfectThreshold = 0.15f;
    public float greatThreshold = 0.25f;
    public float goodThreshold = 0.35f;

    // effect spawn point
    public float effectX = 0f;
    public float effectY = -3.6f;

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                obtained = true;
                canBePressed = false;

                float distance = Mathf.Abs(transform.position.y - hitLineY);
                Vector3 effectPos = new Vector3(effectX, effectY, 0f);

                if (distance < perfectThreshold)
                {
                    GameManager.instance.PerfectHit();
                    Instantiate(perfectEffect, effectPos, perfectEffect.transform.rotation);
                }
                else if (distance < greatThreshold)
                {
                    GameManager.instance.GreatHit();
                    Instantiate(greatEffect, effectPos, greatEffect.transform.rotation);
                }
                else if (distance < goodThreshold)
                {
                    GameManager.instance.GoodHit();
                    Instantiate(goodEffect, effectPos, goodEffect.transform.rotation);
                }
                else
                {
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