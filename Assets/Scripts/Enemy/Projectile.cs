using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    private Vector2 direction;
    private float angle;
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

        rb.linearVelocity = speed * direction;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void ChangeDirection(Vector2 direction, float angle)
    {
        this.direction = direction;
        this.angle = angle;
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
