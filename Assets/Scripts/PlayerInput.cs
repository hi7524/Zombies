using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string verticalAxis = "Vertical";
    public static readonly string horizontalAxis = "Horizontal";
    public static readonly string fireButton = "Fire1";
    public static readonly string reloadButton = "Reload";

    public float Move { get; private set; }
    public float Roatate { get; private set; }
    public bool Fire { get; private set; }
    public bool Reload { get; private set; }

    private void Update()
    {
        Move = Input.GetAxis(verticalAxis);
        Roatate = Input.GetAxis(horizontalAxis);

        Fire = Input.GetButton(fireButton);
        Reload = Input.GetButtonDown(reloadButton);
    }
}