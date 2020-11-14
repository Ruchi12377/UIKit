using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace UIKit
{
    public static class Creator
    {
        private static Sprite _uiSprite;
        private static Font _arial;
        private static int _uiLayerName;
        public static void Init()
        {
            _uiSprite = Ex.GetResources<Sprite>("UI/Skin/UISprite.psd");
            _arial = Resources.GetBuiltinResource<Font>("Arial.ttf");
            _uiLayerName = LayerMask.NameToLayer("UI");
        }
        
        #region Canvas
        public static CanvasObject CreateCanvas(Canvas canvas)
        {
            var canvasObject = new CanvasObject();
            canvasObject.Init(canvas);
            return canvasObject;
        }

        public static CanvasObject CreateCanvas(CanvasType type = CanvasType.Dynamic, string name = "")
        {
            var go = new GameObject();
            go.transform.name = name + "Canvas";
            if (type == CanvasType.Static) go.isStatic = true;
            go.layer = LayerMask.NameToLayer("UI");
            var c = go.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            var cs = go.AddComponent<CanvasScaler>();
            var gr = go.AddComponent<GraphicRaycaster>();
            var co = new CanvasObject(c, cs, gr);
            return co;
        }
        #endregion

        #region EventSystem
        public static EventSystemObject CreateEventSystem()
        {
            var go = new GameObject();
            go.transform.name = "EventSystem";
            var es = go.AddComponent<EventSystem>();
            var sim = go.AddComponent<StandaloneInputModule>();
            var eso = new EventSystemObject(es, sim);
            return eso;
        }
        #endregion
        
        #region Text
        public static Text CreateText(Canvas canvas, string name)
        {
            var go = CreateUIObject(canvas, name);
            var text = go.AddComponent<Text>();
            text.color = Color.black;
            text.font = _arial;
            text.text = "New Text";
            text.alignment = TextAnchor.MiddleCenter;
            return text;
        }
        #endregion
        
        #region Image
        public static Image CreateImage(Canvas canvas, string name)
        {
            Image img;
            try
            {
                var go = CreateUIObject(canvas, name);
                go.transform.localPosition = Vector3.zero;
                img = go == null ? null : go.AddComponent<Image>();
            }
            catch
            {
                return null;
            }
            return img;
        }
        #endregion

        #region Button
        public static Button CreateButton(Canvas canvas, string name)
        {
            Button button = null;
            try
            {
                var img = CreateImage(canvas, name);
                img.rectTransform.sizeDelta = new Vector2(160, 30);
                img.sprite = _uiSprite;
                img.type = Image.Type.Sliced;
                button = img.gameObject.AddComponent<Button>();
                var text = CreateText(canvas, "Text");
                text.text = "Button";
                text.transform.SetParent(img.transform);
                var rectTransform = text.rectTransform;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.anchoredPosition = Vector3.zero;
                rectTransform.sizeDelta = Vector2.zero;
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
            
            return button;
        }
        #endregion

        #region UIObject
        private static GameObject CreateUIObject(Canvas canvas, string name)
        {
            if (canvas == null) throw new ArgumentNullException(nameof(canvas));

            var go = new GameObject {name = name, layer = _uiLayerName};
            go.transform.SetParent(canvas.transform);
            return go;
        }
        #endregion
    }
}
