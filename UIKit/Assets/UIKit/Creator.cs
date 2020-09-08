using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

namespace UIKit
{
    public static class Creator
    {
        public static Sprite UISprite { get; private set; }
        public static void Init()
        {
            UISprite = Ex.GetResources<Sprite>("UI/Skin/UISprite.psd");
        }

        public static CanvasObject CreateCanvas(CanvasType Type = CanvasType.Dymanic, string Name = "")
        {
            GameObject go = new GameObject();
            go.transform.name = Name + "Canvas";
            if (Type == CanvasType.Static) go.isStatic = true;
            go.layer = LayerMask.NameToLayer("UI");
            Canvas c = go.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler cs = go.AddComponent<CanvasScaler>();
            GraphicRaycaster gr = go.AddComponent<GraphicRaycaster>();
            CanvasObject co = new CanvasObject(c, cs, gr);
            return co;
        }

        public static EventSystemObject CreateEventSystem()
        {
            GameObject go = new GameObject();
            go.transform.name = "EventSystem";
            EventSystem es = go.AddComponent<EventSystem>();
            StandaloneInputModule sim = go.AddComponent<StandaloneInputModule>();
            EventSystemObject eso = new EventSystemObject(es, sim);
            return eso;
        }

        public static Text CreateText(Canvas canvas, string name)
        {
            GameObject go = CreateUIbject(canvas, name);
            Text text = go.AddComponent<Text>();
            text.color = Color.black;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = "New Text";
            text.alignment = TextAnchor.MiddleCenter;
            return text;
        }

        public static Image CreateImage(Canvas canvas, string name)
        {
            Image img;
            try
            {
                GameObject go = CreateUIbject(canvas, name);
                go.transform.localPosition = Vector3.zero;
                img = go == null ? null : go.AddComponent<Image>();
            }
            catch
            {
                return null;
            }
            return img;
        }

        public static Button CreateButton(Canvas canvas, string name)
        {
            Button button = null;
            try
            {
                Image img = CreateImage(canvas, name);
                img.rectTransform.sizeDelta = new Vector2(160, 30);
                img.sprite = UISprite;
                img.type = Image.Type.Sliced;
                button = img.gameObject.AddComponent<Button>();
                Text text = CreateText(canvas, "Text");
                text.text = "Button";
                text.transform.SetParent(img.transform);
                text.rectTransform.anchorMin = Vector2.zero;
                text.rectTransform.anchorMax = Vector2.one;
                text.rectTransform.anchoredPosition = Vector3.zero;
                text.rectTransform.sizeDelta = Vector2.zero;
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
            
            return button;
        }

        private static GameObject CreateUIbject(Canvas canvas, string name)
        {
            if (canvas == null)
            {
                try
                {
                    throw new ArgumentNullException();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                return null;
            }
            GameObject go = new GameObject();
            go.name = name;
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.SetParent(canvas.transform);
            return go;
        }
    }
}
