using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace UIKit
{
    public sealed class UIKitManager : MonoBehaviour
    {
        public SetOrCreateCanvasType setOrCreateCanvas = SetOrCreateCanvasType.Both;

        //作成済みのCanvasを使うか
        [NonEditableInPlay]
        public bool useCreatedDynamicCanvas;
        [NonEditableInPlay]
        public bool useCreatedStaticCanvas;
        [NonEditableInPlay]
        public bool useCreatedOtherCanvas;

        //使う際に設定するフィールド
        [NonEditableInPlay]
        public Canvas dynamicCanvas;
        [NonEditableInPlay]
        public Canvas staticCanvas;
        [NonEditableInPlay]
        public List<Canvas> otherCanvas = new List<Canvas>();

        public static CanvasStock Stock { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            //名前の設定
            gameObject.name = "UI_Kit";

            //Creator初期化
            Creator.Init();

            var go = GameObject.Find("EventSystem");
            var es = go != null ? go.GetComponent<EventSystem>() : null;
            var eventSystemObject = new EventSystemObject(es, null);
            //EventStyleがないなら
            if (eventSystemObject.EventSystem == null)
            {
                //EventSystem初期化
                eventSystemObject.Init();
            }
            else
            {
                eventSystemObject.StandaloneInputModule = go.GetComponent<StandaloneInputModule>();
            }

            //Canvas
            CanvasObject useDynamicCanvas = default;
            CanvasObject useStaticCanvas = default;
            //オリジナルCanvas
            var useOtherCanvas = new List<CanvasObject>(otherCanvas.Count);

            //Canvas初期化
            //作成済みのDynamicCanvasを使う場合
            if (useCreatedDynamicCanvas && (setOrCreateCanvas == SetOrCreateCanvasType.Dynamic || setOrCreateCanvas == SetOrCreateCanvasType.Both))
                useDynamicCanvas = CanvasObjectInit(this.dynamicCanvas);
            else if (setOrCreateCanvas == SetOrCreateCanvasType.Dynamic || setOrCreateCanvas == SetOrCreateCanvasType.Both)
                useDynamicCanvas = CanvasObjectInit(CanvasType.Dynamic, "Dynamic");

            //作成済みのStaticCanvasを使う場合
            if (useCreatedStaticCanvas && (setOrCreateCanvas == SetOrCreateCanvasType.Static || setOrCreateCanvas == SetOrCreateCanvasType.Both))
                useStaticCanvas = CanvasObjectInit(this.staticCanvas);
            else if (setOrCreateCanvas == SetOrCreateCanvasType.Static || setOrCreateCanvas == SetOrCreateCanvasType.Both)
                useStaticCanvas = CanvasObjectInit(CanvasType.Static, "Static");

            //オリジナルCanvas初期化
            //作成済みのOtherCanvasを使う場合
            if (useCreatedOtherCanvas)
            {
                foreach (var _ in otherCanvas.Select((value, index) => new { Value = value, Index = index }))
                {
                    useOtherCanvas[_.Index] = CanvasObjectInit(_.Value);
                }
            }
            else
            {
                foreach (var _ in otherCanvas.Select((value, index) => new { Value = value, Index = index }))
                {
                    useOtherCanvas[_.Index] = CanvasObjectInit(CanvasType.Dynamic, _.Value.transform.name);
                }
            }
            //Stockに代入
            var canvasStock = new CanvasStock(eventSystemObject, useDynamicCanvas, useStaticCanvas, useOtherCanvas);
            Stock = canvasStock;
        }

        //キャンバス生成
        private static CanvasObject CanvasObjectInit(Canvas canvas)
        {
            var canvasObject = new CanvasObject();
            canvasObject.Init(canvas);
            return canvasObject;
        }

        private static CanvasObject CanvasObjectInit(CanvasType type, string name)
        {
            var canvasObject = new CanvasObject();
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

            _target.setOrCreateCanvas = (SetOrCreateCanvasType)EditorGUILayout.EnumPopup("SetOrCreateCanvas", _target.setOrCreateCanvas);

            //Dynamic Canvas Field
            if (_target.setOrCreateCanvas == SetOrCreateCanvasType.Dynamic ||
                _target.setOrCreateCanvas == SetOrCreateCanvasType.Both)
            {
                _target.useCreatedDynamicCanvas = EditorGUILayout.ToggleLeft("Use Created Dynamic Canvas", _target.useCreatedDynamicCanvas);
                if (_target.useCreatedDynamicCanvas)
                {
                    EditorGUILayout.LabelField("Target Canvas Obejct");
                    _target.dynamicCanvas = EditorGUILayout.ObjectField("DynamicCanvas", _target.dynamicCanvas, typeof(Canvas), true) as Canvas;
                }
            }

            //Static Canvas Field
            if (_target.setOrCreateCanvas == SetOrCreateCanvasType.Static ||
                _target.setOrCreateCanvas == SetOrCreateCanvasType.Both)
            {
                _target.useCreatedStaticCanvas = EditorGUILayout.ToggleLeft("Use Created Static Canvas", _target.useCreatedStaticCanvas);

                if (_target.useCreatedStaticCanvas)
                {
                    EditorGUILayout.LabelField("Target Canvas Obejct");
                    _target.staticCanvas = EditorGUILayout.ObjectField("StaticCanvas", _target.staticCanvas, typeof(Canvas), true) as Canvas;
                }
            }

            //Other Canvas Fields
            _target.useCreatedOtherCanvas = EditorGUILayout.ToggleLeft("Use Created Other Canvas", _target.useCreatedOtherCanvas);

            if (_target.useCreatedOtherCanvas)
            {
                EditorGUILayout.LabelField("Target Canvas Obejcts");
                var list = _target.otherCanvas;
                var newCount = Mathf.Max(0, EditorGUILayout.IntField("size", list.Count));
                while (newCount < list.Count) list.RemoveAt(list.Count - 1);
                while (newCount > list.Count) list.Add(null);

                for (var i = 0; i < list.Count; i++)
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