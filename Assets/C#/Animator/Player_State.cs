using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State : MonoBehaviour {
    private Player_Animator player_Animator;

    public enum PlatfromDirection { x,y,z};
    public Transform Role;
    public float RoleGroundedDistance;
    public float RoleGroundedCheckOffset;
    public Transform LFrontPlatfrom;
    public Transform LBackPlatfrom;
    public Transform RFrontPlatfrom;
    public Transform RBackPlatfrom;
    public PlatfromDirection platfromDirection;
    public bool PlatfromDirectionMirror;
    public float groundedDistance;
    public float groundedCheckOffset;
    private LayerMask groundLayer=1<<10;
    private bool grounded;

    private CharacterController controller;
    private float speed=0;
    private float speedPara = 3f;
    private int speedDir = 1;
    private float angleSpeed=0;
    private bool isGround=true;
    private float groundDistance;
    private Vector3 motion=Vector3.zero;

    public Transform camera_transform;
    // Use this for initialization
    private void Awake()
    {
        controller = this.GetComponent<CharacterController>();
        player_Animator = this.GetComponent<Player_Animator>();
        grounded = IsGrounded();
    }
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        isGround = controller.isGrounded;
        grounded = IsGrounded();
        if (isGround)
        {
            if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.S))
            {
                speed = Mathf.Lerp(speed, 1, 0.2f);
                speed = Mathf.Abs(speed - 1) < 0.1 ? 1 : speed;
                if (Input.GetKey(KeyCode.S))
                {
                    speedDir = -1;
                }
                else
                {
                    speedDir = 1;
                }
                //controller.SimpleMove( this.GetComponent<Transform>().forward * 5f * speed);
            
                //controller.Move();
            }
            else
            {
                speed = Mathf.Lerp(speed, 0, 0.2f);
                speed = Mathf.Abs(speed) < 0.1 ? 0 : speed;
            }
            angleSpeed= Input.GetAxis("Horizontal");
            //Role.Rotate(Role.up*angleSpeed*5);
       
            if (player_Animator.IsJump())
            {
                player_Animator.JumpEnd();
            }
            Vector3 VerticalDirection = camera_transform.forward;
            VerticalDirection.y = 0;
            VerticalDirection = Vector3.Normalize(VerticalDirection);
            Vector3 HorizontalDirection = Quaternion.Euler(0, 90, 0)*VerticalDirection;
            float slopePara = 0;
            float forwardSlope = GetForwardSlope();
            if(forwardSlope!=float.NaN && Mathf.Abs(forwardSlope) <= 45)
            {
                slopePara = forwardSlope / 45*2;
            }
            motion = (VerticalDirection * speed*speedDir + HorizontalDirection * angleSpeed).normalized*(speedPara-slopePara);

            //Debug.Log(motion.normalized);
            Vector3 dir = Vector3.Slerp(Role.forward,motion.normalized,8f * Time.deltaTime);
            Role.LookAt(Role.position+ dir);
            //Role.Rotate (Quaternion.FromToRotation(Role.forward, motion.normalized)*Role.up*angleSpeed*10);
            //Role.Rotate(Quaternion.Euler(Vector3.Slerp(Role.forward, motion.normalized, Time.deltaTime))*Vector3.up);
            //motion =new Vector3(angleSpeed*3f, 0,speed*3 );
            //motion = Role.TransformDirection(motion);
            if (Input.GetButtonDown("Jump"))
            {
                player_Animator.Jump();
                motion.y = 10;
            }
        }
        else if (!grounded && !player_Animator.IsJump())
        {
            player_Animator.Jump();
        }
        motion.y -= 30f * Time.deltaTime;
        controller.Move(motion * Time.deltaTime);
        
    }
    private void LateUpdate()
    {
        
        //Debug.Log("坡度:"+GetForwardSlope());
    }
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;       
        GUI.skin.label.normal.textColor = Color.black;
        GUI.Label(new Rect(Screen.width * 0.2f, Screen.height * 0.2f, 100, 30), "坡度:" + GetForwardSlope(),style);
    }
    private float GetForwardSlope()
    {
        Ray rayOrigin = new Ray(Role.position + Vector3.up * 0.5f, Vector3.down * 1f);
        Ray rayForward = new Ray(Role.position + Vector3.up * 1f + Role.forward, Vector3.down * 2f);
        Debug.DrawRay(Role.position + Vector3.up * 0.5f, Vector3.down * 1f, Color.black);
        Debug.DrawRay(Role.position + Vector3.up * 1f + Role.forward, Vector3.down * 2f, Color.black);
        RaycastHit hitForward;
        RaycastHit hitOrigin;
        if (Physics.Raycast(Role.position + Vector3.up * 1f + Role.forward, Vector3.down, out hitForward, 2f, groundLayer) &&
            Physics.Raycast(Role.position + Vector3.up * 0.5f, Vector3.down, out hitOrigin, 1f, groundLayer))
        {
            float angle;
            Vector3 axis = Vector3.zero;
            Quaternion.FromToRotation(Role.forward, (hitForward.point - hitOrigin.point).normalized).ToAngleAxis(out angle,out axis);
            if (angle > 180) angle = 360 - angle;
            if (hitForward.point.y < hitOrigin.point.y)
            {
                angle = Mathf.Ceil(-angle);
            }
            else
            {
                angle = Mathf.Floor(angle);
            }
            return angle;
        }
        return float.NaN;
    }
    private void FixedUpdate()
    {

        //grounded = IsGrounded();
        //Debug.Log(grounded);
    }
    public float GetSpeed()
    {
        return speed;
    }
    public float GetAngleSpeed()
    {
        return angleSpeed;
    }
    public bool IsGround()
    {
        return isGround;
    }
    private bool IsGrounded()
    {
        /*
        int DirectionMirror = PlatfromDirectionMirror ? 1 : -1;
        Vector3 LFrontPlatfrom_Up, LBackPlatfrom_Up, RFrontPlatfrom_Up, RBackPlatfrom_Up;
        switch (platfromDirection)
        {
            case PlatfromDirection.x:
                LFrontPlatfrom_Up = LFrontPlatfrom.right;
                LBackPlatfrom_Up = LBackPlatfrom.right;
                RFrontPlatfrom_Up = RFrontPlatfrom.right;
                RBackPlatfrom_Up = RBackPlatfrom.right;
                break;
            case PlatfromDirection.y:
                LFrontPlatfrom_Up = LFrontPlatfrom.up;
                LBackPlatfrom_Up = LBackPlatfrom.up;
                RFrontPlatfrom_Up = RFrontPlatfrom.up;
                RBackPlatfrom_Up = RBackPlatfrom.up;
                break;
            case PlatfromDirection.z:
                LFrontPlatfrom_Up = LFrontPlatfrom.forward;
                LBackPlatfrom_Up = LBackPlatfrom.forward;
                RFrontPlatfrom_Up = RFrontPlatfrom.forward;
                RBackPlatfrom_Up = RBackPlatfrom.forward;
                break;
            default:
                LFrontPlatfrom_Up = Vector3.zero;
                LBackPlatfrom_Up = Vector3.zero;
                RFrontPlatfrom_Up = Vector3.zero;
                RBackPlatfrom_Up = Vector3.zero;
                break;
        }
        bool LGrounded = Physics.Raycast(LFrontPlatfrom.position+ LFrontPlatfrom_Up* DirectionMirror* groundedCheckOffset, LFrontPlatfrom_Up * DirectionMirror, groundedDistance, groundLayer) &&
                         Physics.Raycast(LBackPlatfrom.position + LBackPlatfrom_Up * DirectionMirror * groundedCheckOffset, LBackPlatfrom_Up * DirectionMirror, groundedDistance, groundLayer);
        bool RGrounded = Physics.Raycast(RFrontPlatfrom.position + RFrontPlatfrom_Up * DirectionMirror * groundedCheckOffset, RFrontPlatfrom_Up * DirectionMirror, groundedDistance, groundLayer) &&
                         Physics.Raycast(RBackPlatfrom.position + RBackPlatfrom_Up * DirectionMirror * groundedCheckOffset, RBackPlatfrom_Up * DirectionMirror, groundedDistance, groundLayer);
        */
        bool RoleGrounded = Physics.Raycast(Role.position + Role.up * RoleGroundedCheckOffset, Role.up * -1, RoleGroundedDistance, groundLayer);
        return RoleGrounded;
        //return RoleGrounded||LGrounded||RGrounded;
    }
    private void OnDrawGizmos()
    {
        /*int DirectionMirror = PlatfromDirectionMirror ? 1 : -1;
        Vector3 LFrontPlatfrom_Up, LBackPlatfrom_Up, RFrontPlatfrom_Up, RBackPlatfrom_Up;
        switch (platfromDirection)
        {
            case PlatfromDirection.x:
                LFrontPlatfrom_Up = LFrontPlatfrom.right;
                LBackPlatfrom_Up = LBackPlatfrom.right;
                RFrontPlatfrom_Up = RFrontPlatfrom.right;
                RBackPlatfrom_Up = RBackPlatfrom.right;
                break;
            case PlatfromDirection.y:
                LFrontPlatfrom_Up = LFrontPlatfrom.up;
                LBackPlatfrom_Up = LBackPlatfrom.up;
                RFrontPlatfrom_Up = RFrontPlatfrom.up;
                RBackPlatfrom_Up = RBackPlatfrom.up;
                break;
            case PlatfromDirection.z:
                LFrontPlatfrom_Up = LFrontPlatfrom.forward;
                LBackPlatfrom_Up = LBackPlatfrom.forward;
                RFrontPlatfrom_Up = RFrontPlatfrom.forward;
                RBackPlatfrom_Up = RBackPlatfrom.forward;
                break;
            default:
                LFrontPlatfrom_Up = Vector3.zero;
                LBackPlatfrom_Up = Vector3.zero;
                RFrontPlatfrom_Up = Vector3.zero;
                RBackPlatfrom_Up = Vector3.zero;
                break;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(LFrontPlatfrom.position + LFrontPlatfrom_Up * DirectionMirror * groundedCheckOffset, LFrontPlatfrom.position + LFrontPlatfrom_Up * DirectionMirror * groundedCheckOffset + LFrontPlatfrom_Up * DirectionMirror* groundedDistance);
        Gizmos.DrawLine(LBackPlatfrom.position + LBackPlatfrom_Up * DirectionMirror * groundedCheckOffset, LBackPlatfrom.position + LBackPlatfrom_Up * DirectionMirror * groundedCheckOffset + LBackPlatfrom_Up * DirectionMirror * groundedDistance);
        Gizmos.DrawLine(RFrontPlatfrom.position + RFrontPlatfrom_Up * DirectionMirror * groundedCheckOffset, RFrontPlatfrom.position + RFrontPlatfrom_Up * DirectionMirror * groundedCheckOffset + RFrontPlatfrom_Up * DirectionMirror * groundedDistance);
        Gizmos.DrawLine(RBackPlatfrom.position + RBackPlatfrom_Up * DirectionMirror * groundedCheckOffset, RBackPlatfrom.position + RBackPlatfrom_Up * DirectionMirror * groundedCheckOffset + RBackPlatfrom_Up * DirectionMirror * groundedDistance);
        Gizmos.DrawLine(Role.position + Role.up * RoleGroundedCheckOffset, Role.position + Role.up * RoleGroundedCheckOffset + Role.up * -1 * RoleGroundedDistance);
        Vector3 VerticalDirection =  camera_transform.forward;
        VerticalDirection.y = 0;
        VerticalDirection = Vector3.Normalize(VerticalDirection);
        Vector3 HorizontalDirection = Quaternion.Euler(0, 90, 0) * VerticalDirection;
        Gizmos.DrawLine(Role.position, Role.position + VerticalDirection * 20);
        Gizmos.DrawLine(Role.position, Role.position + HorizontalDirection * 10);
        */
        
    }
}
