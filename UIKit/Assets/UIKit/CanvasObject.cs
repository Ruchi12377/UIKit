using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UIKit
{
    public struct CanvasObject
    {
        public Canvas canvas;
        public CanvasScaler canvasScaler;
        public GraphicRaycaster graphicRaycaster;

        public CanvasObject(Canvas c, CanvasScaler cs, GraphicRaycaster gr)
        {
            canvas = c;
            canvasScaler = cs;
            graphicRaycaster = gr;
        }

        //Canvasの生成&代入
        public void Init(Creator.CanvasType type, string name)
        {
            this = Creator.CreateCanvas(type, name);
        }

        //Canvasの代入
        public void Init(Canvas canvas)
        {
            CanvasObject co = new CanvasObject
            {
                canvas = canvas,
                canvasScaler = canvas.gameObject.GetComponent<CanvasScaler>(),
                graphicRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>()
            };
            this = co;
        }
    }

    public struct EventSystemObject
    {
        public EventSystem eventSystem;
        public StandaloneInputModule standaloneInputModule;

        public EventSystemObject(EventSystem es, StandaloneInputModule sim)
        {
            eventSystem = es;
            standaloneInputModule = sim;
        }

        //EventSystemの生成&代入
        public void Init()
        {
            this = Creator.CreateEventSystem();
        }
    }

    public struct CanvasStock
    {
        public EventSystemObject EventSystem;
        public CanvasObject DynamicCanvas;
        public CanvasObject StaticCanvas;
        public List<CanvasObject> OtherCanvas;

        public CanvasStock(EventSystemObject eso, CanvasObject dc, CanvasObject sc, int Capacity)
        {
            EventSystem = eso;
            DynamicCanvas = dc;
            StaticCanvas = sc;
            //4の倍数に変換
            int _ = Mathf.CeilToInt(Capacity / 4) + 1;
            OtherCanvas = new List<CanvasObject>(_);
        }

        public CanvasObject AddCanvas(Creator.CanvasType type, string name)
        {
            CanvasObject co = Creator.CreateCanvas(type, name);
            OtherCanvas.Add(co);
            return co;
        }
    }
}