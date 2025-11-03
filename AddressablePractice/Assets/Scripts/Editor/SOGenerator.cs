#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// GoogleSheetToSOEditor과 세트인 SO 생성/갱신 유틸리티 클래스
/// </summary>
public static class SOGenerator
{
    private const string FolderPath = "Assets/SO"; //SO 파일 저장 경로

    public static void CreateOrUpdateCardSOs(List<CardSheetData> cards)
    {
        if(!AssetDatabase.IsValidFolder(FolderPath)) //폴더 없으면 자동 생성
        {
            AssetDatabase.CreateFolder("Assets", "SO");
        }

        foreach(var c in cards) //구글 시트 각행 순회
        {
            string assetPath = $"{FolderPath}/{c.ID}.asset";

            CardSO so = AssetDatabase.LoadAssetAtPath<CardSO>(assetPath); //해당경로 SO를 불러옴

            if(so == null) //SO없으면 생성
            {
                so = ScriptableObject.CreateInstance<CardSO>();
                so.cardName = c.cardName;
                so.damage = c.damage;
                AssetDatabase.CreateAsset(so, assetPath);
                Debug.Log($"SO변환기 : {assetPath}에 생성");
            }
            else //있으면 갱신 
            {
                so.cardName = c.cardName; 
                so.damage = c.damage;
                EditorUtility.SetDirty( so );
                Debug.Log($"SO변환기 : 업데이트 {assetPath}");
            }
        }

        //모든 변경사항 저장 및 새로고침 
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"SO변환기 : CardSO {cards.Count}개 생성/업데이트 완료.");
    }
}
#endif