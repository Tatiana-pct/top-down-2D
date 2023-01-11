using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // On déclare nos variables serialisées
    [SerializeField] private float _regularSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;
    [SerializeField] private float _rollSpeed = 10f;
    [SerializeField] private float _rollDuration = 1f;
    [SerializeField] private AnimationCurve _rollEasing;

    // On déclare nos composants
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;

    // On déclare nos variables privées
    private Vector2 _lastDirection;     // Dernière direction empruntée par le joueur
    private float _rollEndTime;         // Temps de fin de roulade

    // Propriété qui retourne si la roulade est terminée ou non
    public bool IsRollEnded { get { return Time.time > _rollEndTime; } }

    private void Awake()
    {
        // Mise en cache des composants
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void DoIdle()
    {
        // On définit la velocité du mouvement à zéro
        Vector2 veloc = Vector2.zero;
        // On applique le mouvement
        ApplyMovement(veloc);
    }
    public void DoWalk()
    {
        // On calcule la velocité du mouvement
        Vector2 veloc = _input.ClampedMovement * _regularSpeed;
        // On applique le mouvement
        ApplyMovement(veloc);
    }
    public void DoSprint()
    {
        // On calcule la velocité du mouvement
        Vector2 veloc = _input.NormalizedMovement * _sprintSpeed;
        // On applique le mouvement
        ApplyMovement(veloc);
    }
    public void StartRoll()
    {
        // On définit l'heure de fin de roulade
        _rollEndTime = Time.time + _rollDuration;
    }
    public void DoRoll()
    {
        // On calcule la velocité du mouvement
        Vector2 veloc = _lastDirection * _rollSpeed;
        // On calcule l'acceleration de la roulade selon sa progression
        float acceleration = _rollEasing.Evaluate(GetRollProgress());
        // On applique le mouvement
        ApplyMovement(veloc * acceleration);
    }

    private void ApplyMovement(Vector2 velocity)
    {
        // On applique la velocité
        _rigidbody.velocity = velocity;

        // Si il y a un mouvement
        if (velocity != Vector2.zero)
        {
            // On stocke la direction de celui-ci (pour une eventuelle roulade qui utilisera la dernière direction empruntée)
            _lastDirection = velocity.normalized;
        }
    }
    private float GetRollProgress()
    {
        // On calcule la durée actuelle de la roulade
        float rollTime = Time.time - (_rollEndTime - _rollDuration);
        // On calcule la progression de la roulade en divisant la durée actuelle par la durée totale
        float rollProgress = rollTime / _rollDuration;
        // On retourne la progression
        return rollProgress;
    }
}
