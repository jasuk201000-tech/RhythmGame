using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool obtained = false;
    public bool canBePressed;
    public KeyCode keyToPress;

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                obtained = true;
                GameManager.instance.NoteHit();
                canBePressed = false;
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

        Debug.Log("EXIT | obtained=" + obtained);

        canBePressed = false;

        if (!obtained)                              // only a real miss if never hit
            GameManager.instance.NoteMissed();
    }
}