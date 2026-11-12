using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class IsometricPlayerControl : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 inputVector;
    private PlayerControls controls;

    [Header("Player Input Settings")]
    public bool isoMovement;
    public Vector2 isoRight = new Vector2(1, 0.5f);
    public Vector2 isoUp = new Vector2(1, 0.5f);

    [Header("8-Directional Sprites")]
    public SpriteRenderer spriteRenderer;

    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    public Sprite spriteUpLeft;
    public Sprite spriteUpRight;
    public Sprite spriteDownLeft;
    public Sprite spriteDownRight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
    }
    
    private void OnEnable()
    {
        controls.Player.Move.performed += ctx => inputVector = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => inputVector = Vector2.zero;
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        if (!isoMovement)
        {
            Vector2 moveDir = inputVector.normalized;
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 isoDirection = (isoRight * inputVector.x + isoUp * inputVector.y).normalized;
            rb.MovePosition(rb.position + isoDirection * moveSpeed * Time.fixedDeltaTime);
        }
        UpdateSpriteDirection(inputVector);
    }

    private void UpdateSpriteDirection(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return;
        }
        direction.Normalize();

        //Define direction thresholds
        if (direction.y > 0.5f)
        {
            if (direction.x > 0.5f)
                spriteRenderer.sprite = spriteUpRight;
            else if (direction.x < -0.5f)
                spriteRenderer.sprite = spriteUpLeft;
            else
                spriteRenderer.sprite = spriteUp;
        }
        else if (direction.y < -0.5f)
        {
            if (direction.x > 0.5f)
                spriteRenderer.sprite = spriteDownRight;
            else if (direction.x < -0.5f)
                spriteRenderer.sprite = spriteDownLeft;
            else
                spriteRenderer.sprite = spriteDown;
        }
        else
        {
            if (direction.x > 0)
                spriteRenderer.sprite = spriteRight;
            else if (direction.x < 0)
                spriteRenderer.sprite = spriteLeft;

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
