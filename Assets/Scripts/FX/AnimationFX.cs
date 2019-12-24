using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFX : MonoBehaviour
{
    [SerializeField] private GameObject explosion = null;
    [SerializeField] private GameObject laserHor = null;
    [SerializeField] private GameObject laserVert = null;
    [SerializeField] private Missile missile = null;

    [SerializeField] private float explosionPersistance = 0f;
    [SerializeField] private float laserPersistance = 0f;
    [SerializeField] private float missileSpeed = 0f;

    [SerializeField] private GameObject mask = null;
    [SerializeField] private GameObject timeStopSprite= null;
    [SerializeField] private float maskSize = 0f;

    public static AnimationFX Instance { get; private set; }

    private IEnumerator coroutine = null;

    private void Start()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion  
    }

    public void PlayExplosionFx(Vector2 place) {
        StartCoroutine(ExplosionPersistance(explosion, place));
    }

    public void PlayLaserFx(Vector2 place, Axis axis)
    {
        GameObject laser;
        if (axis == Axis.Horizontal) {
            laser = laserHor;
        } else 
        {
            laser = laserVert;
        }
        StartCoroutine(LaserPersistance(laser, place));
    }

    public void PlayShootingAnimation(Vector2 place, Block victim) {
        ShootingAnimation(missile, place, victim);
    }

    public void PlayFakeShooting(Vector2 place) {
        ShootingAnimation(missile, place, null);
    }

    public void PlayTimeStopAnimation(float waitTime)
    {
        coroutine = TimeStop(waitTime);
        StopCoroutine(coroutine);
        StartCoroutine(coroutine);
    }


    void ShootingAnimation(Missile missile, Vector2 spawn, Block victim)
    {
        Missile NewMissile = Instantiate(missile, spawn, Quaternion.identity);
        NewMissile.mySpeed = missileSpeed;
        NewMissile.victim = victim;
    }


    IEnumerator ExplosionPersistance(GameObject explosion, Vector2 place)
    {
        GameObject NewExplosion = Instantiate(explosion, place, Quaternion.identity);
        yield return new WaitForSeconds(explosionPersistance);
        Destroy(NewExplosion);
    }

    IEnumerator LaserPersistance(GameObject laser, Vector2 place) {
        GameObject NewLaser = Instantiate(laser, place, Quaternion.identity);
        yield return new WaitForSeconds(laserPersistance);
        Destroy(NewLaser);
    }

    IEnumerator TimeStop(float waitTime) {
        float elapsedTime = 0;
        float delta;

        mask.SetActive(true);
        mask.transform.localScale += new Vector3(maskSize, maskSize, 0);

        while (elapsedTime <= waitTime)
        {
            elapsedTime += Time.deltaTime;
            delta = maskSize - (maskSize * (elapsedTime / waitTime));
            mask.transform.position = Game.Instance.Player.playerBlock.gameObject.transform.position;
            timeStopSprite.transform.position = Game.Instance.Player.playerBlock.gameObject.transform.position;
            mask.transform.localScale = new Vector3(delta, delta, 0);
            yield return null;
        }
        mask.SetActive(false);
    }
}
