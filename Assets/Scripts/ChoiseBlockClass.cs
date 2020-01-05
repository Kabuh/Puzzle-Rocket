using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiseBlockClass : MonoBehaviour
{

    [SerializeField] private GameObject blockSprite;
    [SerializeField] private GameObject boosterSprite;

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
    }
}
