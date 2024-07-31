using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{

    [SerializeField]
    protected GameObject weapon;
    protected float attackSpeed;
    protected bool canAttack;
    protected bool attacking = false;
    protected bool faceRight = true;
    protected float weaponSpeedRotation = 1500;

    public void Attack()
    {
            attacking = true;
            StartCoroutine(Attacking());
    }

    virtual public void Flip() { }

    private IEnumerator Attacking()
    {
        weapon.transform.rotation = Quaternion.Euler(0, 0, 0);
        Quaternion targetRotation;
        if (faceRight) targetRotation = Quaternion.Euler(0, 0, 180);
        else targetRotation = Quaternion.Euler(0, 0, -180);
        while (Quaternion.Angle(weapon.transform.rotation, targetRotation) > 0.1f)
        {
            weapon.transform.rotation = Quaternion.RotateTowards(weapon.transform.rotation,
                targetRotation, weaponSpeedRotation * Time.deltaTime);
            yield return null;
        }

        weapon.transform.rotation = Quaternion.Euler(0, 0, 0);
        attacking = false;
        yield break;
    }
}
