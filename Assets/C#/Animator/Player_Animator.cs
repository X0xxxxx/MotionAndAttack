using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animator : MonoBehaviour {
    private Player_State player_state;
    private Animator animator;
    private float speed;
    private bool attack;
    private int attack_level;
    private float time =0;
    private bool isJump = false;
    // Use this for initialization
    private void Awake()
    {
        animator=this.GetComponent<Animator>();
        speed = animator.GetFloat("Speed");
        attack = animator.GetBool("Attack");
        attack_level = animator.GetInteger("Attack_Level");
        player_state = this.GetComponent<Player_State>();
        
    }
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        
        speed = Mathf.Max(player_state.GetSpeed(),Mathf.Abs(player_state.GetAngleSpeed()));
        animator.SetFloat("Speed", speed);
        if (Input.GetButtonDown("Fire1"))
        {
           
            if (!attack)
            {
                attack = true;
                animator.SetBool("Attack", attack);
            }
            if (Time.time - time <= 1f)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1);
                if (stateInfo.IsName("preparation_for_attack_idle") && attack_level == 0) attack_level += 1;
                else if (stateInfo.IsName("attack(2)") && attack_level == 1) attack_level += 1;
                else if (stateInfo.IsName("attack(3)") && attack_level == 2) attack_level += 1;
                else if (stateInfo.IsName("attack(4)") && attack_level == 3) attack_level += 1;
                else if (stateInfo.IsName("attack_combo(1)") && attack_level == 4) attack_level = 1;

            }
            else
            {
                attack_level = 1;    
            }
            animator.SetInteger("Attack_Level", attack_level);
            time = Time.time;
            
        }
        else if (attack&&Time.time-time>2f)
        {
            animator.GetCurrentAnimatorClipInfo(1);
            attack_level = 0;
            animator.SetInteger("Attack_Level", attack_level);
            attack = false;
            animator.SetBool("Attack", attack);
        }
	}
    private void LateUpdate()
    {
    }
    public bool IsJump()
    {
        return isJump;
    }
    public void Jump()
    {
        isJump = true;        
        animator.SetLayerWeight(1, 0);
        animator.SetBool("Jump", true);
    }
    public void JumpEnd()
    {
        isJump = false;
        animator.SetLayerWeight(1, 1);
        animator.SetBool("Jump", false);
    }
}
