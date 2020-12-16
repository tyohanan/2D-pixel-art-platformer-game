using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage, stunDamageAmount;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;
    
    private bool gotInput, isAttacking, isFirstAttack, attackInput;

    private float lastInputTime = Mathf.NegativeInfinity;

    private Animator anim;
    private PlayerInputHandler InputHandler;
    private PlayerStats PS;
    private PlayerController PC;
    private AttackDetails attackDetails;

    private void Start()
    {
        anim = GetComponent<Animator>();
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
        anim.SetBool("canAttack", combatEnabled);
        InputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        attackInput = InputHandler.AttackInput;
        if (attackInput)
        {
            if (combatEnabled)
            {
                //Attempt combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            //Perform Attack1
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if(Time.time >= lastInputTime + inputTimer)
        {
            //Wait for new input
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        attackDetails.damageAmount = attack1Damage;
        attackDetails.stunDamageAmount = stunDamageAmount;

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
            //Instantiate hit particle
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    private void Damage(AttackDetails attackDetail)
    {
        if (!PC.GetDashStatus())
        {
            int direction;

            PS.DecreaseHealth(attackDetail.damageAmount);

            if (attackDetail.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            PC.Knockback(direction);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }

}
