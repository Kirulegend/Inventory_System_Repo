using UnityEngine;

// ===================== Project_1_Player =====================
public class Project_1_Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float _moveSpeed = 10f; // Base movement speed of the player
    [SerializeField] float _jumpForce = 10f; // Upward force applied when jumping
    [SerializeField] float _mouseSens = 100f; // Mouse sensitivity for camera rotation
    [SerializeField] Vector2 _mouseXBound = new Vector2(-90f, 90f); // Vertical camera rotation clamp
    [SerializeField] LayerMask _groundMask; // Layer to detect ground collision

    [Header("Camera & Raycast")]
    [SerializeField] Transform _camtransform; // Reference to the child camera object

    Rigidbody _rb; // Rigidbody component for physics movement
    Vector2 _mouseRot; // Stores cumulative mouse X and Y movement
    Vector2 _playerMovement; // Stores player input for movement
    Vector3 _pos; // Target position for movement when a bot is selected
    bool _click = false; // Flag indicating whether the player is currently auto-moving to a bot

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the game window
        _rb = GetComponent<Rigidbody>(); // Cache the Rigidbody component
        _camtransform = transform.Find("Cam"); // Find the camera child object
    }

    void Update()
    {
        HandleMovement(); // Handles player movement and jumping
        HandleMouseLook(); // Handles camera rotation based on mouse movement
        PerformRaycast(); // Handles raycasting for interaction with enemies
    }

    void HandleMouseLook()
    {
        // Accumulate raw mouse input and apply sensitivity & deltaTime
        _mouseRot.x += Input.GetAxisRaw("Mouse X") * _mouseSens * Time.deltaTime;
        _mouseRot.y += Input.GetAxisRaw("Mouse Y") * _mouseSens * Time.deltaTime;

        // Clamp vertical look to avoid flipping the camera
        _mouseRot.y = Mathf.Clamp(_mouseRot.y, _mouseXBound.x, _mouseXBound.y);

        // Apply rotation to the player (yaw) and camera (pitch)
        transform.localRotation = Quaternion.Euler(0, _mouseRot.x, 0);
        _camtransform.localRotation = Quaternion.Euler(-_mouseRot.y, 0, 0);
    }

    void HandleMovement()
    {
        // Read WASD/Arrow key input
        _playerMovement.x = Input.GetAxisRaw("Horizontal");
        _playerMovement.y = Input.GetAxisRaw("Vertical");

        // Calculate and normalize movement direction
        Vector3 moveDirection = (transform.right * _playerMovement.x + transform.forward * _playerMovement.y).normalized;

        // Apply horizontal movement while preserving vertical velocity
        _rb.linearVelocity = new Vector3(moveDirection.x * _moveSpeed, _rb.linearVelocity.y, moveDirection.z * _moveSpeed);

        // Increase move speed when sprinting (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift)) _moveSpeed *= 1.5f;
        if (Input.GetKeyUp(KeyCode.LeftShift)) _moveSpeed /= 1.5f;

        // Jump if grounded and space is pressed
        if (GroundCheck() && Input.GetKeyDown(KeyCode.Space))
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _jumpForce, _rb.linearVelocity.z);
        }

        // Start moving toward the bot if LeftControl is pressed
        if (_pos != Vector3.zero && Input.GetKeyDown(KeyCode.LeftControl) && !_click) _click = true;

        // Continue moving toward the selected bot until arrival
        if (_click) transform.position = Vector3.MoveTowards(transform.position, _pos, Time.deltaTime * 100);
        if (_pos == transform.position) _click = false;
    }

    bool GroundCheck()
    {
        // Check if the player is standing on ground using a BoxCast
        return Physics.BoxCast(
            transform.position - Vector3.up * 0.5f,
            new Vector3(0.5f, 0.25f, 0.5f),
            Vector3.down,
            Quaternion.identity,
            0.5f,
            _groundMask
        );
    }

    void OnDrawGizmos()
    {
        // Visualize ground check gizmo in scene view
        Gizmos.color = GroundCheck() ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position + Vector3.down, new Vector3(1, 0.25f, 1));
    }

    void OnTriggerEnter(Collider other)
    {
        // Teleport player back to origin if they touch object named "Tel"
        if (other.name == "Tel")
        {
            transform.position = Vector3.up;
        }
        // Apply knockback force when colliding with damage objects
        else if (other.CompareTag("Damage"))
        {
            _rb.AddForce((-transform.forward + transform.up) * 2f, ForceMode.Impulse);
        }
    }

    void PerformRaycast()
    {
        // Perform raycast from camera toward angled forward direction
        Vector3 camPos = _camtransform.position;
        Quaternion rotation = Quaternion.AngleAxis(_camtransform.localRotation.x, -_camtransform.right);
        Vector3 direction = rotation * transform.forward;

        if (Physics.Raycast(camPos, direction, out RaycastHit hit, 10f))
        {
            // Ignore if hit a sphere collider (special case)
            if (hit.collider is SphereCollider) return;

            // If hit a bot, store its position and optionally trigger hit animation
            if (hit.collider.name == "Bot")
            {
                _pos = hit.collider.transform.position - transform.forward * 1.5f;
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    hit.collider.GetComponent<Project_1_Enemy>()._ani.SetTrigger("Hit");
                }
                Debug.DrawRay(camPos, direction * hit.distance, Color.green);
            }
            else if (!_click)
            {
                _pos = Vector3.zero; // Reset target position if not a bot
            }
        }
        else if (!_click)
        {
            _pos = Vector3.zero; // Reset if ray hits nothing
        }
    }
}
