using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

//UI의 이미지, 텍스트 같은 시각을 보유하여 처리하는 클래스
public class SkillInfoView : MonoBehaviour
{
    //UI 컴포넌트 등록
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text nameText;
    [Space]
    [SerializeField] TMP_Text amountText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text gradeText;
    [Space]
    public BaseButton levelUpButton;
    public BaseButton gradeUpButton;
    public BaseButton combineButton;


    //스킬에 대한 고정 정보 UI 세팅
    public void InitView(SkillTable data)
    {
        iconImage.sprite = UILoader.Instance.LoadIcon(data.iconKey);
        nameText.text = data.itemName;            
    }
    //플레이어 정보 UI 세팅
    public void InitInfo(SkillData data)
    {
        amountText.text = data.amount.ToString();
        levelText.text = $"Lv.{data.level}";
        gradeText.text = $"Grade.{data.grade}";
    }

    //그 외 이펙트나 UI 시각 처리 담당
}