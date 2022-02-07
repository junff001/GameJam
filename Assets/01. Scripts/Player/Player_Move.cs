using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : Player
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float power = 20f;

    protected override void Start()
    {
        EventManager.AddEvent("JUMP", Jump);
    }

    protected override void FixedUpdate()
    {

    }

    protected override void Update()
    {
        Run();
    }

    void Run()
    {
        rigid.velocity = new Vector2(Vector2.right.x * speed, rigid.velocity.y);
    }

    void Jump()
    {
        rigid.AddForce(Vector2.up * power, ForceMode2D.Impulse);
    }

    void OnDestroy()
    {
        EventManager.RemoveEvent("JUMP");
    }
}
