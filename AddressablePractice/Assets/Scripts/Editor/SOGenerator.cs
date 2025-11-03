#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public static class SOGenerator
{
    private const string FolderPath = "Assets/SO";

    public static void CreateOrUpdateCardSOs(List<CardSheetData> cards)
    {
        if(!AssetDatabase.IsValidFolder(FolderPath))
        {
            AssetDatabase.CreateFolder("Assets", "SO");
        }

        foreach(var c in cards)
        {
            string assetPath = $"{FolderPath}/{c.ID}.asset";

            CardSO so = AssetDatabase.LoadAssetAtPath<CardSO>(assetPath);

            if(so == null)
            {
                so = ScriptableObject.CreateInstance<CardSO>();
                so.cardName = c.cardName;
                so.damage = c.damage;
                AssetDatabase.CreateAsset(so, assetPath);
                Debug.Log($"SO변환기 : {assetPath}에 생성");
            }
            else
            {
                so.cardName = c.cardName;
                so.damage = c.damage;
                EditorUtility.SetDirty( so );
                Debug.Log($"SO변환기 : 업데이트 {assetPath}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"SO변환기 : CardSO {cards.Count}개 생성/업데이트 완료");
    }
}
#endif