using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeAttack : MonoBehaviour {
    private Transform transform;
    private Animator animator;
    private bool RAttacked = false;
    private bool LAttacked = false;
    private void Awake()
    {
        transform = this.GetComponent<Transform>();
        animator = this.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void BeAttacked(Vector3 AttackedPoint)
    {
        
        Vector3 ThisForward = transform.forward;
        Vector3 ThisRight = transform.right;
        Vector3 AttackedDir = (AttackedPoint - transform.position - Vector3.up * (AttackedPoint.y - transform.position.y)).normalized;
        float dot = Vector3.Dot(ThisForward, AttackedDir);
        float angle = Mathf.Rad2Deg * dot;
        float dotR= Vector3.Dot(ThisRight, AttackedDir);
        bool RBeAttacked = false, LBeAttacked = false;
        if (angle < 10f) return;
        if (angle > 70)
        {
            RBeAttacked = LBeAttacked = true;
        }
        else if (dotR > 0)
        {
            RBeAttacked = true;
        }
        else
        {
            LBeAttacked = true;
        }

        StopCoroutine(SetIdleAfter(0.5f));
        RAttacked = RBeAttacked;
        LAttacked = LBeAttacked;
        animator.SetBool("RAttacked", RBeAttacked);
        animator.SetBool("LAttacked", LBeAttacked);
        StartCoroutine(SetIdleAfter(0.5f));
        
        
    }
    IEnumerator SetIdleAfter(float time)
    {
        
        yield return new WaitForSeconds(time);
        animator.SetBool("RAttacked", false);
        animator.SetBool("LAttacked", false);
        RAttacked = false;
        LAttacked = false;
    }
    public void Asd()
    {
        Debug.Log("Asd");
    }
    
}
