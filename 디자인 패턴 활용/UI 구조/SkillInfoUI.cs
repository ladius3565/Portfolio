using UI;
using UnityEngine;

//UI�� �Է°� �׿� ���� ������Ʈ ������ ����ϴ� Ŭ����
public class SkillInfoUI : UIPopup, IObserver<eSkillEventType>
{
    [SerializeField] SkillInfoView view;
    private eSkillType curType;

    //UI�� ���� ������ �� �ʱ�ȭ
    public override void Init()
    {
        base.Init();                
        //View�� ������ ��ư�� �̺�Ʈ ��� ó��
        view.levelUpButton.onClick.AddListener(OnClickLevelUp);
        view.gradeUpButton.onClick.AddListener(OnClickGradeUp);
        view.combineButton.onClick.AddListener(OnClickCombine);
    }
    //UI�� ���� �� �̺�Ʈ
    public override void Open()
    {
        base.Open();
        GameDM.Instance.Skill.Register(this);
    }
    //UI�� ���� �� �̺�Ʈ
    public override void Close()
    {
        GameDM.Instance.Skill.Remove(this);
        base.Close();        
    }

    //UI�� ���� �� � ��ų�� ���� ������ ������ �Է� �޴� �Լ�
    public void ShowSkill(eSkillType skill)
    {
        curType = skill;

        //UI���� ��ų�� ������ �̸� ����� ������ ������Ʈ
        var table = DataTable.Instance.GetSkillTable(curType);
        view.InitView(table);

        //UI���� ��ų�� �÷��̾� ������ ������Ʈ
        var data = GameDM.Instance.Skill.GetData(curType);
        view.InitInfo(data);
    }

    //Model���� ȣ�� ���� �� ������Ʈ ó�� �Լ�
    public void OnNotify(eSkillEventType type)
    {
        switch (type)
        {
            case eSkillEventType.LevelUp:
                //������ ���� ����Ʈ ó��
                break;            
            case eSkillEventType.GradeUp:
                //��޾� ���� ����Ʈ ó��
                break;
            case eSkillEventType.Combine:
                //�ռ� ���� ����Ʈ ó��
                break;
        }
        var data = GameDM.Instance.Skill.GetData(curType);
        view.InitInfo(data);
    }

    //��ư �Է¿� ���� �Լ�
    private void OnClickLevelUp()
    {
        GameDM.Instance.Skill.LevelUp(curType);
    }    
    private void OnClickGradeUp()
    {
        GameDM.Instance.Skill.GradeUp(curType);
    }
    private void OnClickCombine()
    {
        GameDM.Instance.Skill.Combine(curType);
    }
}