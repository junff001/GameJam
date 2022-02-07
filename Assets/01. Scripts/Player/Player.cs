using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{ 
    protected Rigidbody2D rigid;
    protected BoxCollider2D collider;
    protected SpriteRenderer sprite;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected abstract void Start();
    protected abstract void Update();
    protected abstract void FixedUpdate();
}
