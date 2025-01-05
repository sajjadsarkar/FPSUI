using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum fireMode { none, semi, auto, burst, shotgun, launcher }
public enum Ammo { Magazines, Bullets }
public enum Aim { Simple, Sniper }

public class WeaponScriptNEW : MonoBehaviour
{
    [HideInInspector]
    public fireMode currentMode = fireMode.semi;
    public fireMode firstMode = fireMode.semi;
    public fireMode secondMode = fireMode.launcher;
    public Ammo ammoMode = Ammo.Magazines;
    public Aim aimMode = Aim.Simple;

    [Header("Weapon configuration")]
    public LayerMask layerMask;
    public int damage = 50;
    public int bulletsPerMag = 50;
    public int magazines = 5;
    private float fireRate = 0.1f;
    public float fireRateFirstMode = 0.1f;
    public float fireRateSecondMode = 0.1f;
    public float range = 250.0f;
    public float force = 200.0f;

    [Header("Accuracy Settings")]
    public float baseInaccuracyAIM = 0.005f;
    public float baseInaccuracyHIP = 1.5f;
    public float inaccuracyIncreaseOverTime = 0.2f;
    public float inaccuracyDecreaseOverTime = 0.5f;
    private float maximumInaccuracy;
    public float maxInaccuracyHIP = 5.0f;
    public float maxInaccuracyAIM = 1.0f;
    private float triggerTime = 0.05f;
    private float baseInaccuracy;

    [Header("Aiming")]
    public Vector3 aimPosition;
    public bool aiming;
    private Vector3 curVect;
    private Vector3 hipPosition = Vector3.zero;
    public float aimSpeed = 0.25f;
    public float zoomSpeed = 0.5f;
    public int FOV = 40;
    public int weaponFOV = 45;

    private float scopeTime;
    private bool inScope = false;
    Image scopeTexture;

    [Header("Burst Settings")]
    public int shotsPerBurst = 3;
    public float burstTime = 0.07f;

    [Header("Shotgun Settings")]
    public int pelletsPerShot = 10;

