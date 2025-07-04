using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PLAYER_INPUTS : MonoBehaviour
{
    public IAS_Player IAS;
    public InputActionAsset IAA;
    public InputAction IA_Move;
    public InputAction IA_Fetch;
    public InputAction IA_Interact_Bark;
    public InputAction IA_Sprint;
    public InputAction IA_Pause;
    public InputAction IA_Jump;
    public InputAction IA_Cam;
    public Vector3 InputDirection;
    public Vector3 CamDirection;
    // Start is called before the first frame update
    void Start()
    {
        IAS = new IAS_Player();
        IA_Move = IAS.Movement.Move_Axis;
        IA_Jump = IAS.Movement.Jump;
        IA_Cam = IAS.Movement.Camera_Axis;
        IA_Fetch = IAS.Movement.Fetch;
        IA_Interact_Bark = IAS.Movement.InteractBark;
        IA_Sprint = IAS.Movement.Sprint;
        IA_Pause = IAS.Movement.Pause;
        IA_Pause.Enable();
        IA_Move.Enable();
        IA_Jump.Enable();
        IA_Cam.Enable();
        IA_Fetch.Enable();
        IA_Interact_Bark.Enable();
        IA_Sprint.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        SetInputDir();
    }
    
    void SetInputDir()
    {
        Vector3 temp = IA_Move.ReadValue<Vector2>();
        InputDirection = new Vector3(temp.x, 0, temp.y);
        Vector3 temp2 = IA_Cam.ReadValue<Vector2>();
        CamDirection = new Vector3(-temp2.y, temp2.x, 0);
    }
}
