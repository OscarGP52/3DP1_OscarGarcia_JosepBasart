using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class BulletBehavior : MonoBehaviour
{
    public GameObject bullet;

    public float force, upwardforce;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerClik;
    public bool allowbuttonhold;

    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    public Camera main_Camera;
    public Transform objectivePoint;

    public GameObject fogonazo;
    public TextMeshProUGUI ammoDisplay;

    public bool allowinvoke = true;

    public void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;

    }

    public void Update()
    {
        MyInput();

        if(ammoDisplay != null)
        {
            ammoDisplay.SetText(bulletsLeft/ bulletsPerClik + " / " + magSize / bulletsPerClik);
        }
    }

    public void MyInput()
    {
        if (allowbuttonhold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
        {
            Reload();
        }
        if (readyToShoot && shooting && !reloading && bulletsLeft <=0)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = main_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 directionWithoutSpread = targetPoint - objectivePoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bullet, objectivePoint.position, Quaternion.identity );

        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * force, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(main_Camera.transform.up* upwardforce, ForceMode.Impulse);

        if(fogonazo != null)
        {
            Instantiate(fogonazo, objectivePoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        if (allowinvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowinvoke = false;
        }
        if(bulletsShot < bulletsPerClik && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowinvoke = true;
    }

    private void Reload()
    {
        reloading= true;
        Invoke("ReloadFinished",reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magSize;
        reloading = false;
    }
     
}
