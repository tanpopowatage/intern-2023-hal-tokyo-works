using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCard : MonoBehaviour
{
    public bool pressed = false;
    public bool horverd = false;
    public Vector2 mousePos;

    public GameObject SpawnObject;

    Vector2 InitPos;

    // Start is called before the first frame update
    void Start()
    {
        InitPos = GetComponent<RectTransform>().anchoredPosition;
    }

    

    // Update is called once per frame
    void Update()
    {
        
        Vector2 CardPos = GetComponent<RectTransform>().anchoredPosition;
        mousePos = Input.mousePosition;
        var CardSize = GetComponent<RectTransform>().sizeDelta;
        

        if(CardPos.x + CardSize.x > mousePos.x && CardPos.x - CardSize.x < mousePos.x &&
        CardPos.y + CardSize.y > mousePos.y && CardPos.y - CardSize.y < mousePos.y)
        {
            horverd = true;
        }
        else
        {
            horverd = false;
        }

        if(horverd)
        {
           
            if(Input.GetMouseButton(0))
            {
                press();
            }
            else
            {
                release();
            }

            
        }
        
        if(pressed)
        {
            GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        }
    }

    public void press()
    {
        pressed = true;
    }

    public void release()
    {
        pressed = false;

        if(GetComponent<RectTransform>().anchoredPosition.y > 200)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                 Debug.Log(hit.point);
                 Instantiate(SpawnObject, hit.point, Quaternion.identity);
            }
        }

        GetComponent<RectTransform>().anchoredPosition = InitPos;
    }
}
