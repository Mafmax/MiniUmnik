using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Gun : MonoBehaviour
{
    public Bullet bulletExample;
    private Animator animator;
    private Transform End { get; set; }
    private bool CanShoot { get; set; }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        CanShoot = true;
        End = GetComponentsInChildren<Transform>().Where(x => x.name.Equals("End", System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }

    public void RunLogic(bool isRun)
    {
        animator.SetBool("IsRun", isRun);
    }
    public void Shoot(Vector3 target)
    {
        if (!CanShoot)
        {
            return;
        }
        else
        {

            var bullet = Instantiate(bulletExample);
            bullet.transform.position = End.position;
            bullet.Shoot(target - transform.position, 1000);
            CanShoot = false;
            animator.SetTrigger("Shoot");

            StartCoroutine(ShootDelayCoroutine());
        }
    }
    private IEnumerator ShootDelayCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        CanShoot = true;
        animator.ResetTrigger("Shoot");
    }

}
