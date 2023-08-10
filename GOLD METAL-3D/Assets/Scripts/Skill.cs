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

    private bool isSkill1CoolDown = false;

    private float currentSkill1CoolDown;
    // Start is called before the first frame update
    void Start()
    {
        SkillImage1.fillAmount = 0;

        SkillText1.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Skill1Input();

        SkillCoolDown(ref currentSkill1CoolDown, skillCoolDown, ref isSkill1CoolDown, SkillImage1, SkillText1);
    }

    private void Skill1Input()
    {
        if(Input.GetKeyDown(skillKey) && !isSkill1CoolDown)
        {
            isSkill1CoolDown = true;
            currentSkill1CoolDown = skillCoolDown;
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
