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

    private void FixedUpdate()
    {
        rb.linearVelocity = speed * direction;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Update()
    {
        if (Time.time >= spawnTime + lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeDirection(Vector2 direction, float angle)
    {
        this.direction = direction;
        this.angle = angle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        int layerIndex = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerIndex);

        if (layerName == "Ground" || layerName == "Wall")
        {
            Destroy(gameObject);
            return;
        }

        if (tag == "Player")
        {
            speed = 0f;
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.Die();
            }

            Destroy(gameObject);
        }
    }
}
