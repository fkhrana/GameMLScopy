using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 input;
    public Animator animator;

    private Vector2 lastMoveDir = Vector2.down;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            // Buat kamera baru khusus player lokal
            GameObject camObj = new GameObject("PlayerCamera");
            Camera cam = camObj.AddComponent<Camera>();
            camObj.tag = "MainCamera"; // biar bisa diakses sebagai Camera.main
            camObj.AddComponent<AudioListener>(); // penting, biasanya ada di main camera
            CameraFollow camFollow = camObj.AddComponent<CameraFollow>();

            camFollow.target = transform;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.W)) lastMoveDir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S)) lastMoveDir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A)) lastMoveDir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D)) lastMoveDir = Vector2.right;

        PlayMovementAnimation(input);
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        rb.MovePosition(rb.position + input.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void PlayMovementAnimation(Vector2 direction)
    {
        bool isMoving = direction != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }
        else
        {

            animator.SetFloat("moveX", lastMoveDir.x);
            animator.SetFloat("moveY", lastMoveDir.y);
        }
    }
}
