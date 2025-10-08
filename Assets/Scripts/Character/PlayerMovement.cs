using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterStateMachine machine;

    void Start()
    {
        machine.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        machine.UpdateState();
    }
}
