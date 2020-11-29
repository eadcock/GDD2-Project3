using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim
{
    Animator enemeyAnimator;
    public EnemyAnim(Animator _animator)
    {
        enemeyAnimator = _animator;
    }

    public void IsWalking(bool _walking)
    {
        enemeyAnimator.SetBool("Walking", _walking);
    }

    public void Attack()
    {
        enemeyAnimator.SetTrigger("Attack");
    }

    public void GetHit()
    {
        enemeyAnimator.SetTrigger("Hit");
    }

    public void Die()
    {
        enemeyAnimator.SetTrigger("Die");
    }
}
