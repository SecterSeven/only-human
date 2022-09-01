using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _muzzleFlashAnimator;
   
    public float walkSpeed = 15f;
    public float runSpeed = 20f;
    public float jumpHeight = 5f;

    public static int health = 100;
    public Slider HealthBar;
    public GameOverScreen GameOverScreen;

    public Transform groundCheckTransform;
    public float groundCheckRadius = 0.2f;

    public Transform targetTransform;
    public LayerMask mouseAimMask;
    public LayerMask groundMask;

    public GameObject bulletPrefab;
    public Transform muzzleTransform;

    public AnimationCurve recoilCurve;
    public float recoilDuration = 0.25f;
    public float recoilMaxRotation = 45f;
    public Transform rightLowerArm;
    public Transform rightHand;
    public Transform leftLowerArm;
    public Transform leftHand;

    private float inputMovement;
    private Animator animator;
    private Rigidbody rbody;
    private bool isGrounded;
    private Camera mainCamera;
    private float recoilTimer;
    private AudioSource audioShoot;


    private int FacingSign
    {
        get
        {
            Vector3 perp = Vector3.Cross(transform.forward, Vector3.forward);
            float dir = Vector3.Dot(perp, transform.up);
            return dir > 0f ? -1 : dir < 0f ? 0 : 1;
        }
    }

    void Start()
    {
        health = 100;
        animator = this.transform.GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        HealthBar.value = health;
        audioShoot = GetComponent<AudioSource>();
        mainCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.value = health;

        PlayerTakeDamage();
        Shoot();


        inputMovement = Input.GetAxis("Horizontal");

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTransform.position = hit.point;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rbody.velocity = new Vector3(rbody.velocity.x, 0, 0);
            rbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -1 * Physics.gravity.y), ForceMode.VelocityChange);
            animator.SetBool("jump",true);
        }else
        {
            animator.SetBool("jump",false);
        }

        bool shiftHold = Input.GetKey(KeyCode.LeftShift);
        
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            animator.SetBool("isRun",true);
            walkSpeed = 25f;
        }else
        {
            animator.SetBool("isRun",false);
            walkSpeed = 15f;
        }


    }

    public void PlayerTakeDamage()
    {
        if (health >= 0)
        {
            
            
        }else if (health <= 0)
        {
            health = 0;
            //rbody = Rigidbody.Static;
            animator.SetTrigger("Death");
            this.enabled = false;
            GameOverScreen.Show();
        }
    }


    private void Shoot()
    {    if (Input.GetButtonDown("Fire1"))
        {
            //Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

            Fire();

            audioShoot.PlayOneShot(audioShoot.clip);

            _muzzleFlashAnimator.SetTrigger("Shoot");

            UtilsClass.ShakeCamera(.3f, .05f);

        }
    }

    
    private void Fire()
    {
        recoilTimer = Time.time;
    
        var go = Instantiate(bulletPrefab);
        go.transform.position = muzzleTransform.position;
        
        var bullet = go.GetComponent<Bullet>();
        bullet.Fire(go.transform.position, muzzleTransform.eulerAngles, gameObject.layer);

        
        
        //CinemachineShake.Instance.ShakeCamera(5f, .1f);
    }

    private void LateUpdate()
    {
        // Recoil Animation
        if (recoilTimer < 0)
        {
            return;
        }

        float curveTime = (Time.time - recoilTimer) / recoilDuration;
        if (curveTime > 1f)
        {
            recoilTimer = -1;
        }
        else
        {
            rightLowerArm.Rotate(Vector3.forward, recoilCurve.Evaluate(curveTime) * recoilMaxRotation, Space.Self);
            leftLowerArm.Rotate(Vector3.forward, recoilCurve.Evaluate(curveTime) * recoilMaxRotation, Space.Self);
        }


    }
    private void FixedUpdate()
    {
        // Movement
        rbody.velocity = new Vector3(inputMovement * walkSpeed, rbody.velocity.y, 0);
        animator.SetFloat("speed", (FacingSign * rbody.velocity.x) / walkSpeed);

        //Run
        //rbody.velocity = new Vector3(inputMovement * runSpeed, rbody.velocity.y, 0);
        //animator.SetFloat("runSpeed", (FacingSign * rbody.velocity.x) / runSpeed);
        

        // Facing Rotation
        rbody.MoveRotation(Quaternion.Euler(new Vector3(0, (90 * Mathf.Sign(targetTransform.position.x - transform.position.x)) < 0 ? 180 : 0, 0)));
        
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        animator.SetBool("isGrounded", isGrounded);

    }
    private void OnAnimatorIK()
    {
        // Weapon Aim at Target IK
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, targetTransform.position);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, targetTransform.position);

        // Look at target IK
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(targetTransform.position);
    }
}
