using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IMovement Movement;
    Vector2 movementVector;
    // Start is called before the first frame update
    void Start()
    {
        Movement = GetComponent<IMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");
        Movement.SetMoveDirection(movementVector);
    }
}
