using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Heart : MonoBehaviour
{
    public Sprite onHeart;
    public Sprite offHeart;

    public SpriteRenderer sr;
    public int hpNumber;

    private void Update()
    {
        if (GameManager.instance.hp >= hpNumber)
        {
            sr.sprite = onHeart;
        }
        else
        {
            sr.sprite = offHeart;
        }
    }
}
