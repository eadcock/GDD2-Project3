using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public RoomManager[] rooms = new RoomManager[2];

    SpriteRenderer sr;

    public bool IsLocked { get; set; }

    [SerializeField]
    private Sprite lockedSprite;
    [SerializeField]
    private Sprite unlockedSprite;

    public Direction direction;

    // Start is called before the first frame update
    void Start()
    {
        IsLocked = false;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lock()
    {
        IsLocked = true;

        sr.sprite = lockedSprite;
    }

    public void Unlock()
    {
        IsLocked = false;

        sr.sprite = unlockedSprite;
    }
}
