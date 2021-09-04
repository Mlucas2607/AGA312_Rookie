using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Variables")]
    public int maxHealth = 100;
    public int healthRecharge = 1;
    public int currentHealth;

    [Header("Ship Movement")]
    public float forwardspeed = 25f;
    public float strafeSpeed = 7.5f;
    public float hoverSpeed = 5f;
    public float forwardAcceleration = 2.5f, strafeAcceleration = 2f, hoverAcceleration = 2f;
    public float rollSpeed = 90f, rollAcceleration = 3.5f;
    public float torgue = 100f;
    public float lookRateSpeed = 90f;
    

    //Private Movement Variables
    private Rigidbody rb;
    private float rollInput;
    private Vector2 lookInput, screenCenter, mouseDistance;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;

    [Header("Primary Weapon Variables")]
    public float gunRange = 1000f;
    public int gunDamage = 10;
    public float fireRate = 0.1f;
    private float nextShootTime;

    [Header("Secondary Weapon Variables")]
    public float railgunRange = 1000f;
    public int railgunDamage = 1000;
    public float railgunFireRate = 5f;
    private float railgunNextShootTime;

    [Header("FX Components")]
    public GameObject mainThruster;
    public Transform muzzlePos;
    public Transform rayPos;
    public GameObject hitEffectDefault;
    public GameObject hitEffectEnemy;
    public GameObject muzzleFlash;
    public GameObject laserEffect;

    [Header("Railgin fx Components")]
    public Transform railgunMuzzle;
    public GameObject railgunMuzzleFlash;
    public GameObject railgunBulletEffect;
    public GameObject railgunHitEffect;

    public UIManager ui;

    void Start()
    {
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        rb = gameObject.GetComponent<Rigidbody>();
        //currentHealth = maxHealth;
        //lastAttackTime = Time.time + healDelay;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            Shoot();

        if (Input.GetKey(KeyCode.Mouse1))
            ShootRailgun();

    }

    private void FixedUpdate()
    {
        RotatePlayer();
        MovePlayer();
    }

    void Shoot()
    {
        if (Time.time < nextShootTime)
            return;

        RaycastHit hit;
        Ray shootRay = new Ray(rayPos.position, transform.forward);
        if (Physics.Raycast(shootRay, out hit, gunRange))
        {
            if (hit.collider.tag == "Enemy")
            {
                Instantiate(hitEffectEnemy, hit.point, transform.rotation);
                //hit.collider.GetComponent<EnemyShip>().TakeDamage(gunDamage);
                hit.collider.SendMessage("TakeDamage", gunDamage);
            }
            else
                Instantiate(hitEffectDefault, hit.point, transform.rotation);
        }

        nextShootTime = Time.time + fireRate;
        Instantiate(muzzleFlash, muzzlePos.position, transform.rotation,transform);
        Instantiate(laserEffect, muzzlePos.position, transform.rotation,transform);
    }

    void ShootRailgun()
    {
        if (Time.time < railgunNextShootTime)
            return;

        RaycastHit hit;
        Ray shootRay = new Ray(rayPos.position, transform.forward);
        if (Physics.Raycast(shootRay, out hit, gunRange))
        {
            if (hit.collider.tag == "Boss")
            {
                Instantiate(railgunHitEffect, hit.point, transform.rotation);
                //hit.collider.GetComponent<EnemyShip>().TakeDamage(gunDamage);
                hit.collider.SendMessage("TakeDamageType2", railgunDamage);
            }
            else if(hit.collider.tag == "Enemy")
            {
                Instantiate(railgunHitEffect, hit.point, transform.rotation);
                //hit.collider.GetComponent<EnemyShip>().TakeDamage(gunDamage);
                hit.collider.SendMessage("TakeDamage", railgunDamage);
            }
            else
                Instantiate(railgunHitEffect, hit.point, transform.rotation);
        }

        railgunNextShootTime = Time.time + railgunFireRate;
        Instantiate(railgunMuzzleFlash, railgunMuzzle.position, transform.rotation, transform);
        Instantiate(railgunBulletEffect, railgunMuzzle.position, transform.rotation, transform);
    }

    private void MovePlayer()
    {
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardspeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

        rb.AddRelativeForce(Vector3.forward * activeForwardSpeed);
        rb.AddRelativeForce(Vector3.right * activeStrafeSpeed);
        rb.AddRelativeForce(Vector3.up * activeHoverSpeed);

        float thrusterScale = Mathf.Clamp(activeForwardSpeed, 4f, 50f);
        Vector3 scaleChange = new Vector3(mainThruster.transform.localScale.x,thrusterScale, mainThruster.transform.localScale.z);
        mainThruster.transform.localScale = scaleChange;      
    }

    private void RotatePlayer()
    {
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;
        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

        float roll = rollInput;
        float pitch = -mouseDistance.y * lookRateSpeed * Time.deltaTime;
        float yaw = mouseDistance.x * lookRateSpeed * Time.deltaTime;

        rb.AddRelativeTorque(Vector3.back * torgue * roll);
        rb.AddRelativeTorque(Vector3.right * torgue * pitch);
        rb.AddRelativeTorque(Vector3.up * torgue * yaw);

    }

    public void TakeDamage(int dmgAmt)
    {
        if (currentHealth <= 0)
        {
            PlayerDead();
            return;
        }
        currentHealth -= dmgAmt;
    }

    void PlayerDead()
    {
        ui.PlayerDied();
    }

    public void HealPlayer(int healAmt)
    {
        if (currentHealth >= maxHealth)
            return;

        currentHealth += healAmt;
    }
}

