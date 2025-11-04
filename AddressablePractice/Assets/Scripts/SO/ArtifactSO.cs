using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RarityType
{
    Common,
    UnCommon,
    Rare
}

[Serializable]
public class ArtifactSheetData : BaseSheetData
{
    public string Rarity;
}
public class ArtifactSO : BaseSO
{
    public RarityType rarity;

    public override void ApplyData(object sheetData)
    {
        if (sheetData is not ArtifactSheetData data) return;

        ID = data.ID;
        Name = data.Name;
        Description = data.Description;
        SpriteID = data.SpriteID;
        Sprite = null;
        Type = "Artifact";

        if (Enum.TryParse(data.Rarity, true, out RarityType parsed))
        {
            rarity = parsed;
        }
        else
        {
            Debug.LogError($"ArtifactSO : {ID} : 잘못된 RarityType값 {data.Rarity}");
        }
    }
}
