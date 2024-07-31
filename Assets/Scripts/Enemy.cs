using System.Collections;
using UnityEngine;

public class Enemy : BaseUnit
{
    static public GameObject player;

    public void Start()
    {
        StartCoroutine(Attacking());
    }

    public void Initialize(float attackSpeed)
    {
        this.attackSpeed = attackSpeed;
    }

    private IEnumerator Attacking()
    {
        while (true)
        {
            if (!attacking) Attack();
            yield return new WaitForSecondsRealtime(1/attackSpeed);
        }
    }

    public override void Flip()
    {
        if ((player.transform.position.x - transform.position.x) > 0 && !faceRight && !attacking)
        {
            faceRight = !faceRight;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if ((player.transform.position.x - transform.position.x) < 0 && faceRight && !attacking)
        {
            faceRight = !faceRight;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void Update()
    {
        Flip();
    }
}
