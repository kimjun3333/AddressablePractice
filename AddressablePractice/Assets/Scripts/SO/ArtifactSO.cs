using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArtifactSheetData : BaseSheetData
{
    public RarityType Rarity;
}
public class ArtifactSO : BaseSO
{
    public RarityType rarity;

    public override void ApplyData(object sheetData)
    {
        if (sheetData is not ArtifactSheetData data) return;

        ApplyBaseData(data);

        rarity = data.Rarity;
        Type = "Artifact";
    }
}
