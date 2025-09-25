using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab1;
    public GameObject projectilePrefab2; 
    public Transform firePoint;

    private Camera cam;
    private int currentProjectileIndex = 0; 

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchProjectile();
        }
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        GameObject selectedProjectile = currentProjectileIndex == 0 ? projectilePrefab1 : projectilePrefab2;
        Instantiate(selectedProjectile, firePoint.position, Quaternion.LookRotation(direction));
    }

    void SwitchProjectile()
    {
        currentProjectileIndex = (currentProjectileIndex + 1) % 2;
    }
}