using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering.Universal;

public class BulletBehavior : MonoBehaviour
{
    [Header("Bullet Stats")]
    public float force, upwardforce;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerClik;
    public bool allowbuttonhold;

    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    [Header("Dependencies")]
    public GameObject bullet;
    public Camera main_Camera;
    public Transform objectivePoint;

    public Animation m_Animation;
    public AnimationClip m_ReloadAnimationClip;
    public AnimationClip m_ShootAnimationClip;
    public AnimationClip m_IdleAnimationClip;

    public GameObject fogonazo;
    public TextMeshProUGUI ammoDisplay;

    public CPoolElements m_BulletPool;

    public bool allowinvoke = true;

    public void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;
        m_BulletPool = new CPoolElements();
        magSize = 10;
    }
    public void Update()
    {
        if (!shooting && !reloading)
        {
            SetIdleAnimation();
        }
        m_BulletPool.Init(10, bullet);
        MyInput();
        UpdateAmmoDisplay();
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

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < 10 && magSize >0 && !reloading)
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
        SetShootingAnimation();
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
        SetReloadingAnimation();
        reloading= true;
        Invoke("ReloadFinished",reloadTime);
    }
    private void ReloadFinished()
    {
        int bulletsToLoad = 10 - bulletsLeft;
        magSize -= bulletsToLoad;
        if (magSize < 0)
        {
            bulletsLeft += bulletsToLoad + magSize;
            magSize = 0;
            reloading = false;
        }
        else
        {
            bulletsLeft += bulletsToLoad;
            reloading = false;
        }
    }
    
    public void addAmmo(int ammo)
    {
        magSize += ammo;
        UpdateAmmoDisplay();
    }
    public void UpdateAmmoDisplay()
    {
        GameObject.Find("Prueba UI").GetComponent<UIsystem>().UpdateAmmo(bulletsLeft, magSize);
    }

    public void SetReloadingAnimation()
    {
        m_Animation.Stop();
        m_Animation.CrossFade(m_ReloadAnimationClip.name, 0.1f);
    }
    public void SetShootingAnimation()
    {
        m_Animation.Stop();
        m_Animation.CrossFade(m_ShootAnimationClip.name, 0.19f);
    }
    public void SetIdleAnimation()
    {
        if (!m_Animation.IsPlaying(m_IdleAnimationClip.name))
        {
            m_Animation.CrossFade(m_IdleAnimationClip.name, 0.19f);
        }
    }

    public int GetBulletsLeft()
    {
        return bulletsLeft;
    }

    public int GetMagSize()
    {
        return magSize;
    }
}
