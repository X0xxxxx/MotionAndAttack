using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public Transform LFist;
    public Transform RFist;
    private Animator animator;
    private LayerMask attackTriggerLayer = 1 << 8;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;Gizmos.DrawSphere(LFist.position, 0.2f);
    }
    private void AttackRFist()
    {
        Collider[] colliders= Physics.OverlapSphere(RFist.position, 0.2f, attackTriggerLayer);
        foreach(Collider collider in colliders)
        {
            EnemyBeAttack enemyBeAttack = collider.GetComponent<EnemyBeAttack>();
            if (enemyBeAttack != null)
            {
                enemyBeAttack.BeAttacked(RFist.position);
            }
        }

    }
    private void AttackLFist()
    {
        Collider[] colliders = Physics.OverlapSphere(LFist.position, 0.2f, attackTriggerLayer);
        foreach (Collider collider in colliders)
        {
            EnemyBeAttack enemyBeAttack = collider.GetComponent<EnemyBeAttack>();
            if (enemyBeAttack != null)
            {
                enemyBeAttack.BeAttacked(LFist.position);
            }
        }

    }
    private void AttackTwoFist()
    {
        Vector3 attackedPoint = (RFist.position + LFist.position) / 2;
        Collider[] colliders = Physics.OverlapSphere(attackedPoint, 0.2f, attackTriggerLayer);
        foreach (Collider collider in colliders)
        {
            EnemyBeAttack enemyBeAttack = collider.GetComponent<EnemyBeAttack>();
            if (enemyBeAttack != null)
            {
                enemyBeAttack.BeAttacked(attackedPoint);
            }
        }

    }
}
