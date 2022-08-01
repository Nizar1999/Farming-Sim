using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Stats
    [SerializeField] float speed;

    //Movement State
    Vector3 movement;
    public float horizontalDirection;
    public float verticalDirection;
    public float lastHDir;
    public float lastVDir;

    //Components
    private Rigidbody2D rb;
    private Animator ar;
    private PlayerInventory pi;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ar = GetComponent<Animator>();
        pi = GetComponent<PlayerInventory>();
    }

    private void FixedUpdate()
    {
        if(movement.x != horizontalDirection | movement.y != verticalDirection | rb.velocity.x != 0 | rb.velocity.y != 0)
        {
            pi.updateFacingTile(movement.x, movement.y);
        }
        movement.x = horizontalDirection;
        movement.y = verticalDirection;
        rb.velocity = movement * Time.deltaTime * speed;
        
    }

    //Movement Functions
    public void movePlayer(InputAction.CallbackContext context)
    {
        horizontalDirection = context.action.ReadValue<Vector2>().x;
        verticalDirection = context.action.ReadValue<Vector2>().y;
        ar.SetBool("Moving", horizontalDirection != 0 || verticalDirection != 0);
        faceCorrectDirection();
    }


    //Utility
    void faceCorrectDirection()
    {
        //do something
        if (horizontalDirection > 0)
        {
            Debug.Log("Moving Right");
            lastHDir = 1;
            lastVDir = 0;
        }
        if (horizontalDirection < 0)
        {
            Debug.Log("Moving Left");
            lastHDir = -1;
            lastVDir = 0;
        }
        if (verticalDirection < 0)
        {
            Debug.Log("Moving Down");
            lastVDir = -1;
            lastHDir = 0;
        }
        if (verticalDirection > 0)
        {
            Debug.Log("Moving Up");
            lastVDir = 1;
            lastHDir = 0;
        }

        ar.SetFloat("VerticalDir", verticalDirection);
        ar.SetFloat("HorizontalDir", horizontalDirection);
        ar.SetFloat("LastVDir", lastVDir);
        ar.SetFloat("LastHDir", lastHDir);
    }
}
