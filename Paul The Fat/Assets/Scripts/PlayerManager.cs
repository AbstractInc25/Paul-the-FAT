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
    private bool StartJump = false;
    private bool StartRotation = false;

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
        
        _inputX = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        if (isTouchingRight() || isTouchingLeft())
        {
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);
        }

        if (_inputX.x > 0 && !isTouchingRight())
        {
            
            transform.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
        }
        else if (_inputX.x < 0 && !isTouchingLeft())
        {
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && AllowJump)
        {
            StartRotation = true;
            // rotation begins; actual jump happens after rotation completes
            
        }
        if (isGrounded())
        {
            _currentJumpAmount = 0;
            AllowJump = true;
            
        }

        if (StartRotation)
        {
            StartJump = rotate_until(45f);
        }

        if (StartJump)
        {
            StartRotation = false;
            //transform.localRotation = Quaternion.Euler(0, 0, 90);
            body.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            //transform.localRotation = Quaternion.Euler(0, 0, 90);
            _currentJumpAmount++;
            if (_currentJumpAmount >= _jumpAmount)
            {
                AllowJump = false;
            }
            StartJump = false;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(PolygonCollider.bounds.center, PolygonCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        
        return raycastHit.collider != null;
    }

    private bool isTouchingRight()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(PolygonCollider.bounds.center, PolygonCollider.bounds.size, 0f, Vector2.right, 0.1f, groundLayer);
        
        return raycastHit.collider != null;
    }

    private bool isTouchingLeft()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(PolygonCollider.bounds.center, PolygonCollider.bounds.size, 0f, Vector2.left, 0.1f, groundLayer);
        
        return raycastHit.collider != null;
    }



    private bool rotate_until(float targetAngle)
    {
        float currentAngle = transform.localEulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, _rotationSpeed * 20 * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0f, 0f, newAngle);
        return Mathf.Abs(Mathf.DeltaAngle(newAngle, targetAngle)) < 0.01f;
        
    }
    
        
        

    
}
