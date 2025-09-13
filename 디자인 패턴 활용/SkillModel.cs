using System.Collections.Generic;

//��ų ����
public enum eSkillType
{
    None,
    Skill00,
    Skill01,
    Skill02,
}

//SkillModel���� ������ ���� ���� �̺�Ʈ ����
public enum eSkillEventType
{
    LevelUp,
    GradeUp,
    Combine,    
}

//��ų�� ������ ���̺�
public class SkillTable
{
    public string iconKey;
    public string itemName;
}

//��ų�� �÷��̾� ����
public class SkillData
{
    public eSkillType skill;
    public int level;
    public int grade;
    public int amount;

    public SkillData() { }
    public SkillData(eSkillType type)
    {
        skill = type;
        level = 0;
        grade = 0;
        amount = 0;
    }
}

//��ų�� ���� ó���� ����ϴ� Ŭ����(Model�� �ش�)
public class SkillModel : BaseSubject<eSkillEventType>
{
    //�÷��̾��� ���� ���� ��ų ����
    public List<SkillData> skillData;


    public void LevelUp(eSkillType skill)
    {
        //��ų ������ ó��
        Notify(eSkillEventType.LevelUp);
    }         
    public void GradeUp(eSkillType skill)
    {
        //��ų ��޾� ó�� 
        Notify(eSkillEventType.GradeUp);
    }    
    public void Combine(eSkillType skill)
    {
        //��ų �ռ� ó��
        Notify(eSkillEventType.Combine);
    }    

    //�÷��̾� ������ �����ϴ� �Լ�
    public SkillData GetData(eSkillType type)
    {
        var data = skillData.Find(x => x.skill == type);
        if (data == null)
        {
            data = new(type);
            skillData.Add(data);
        }
        return data;
    }
}