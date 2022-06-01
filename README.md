# Dino Run Remake
**장르** : 캐주얼  
**개발 참여인원** : 2명  
**개발 기간** : 5일  

담당 파트
-------------   
1. 화면 스크롤 
2. 플레이어 점프 
3. 플레이어 충돌 관련 이벤트 구현
4. 플레이어 스탯 매니저 구현
5. Input 매니저 구현 

코드 설명
--------------  
**화면 스크롤**  
![image](https://user-images.githubusercontent.com/71419212/171318258-d0607b6a-8626-4b35-973a-a1fc0cf9e917.png)
SpawnArea()에서 AreaGroup 오브젝트를 이동시키고 AreaGroup의 위치가 startPos의 위치와 같은지 판단합니다. 위치가 같다면 AreaGroup의 위치를 제자리로 이동시킵니다. 
결과적으로 이 SpawnArea()는 씬에 Area 오브젝트 두개를 반복하여 보여주는 역할을 합니다.
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScrollingArea : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private Vector3 startPos;
    private Vector3 firstArea;
    
    void Start()
    {
        startPos = transform.position;
        firstArea = transform.GetChild(0).TransformPoint(transform.GetChild(0).position);
    }

    void Update()
    {
        SpawnArea();
    }

    void SpawnArea()
    {
        transform.position -= Vector3.right * Time.deltaTime * (speed + GameManager.Instance.SpeedUP);

        if (transform.GetChild(1).TransformPoint(transform.GetChild(1).position).x <= firstArea.x)
        {
            transform.position = startPos;
        }
    }
}

```   

  
**플레이어 점프 구현**  
Rigidbody에 AddForce를 가하여 점프를 구현하였습니다. Physics2D.OverlapBox()로 Ground 체크를 하고 jumpCount로 2단 점프 가능여부를 판단합니다.
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Move : Player
{
    [SerializeField] private LayerMask layer;
    private int jumpCount=2;

    protected override void Start()
    {
        base.Start();
        EventManager.AddEvent_Action("JUMP", Jump);
    }

    protected override void Update()
    {
        if (IsGround())
        {
            jumpCount = 1;
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 4.5f), -1.65f);

    }

    void Jump()
    {
        if (jumpCount != 0)
        {
            rigid.velocity = Vector3.zero;
            rigid.AddForce(Vector2.up * PlayerStatsManager.Instance.JumpPower, ForceMode2D.Impulse);
            SoundManager.Instance.GetJump().Play();
            jumpCount--;
        }
    }

    bool IsGround()
    {
        return Physics2D.OverlapBox(collider.bounds.center, collider.bounds.size, 180f, layer);
    }


    void OnDestroy()
    {
        EventManager.RemoveEvent("JUMP");
    }
}

``` 
**플레이어 충돌 관련 이벤트 구현**   
모든 장애물이 갖고 있는 Obstacle 스크립트입니다. 플레이어가 장애물과 충돌 시 OnDamage()가 실행되며 내부에서 PlayerStatsManager의 TakeDamage()가 실행됩니다.
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Vector3 position;

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual float RandomSpeed(float min, float max)
    {
        return Random.Range(min, max);
    }


    protected virtual void Spawn()
    {
        transform.position = position;
    }

    protected virtual void Move(float speed)
    {
        if(GameManager.Instance.gameState == GameManager.GameState.Playing)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    protected virtual void OnDamage()
    {
        PlayerStatsManager.Instance.TakeDamage();
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    protected virtual void Return()
    {
        if(transform.position.x < -10)
        {
            ObjectPool.instacne.ReturnGameObject(this.gameObject);
        }
    }
}

``` 
**플레이어 스탯 매니저 구현**  
싱글톤으로 만들어 체력, 점프력 등을 관리합니다.
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    #region Sigleton
    private static PlayerStatsManager _instance;
    public static PlayerStatsManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                var obj = FindObjectOfType<PlayerStatsManager>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    var playerStatsManager = new GameObject("PlayerStatsManager").AddComponent<PlayerStatsManager>();
                    _instance = playerStatsManager;
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        var objs = FindObjectsOfType<PlayerStatsManager>();
        if (objs.Length != 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [SerializeField] private int hp = 1;
    public int HP { get => hp; }

    [SerializeField] private int maxHp = 3;
    public int MaxHp { get => maxHp; }

    [SerializeField] private float jumpPower = 20f;
    public float JumpPower { get => jumpPower; }

    public void TakeDamage()
    {
        this.hp--;
        if (hp < 1)
        {
            Die();
        }
        ClampHP();
    }
    public void InitHp()
    {
        hp = 1;
    }

    public void TakeHeal(int heal)
    {
        this.hp += heal;
        ClampHP();
    }

    void ClampHP()
    {
        hp = Mathf.Clamp(hp, 0, maxHp);
    }

    void Die()
    {
        GameManager.Instance.gameState = GameManager.GameState.GameOver;
        GameManager.Instance.GameOver();
        GameManager.Instance.InitScore();
        SoundManager.Instance.StopGame();
    }
}

``` 
**Input 매니저 구현**  
입력은 따로 스크립트를 구분하였고 입력 시 EventManager의 담겨있는 이벤트를 실행시킵니다.
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMananger : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            EventManager.TriggerEvent_Action("JUMP");
        }  
    }
}

```
