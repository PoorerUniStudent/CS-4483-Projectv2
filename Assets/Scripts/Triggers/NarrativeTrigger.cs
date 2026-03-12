using TMPro;
using UnityEngine;

public class NarrativeTrigger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp;
    [SerializeField]
    private string message;

    private bool disabled;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (disabled)
        {
            return;
        }

        if (collision.tag == "Player")
        {
            tmp.text = message;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (disabled)
        {
            return;
        }

        if (collision.tag == "Player")
        {
            tmp.text = "";
        }
    }

    public void Disable()
    {
        disabled = true;
        tmp.text = "";
    }
}
