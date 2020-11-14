using UnityEngine;
using UnityEditor;

public static class Ex
{
    //Build In Assetを取得する
    public static T GetResources<T>(string path) where T : Object
    {
        T _ = null;
#if UNITY_EDITOR
        _ = AssetDatabase.GetBuiltinExtraResource<T>(path);
#else
          _ = Resources.GetBuiltinResource<T>(path);
#endif
        return _;
    }
}
