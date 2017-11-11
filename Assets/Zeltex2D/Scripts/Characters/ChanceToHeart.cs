using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceToHeart : MonoBehaviour
{
    public Sprite HeartSprite;

    private void Start()
    {
        if (Random.Range(1, 100) >= 86)
        {
            GetComponent<SpriteRenderer>().sprite = HeartSprite;
            transform.parent.name = "Blorp";
        }
    }
}
