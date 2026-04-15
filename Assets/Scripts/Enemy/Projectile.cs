using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    private int direction = 1;
    private float spawnTime;
    private float lifetime = 3.0f;
    private bool canDamage = true;
    private Rigidbody2D rb;
    private void Start()
    {
        spawnTime = Time.time;

        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Time.time >= spawnTime + lifetime)
        {
            Destroy(gameObject);
        }

        rb.linearVelocity = new Vector2(speed * direction, 0);
    }

    public void ChangeDirection(int direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        if (tag == "Player" || tag == "Ground" || tag == "Wall")
        {
            Destroy(gameObject);

            if (tag == "Player")
            {
                Player player = collision.GetComponent<Player>();

                if (player != null)
                {
                    player.Die();
                }
            }
        }
    }
}
