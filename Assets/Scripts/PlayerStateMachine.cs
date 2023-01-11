using UnityEngine;

// On d�clare les quatre �tats du personnage joueur
public enum PlayerState
{
    IDLE,
    WALKING,
    SPRINTING,
    ROLLING,
}


public class PlayerStateMachine : MonoBehaviour
{
    // On stocke l'�tat courant dans une variable globale
    private PlayerState _currentState;

    // Les composants
    private PlayerInput _playerInput;           
    private PlayerController _playerController;
    private Animator _animator;


    private void Awake()
    {
        // On mets en cache nos composants
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        // Update de l'�tat en cours
        OnStateUpdate(_currentState);
        _animator.SetFloat("Horizontal",_playerInput.Movement.x);
        _animator.SetFloat("Vertical",_playerInput.Movement.y);
    }
    private void FixedUpdate()
    {
        // FixedUpdate de l'�tat en cours
        OnStateFixedUpdate(_currentState);
    }
    //-----------------------------------------------------------------------------------ON STATE------------------------------------------------------------------
    // M�thode appel�e lorsque l'on entre dans un �tat
    private void OnStateEnter(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                OnEnterIdle();
                break;
            case PlayerState.WALKING:
                OnEnterWalking();
                break;
            case PlayerState.ROLLING:
                OnEnterRolling();
                break;
            case PlayerState.SPRINTING:
                OnEnterSprinting();
                break;
            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }
    // M�thode appel�e � chaque frame dans un �tat
    private void OnStateUpdate(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                OnUpdateIdle();
                break;
            case PlayerState.WALKING:
                OnUpdateWalking();
                break;
            case PlayerState.ROLLING:
                OnUpdateRolling();
                break;
            case PlayerState.SPRINTING:
                OnUpdateSprinting();
                break;
            default:
                Debug.LogError("OnStateUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    // M�thode appel�e � chaque "frame physique" dans un �tat
    private void OnStateFixedUpdate(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                OnFixedUpdateIdle();
                break;
            case PlayerState.WALKING:
                OnFixedUpdateWalking();
                break;
            case PlayerState.ROLLING:
                OnFixedUpdateRolling();
                break;
            case PlayerState.SPRINTING:
                OnFixedUpdateSprinting();
                break;
            default:
                Debug.LogError("OnStateFixedUpdate: Invalid state " + state.ToString());
                break;
        }
    }
    // M�thode appel�e lorsque l'on sort d'un �tat
    private void OnStateExit(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                OnExitIdle();
                break;
            case PlayerState.WALKING:
                OnExitWalking();
                break;
            case PlayerState.ROLLING:
                OnExitRolling();
                break;
            case PlayerState.SPRINTING:
                OnExitSprinting();
                break;
            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }
    // M�thode appel�e pour passer d'un �tat � un autre
    //-----------------------------------------------------------------------------------END ON STATE------------------------------------------------------------------


    //-----------------------------------------------------------------------------------TRANSITION------------------------------------------------------------------
    private void TransitionToState(PlayerState toState)
    {
        OnStateExit(_currentState);
        _currentState = toState;
        OnStateEnter(toState);
    }
    //-----------------------------------------------------------------------------------END TRANSITION------------------------------------------------------------------


    //-----------------------------------------------------------------------------------IDLE------------------------------------------------------------------
    // Ce qu'il se passe lorsque l'on entre dans l'�tat IDLE
    private void OnEnterIdle()
    {
        _animator.SetTrigger("isIdle");
    }
    // Ce qu'il se passe � chaque frame dans l'�tat IDLE
    private void OnUpdateIdle()
    {
        // Si il y a un mouvement
        if (_playerInput.HasMovement)
        {
            // Si la touche Roll est enfonc�e
            if (_playerInput.Roll)
            {
                // On passe en SPRINTING
                TransitionToState(PlayerState.SPRINTING);
            }
            // Sinon
            else
            {
                // On passe en RUNNING
                TransitionToState(PlayerState.WALKING);
            }
        }
        // Sinon si on appuie la touche Roll
        else if (_playerInput.RollDown)
        {
            TransitionToState(PlayerState.ROLLING);
        }
    }
    // Ce qu'il se passe � chaque "frame physique" dans l'�tat IDLE
    private void OnFixedUpdateIdle()
    {
        // On commande au joueur de rester immobile
        _playerController.DoIdle();
    }
    // Ce qu'il se passe lorsque l'on sort de l'�tat IDLE
    private void OnExitIdle()
    {
        
    }
    //-----------------------------------------------------------------------------------END IDLE------------------------------------------------------------------


    //-----------------------------------------------------------------------------------WALK------------------------------------------------------------------
    // Ce qu'il se passe lorsque l'on entre dans l'�tat WALKING
    private void OnEnterWalking()
    {
        _animator.SetTrigger("isWalking");
    }
    // Ce qu'il se passe � chaque frame dans l'�tat WALKING
    private void OnUpdateWalking()
    {
        // Si il n'y a pas de mouvement
        if (!_playerInput.HasMovement)
        {
            // On passe en IDLE
            TransitionToState(PlayerState.IDLE);
        }
        // Sinon si on appuie la touche Roll
        else if (_playerInput.RollDown)
        {
            // On passe en ROLLING
            TransitionToState(PlayerState.ROLLING);
        }
    }
    // Ce qu'il se passe � chaque "frame physique" dans l'�tat WALKING
    private void OnFixedUpdateWalking()
    {
        // On commande au joueur de marcher
        _playerController.DoWalk();
    }
    // Ce qu'il se passe lorsque l'on sort de l'�tat WALKING
    private void OnExitWalking()
    {
        
    }
    //-----------------------------------------------------------------------------------ENS WALK------------------------------------------------------------------



    //-----------------------------------------------------------------------------------SPRINT------------------------------------------------------------------
    // Ce qu'il se passe lorsque l'on entre dans l'�tat SPRINTING
    private void OnEnterSprinting()
    {
        _animator.SetTrigger("isSprinting");
    }
    // Ce qu'il se passe � chaque frame dans l'�tat SPRINTING
    private void OnUpdateSprinting()
    {
        // Si il n'y a pas de mouvement
        if (!_playerInput.HasMovement)
        {
            // On passe en IDLE
            TransitionToState(PlayerState.IDLE);
        }
        // Sinon (il y a mouvement) si la touche Roll n'est plus enfonc�e
        else if (!_playerInput.Roll)
        {
            // On passe en RUNNING
            TransitionToState(PlayerState.WALKING);
        }
    }
    // Ce qu'il se passe � chaque "frame physique" dans l'�tat SPRINTING
    private void OnFixedUpdateSprinting()
    {
        // On commande au joueur de courir
        _playerController.DoSprint();
    }
    // Ce qu'il se passe lorsque l'on sort de l'�tat SPRINTING
    private void OnExitSprinting()
    {
        
    }
    //-----------------------------------------------------------------------------------END SPRINT------------------------------------------------------------------


    //-----------------------------------------------------------------------------------ROLL------------------------------------------------------------------
    // Ce qu'il se passe lorsque l'on entre dans l'�tat ROLLING
    private void OnEnterRolling()
    {
        // On commande au joueur de d�marrer sa roulade
        _playerController.StartRoll();

        _animator.SetTrigger("IsRolling");
    }
    // Ce qu'il se passe � chaque frame dans l'�tat ROLLING
    private void OnUpdateRolling()
    {
        // Si la roulade est termin�e
        if (_playerController.IsRollEnded)
        {
            // Si il y a mouvement et que la touche ROLL est enfonc�e
            if (_playerInput.HasMovement && _playerInput.Roll)
            {
                // On passe en SPRINTING
                TransitionToState(PlayerState.SPRINTING);
            }
            // Si il y a mouvement et que la touche ROLL n'est pas enfonc�e
            else if (_playerInput.HasMovement && !_playerInput.Roll)
            {
                // On passe en RUNNING
                TransitionToState(PlayerState.WALKING);
            }
            // Si il n'y a pas de mouvement
            else if (!_playerInput.HasMovement)
            {
                // On passe en IDLE
                TransitionToState(PlayerState.IDLE);
            }
        }
    }
    // Ce qu'il se passe � chaque "frame physique" dans l'�tat ROLLING
    private void OnFixedUpdateRolling()
    {
        // On commande au joueur de continuer sa roulade
        _playerController.DoRoll();
    }
    // Ce qu'il se passe lorsque l'on sort de l'�tat ROLLING
    private void OnExitRolling()
    {
       
    }
    //-----------------------------------------------------------------------------------END ROLL------------------------------------------------------------------
    private void OnGUI()
    {
        // On affiche l'�tat en cours pour le debug
        GUIStyle style = new GUIStyle() { fontSize = 50, fontStyle = FontStyle.Bold };
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(50, 50, 100, 100), _currentState.ToString(), style);
    }
}
