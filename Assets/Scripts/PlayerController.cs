using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem extraBounceAura;
    public ParticleSystem explosionWaveAura;
    public ParticleSystem forceAura;
    public ParticleSystem extraGripAura;

    public AudioSource audioSource;
    public AudioClip bounceAudio;

    public GameObject centerPitTop;
    public GameObject centerPitBottom;

    public GameObject projectilePrefab;
    private GameObject projectile;

    private Rigidbody playerRb;
    private GameObject focalPoint;

    private GameManager gameManager;

    private float speed;
    private static float standardSpeed = 5f;
    private static float extraGripSpeed = 9f;

    public static float powerupPushStrength = 15.0f;
    public static float eliteEnemyPowerupDecrease = 12.0f;
    public static float bossPoweupDecrease = 16.0f;

    public static float explosionForce = 33f;
    public static float explosionRadius = 18.0f;
    public static float explosionJumpHeight = 7f;

    public static float forceAuraRadius = 4f;
    public static float forceAuraPushFactor = 0.5f;

    private static float continuousBuffAffectTime = 7.0f;
    private static float quickBuffAffectTime = 2.0f;

    private static float destroyPosY = -10;

    private bool hasExtraBounce;
    private bool hasExplosionEffect;
    private bool hasForceAura;
    private bool hasExtraGrip;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");

        hasExtraBounce = false;
        hasExplosionEffect = false;

        speed = standardSpeed;

        extraBounceAura = Instantiate(extraBounceAura, transform.position, extraBounceAura.transform.rotation);
        explosionWaveAura = Instantiate(explosionWaveAura, transform.position, explosionWaveAura.transform.rotation);
        extraGripAura = Instantiate(extraGripAura, transform.position, explosionWaveAura.transform.rotation);
        forceAura = Instantiate(forceAura, transform.position, explosionWaveAura.transform.rotation);
        extraBounceAura.Stop();
        explosionWaveAura.Stop();
        extraGripAura.Stop();
        explosionWaveAura.Stop();
    }

    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);

        UpdateAuras();

        CheckDeath();
    }

    private void UpdateAuras()
    {
        extraBounceAura.transform.position = transform.position;
        extraGripAura.transform.position = transform.position;
        forceAura.transform.position = transform.position;
        explosionWaveAura.transform.position = new Vector3(
            transform.position.x,
            transform.position.y - 0.8f, //exlosion effect should stay on the ground
            transform.position.z);

        if (hasExtraBounce) {
            extraBounceAura.Play();
        }
        else {
            extraBounceAura.Stop();
        }
        if (hasExplosionEffect) {
            explosionWaveAura.Play();
        }
        else {
            explosionWaveAura.Stop();
        }
        if (hasExtraGrip) {
            extraGripAura.Play();
        }
        else {
            extraGripAura.Stop();
        }
        if (hasForceAura) {
            forceAura.Play();
        }
        else {
            forceAura.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case nameof(PowerupType.EXTRA_BOUNCE):
                OnExtraBouncePick(other);
                break;
            case nameof(PowerupType.BULLET_LAUNCH):
                OnBulletLaunchPick(other);
                break;
            case nameof(PowerupType.EXPLOSION_WAVE):
                OnExplosionWavePick(other);
                break;
            case nameof(PowerupType.EXTRA_GRIP):
                OnExtraGripPicked(other);
                break;
            case nameof(PowerupType.FORCE_AURA):
                OnForceAuraPicked(other);
                break;
            case nameof(PowerupType.OPEN_CENTER_PIT):
                //TODO only with boss?
                OnCenterPitOpen(other);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided with " + collision.gameObject.name + " with powerup = " + hasExtraBounce);

        if (collision.gameObject.CompareTag(nameof(EnemyType.Enemy))
            || collision.gameObject.CompareTag(nameof(EnemyType.EliteEnemy))
            || collision.gameObject.CompareTag(nameof(EnemyType.Boss)))
        {
            OnCollisionWithEnemy(collision);
        }
    }

    private void OnCollisionWithEnemy(Collision collision)
    {
        audioSource.PlayOneShot(bounceAudio); //TODO add more smash clips

        Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
        Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

        if (collision.gameObject.CompareTag(nameof(EnemyType.Enemy)) && hasExtraBounce)
        {
            enemyRb.AddForce(awayFromPlayer * powerupPushStrength, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag(nameof(EnemyType.EliteEnemy)) && hasExtraBounce)
        {
            enemyRb.AddForce(awayFromPlayer * (powerupPushStrength - eliteEnemyPowerupDecrease), ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag(nameof(EnemyType.Boss)))
        {
            enemyRb.AddForce(awayFromPlayer * (powerupPushStrength - bossPoweupDecrease), ForceMode.Impulse);
        }
    }

    private void OnExtraBouncePick(Collider other)
    {
        hasExtraBounce = true;

        other.gameObject.GetComponent<ObjectBounce>().DestroyEffects();

        gameManager.SetPickedPowerupDescription("Extra Bounce");

        StartCoroutine(PowerupCountdownRoutine());
    }

    private void OnBulletLaunchPick(Collider other)
    {
        gameManager.SetPickedPowerupDescription("Bullet Launch");

        other.gameObject.GetComponent<ObjectBounce>().DestroyEffects();

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i].gameObject;

            projectile = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.identity);
            projectile.GetComponent<BulletLaunch>().Fire(enemy.transform);
        }
    }

    private void OnExplosionWavePick(Collider other)
    {
        gameManager.SetPickedPowerupDescription("Explosion Wave");

        other.gameObject.GetComponent<ObjectBounce>().DestroyEffects();

        hasExplosionEffect = true;
        StartCoroutine(ExplosionEffectCountdownRoutine());

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * explosionJumpHeight, ForceMode.Impulse);

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i].gameObject;

            Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();

            enemyRb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }
    }

    private void OnExtraGripPicked(Collider other)
    {
        gameManager.SetPickedPowerupDescription("Extra Grip");

        other.gameObject.GetComponent<ObjectBounce>().DestroyEffects();

        hasExtraGrip = true;

        SetExtraGripSpeed();

        StartCoroutine(ExtraGripSpeedEffectCountdownRoutine());
    }

    private void OnForceAuraPicked(Collider other)
    {
        gameManager.SetPickedPowerupDescription("Force Aura");

        other.gameObject.GetComponent<ObjectBounce>().DestroyEffects();

        hasForceAura = true;

        StartCoroutine(ForceFieldEffectConutdownRoutine());
    }

    private void OnCenterPitOpen(Collider other)
    {
        gameManager.SetPickedPowerupDescription("Open Center Pit");

        other.gameObject.GetComponent<ObjectBounce>().DestroyEffects();

        centerPitTop.GetComponent<CenterPitController>().OpenPit();
        centerPitBottom.GetComponent<CenterPitController>().OpenPit();
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(continuousBuffAffectTime);
        hasExtraBounce = false;
    }

    IEnumerator ExplosionEffectCountdownRoutine()
    {
        yield return new WaitForSeconds(quickBuffAffectTime);
        hasExplosionEffect = false;
    }

    IEnumerator ExtraGripSpeedEffectCountdownRoutine()
    {
        yield return new WaitForSeconds(continuousBuffAffectTime);
        ResetSpeed();
        hasExtraGrip = false;
    }

    IEnumerator ForceFieldEffectConutdownRoutine()
    {
        yield return new WaitForSeconds(continuousBuffAffectTime);
        hasForceAura = false;
    }

    private void SetExtraGripSpeed()
    {
        speed = extraGripSpeed;
    }

    private void ResetSpeed()
    {
        speed = standardSpeed;
    }

    public bool HasForceAura()
    {
        return hasForceAura;
    }

    private void CheckDeath()
    {
        if (transform.position.y < destroyPosY)
        {
            Destroy(gameObject);

            //also could be nice to show end game stats for a few sec
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
