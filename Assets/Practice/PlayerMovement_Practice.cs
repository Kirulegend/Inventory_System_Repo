using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement_Practice : MonoBehaviour
{
    public Rigidbody2D _RB;
    public float _moveSpeed;
    public float _jumpForce;
    public LayerMask _layerMask;
    public Camera _camera;
    public Vector2 _camMin;
    public Vector2 _camMax;
    public Vector2 _playerClamp;
    public Vector2 _touchStartPos;
    public Vector2 _magnitude;
    public bool _jumpTime;
    public float _jumpTimer;
    public float _tempJumpTimer;
    public float _Hor;
    Touch _touch;
    void Start()
    {
        _camera = Camera.main;
        _RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        PlayerMovementKB();
#endif

#if UNITY_ANDROID
        PlayerMovementTC();
#endif
        CamClamp();
        PlayerClamp();
    }

    void PlayerMovementKB()
    {
        //Move
        _Hor = Input.GetAxisRaw("Horizontal");
        if( _Hor != 0)
        {
            _RB.linearVelocity = new Vector2(_Hor * _moveSpeed, _RB.linearVelocity.y);
        }
        else
        {
            _RB.linearVelocity = new Vector2(0, _RB.linearVelocity.y);
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            _RB.linearVelocity = new Vector2(_RB.linearVelocityX, _jumpForce);
        }
    }

    void PlayerMovementTC()
    {
        if (Input.touchCount == 1)
        {
            _touch = Input.GetTouch(0);

            switch (_touch.phase)
            {
                //Jump
                case TouchPhase.Began:
                    _touchStartPos = _camera.ScreenToWorldPoint(_touch.position);
                    if (GroundCheck())
                    {
                        _RB.linearVelocity = new Vector2(_RB.linearVelocityX, _jumpForce);
                    }
                    break;
                //Move
                case TouchPhase.Moved:
                    _magnitude = _camera.ScreenToWorldPoint(_touch.position) - new Vector3(_touchStartPos.x, _touchStartPos.y, 0);
                    if (_magnitude.x != 0)
                    {
                        _RB.linearVelocity = new Vector2(_magnitude.x * _moveSpeed / 5, _RB.linearVelocity.y);
                    }
                    else
                    {
                        _RB.linearVelocity = new Vector2(0, _RB.linearVelocity.y);
                    }
                    break;
            }
        }
    }

    void CamClamp()
    {
        Vector3 Pos = transform.position;
        Pos.x = Mathf.Clamp(Pos.x, _camMin.x, _camMax.x);
        Pos.y = Mathf.Clamp(Pos.y, _camMin.y, _camMax.y);
        _camera.transform.position = new Vector3(Pos.x, Pos.y, _camera.transform.position.z);
    }

    void PlayerClamp()
    {
        Vector3 Pos = transform.position;
        Pos.x = Mathf.Clamp(Pos.x, _playerClamp.x, _playerClamp.y);
        transform.position = Pos;
    }
    bool GroundCheck()
    {
        return Physics2D.BoxCast(transform.position, new Vector3(1, .25f, 0), 0, Vector3.down, .5f, _layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck() ? Color.green : Color.red;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + -.5f, transform.position.z), new Vector3(1, .25f, 0));
    }
}
