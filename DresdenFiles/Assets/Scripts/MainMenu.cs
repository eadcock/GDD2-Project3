using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(Play);
    }

    public void Update()
    {
        
    }
    public void Play()
    {
        Initiate.Fade("SampleScene", Color.white, 1.0f);
    }
}
