using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundObjects;
    public Transform groundCheck;
    bool facingRight = true;
    bool isJumping = false;
    bool isGrounded = false;
    bool isAttacking = false;
    float moveDirection;
    float moveSpeed = 8;
    float jumpForce = 700;
    float checkRadius = 0.2f;
    Rigidbody2D rb;
    Animator animator;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        ProcessInput();
        UpdateTriggers();
        Animate();
    }
    void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);
        Move();
        Jump();
        Attack();
    }
    void Attack(){
        if(isAttacking){
            animator.SetTrigger("attack");
            print(isAttacking);
            isAttacking = false;
        }
    }
    void Jump(){
        if(isJumping && isGrounded){
            rb.AddForce(new Vector2(0f, jumpForce));
            animator.SetTrigger("jump");
            isJumping = false;
        }
    }
    void Move(){
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }
    void Animate(){
        if(moveDirection > 0 && !facingRight){
            FlipCharacter();
        }else if(moveDirection < 0 && facingRight){
            FlipCharacter();
        }
    }
    
    void FlipCharacter(){
        facingRight = !facingRight;
        transform.Rotate(0f,180f,0f);
    }
    void ProcessInput(){
        moveDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(1)){
            isJumping = true;
        }

        if(Input.GetMouseButtonDown(0)){
            isAttacking = true;
            Attack();
        }
    }

    void UpdateTriggers(){
        if(moveDirection != 0 && isGrounded && !isAttacking){
            animator.SetTrigger("run");
        }else if(!isGrounded){
            animator.SetTrigger("jump");
        }
        else{
            animator.SetTrigger("idle");
        }
    }

}
