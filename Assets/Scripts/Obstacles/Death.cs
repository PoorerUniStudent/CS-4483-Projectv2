using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField]
    private Vector2 respawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.transform.position = respawnPoint;
        }
    }
}
