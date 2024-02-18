using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Fields and Properties
    public enum MovementType { Vertical, Horizontal, Zigzag }
    public MovementType movementType;

    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    private float speed;

    [SerializeField]
    private float zigzagFrequency = 2f;
    [SerializeField]
    private float zigzagMagnitude = 0.5f;
    private PassengerSpawner spawner;
    // For zigzag movement calculation
    private Vector3 axis;
    private Vector3 direction;
    #endregion

    #region Unity Methods
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        spawner = GameObject.Find("Spawner").gameObject.GetComponent<PassengerSpawner>();
        // Set initial direction based on spawn side for horizontal and zigzag enemies
        switch (movementType)
        {
            case MovementType.Horizontal:
                direction = transform.position.x < 0 ? Vector3.right : Vector3.left;
                break;
            case MovementType.Zigzag:
                axis = transform.position.y > 0 ? Vector3.down : (transform.position.x > 0 ? Vector3.left : Vector3.right);
                direction = axis == Vector3.down ? Vector3.down : (axis == Vector3.left ? Vector3.left : Vector3.right);
                break;
            default:
                direction = Vector3.down;
                break;
        }
    }

    void Update()
    {
        switch (movementType)
        {
            case MovementType.Vertical:
                MoveVertical();
                break;
            case MovementType.Horizontal:
                MoveHorizontal();
                break;
            case MovementType.Zigzag:
                MoveZigzag();
                break;
        }
        if (IsOffScreen())
        {
            Destroy(gameObject);
        }
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag ==  "Player") {

           
            collision.gameObject.GetComponent<PlayerController>().DealDamage(1);
            Destroy(gameObject);
        }
    }
    #region Movement Methods
    void MoveVertical()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
    }

    void MoveHorizontal()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void MoveZigzag()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (direction == Vector3.down)
        {
            transform.position += Vector3.right * Mathf.Sin(Time.time * zigzagFrequency) * zigzagMagnitude;
        }
        else
        {
            transform.position += Vector3.down * Mathf.Sin(Time.time * zigzagFrequency) * zigzagMagnitude;
        }
    }

    bool IsOffScreen()
    {
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        return screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y < 0 || screenPosition.y > 1;
    }

    
    #endregion

    #region Inspector Methods
    // This region can be used for methods that are specifically meant to be called or shown in the Unity Inspector if needed in the future.
    #endregion
}
