using UnityEngine;
using UnityEditor;

namespace UIKit
{
    public sealed class UIKitManager : Singleton<UIKitManager>
    {
        public bool UseCreatedCanvas = false;
        public Canvas DynamicCanvas = null;
        public Canvas StaticCanvas = null;

        public CanvasStock Stock { get; private set; }

        private void Awake()
        {
            //名前の設定
            gameObject.name = "UI_Kit";

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

            if (UseCreatedCanvas)
            {
                //Canvas初期化
                dynamicCanvas.Init(DynamicCanvas);
                staticCanvas.Init(StaticCanvas);
            }
            else
            {
                //Canvas初期化
                dynamicCanvas.Init(Creator.CanvasType.Dymanic, "Dynamic");
                staticCanvas.Init(Creator.CanvasType.Static, "Static");
            }

            //Stockに代入
            CanvasStock canvasStock = new CanvasStock();
            canvasStock.DynamicCanvas = dynamicCanvas;
            canvasStock.StaticCanvas = staticCanvas;
            Stock = canvasStock;
        }
    }

    #if UNITY_EDITOR
[CustomEditor(typeof(UIKitManager))]
    public class HogeObjectEditor : Editor
    {
        private UIKitManager _target;

        private void Awake()
        {
            _target = target as UIKitManager;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            _target.UseCreatedCanvas = EditorGUILayout.ToggleLeft("Use Created Canvas", _target.UseCreatedCanvas);
            if (_target.UseCreatedCanvas)
            {
                EditorGUILayout.LabelField("Target Canvas Obejcts");
                _target.DynamicCanvas = EditorGUILayout.ObjectField("DynamicCanvas", _target.DynamicCanvas, typeof(Canvas), true) as Canvas;
                _target.StaticCanvas = EditorGUILayout.ObjectField("StaticCanvas", _target.StaticCanvas, typeof(Canvas), true) as Canvas;
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

