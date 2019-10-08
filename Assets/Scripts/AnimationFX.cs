using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFX : MonoBehaviour
{
    public GameObject explosion;
    public GameObject laserHor;
    public GameObject laserVert;
    public Missile missile;

    public float explosionPersistance;
    public float laserPersistance;
    public float missileSpeed;

    public static AnimationFX Instance { get; private set; }

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

    


}
