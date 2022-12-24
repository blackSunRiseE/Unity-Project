using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class BaseEnemyAI : MonoBehaviour
{
    [HideInInspector] public float damagedAnimationDuration;
    [HideInInspector] public bool getHit = false;
    [HideInInspector] public float health;
    [HideInInspector] public Animator animator;
    [SerializeField] public float maxHealth;
    [SerializeField] public float rangeToStopAct;
    protected NavMeshAgent enemy;
    [HideInInspector] public float deadAnimationDuration;
    public void ChasePlayer(Transform player, float moveSpeed)
    {
        Vector3 playerPosition = new Vector3(player.position.x,
                                             transform.position.y,
                                             player.position.z);
        enemy.SetDestination(playerPosition);
    }

    public void StopUnit(Vector3 position)
    {
        enemy.SetDestination(position);
    }

    public void LookAtPlayer(Transform selfPosition, Vector3 playerPosition)
    {
        Vector3 playerPos = new Vector3(playerPosition.x,
                                             selfPosition.position.y,
                                             playerPosition.z);
        selfPosition.LookAt(playerPos);
    }

    public void DestroyEnemy(GameObject gameObject)
    {
        deadAnimationDuration = 2;
        Destroy(gameObject, deadAnimationDuration);
    }
    public AnimationClip FindAnimation(Animator animator, string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }
    public void TakeDamage(float damage)
    {
        AnimationClip damagedClip = FindAnimation(animator, "Damaged");
        if (damagedClip != null)
        {
            damagedAnimationDuration = damagedClip.length;
        }
        getHit = true;
        health -= damage;
        transform.Find("Canvas").Find("Health Bar").GetComponent<EnemyUI>().SetHealth((int)health);
    }
    public float GetDistanceToPlayer(Vector3 playerPosition)
    {
        return Vector3.Distance(transform.position, playerPosition);
    }

}
