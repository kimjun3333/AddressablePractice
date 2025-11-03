using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 매니저 클래스에 사용하는 제네릭 싱글톤 베이스 클래스.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    /// <summary>
    /// 싱글톤 인스턴스 프로퍼티
    /// 없으면 자동으로 찾거나 생성
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindAnyObjectByType(typeof(T));

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);

                    instance = obj.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if(transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
