using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _rotationSpeed = 100f;
    [SerializeField]
    private int _jumpAmount = 4;
    [SerializeField]
    private LayerMask groundLayer;
    private int _currentJumpAmount = 0;

    private bool AllowJump = true;

    private PolygonCollider2D PolygonCollider;
    private Rigidbody2D body;
    private Vector2 _inputX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        PolygonCollider = GetComponent<PolygonCollider2D>();
        
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(_inputX.x * _speed, body.linearVelocity.y);
        body.linearVelocity = movement;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputX.x > 0)
        {
            transform.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
        }
        else if (_inputX.x < 0)
        {
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
        }


        _inputX = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        if (Input.GetKeyDown(KeyCode.Space) && AllowJump)
        {
            body.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            _currentJumpAmount++;
            if (_currentJumpAmount >= _jumpAmount)
            {
                AllowJump = false;
            }
        }
        if (isGrounded())
        {
            _currentJumpAmount = 0;
            AllowJump = true;
            
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(PolygonCollider.bounds.center, PolygonCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        
        return raycastHit.collider != null;
    }
}
