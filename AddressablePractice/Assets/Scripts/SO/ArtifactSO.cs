using System;
using UnityEngine;

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

        ApplyBaseData(data);

        Type = "Artifact";

        if (Enum.TryParse(data.Rarity, true, out RarityType parsedRarity))
            rarity = parsedRarity;
        else
            Debug.LogError($"{data.Rarity}은 RarityType 형식에 맞지않습니다.");
    }
}
