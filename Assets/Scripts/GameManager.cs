using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button bubble;
    public Image map;
    private float basicTimer = 0;

    private void Start()
    {
       
        //Debug.Log(map.sprite.texture.GetPixel((int)targetPos.x * 4, (int)targetPos.y * 4).linear);    //ZISTENIE FARBY NA MAPE
    }
    private void Update()
    {
        basicTimer += Time.deltaTime;
        if (basicTimer >= 2)
        {
            basicTimer = 0;
            Button bubbleInstance = Instantiate(bubble, map.transform);
            
            Vector2 targetPos = new Vector2(Random.Range(0, map.rectTransform.rect.width), Random.Range(0, map.rectTransform.rect.height));
            bubbleInstance.GetComponent<RectTransform>().anchoredPosition = targetPos;
        }
    }
}
