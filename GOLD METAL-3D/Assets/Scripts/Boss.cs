using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public GameObject Rock;
    public Transform missilePortA;
    public Transform missilePortB;
    public Transform ThrowPos;
    public bool isLook;
    Vector3 lookVec;
    Vector3 tauntVec;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think());
    }

    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(tauntVec);
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            // break문 생략하여 확률조정
            case 0:
            case 1:
                //StartCoroutine(MissileShot());
                // 미사일 발사 패턴
            case 2:
            case 3:
                //StartCoroutine(RockShot());
                // 돌 굴러가는 패턴
            case 4:
                StartCoroutine(ThrowAttack());
                //StartCoroutine(Taunt());
                //StartCoroutine(ComboAttack());
                // 점프 공격 패턴
                break;
        }
    }

    IEnumerator MissileShot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileB.target = target;
        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }

    IEnumerator ThrowAttack()
    {
        // **** 수정해야함

        isLook = true;
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantRock = Instantiate(Rock, ThrowPos.position, ThrowPos.rotation);
        Rigidbody rigidRock = instantRock.GetComponent<Rigidbody>();
        rigidRock.AddForce(target.position, ForceMode.Impulse);
        //rigidRock.AddTorque(Vector3.back * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(2f);
        isLook = false;

        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        isLook = false;
        anim.SetTrigger("doBigShot");
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        // **** 높이 수정해야함

        tauntVec = target.position + lookVec;
        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);

        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }

    IEnumerator ComboAttack()
    {
        //** collider 추가해야함
        isLook = false;

        anim.SetTrigger("doShout");

        yield return new WaitForSeconds(2.0f);

        isLook = true;


        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("doCombo");
        //nav.isStopped = false;
        meleeArea.enabled = true;
        rigid.velocity = transform.forward * 30;
        yield return new WaitForSeconds(1.0f);
        rigid.velocity = transform.forward * 30;
        yield return new WaitForSeconds(1.0f);
        rigid.velocity = transform.forward * 30;

        yield return new WaitForSeconds(2.0f);
        meleeArea.enabled = false;
        nav.isStopped = true;
        StartCoroutine(Think());
    }
}
