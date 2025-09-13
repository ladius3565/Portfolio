using UI;
using UnityEngine;

//UI의 입력과 그에 따른 업데이트 동작을 담당하는 클래스
public class SkillInfoUI : UIPopup, IObserver<eSkillEventType>
{
    [SerializeField] SkillInfoView view;
    private eSkillType curType;

    //UI가 최초 생성될 때 초기화
    public override void Init()
    {
        base.Init();                
        //View가 보유한 버튼에 이벤트 등록 처리
        view.levelUpButton.onClick.AddListener(OnClickLevelUp);
        view.gradeUpButton.onClick.AddListener(OnClickGradeUp);
        view.combineButton.onClick.AddListener(OnClickCombine);
    }
    //UI가 열릴 때 이벤트
    public override void Open()
    {
        base.Open();
        GameDM.Instance.Skill.Register(this);
    }
    //UI가 닫힐 때 이벤트
    public override void Close()
    {
        GameDM.Instance.Skill.Remove(this);
        base.Close();        
    }

    //UI가 열릴 때 어떤 스킬에 대한 정보를 보는지 입력 받는 함수
    public void ShowSkill(eSkillType skill)
    {
        curType = skill;

        //UI에서 스킬의 아이콘 이름 등등의 정보를 업데이트
        var table = DataTable.Instance.GetSkillTable(curType);
        view.InitView(table);

        //UI에서 스킬의 플레이어 정보를 업데이트
        var data = GameDM.Instance.Skill.GetData(curType);
        view.InitInfo(data);
    }

    //Model에서 호출 받을 때 업데이트 처리 함수
    public void OnNotify(eSkillEventType type)
    {
        switch (type)
        {
            case eSkillEventType.LevelUp:
                //레벨업 관련 이펙트 처리
                break;            
            case eSkillEventType.GradeUp:
                //등급업 관련 이펙트 처리
                break;
            case eSkillEventType.Combine:
                //합성 관련 이펙트 처리
                break;
        }
        var data = GameDM.Instance.Skill.GetData(curType);
        view.InitInfo(data);
    }

    //버튼 입력에 대한 함수
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