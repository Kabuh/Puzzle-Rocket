using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiseBlockClass : MonoBehaviour
{

    [SerializeField] private GameObject blockSprite = null;
    [SerializeField] private GameObject boosterSprite = null;
    public bool isRight = false;
    public bool isLeft = false;

    private Sprite BlockSprite
    {
        get
        {
            return blockSprite.GetComponent<SpriteRenderer>().sprite;
        }
        set
        {
            blockSprite.GetComponent<SpriteRenderer>().sprite = value;
        }
    }
    private Sprite BoosterSprite
    {
        get
        {
            return boosterSprite.GetComponent<SpriteRenderer>().sprite;
        }
        set
        {
            boosterSprite.GetComponent<SpriteRenderer>().sprite = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("boop");
        HubManagerScript.Instance.Cast(isLeft, isRight);
    }


    private void Awake()
    {
        HubManagerScript.Instance.HubSetup(this);
    }

    public void SetBlock(Block block) {

        BlockSprite = block.artChild.GetComponent<SpriteRenderer>().sprite;
    }
    public void SetBooster(BoosterObject booster)
    {

        BoosterSprite = booster.artChild.GetComponent<SpriteRenderer>().sprite;
        if (booster.boosterType == DatabaseProvider.Database.TimeStop) {
            boosterSprite.transform.localScale = new Vector2(0.5f, 0.5f);
        }
    }

    public void SelfDestroy() {
        Destroy(this.gameObject);
    }
}
