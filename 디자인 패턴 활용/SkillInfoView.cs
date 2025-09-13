using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

//UI�� �̹���, �ؽ�Ʈ ���� �ð��� �����Ͽ� ó���ϴ� Ŭ����
public class SkillInfoView : MonoBehaviour
{
    //UI ������Ʈ ���
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


    //��ų�� ���� ���� ���� UI ����
    public void InitView(SkillTable data)
    {
        iconImage.sprite = UILoader.Instance.LoadIcon(data.iconKey);
        nameText.text = data.itemName;            
    }
    //�÷��̾� ���� UI ����
    public void InitInfo(SkillData data)
    {
        amountText.text = data.amount.ToString();
        levelText.text = $"Lv.{data.level}";
        gradeText.text = $"Grade.{data.grade}";
    }

    //�� �� ����Ʈ�� UI �ð� ó�� ���
}