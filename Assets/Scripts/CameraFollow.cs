using UnityEngine;

// Les différents type de déplacement de la camera
public enum FollowType
{
    SNAP,
    LERP,
    SMOOTHDAMP
}

// Les différentes méthodes UPDATE dans lequel appliquer le déplacement
public enum UpdateType
{
    UPDATE,
    FIXED_UPDATE,
    LATE_UPDATE
}

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    // Paramètres communs
    [SerializeField] private FollowType _followType = FollowType.SNAP;
    [SerializeField] private UpdateType _updateType = UpdateType.UPDATE;
    [SerializeField] private Transform _target;

    // Paramètre pour le LERP et le SMOOTHDAMP
    [SerializeField] private float _minDistance = 0.1f;

    // Paramètres pour le LERP
    [SerializeField] private float _speed = 2f;

    // Paramètres pour le SMOOTHDAMP
    [SerializeField] private float _smoothTime = 0.5f;
    [SerializeField] private float _maxSpeed = 10f;

    // Transform de la camera
    private Transform _transform;
    // Vecteur necessaire pour l'utilisation de SMOOTHDAMP
    private Vector2 _smoothDampVelocity;


    private void Awake()
    {
        _transform = transform;
    }
    private void Update()
    {
        if (_updateType == UpdateType.UPDATE)
        {
            DoUpdate();
        }
    }
    private void FixedUpdate()
    {
        if (_updateType == UpdateType.FIXED_UPDATE)
        {
            DoUpdate();
        }
    }
    private void LateUpdate()
    {
        if (_updateType == UpdateType.LATE_UPDATE)
        {
            DoUpdate();
        }
    }

    private void DoUpdate()
    {
        switch (_followType)
        {
            case FollowType.SNAP:
                Snap();
                break;
            case FollowType.LERP:
                Lerp();
                break;
            case FollowType.SMOOTHDAMP:
                SmoothDamp();
                break;
            default:
                break;
        }
    }

    private void Snap()
    {
        Vector3 newPosition = _target.position;
        newPosition.z = _transform.position.z;

        _transform.position = newPosition;
    }

    private void Lerp()
    {
        if (Vector2.Distance(_transform.position, _target.position) < _minDistance)
        {
            return;
        }

        Vector3 newPosition = Vector2.Lerp(_transform.position, _target.position, _speed * Time.deltaTime);
        newPosition.z = _transform.position.z;
        _transform.position = newPosition;
    }

    private void SmoothDamp()
    {
        if (Vector2.Distance(_transform.position, _target.position) < _minDistance)
        {
            return;
        }

        Vector3 newPosition = Vector2.SmoothDamp(_transform.position, _target.position, ref _smoothDampVelocity, _smoothTime, _maxSpeed);
        newPosition.z = _transform.position.z;
        _transform.position = newPosition;
    }
}