using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombatController : MonoBehaviour
{
    public UnityEvent OnBasicAttack;

    [SerializeField] private Hitbox basicAttackHitbox;
    [SerializeField] private Animator animator;
    [SerializeField] private float basicAttackCooldown = 0.8f;
    [SerializeField] private float basicAttackMovementStopTime = 0.3f;
    
    private bool canBasicAttack = true;
    private Coroutine basicAttackCoroutine;

    #region Getters Setters
    public bool CanBasicAttack() { return canBasicAttack; }
    public float GetBasicAttackCooldown() { return basicAttackCooldown; }
    public float GetBasicAttackMovementStopTime() { return basicAttackMovementStopTime; }
    #endregion

    public void BasicAttack()
    {
        if(canBasicAttack)
        {
            canBasicAttack = false;

            // the animation turns the hitbox on and off
            animator.SetTrigger("attack");

            OnBasicAttack.Invoke();

            if (basicAttackCoroutine != null)
            {
                Debug.LogWarning("Basic attack already in progress. Restarting it.", this);
                StopCoroutine(basicAttackCoroutine);
            }

            basicAttackCoroutine = StartCoroutine(BasicAttackCoroutine());
        }
        
    }
    private IEnumerator BasicAttackCoroutine()
    {
        yield return new WaitForSeconds(basicAttackCooldown);
        canBasicAttack = true;
    }
}