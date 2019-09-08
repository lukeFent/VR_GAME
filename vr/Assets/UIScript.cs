using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    //Reload UI
    public Text r_Text;
    GameObject player;
    Color r_color;
    // Start is called before the first frame update
    void Start()
    {
        r_color = r_Text.color;
        player = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {

        r_Text.color = Color.Lerp(r_color, Color.black, Mathf.PingPong(Time.time,1));
    }

}
