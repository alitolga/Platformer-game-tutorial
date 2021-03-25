using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0;
    public int damage = 10;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;

    float timeToFire = 0;
    Transform firePoint;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    private void Awake()
    {
        firePoint = transform.Find("Firepoint");
        if(firePoint == null) {
            Debug.LogError("Firepoint object not found");
        }


    }


    // Update is called once per frame
    void Update()
    {
        if(fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }


    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, (mousePosition - firePointPosition), 100, whatToHit);
        if (Time.time >= timeToSpawnEffect)
        {
            Effect();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
        //Debug.DrawLine(firePointPosition, mousePosition);
        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                Debug.Log("Hit enemy " + hit.collider.name + "and did damage " + damage);
                enemy.DamageEnemy(damage);
            }
        }
            //Debug.DrawLine(firePointPosition, hit.point, Color.red);


    }

    void Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        Transform clone = (Transform) Instantiate(MuzzleFlashPrefab, firePoint.position+Vector3.right*0.5f, firePoint.rotation * Quaternion.Euler(0,0,-90));
        clone.parent = firePoint;
        float size = Random.Range(0.2f, 0.4f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);
    }


}
