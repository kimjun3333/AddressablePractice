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

    public abstract void ApplyData(object sheetData); //런타임 갱신용 매서드
}
