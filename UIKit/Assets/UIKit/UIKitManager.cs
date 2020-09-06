using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace UIKit
{
    public sealed class UIKitManager : Singleton<UIKitManager>
    {
        public SetOrCreateCanvasType SetOrCreateCanvas = SetOrCreateCanvasType.Both;

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

            //Creator初期化
            Creator.Init();

            GameObject go = GameObject.Find("EventSystem");
            EventSystem es = go?.GetComponent<EventSystem>();
            EventSystemObject eventSystemObject = new EventSystemObject(es, null);
            //EventStyleがないなら
            if(eventSystemObject.eventSystem == null)
            {
                //EventSystem初期化
                eventSystemObject.Init();
            }
            else
            {
                eventSystemObject.standaloneInputModule = go.GetComponent<StandaloneInputModule>();
            }

            //Canvas
            CanvasObject dynamicCanvas = default;
            CanvasObject staticCanvas = default;
            //オリジナルCanvas
            List<CanvasObject> otherCanvas = new List<CanvasObject>();

            //Canvas初期化
            //作成済みのDynamicCanvasを使う場合
            if (UseCreatedDynamicCanvas && (SetOrCreateCanvas == SetOrCreateCanvasType.Dynamic || SetOrCreateCanvas == SetOrCreateCanvasType.Both))
                dynamicCanvas = CreateOnlyCanvas(DynamicCanvas);
            else if (SetOrCreateCanvas == SetOrCreateCanvasType.Dynamic || SetOrCreateCanvas == SetOrCreateCanvasType.Both)
                dynamicCanvas = CreateOnlyCanvas(Creator.CanvasType.Dymanic, "Dynamic");

            //作成済みのStaticCanvasを使う場合
            if (UseCreatedStaticCanvas && (SetOrCreateCanvas == SetOrCreateCanvasType.Static || SetOrCreateCanvas == SetOrCreateCanvasType.Both))
                staticCanvas = CreateOnlyCanvas(StaticCanvas);
            else if (SetOrCreateCanvas == SetOrCreateCanvasType.Static || SetOrCreateCanvas == SetOrCreateCanvasType.Both)
                staticCanvas = CreateOnlyCanvas(Creator.CanvasType.Static, "Static");

            //オリジナルCanvas初期化
            //作成済みのOtherCanvasを使う場合
            if (UseCreatedOtherCanvas)
            {
                foreach (var _ in OtherCanvas.Select((Value, Index) => new { Value, Index }))
                {
                    otherCanvas[_.Index] = CreateOnlyCanvas(_.Value);
                }
            }
            else
            {
                foreach (var _ in OtherCanvas.Select((Value, Index) => new { Value, Index }))
                {
                    otherCanvas[_.Index] = CreateOnlyCanvas(Creator.CanvasType.Dymanic, _.Value.transform.name);
                }
            }
            //Stockに代入
            CanvasStock canvasStock = new CanvasStock(eventSystemObject, dynamicCanvas, staticCanvas, 0);
            Stock = canvasStock;
        }

        //キャンバス生成
        private static CanvasObject CreateOnlyCanvas(Canvas canvas)
        {
            CanvasObject canvasObject = new CanvasObject();
            canvasObject.Init(canvas);
            return canvasObject;
        }

        private static CanvasObject CreateOnlyCanvas(Creator.CanvasType type, string name)
        {
            CanvasObject canvasObject = new CanvasObject();
            canvasObject.Init(type, name);
            return canvasObject;
        }

        public static CanvasObject CreateCanvas(Canvas canvas)
        {
            CanvasObject canvasObject = new CanvasObject();
            canvasObject.Init(canvas);
            return canvasObject;
        }

        public static CanvasObject CreateCanvas(Creator.CanvasType type, string name)
        {
            CanvasObject canvasObject = new CanvasObject();
            canvasObject.Init(type, name);
            return canvasObject;
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

            _target.SetOrCreateCanvas = (SetOrCreateCanvasType)EditorGUILayout.EnumPopup("SetOrCreateCanvas", _target.SetOrCreateCanvas);

            //Dynamic Canvas Field
            if (_target.SetOrCreateCanvas == SetOrCreateCanvasType.Dynamic ||
                _target.SetOrCreateCanvas == SetOrCreateCanvasType.Both) 
            {
                _target.UseCreatedDynamicCanvas = EditorGUILayout.ToggleLeft("Use Created Dynamic Canvas", _target.UseCreatedDynamicCanvas);
                if (_target.UseCreatedDynamicCanvas)
                {
                    EditorGUILayout.LabelField("Target Canvas Obejct");
                    _target.DynamicCanvas = EditorGUILayout.ObjectField("DynamicCanvas", _target.DynamicCanvas, typeof(Canvas), true) as Canvas;
                }
            }

            //Static Canvas Field
            if (_target.SetOrCreateCanvas == SetOrCreateCanvasType.Static ||
                _target.SetOrCreateCanvas == SetOrCreateCanvasType.Both)
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