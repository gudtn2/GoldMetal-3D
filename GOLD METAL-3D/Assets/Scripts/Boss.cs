using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public GameObject Rock;
    public BoxCollider meleeCollider1;
    public BoxCollider meleeCollider2;
    public BoxCollider meleeCollider3;
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
            // break�� �����Ͽ� Ȯ������
            case 0:
            case 1:
                //StartCoroutine(ThrowAttack());
                //break;
            // �̻��� �߻� ����
            case 2:
            case 3:
                //StartCoroutine(Taunt());
                //break;
            // �� �������� ����
            case 4:
                //StartCoroutine(ComboAttack());
                StartCoroutine(Taunt());

                // ���� ���� ����
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
        // **** �����ؾ���

        isLook = true;
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.6f);
        isLook = false;
        GameObject instantRock = Instantiate(Rock, ThrowPos.position, Quaternion.LookRotation(target.position - lookVec));
        Rigidbody rigidRock = instantRock.GetComponent<Rigidbody>();
        float targetDistance = Vector3.Distance(instantRock.transform.position, target.position);

        // ������ �������� ��ü�� Ÿ�ٿ� �����µ� �ʿ��� �ӵ��� ���
        float projectile_Velocity = targetDistance / (Mathf.Sin(2 * 45.0f * Mathf.Deg2Rad));
        // �ӵ��� XY�� ���
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(45.0f * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(45.0f * Mathf.Deg2Rad);

        // ���� �ð� ���
        float flightDuration = targetDistance / Vx;

        // Ÿ�� �������� �߻�ü�� ȸ��
        instantRock.transform.rotation = Quaternion.LookRotation(target.position - instantRock.transform.position);
        Debug.Log(target.position);
        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            instantRock.transform.Translate(0, (Vy - (9.81f * elapse_time)) * Time.deltaTime, Vx * (Time.deltaTime * 5));

            elapse_time += Time.deltaTime;

            yield return null;
        }
            //rigidRock.velocity = rigidRock.transform.forward * 20;
        rigidRock.AddTorque(Vector3.back * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

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
        // **** ���� �����ؾ���

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
        //** collider �߰��ؾ���
        isLook = false;

        anim.SetTrigger("doShout");

        yield return new WaitForSeconds(2.4f);

        isLook = true;


        yield return new WaitForSeconds(0.1f);
        isLook = false;
        anim.SetTrigger("doCombo");
        //nav.isStopped = false;
        meleeArea.enabled = true;
        tauntVec = target.position + lookVec;
        nav.isStopped = false;
        meleeCollider1.enabled = true;
        //rigid.velocity = transform.forward * 7;
        yield return new WaitForSeconds(1.0f);
        meleeCollider1.enabled = false;
        meleeCollider2.enabled = true;
        //rigid.velocity = transform.forward * 7;
        yield return new WaitForSeconds(1.0f);
        meleeCollider2.enabled = false;
        meleeCollider3.enabled = true;
        //rigid.velocity = transform.forward * 7;
        yield return new WaitForSeconds(1.0f);
        meleeCollider3.enabled = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        yield return new WaitForSeconds(1.0f);
        meleeArea.enabled = false;
        nav.isStopped = true;
        StartCoroutine(Think());
    }
}
