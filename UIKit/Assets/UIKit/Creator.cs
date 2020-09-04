using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UIKit
{
    public static class Creator
    {
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

        public enum CanvasType
        {
            Dymanic,
            Static
        };

        public static EventSystemObject CreateEventSystem()
        {
            GameObject go = new GameObject();
            go.transform.name = "EventSystem";
            EventSystem es = go.AddComponent<EventSystem>();
            StandaloneInputModule sim = go.AddComponent<StandaloneInputModule>();
            EventSystemObject eso = new EventSystemObject(es, sim);
            return eso;
        }

        public static Image CreateImage(Canvas canvas, string name)
        {
            GameObject go = new GameObject();
            go.name = name;
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.SetParent(canvas.transform);
            Image img = go.AddComponent<Image>();
            img.rectTransform.localPosition = Vector3.zero;
            return img;
        }
    }
}
