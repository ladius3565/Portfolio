using System.Collections.Generic;

//스킬 종류
public enum eSkillType
{
    None,
    Skill00,
    Skill01,
    Skill02,
}

//SkillModel에서 옵저버 한테 보낼 이벤트 종류
public enum eSkillEventType
{
    LevelUp,
    GradeUp,
    Combine,    
}

//스킬의 데이터 테이블
public class SkillTable
{
    public string iconKey;
    public string itemName;
}

//스킬의 플레이어 정보
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

//스킬의 내부 처리를 담당하는 클래스(Model에 해당)
public class SkillModel : BaseSubject<eSkillEventType>
{
    //플레이어의 보유 중인 스킬 정보
    public List<SkillData> skillData;


    public void LevelUp(eSkillType skill)
    {
        //스킬 레벨업 처리
        Notify(eSkillEventType.LevelUp);
    }         
    public void GradeUp(eSkillType skill)
    {
        //스킬 등급업 처리 
        Notify(eSkillEventType.GradeUp);
    }    
    public void Combine(eSkillType skill)
    {
        //스킬 합성 처리
        Notify(eSkillEventType.Combine);
    }    

    //플레이어 정보를 리턴하는 함수
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