    [Header("Launcher")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 30.0f;
    public float projectileGravity = 0.0f;
    public int projectiles = 20;
    public Transform launchPosition;
    public bool reloadProjectile = false;
    public AudioClip soundReloadLauncher;
    public Renderer rocket = null;

    [Header("Kickback")]
    public Transform kickGO;
    public float kickUp = 0.5f;
    public float kickSideways = 0.5f;

    [Header("Bulletmarks")]
    public GameObject Concrete;
    public GameObject Wood;
    public GameObject Metal;
    public GameObject Dirt;
    public GameObject Blood;
    public GameObject Water;
    public GameObject Untagged;

    [Header("Audio")]
    public AudioSource aSource;
    public AudioClip soundDraw;
    public AudioClip soundFire;
    public AudioClip soundReload;
    public AudioClip soundEmpty;
    public AudioClip switchModeSound;

    [Header("Animation Settings")]
    public Animation weaponAnim;
    public string fireAnim = "Fire";
    [Range(0.0f, 5.0f)]
    public float fireAnimSpeed = 1.0f;
    public string drawAnim = "Draw";
    [Range(0.0f, 5.0f)]
    public float drawAnimSpeed = 1.0f;
    [Range(0.0f, 5.0f)]
    public float drawTime = 1.5f;
    public string reloadAnim = "Reload";
    [Range(0.0f, 5.0f)]
    public float reloadAnimSpeed = 1.0f;
    [Range(0.0f, 5.0f)]
    public float reloadTime = 1.5f;
    public string fireEmptyAnim = "FireEmpty";
    [Range(0.0f, 5.0f)]
    public float fireEmptyAnimSpeed = 1.0f;
    public string switchAnim = "SwitchAnim";
    [Range(0.0f, 5.0f)]
    public float switchAnimSpeed = 1.0f;
    public string fireLauncherAnim = "FireLauncher";

    [Header("Other")]
    public FPSController fpscontroller;
    public WeaponManager wepManager;
    public Renderer muzzleFlash;
    public Light muzzleLight;
    public Camera mainCamera;
    public Camera wepCamera;
    public bool withSilencer = false;

    [HideInInspector] public bool reloading = false;
    [HideInInspector] public bool selected = false;
    private bool canSwicthMode = true;
    private bool draw;
    private bool playing = false;
    private bool isFiring = false;
    private bool bursting = false;
    private int m_LastFrameShot = -10;
    private float nextFireTime = 0.0f;
    private int bulletsLeft = 0;
    private RaycastHit hit;
    private float camFOV = 60.0f;
    private CanvasManager canvas;

    void Start()
    {
        canvas = CanvasManager.instance;
        muzzleFlash.enabled = false;
        muzzleLight.enabled = false;
        bulletsLeft = bulletsPerMag;
        currentMode = firstMode;
        fireRate = fireRateFirstMode;
        aiming = false;

        if (ammoMode == Ammo.Bullets)
        {
            magazines = magazines * bulletsPerMag;
        }

        if (aimMode == Aim.Sniper)
        {
            scopeTexture = CanvasManager.instance.sniperScope;
        }
    }

    void Update()
    {
        if (selected)
        {
            // if (Input.GetButtonDown("Fire"))
            // {
            //     if (currentMode == fireMode.semi)
            //     {
            //         FireSemi();
            //     }
            //     else if (currentMode == fireMode.launcher)
            //     {
            //         FireLauncher();
            //     }
            //     else if (currentMode == fireMode.burst)
            //     {
            //         StartCoroutine(FireBurst());
            //     }
            //     else if (currentMode == fireMode.shotgun)
            //     {
            //         FireShotgun();
            //     }

            //     if (bulletsLeft > 0)
            //         isFiring = true;
            // }

            // if (Input.GetButton("Fire"))
            // {
            //     if (currentMode == fireMode.auto)
            //     {
            //         FireSemi();
            //         if (bulletsLeft > 0)
            //             isFiring = true;
            //     }
            // }

            if (Input.GetButtonDown("Reload"))
            {
                StartCoroutine(Reload());
            }
        }

        bool work = false;
        if (work == true && !reloading && selected)
        {
            if (!aiming)
            {
                aiming = true;
                canvas.crossAlpha.alpha = 0f;
                curVect = aimPosition - transform.localPosition;
                scopeTime = Time.time + aimSpeed;
            }
            if (transform.localPosition != aimPosition && aiming)
            {
                if (Mathf.Abs(Vector3.Distance(transform.localPosition, aimPosition)) < curVect.magnitude / aimSpeed * Time.deltaTime)
                {
                    transform.localPosition = aimPosition;
                }
                else
                {
                    transform.localPosition += curVect / aimSpeed * Time.deltaTime;
                }
            }

            if (aimMode == Aim.Sniper)
            {
                if (Time.time >= scopeTime && !inScope)
                {
                    inScope = true;
                    scopeTexture.color = new Color(1, 1, 1, 0.9f);
                    Component[] gos = GetComponentsInChildren<Renderer>();
                    foreach (var go in gos)
                    {
                        Renderer a = go as Renderer;
                        a.enabled = false;
                    }
                }
            }
        }
        else
        {
            if (aiming)
            {
                aiming = false;
                canvas.crossAlpha.alpha = 1f;
                inScope = false;
                if (aimMode == Aim.Sniper) scopeTexture.color = new Color(1, 1, 1, 0f);
                curVect = hipPosition - transform.localPosition;
                if (aimMode == Aim.Sniper)
                {
                    Component[] go = GetComponentsInChildren<Renderer>();
                    foreach (var g in go)
                    {
                        Renderer b = g as Renderer;
                        if (b.name != "muzzle_flash")
                            b.enabled = true;
                    }
                }
            }

            if (Mathf.Abs(Vector3.Distance(transform.localPosition, hipPosition)) < curVect.magnitude / aimSpeed * Time.deltaTime)
            {
                transform.localPosition = hipPosition;
            }
            else
            {
                transform.localPosition += curVect / aimSpeed * Time.deltaTime;
            }
        }

        if (aiming)
        {
            maximumInaccuracy = maxInaccuracyAIM;
            baseInaccuracy = baseInaccuracyAIM;
            mainCamera.fieldOfView -= FOV * Time.deltaTime / zoomSpeed;
            if (mainCamera.fieldOfView < FOV)
            {
                mainCamera.fieldOfView = FOV;
            }
        }
        else
        {
            maximumInaccuracy = maxInaccuracyHIP;
            baseInaccuracy = baseInaccuracyHIP;
            mainCamera.fieldOfView += camFOV * Time.deltaTime * 3;
            if (mainCamera.fieldOfView > camFOV)
            {
                mainCamera.fieldOfView = camFOV;
            }
        }

        if (fpscontroller.velMagnitude > 3.0f)
        {
            triggerTime += inaccuracyDecreaseOverTime / 2f;
        }

        if (isFiring)
        {
            triggerTime += inaccuracyIncreaseOverTime;
        }
        else
        {
            if (fpscontroller.velMagnitude < 3.0f)
                triggerTime -= inaccuracyDecreaseOverTime;
        }

        if (triggerTime >= maximumInaccuracy)
        {
            triggerTime = maximumInaccuracy;
        }

        if (triggerTime <= baseInaccuracy)
        {
            triggerTime = baseInaccuracy;
        }

        wepManager.crosshairSize = triggerTime;

        if (nextFireTime > Time.time)
        {
            isFiring = false;
        }

        if (Input.GetButtonDown("switchFireMode") && secondMode != fireMode.none && canSwicthMode)
        {
            if (currentMode != firstMode)
            {
                StartCoroutine(FirstFireMode());
            }
            else
            {
                StartCoroutine(SecondFireMode());
            }
        }
    }

    void LateUpdate()
    {
        if (withSilencer || inScope) return;

        if (m_LastFrameShot == Time.frameCount)
        {
            muzzleFlash.transform.localRotation = Quaternion.AngleAxis(Random.value * 360, Vector3.forward);
            muzzleFlash.enabled = true;
            muzzleLight.enabled = true;
        }
        else
        {
            muzzleFlash.enabled = false;
            muzzleLight.enabled = false;
        }
    }

    IEnumerator FirstFireMode()
    {
        canSwicthMode = false;
        selected = false;
        weaponAnim.Rewind(switchAnim);
        weaponAnim.Play(switchAnim);
        aSource.clip = switchModeSound;
        aSource.Play();
        yield return new WaitForSeconds(0.6f);
        currentMode = firstMode;
        fireRate = fireRateFirstMode;
        selected = true;
        canSwicthMode = true;
    }

    IEnumerator SecondFireMode()
    {
        canSwicthMode = false;
        selected = false;
        aSource.clip = switchModeSound;
        aSource.Play();
        weaponAnim.Play(switchAnim);
        yield return new WaitForSeconds(0.6f);
        currentMode = secondMode;
        fireRate = fireRateSecondMode;
        selected = true;
        canSwicthMode = true;
    }

    public void FireSemi()
    {
        if (reloading || bulletsLeft <= 0)
        {
            if (bulletsLeft == 0)
            {
                StartCoroutine(OutOfAmmo());
            }
            return;
        }

        if (Time.time - fireRate > nextFireTime)
            nextFireTime = Time.time - Time.deltaTime;

        while (nextFireTime < Time.time)
        {
            FireOneBullet();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void FireLauncher()
    {
        if (reloading || projectiles <= 0)
        {
            if (projectiles == 0)
            {
                StartCoroutine(OutOfAmmo());
            }
            return;
        }

        if (Time.time - fireRate > nextFireTime)
            nextFireTime = Time.time - Time.deltaTime;

        while (nextFireTime < Time.time)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    public IEnumerator FireBurst()
    {
        int shotCounter = 0;

        if (reloading || bursting || bulletsLeft <= 0)
        {
            if (bulletsLeft <= 0)
            {
                StartCoroutine(OutOfAmmo());
            }
            yield break;
        }

        if (Time.time - fireRate > nextFireTime)
            nextFireTime = Time.time - Time.deltaTime;

        if (Time.time > nextFireTime)
        {
            while (shotCounter < shotsPerBurst)
            {
                bursting = true;
                shotCounter++;
                if (bulletsLeft > 0)
                {
                    FireOneBullet();
                }
                yield return new WaitForSeconds(burstTime);
            }
            nextFireTime = Time.time + fireRate;
        }
        bursting = false;
    }

    public void FireShotgun()
    {
        if (reloading || bulletsLeft <= 0 || draw)
        {
            if (bulletsLeft == 0)
            {
                StartCoroutine(OutOfAmmo());
            }
            return;
        }

        int pellets = 0;

        if (Time.time - fireRate > nextFireTime)
            nextFireTime = Time.time - Time.deltaTime;

        if (Time.time > nextFireTime)
        {
            while (pellets < pelletsPerShot)
            {
                FireOnePellet();
                pellets++;
            }
            bulletsLeft--;
            nextFireTime = Time.time + fireRate;
        }
        canvas.UpdateBullets(bulletsLeft);
        weaponAnim.Rewind(fireAnim);
        weaponAnim.Play(fireAnim);

        aSource.PlayOneShot(soundFire, 1.0f);

        m_LastFrameShot = Time.frameCount;
        kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles - new Vector3(kickUp, Random.Range(-kickSideways, kickSideways), 0));
    }

    void FireOneBullet()
    {
        if (nextFireTime > Time.time || draw)
        {
            if (bulletsLeft <= 0)
            {
                StartCoroutine(OutOfAmmo());
            }
            return;
        }

        Vector3 dir = gameObject.transform.TransformDirection(new Vector3(Random.Range(-0.01f, 0.01f) * triggerTime, Random.Range(-0.01f, 0.01f) * triggerTime, 1));
        Vector3 pos = transform.parent.position;

        if (Physics.Raycast(pos, dir, out hit, range, layerMask))
        {
            Vector3 contact = hit.point;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            float rScale = Random.Range(0.5f, 1.0f);

            if (hit.rigidbody)
                hit.rigidbody.AddForceAtPosition(force * dir, hit.point);

            if (hit.collider.CompareTag("Concrete"))
            {
                GameObject concMark = Instantiate(Concrete, contact, rot) as GameObject;
                concMark.transform.localPosition += .02f * hit.normal;
                concMark.transform.localScale = new Vector3(rScale, rScale, rScale);
                concMark.transform.parent = hit.transform;
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                Instantiate(Blood, contact, rot);
            }
            else if (hit.collider.CompareTag("Damage"))
            {
                Instantiate(Blood, contact, rot);
            }
            else if (hit.collider.CompareTag("Wood"))
            {
                GameObject woodMark = Instantiate(Wood, contact, rot) as GameObject;
                woodMark.transform.localPosition += .02f * hit.normal;
                woodMark.transform.localScale = new Vector3(rScale, rScale, rScale);
                woodMark.transform.parent = hit.transform;
            }
            else if (hit.collider.CompareTag("Metal"))
            {
                GameObject metalMark = Instantiate(Metal, contact, rot) as GameObject;
                metalMark.transform.localPosition += .02f * hit.normal;
                metalMark.transform.localScale = new Vector3(rScale, rScale, rScale);
                metalMark.transform.parent = hit.transform;
            }
            else if (hit.collider.CompareTag("Dirt") || hit.collider.CompareTag("Grass"))
            {
                GameObject dirtMark = Instantiate(Dirt, contact, rot) as GameObject;
                dirtMark.transform.localPosition += .02f * hit.normal;
                dirtMark.transform.localScale = new Vector3(rScale, rScale, rScale);
                dirtMark.transform.parent = hit.transform;
            }
            else if (hit.collider.CompareTag("Water"))
            {
                Instantiate(Water, contact, rot);
            }
            else if (hit.collider.CompareTag("Usable"))
            {

            }
            else
            {
                GameObject def = Instantiate(Untagged, contact, rot) as GameObject;
                def.transform.localPosition += .02f * hit.normal;
                def.transform.localScale = new Vector3(rScale, rScale, rScale);
                def.transform.parent = hit.transform;
            }

            IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            if (damagable != null) damagable.ApplyDamage(damage);
        }

        aSource.PlayOneShot(soundFire);
        m_LastFrameShot = Time.frameCount;

        weaponAnim[fireAnim].speed = fireAnimSpeed;
        weaponAnim.Rewind(fireAnim);
        weaponAnim.Play(fireAnim);

        kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles - new Vector3(kickUp, Random.Range(-kickSideways, kickSideways), 0));

        bulletsLeft--;
        canvas.UpdateBullets(bulletsLeft);
    }

    void FireOnePellet()
    {
        Vector3 dir = gameObject.transform.TransformDirection(new Vector3(Random.Range(-0.01f, 0.01f) * triggerTime, Random.Range(-0.01f, 0.01f) * triggerTime, 1));
        Vector3 pos = transform.parent.position;

        if (Physics.Raycast(pos, dir, out hit, range, layerMask))
        {
            Vector3 contact = hit.point;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            float rScale = Random.Range(0.5f, 1.0f);

            if (hit.rigidbody)
                hit.rigidbody.AddForceAtPosition(force * dir, hit.point);

            if (hit.collider.CompareTag("Concrete"))
            {
                GameObject concMark = Instantiate(Concrete, contact, rot) as GameObject;
                concMark.transform.localPosition += .02f * hit.normal;
                concMark.transform.localScale = new Vector3(rScale, rScale, rScale);
                concMark.transform.parent = hit.transform;
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                Instantiate(Blood, contact, rot);
            }
            else if (hit.collider.CompareTag("Damage"))
            {
                Instantiate(Blood, contact, rot);
            }
            else if (hit.collider.CompareTag("Wood"))
            {
                GameObject woodMark = Instantiate(Wood, contact, rot) as GameObject;
                woodMark.transform.localPosition += .02f * hit.normal;
                woodMark.transform.localScale = new Vector3(rScale, rScale, rScale);
                woodMark.transform.parent = hit.transform;
            }
            else if (hit.collider.CompareTag("Metal"))
            {
                GameObject metalMark = Instantiate(Metal, contact, rot) as GameObject;
                metalMark.transform.localPosition += .02f * hit.normal;
                metalMark.transform.localScale = new Vector3(rScale, rScale, rScale);
                metalMark.transform.parent = hit.transform;
            }
            else if (hit.collider.CompareTag("Dirt") || hit.collider.CompareTag("Grass"))
            {
                GameObject dirtMark = Instantiate(Dirt, contact, rot) as GameObject;
                dirtMark.transform.localPosition += .02f * hit.normal;
                dirtMark.transform.localScale = new Vector3(rScale, rScale, rScale);
                dirtMark.transform.parent = hit.transform;
            }
            else if (hit.collider.CompareTag("Water"))
            {
                Instantiate(Water, contact, rot);
            }
            else if (hit.collider.CompareTag("Usable"))
            {
            }
            else
            {
                GameObject def = Instantiate(Untagged, contact, rot) as GameObject;
                def.transform.localPosition += .02f * hit.normal;
                def.transform.localScale = new Vector3(rScale, rScale, rScale);
                def.transform.parent = hit.transform;
            }

            IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            if (damagable != null) damagable.ApplyDamage(damage);
        }
    }

    void FireProjectile()
    {
        if (projectiles < 1 || draw)
        {
            return;
        }

        float[] info = new float[2];
        info[0] = projectileSpeed;
        info[1] = projectileGravity;

        GameObject projectile = Instantiate(projectilePrefab, launchPosition.position, launchPosition.rotation) as GameObject;
        Projectile g = projectile.GetComponent<Projectile>();
        g.SetUp(info);

        weaponAnim[fireAnim].speed = fireAnimSpeed;
        weaponAnim.Rewind(fireAnim);
        weaponAnim.Play(fireAnim);

        projectiles--;
        canvas.UpdateProjectileUI(projectiles);

        if (reloadProjectile)
            StartCoroutine(ReloadLauncher());
    }

    IEnumerator OutOfAmmo()
    {
        if (reloading || playing) yield break;

        playing = true;
        aSource.PlayOneShot(soundEmpty, 0.3f);
        if (fireEmptyAnim != "")
        {
            weaponAnim.Rewind(fireEmptyAnim);
            weaponAnim.Play(fireEmptyAnim);
        }
        yield return new WaitForSeconds(0.2f);
        playing = false;
    }

    IEnumerator ReloadLauncher()
    {
        if (projectiles > 0)
        {
            wepManager.canSwitch = false;
            reloading = true;
            canSwicthMode = false;

            if (rocket != null)
                StartCoroutine(DisableProjectileRenderer());

            yield return new WaitForSeconds(0.5f);
            if (soundReloadLauncher)
                aSource.PlayOneShot(soundReloadLauncher);

            weaponAnim[reloadAnim].speed = reloadAnimSpeed;
            weaponAnim.Play(reloadAnim);

            yield return new WaitForSeconds(reloadTime);
            canSwicthMode = true;
            reloading = false;
            wepManager.canSwitch = true;
            canvas.UpdateProjectileUI(projectiles);
        }
        else
        {
            if (rocket != null && projectiles == 0)
            {
                rocket.enabled = false;
            }
        }
    }

    IEnumerator DisableProjectileRenderer()
    {
        rocket.enabled = false;
        yield return new WaitForSeconds(reloadTime / 1.5f);
        rocket.enabled = true;
    }

    void EnableProjectileRenderer()
    {
        if (rocket != null)
        {
            rocket.enabled = true;
        }
    }

    public IEnumerator Reload()
    {
        if (reloading) yield break;

        if (ammoMode == Ammo.Magazines)
        {
            reloading = true;
            canSwicthMode = false;
            if (magazines > 0 && bulletsLeft != bulletsPerMag)
            {
                weaponAnim[reloadAnim].speed = reloadAnimSpeed;
                weaponAnim.Play(reloadAnim, PlayMode.StopAll);
                aSource.PlayOneShot(soundReload);
                yield return new WaitForSeconds(reloadTime);
                magazines--;
                bulletsLeft = bulletsPerMag;
            }
            canvas.UpdateAmmoUI(bulletsLeft, magazines, projectiles);
            reloading = false;
            canSwicthMode = true;
            isFiring = false;
        }
        else if (ammoMode == Ammo.Bullets)
        {
            if (magazines > 0 && bulletsLeft != bulletsPerMag)
            {
                if (magazines > bulletsPerMag)
                {
                    canSwicthMode = false;
                    reloading = true;
                    weaponAnim[reloadAnim].speed = reloadAnimSpeed;
                    weaponAnim.Play(reloadAnim, PlayMode.StopAll);
                    aSource.PlayOneShot(soundReload, 0.7f);
                    yield return new WaitForSeconds(reloadTime);
                    magazines -= bulletsPerMag - bulletsLeft;
                    bulletsLeft = bulletsPerMag;
                    canSwicthMode = true;
                    reloading = false;
                    canvas.UpdateAmmoUI(bulletsLeft, magazines, projectiles);
                    yield break;
                }
                else
                {
                    canSwicthMode = false;
                    reloading = true;
                    weaponAnim[reloadAnim].speed = reloadAnimSpeed;
                    weaponAnim.Play(reloadAnim, PlayMode.StopAll);
                    aSource.PlayOneShot(soundReload);
                    yield return new WaitForSeconds(reloadTime);
                    var bullet = Mathf.Clamp(bulletsPerMag, magazines, bulletsLeft + magazines);
                    magazines -= (bullet - bulletsLeft);
                    bulletsLeft = bullet;
                    canSwicthMode = true;
                    reloading = false;
                    canvas.UpdateAmmoUI(bulletsLeft, magazines, projectiles);
                    yield break;
                }
            }
        }
    }

    IEnumerator DrawWeapon()
    {
        draw = true;
        wepCamera.fieldOfView = weaponFOV;
        canSwicthMode = false;
        aSource.clip = soundDraw;
        aSource.Play();
        StartCoroutine(UpdateUI());
        weaponAnim[drawAnim].speed = drawAnimSpeed;
        weaponAnim.Rewind(drawAnim);
        weaponAnim.Play(drawAnim, PlayMode.StopAll);
        yield return new WaitForSeconds(drawTime);

        draw = false;
        reloading = false;
        canSwicthMode = true;
        selected = true;
    }

    IEnumerator UpdateUI()
    {
        yield return 0;
        canvas.ShowCrosshair(1);
        canvas.crossAlpha.alpha = 1f;
        canvas.UpdateAmmoUI(bulletsLeft, magazines, projectiles);
        if (firstMode == fireMode.launcher || secondMode == fireMode.launcher)
        {
            canvas.ShowProjectilesUI(true);
        }
        else canvas.ShowProjectilesUI(false);
    }

    public void Deselect()
    {
        selected = false;
        bursting = false;
        aiming = true;
        canvas.crossAlpha.alpha = 0f;
        if (aimMode == Aim.Sniper) scopeTexture.color = new Color(1, 1, 1, 0f);
        mainCamera.fieldOfView = camFOV;
        transform.localPosition = hipPosition;

        if (rocket != null && projectiles > 0)
        {
            rocket.enabled = true;
        }
    }
    public void ToggleScope()
    {
        inScope = !inScope;

        if (inScope)
        {
            scopeTexture.color = new Color(1, 1, 1, 0.9f);
            Component[] gos = GetComponentsInChildren<Renderer>();
            foreach (var go in gos)
            {
                Renderer a = go as Renderer;
                a.enabled = false;
            }
            mainCamera.fieldOfView = FOV;
        }
        else
        {
            scopeTexture.color = new Color(1, 1, 1, 0f);
            Component[] go = GetComponentsInChildren<Renderer>();
            foreach (var g in go)
            {
                Renderer b = g as Renderer;
                if (b.name != "muzzle_flash")
                    b.enabled = true;
            }
            mainCamera.fieldOfView = camFOV;
        }
    }

}