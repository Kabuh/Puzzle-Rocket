using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFX : MonoBehaviour
{
    public GameObject explosion;

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


    IEnumerator ExplosionPersistance(GameObject explosion, Vector2 place)
    {
        GameObject NewExplosion = Instantiate(explosion, place, Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Destroy(NewExplosion);
    }
}
