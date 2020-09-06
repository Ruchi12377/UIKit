using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace UIKit
{
    public sealed class UIKitManager : Singleton<UIKitManager>
    {
        public SetOrCreateCanvasType SetOrCreateCanvas = SetOrCreateCanvasType.Both;
        public enum SetOrCreateCanvasType
        {
            None,
            Dynamic,
            Static,
            Both,
        };

        //作成済みのCanvasを使うか
        public bool UseCreatedDynamicCanvas = false;
        public bool UseCreatedStaticCanvas = false;
        public bool UseCreatedOtherCanvas = false;

        //使う際に設定するフィールド
        public Canvas DynamicCanvas = null;
        public Canvas StaticCanvas = null;
        public List<Canvas> OtherCanvas = new List<Canvas>();

        public static CanvasStock Stock { get; private set; }

        private void Awake()
        {
            //名前の設定
            gameObject.name = "UI_Kit";

            //EventStyleがないなら
            if(GameObject.Find("EventSystem") == null)
            {
                //EventSysytem作成
                EventSystemObject eventSystemObject = new EventSystemObject();

                //EventSystem初期化
                eventSystemObject.Init();
            }

            //Canvas生成
            CanvasObject dynamicCanvas = new CanvasObject();
            CanvasObject staticCanvas = new CanvasObject();

            //オリジナルCanvas生成
            List<CanvasObject> otherCanvas = new List<CanvasObject>();

            //Canvas初期化
            if (UseCreatedDynamicCanvas &&
                (SetOrCreateCanvas == SetOrCreateCanvasType.Dynamic || SetOrCreateCanvas == SetOrCreateCanvasType.Both))
                dynamicCanvas.Init(DynamicCanvas);
            else dynamicCanvas.Init(Creator.CanvasType.Dymanic, "Dynamic");

            if(UseCreatedStaticCanvas &&
                (SetOrCreateCanvas == SetOrCreateCanvasType.Static || SetOrCreateCanvas == SetOrCreateCanvasType.Both))
                staticCanvas.Init(StaticCanvas);
            else staticCanvas.Init(Creator.CanvasType.Static, "Static");

            //オリジナルCanvas初期化
            if (UseCreatedOtherCanvas) OtherCanvas.ForEach(_ => staticCanvas.Init(_));
            else otherCanvas.ForEach(_ => _.Init(Creator.CanvasType.Dymanic, _.canvas.transform.name));

            //Stockに代入
            CanvasStock canvasStock = new CanvasStock();
            canvasStock.DynamicCanvas = dynamicCanvas;
            canvasStock.StaticCanvas = staticCanvas;
            Stock = canvasStock;
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(UIKitManager))]
    public class UIKitManagerEditor : Editor
    {
        private UIKitManager _target;

        private void Awake()
        {
            _target = target as UIKitManager;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            //ScriptField
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            _target.SetOrCreateCanvas = (UIKitManager.SetOrCreateCanvasType)EditorGUILayout.EnumPopup("SetOrCreateCanvas", _target.SetOrCreateCanvas);

            //Dynamic Canvas Field
            if (_target.SetOrCreateCanvas == UIKitManager.SetOrCreateCanvasType.Dynamic ||
                _target.SetOrCreateCanvas == UIKitManager.SetOrCreateCanvasType.Both) 
            {
                _target.UseCreatedDynamicCanvas = EditorGUILayout.ToggleLeft("Use Created Dynamic Canvas", _target.UseCreatedDynamicCanvas);
                if (_target.UseCreatedDynamicCanvas)
                {
                    EditorGUILayout.LabelField("Target Canvas Obejct");
                    _target.DynamicCanvas = EditorGUILayout.ObjectField("DynamicCanvas", _target.DynamicCanvas, typeof(Canvas), true) as Canvas;
                }
            }

            //Static Canvas Field
            if (_target.SetOrCreateCanvas == UIKitManager.SetOrCreateCanvasType.Static ||
                _target.SetOrCreateCanvas == UIKitManager.SetOrCreateCanvasType.Both)
            {
                _target.UseCreatedStaticCanvas = EditorGUILayout.ToggleLeft("Use Created Static Canvas", _target.UseCreatedStaticCanvas);

                if (_target.UseCreatedStaticCanvas)
                {
                    EditorGUILayout.LabelField("Target Canvas Obejct");
                    _target.StaticCanvas = EditorGUILayout.ObjectField("StaticCanvas", _target.StaticCanvas, typeof(Canvas), true) as Canvas;
                }
            }

            //Other Canvas Fields
            _target.UseCreatedOtherCanvas = EditorGUILayout.ToggleLeft("Use Created Other Canvas", _target.UseCreatedOtherCanvas);

            if (_target.UseCreatedOtherCanvas)
            {
                EditorGUILayout.LabelField("Target Canvas Obejcts");
                List<Canvas> list = _target.OtherCanvas;
                int newCount = Mathf.Max(0, EditorGUILayout.IntField("size", list.Count));
                while (newCount < list.Count) list.RemoveAt(list.Count - 1);
                while (newCount > list.Count) list.Add(null);

                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = EditorGUILayout.ObjectField("DynamicCanvas", list[i], typeof(Canvas), true) as Canvas;
                }
            }

            // GUIの更新があったら実行
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_target);
            }
        }
    }
    #endif
}