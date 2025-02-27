using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rope rope;
    
    private static Vector2 moveDirection = Vector2.up;
    private Rigidbody2D rb;
    private PlayerAnimation playerAnimation;
    
    private StickyBall currentStickyBall;

    private bool isAttached = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (!isAttached)
        {
            if (rb.velocity.magnitude < moveSpeed) rb.AddForce(moveDirection * moveSpeed, ForceMode2D.Force);
        }
        else
        {
            Vector2 direction = (currentStickyBall.GetPosition() - transform.position).normalized;
            rb.velocity = Vector2.ClampMagnitude(direction * moveSpeed, moveSpeed);
            rope.UpdateRope(transform.position, currentStickyBall.GetPosition());
        }
    }

    public void HandleStickyInteraction(StickyBall stickyBall)
    {
        if (currentStickyBall == stickyBall)
        {
            rope.Detach();
            currentStickyBall = null;
            isAttached = false;
        }
        else
        {
            SoundManager.Instance.PlaySoundEffect(0);
            if (currentStickyBall != null)
            {
                rope.Detach();
            }

            currentStickyBall = stickyBall;
            rope.Attach(transform.position, currentStickyBall.GetPosition());
            isAttached = true;
            playerAnimation.AttachRope(currentStickyBall.GetPosition());
        }
    }
}