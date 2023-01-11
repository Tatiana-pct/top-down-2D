using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Axe deplacement
    private Vector2 _movement;

    // Bouton roulade
    private bool _roll;
    private bool _rollDown;

    public Vector2 Movement { get => _movement; }
    public Vector2 NormalizedMovement { get => _movement.normalized; }
    public Vector2 ClampedMovement { get => Vector2.ClampMagnitude(_movement, 1f); }
    public bool HasMovement { get => _movement != Vector2.zero; }
    public bool Roll { get => _roll; }
    public bool RollDown { get => _rollDown; }

    private void Update()
    {
        // On stocke les valeurs brute, normalisée et clampée de l'axe de déplacement
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        

        // On stocke la valeur de l'input Roll
        _roll = Input.GetButton("Roll");
        // On stocke la valeur 'down' de l'input Roll
        _rollDown = Input.GetButtonDown("Roll");
    }
}
