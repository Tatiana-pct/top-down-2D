using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // On d�clare nos variables serialis�es
    [SerializeField] private float _regularSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;
    [SerializeField] private float _rollSpeed = 10f;
    [SerializeField] private float _rollDuration = 1f;
    [SerializeField] private AnimationCurve _rollEasing;

    // On d�clare nos composants
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;

    // On d�clare nos variables priv�es
    private Vector2 _lastDirection;     // Derni�re direction emprunt�e par le joueur
    private float _rollEndTime;         // Temps de fin de roulade

    // Propri�t� qui retourne si la roulade est termin�e ou non
    public bool IsRollEnded { get { return Time.time > _rollEndTime; } }

    private void Awake()
    {
        // Mise en cache des composants
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void DoIdle()
    {
        // On d�finit la velocit� du mouvement � z�ro
        Vector2 veloc = Vector2.zero;
        // On applique le mouvement
        ApplyMovement(veloc);
    }
    public void DoWalk()
    {
        // On calcule la velocit� du mouvement
        Vector2 veloc = _input.ClampedMovement * _regularSpeed;
        // On applique le mouvement
        ApplyMovement(veloc);
    }
    public void DoSprint()
    {
        // On calcule la velocit� du mouvement
        Vector2 veloc = _input.NormalizedMovement * _sprintSpeed;
        // On applique le mouvement
        ApplyMovement(veloc);
    }
    public void StartRoll()
    {
        // On d�finit l'heure de fin de roulade
        _rollEndTime = Time.time + _rollDuration;
    }
    public void DoRoll()
    {
        // On calcule la velocit� du mouvement
        Vector2 veloc = _lastDirection * _rollSpeed;
        // On calcule l'acceleration de la roulade selon sa progression
        float acceleration = _rollEasing.Evaluate(GetRollProgress());
        // On applique le mouvement
        ApplyMovement(veloc * acceleration);
    }

    private void ApplyMovement(Vector2 velocity)
    {
        // On applique la velocit�
        _rigidbody.velocity = velocity;

        // Si il y a un mouvement
        if (velocity != Vector2.zero)
        {
            // On stocke la direction de celui-ci (pour une eventuelle roulade qui utilisera la derni�re direction emprunt�e)
            _lastDirection = velocity.normalized;
        }
    }
    private float GetRollProgress()
    {
        // On calcule la dur�e actuelle de la roulade
        float rollTime = Time.time - (_rollEndTime - _rollDuration);
        // On calcule la progression de la roulade en divisant la dur�e actuelle par la dur�e totale
        float rollProgress = rollTime / _rollDuration;
        // On retourne la progression
        return rollProgress;
    }
}
