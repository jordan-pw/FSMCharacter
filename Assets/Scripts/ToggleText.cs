using UnityEngine;
using TMPro;

public class ToggleText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display;

    [SerializeField]
    GameObject player;

    PlayerController pc;

    bool sprintToggleStatus;
    bool crouchToggleStatus;

    float stamina;

    private void Awake()
    {
         pc = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        sprintToggleStatus = pc.toggleSprint;
        crouchToggleStatus = pc.toggleCrouch;
        stamina = pc.PlayerStamina.stamina;

        display.SetText($"" +
            $"SprintToggle\n{sprintToggleStatus}" +
            $"\nCrouchToggle\n{crouchToggleStatus}" +
            $"\nStaminia\n{stamina}");
    }
}