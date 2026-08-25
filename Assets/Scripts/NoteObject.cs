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
            Debug.Log("KEY DOWN: " + keyToPress + " | canBePressed=" + canBePressed);  // B

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
        Debug.Log("ENTER: " + other.name + " tag=" + other.tag);  // C
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