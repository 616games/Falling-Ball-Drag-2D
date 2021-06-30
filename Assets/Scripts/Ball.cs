using UnityEngine;

public class Ball : MonoBehaviour
{
    #region --Fields / Properties--
    
    /// <summary>
    /// Mass of the ball.
    /// </summary>
    [SerializeField]
    [Range(5.0f, 100.0f)]
    private float _mass;

    /// <summary>
    /// Gravitational constant used to define gravitational force.
    /// </summary>
    [SerializeField]
    private float _gravitationalConstant = .0001f;

    /// <summary>
    /// Coefficient of drag for water.
    /// </summary>
    [SerializeField]
    private float _waterDragCoefficient = 15f;
    
    /// <summary>
    /// Amount of force the ball experiences upon impact with the water.
    /// </summary>
    [SerializeField]
    private Vector3 _impactForce = new Vector3(0, .08f, 0);
    
    /// <summary>
    /// Speed and direction of the ball.
    /// </summary>
    private Vector3 _velocity;

    /// <summary>
    /// How fast the velocity of the ball is changing.
    /// </summary>
    private Vector3 _acceleration;
    
    /// <summary>
    /// Cached Transform component.
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Gravitational force.
    /// </summary>
    private Vector3 _gravity;

    /// <summary>
    /// Controls how long the ball experiences the impact force to simulate initially hitting the water.
    /// </summary>
    private float _impactTime = 1f;
    
    /// <summary>
    /// Used to determine when the impact force should no longer be applied.
    /// </summary>
    private float _impactTimer = 2f;
    
    #endregion
    
    #region --Unity Specific Methods--
    
    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        Fall();
    }
    
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.CompareTag("Water"))
        {
            ApplyForce(_impactForce * _mass);
        }
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_other.gameObject.CompareTag("Water"))
        {
            ApplyForce(CalculateDrag(_waterDragCoefficient));
        }
    }
    
    #endregion
    
    #region --Custom Methods--

    /// <summary>
    /// Initializes variables and caches components.
    /// </summary>
    private void Init()
    {
        _transform = transform;
        _gravity = new Vector3(0, -_gravitationalConstant * _mass, 0);
    }
    
    /// <summary>
    /// Handles ball movement.
    /// </summary>
    private void Fall()
    {
        if (_impactTimer < _impactTime)
        {
            _impactTimer += Time.deltaTime;
        }
        
        ApplyForce(_gravity);
        _velocity += _acceleration;
        _transform.position += _velocity;
        _acceleration = Vector3.zero;
    }

    /// <summary>
    /// Calculates the drag force.
    /// </summary>
    private Vector3 CalculateDrag(float _coefficientOfDrag)
    {
        float _speedSquared = _velocity.sqrMagnitude;
        float _dragMagnitude = _coefficientOfDrag * _speedSquared;
        Vector3 _direction = -1f * _velocity.normalized;
        Vector3 _drag = _direction * _dragMagnitude;

        return _drag;
    }

    /// <summary>
    /// Applies any force provided to the acceleration of the ball.
    /// </summary>
    /// <param name="_force"></param>
    private void ApplyForce(Vector3 _force)
    {
        if (_mass <= 0.0f)
        {
            _mass = 1f;
        }
        
        _acceleration += _force / _mass;
    }

    #endregion
    
}
