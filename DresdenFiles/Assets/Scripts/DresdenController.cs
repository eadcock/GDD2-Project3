using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using quiet;

public class DresdenController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime;
        transform.position = transform.position.RemoveZDim() + movement * speed;
    }
}
