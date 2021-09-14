using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UIKit
{
    public static class Creator
    {
        #region Variables

        public static Sprite UISprite { get; private set; }
        public static Font Arial { get; private set; }
        private static int _uiLayerName;

        public static CanvasCache Cache { get; private set; }
        public static Canvas DefaultCanvas => Cache.GetCanvases().First();

        #endregion

        #region Initialize

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            _uiLayerName = LayerMask.NameToLayer("UI");

            //リソースを取得
            UISprite = Resources.Load<UiData>("UIData").uiSprite;
            Arial = Resources.GetBuiltinResource<Font>("Arial.ttf");

            Assert.IsNotNull(UISprite);
            Assert.IsNotNull(Arial);

            void SetUpCanvasAndEventSystem()
            {
                var eventSystem = GameObject.Find("EventSystem")?.GetComponent<EventSystem>();
                eventSystem ??= CreateEventSystem();
                
                Assert.IsNotNull(eventSystem);

                var canvas = Object.FindObjectOfType<Canvas>();
                canvas ??= CreateCanvas();
                Cache = new CanvasCache(eventSystem, new List<Canvas> {canvas});
            }
            
            //Sceneが変わるとCanvasとかも変わるから
            SceneManager.sceneLoaded += (s, m) =>
            {
                SetUpCanvasAndEventSystem();
            };
        }

        #endregion
        
        #region Canvas

        public static Canvas CreateCanvas(string name = "Canvas")
        {
            var go = new GameObject();
            go.transform.name = name;
            go.layer = LayerMask.NameToLayer("UI");
            var c = go.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            return c;
        }

        #endregion

        #region EventSystem

        public static EventSystem CreateEventSystem()
        {
            var go = new GameObject();
            go.transform.name = "EventSystem";
            var es = go.AddComponent<EventSystem>();
            return es;
        }

        #endregion

        #region Text

        public static Text CreateText(Canvas canvas, string name)
        {
            var go = CreateUIObject(canvas, name);
            var text = go.AddComponent<Text>();
            text.color = Color.black;
            text.font = Arial;
            text.text = "New Text";
            text.alignment = TextAnchor.MiddleCenter;
            return text;
        }

        #endregion

        #region Image

        public static Image CreateImage(Canvas canvas, string name)
        {
            var go = CreateUIObject(canvas, name);
            go.transform.localPosition = Vector3.zero;
            var img = go == null ? null : go.AddComponent<Image>();
            Assert.IsNotNull(img);
            
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
                img.sprite = UISprite;
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
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return button;
        }

        #endregion

        #region Outline

        public static Outline SetOutline(Graphic graphic, Vector2 distance)
        {
            var outline = graphic.gameObject.AddComponent<Outline>();
            outline.effectDistance = distance;
            
            return outline;
        }

        #endregion

        #region UIObject

        private static GameObject CreateUIObject(Canvas canvas, string name)
        {
            Assert.IsNotNull(canvas);

            var go = new GameObject {name = name, layer = _uiLayerName};
            go.transform.SetParent(canvas.transform);
            return go;
        }

        #endregion
    }
}