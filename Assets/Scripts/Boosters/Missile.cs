using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Vector2 myTarget;
    public float mySpeed = 0;
    public Block victim;

    bool notDead = true;

    private void Start()
    {
        if (victim == null)
        {
            myTarget = new Vector2(transform.position.x, transform.position.y + 100);
        }
        else {
            myTarget = victim.gameObject.transform.position;
        }
    }

    void Update()
    {
        transform.position += Vector3.up * mySpeed * Time.deltaTime;
        if (notDead) {
            if (transform.position.y >= (myTarget.y - Game.Instance.CombinedGrid.step / 2))
            {
                if (victim != null) {
                    foreach (Element element in victim.dElements)
                    {
                        AnimationFX.Instance.PlayExplosionFx(element.gameObject.transform.position);
                    }
                    victim.SelfDestroy();
                }
                notDead = false;
                Destroy(this.gameObject);
            }
        }
        
    }
}
