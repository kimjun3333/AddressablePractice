using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class URLContainer
{
    public static string cardURL = "https://opensheet.elk.sh/1A-gVODBB_1QuNV0ko6rmo7rmnOWINJLhM_hyKEv-xks/Card";
    public static string artifactURL = "https://opensheet.elk.sh/1A-gVODBB_1QuNV0ko6rmo7rmnOWINJLhM_hyKEv-xks/Artifact";

    public static readonly Dictionary<Type, (Type dataType, string url)> SheetMap = new()
    {
        { typeof(CardSO), (typeof(CardSheetData), cardURL) },
        { typeof(ArtifactSO), (typeof(ArtifactSheetData), artifactURL) },
    };
}

