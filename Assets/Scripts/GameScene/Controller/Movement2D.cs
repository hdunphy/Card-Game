using UnityEngine;

public interface IMovement
{
    public void SetMoveDirection(Vector2 _dir);
    public void SetCanMove(bool _canMove);
}

public class Movement2D : MonoBehaviour, IMovement
{
    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private float MoveSpeed;

    private Vector2 moveDirection;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            Rb.velocity = (moveDirection * MoveSpeed * Time.deltaTime);
        }
        else
        {
            Rb.velocity = Vector2.zero;
        }
    }

    public void SetCanMove(bool _canMove)
    {
        canMove = _canMove;
    }

    public void SetMoveDirection(Vector2 _dir)
    {
        moveDirection = _dir;
    }
}