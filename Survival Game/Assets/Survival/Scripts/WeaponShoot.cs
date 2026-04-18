using UnityEngine;
using XtremeFPS.InputHandling;

public class WeaponShoot : MonoBehaviour
{
    public FPSInputManager fPSInputManager;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        if(fPSInputManager.isFiringHold | fPSInputManager.isFiringTapped)
        {   
            if(Time.time >= nextFireTime)
            {
                RaycastHit hit;
                Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity);

                if(hit.collider.tag == "Enemy")
                {
                    Debug.Log("Hit Enemy");
                    EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();

                    if (enemy != null)
                    {
                        enemy.TakeDamage(10);
                        nextFireTime = Time.time + fireRate;
                    }
                }
            }
        }
    }
}
