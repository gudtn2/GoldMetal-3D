using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [Header("Skill1")]
    public Image SkillImage1;
    public Text SkillText1;
    public KeyCode skillKey;
    public float skillCoolDown = 5f;

    public Canvas skill1Canvas;
    public Image skill1Indicator;
    public float maxAbility1Distance = 7;

    private bool isSkill1CoolDown = false;

    private float currentSkill1CoolDown;

    private Vector3 position;
    private RaycastHit hit;
    private Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        SkillImage1.fillAmount = 0;

        SkillText1.text = "";
        skill1Indicator.enabled = false;

        skill1Canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Skill1Input();

        SkillCoolDown(ref currentSkill1CoolDown, skillCoolDown, ref isSkill1CoolDown, SkillImage1, SkillText1);

        Ability1Canvas();
    }

    private void Ability1Canvas()
    {
        int layerMask = ~LayerMask.GetMask("Player");

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if(hit.collider.gameObject != this.gameObject)
            {
                position = hit.point;
            }
        }

        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbility1Distance);

        var newHitPos = transform.position + hitPosDir * distance;
        skill1Canvas.transform.position = (newHitPos);
    }

    private void Skill1Input()
    {
        // 스킬 범위 불러오기
        if(Input.GetKeyDown(skillKey) && !isSkill1CoolDown)
        {
            skill1Canvas.enabled = true;
            skill1Indicator.enabled = true;

            Cursor.visible = false;
        }
        // 스킬 실행
        if (skill1Canvas.enabled && Input.GetMouseButtonDown(0))
        {            
            isSkill1CoolDown = true;

            currentSkill1CoolDown = skillCoolDown;

            skill1Canvas.enabled = false;
            skill1Indicator.enabled = false;

            Cursor.visible = true;
        }
    }

    private void SkillCoolDown(ref float currentCoolDown, float maxCoolDown, ref bool isCoolDown, Image skillImage, Text skillText)
    {
        if(isCoolDown)
        {
            currentCoolDown -= Time.deltaTime;

            if (currentCoolDown <= 0f)
            {
                isCoolDown = false;
                currentCoolDown = 0f;

                if(skillImage != null)
                {
                    skillImage.fillAmount = 0f;
                }
                if(skillText != null)
                {
                    skillText.text = "";
                }
            }
            else
            {
                if(skillImage != null)
                {
                    skillImage.fillAmount = currentCoolDown / maxCoolDown;
                }
                if(skillText != null)
                {
                    skillText.text = Mathf.Ceil(currentCoolDown).ToString();
                }
            }
        }
    }
}
