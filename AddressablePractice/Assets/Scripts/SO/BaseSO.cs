using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO의 베이스가 되는 클래스
/// </summary>
public abstract class BaseSO : ScriptableObject
{
    public string ID;
    public string Name;
    public string Description;
    public string Type; //이부분은 Enum으로 바뀔수도 or Sheet이름으로 할수도 있음

    public string SpriteID;
    public Sprite Sprite;

    public virtual void ApplyBaseData(BaseSheetData baseData)
    {
        if (baseData == null) return;

        ID = baseData.ID;
        Name = baseData.Name;
        Description = baseData.Description;
        SpriteID = baseData.SpriteID;
        Sprite = null; //스프라이트는 Addressable로 시작시 로드하게 => 스프라이트 ID가 유효하지 않은 상황이면 null로 빠지고 있으면 그 이후에 다시 재할당
    }

    public abstract void ApplyData(object sheetData); //런타임 갱신용 매서드
}
