using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
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
        private EventSystemObject EventSystem;
        private CanvasObject DynamicCanvas;
        private CanvasObject StaticCanvas;
        private List<CanvasObject> OtherCanvas;

        public CanvasStock(EventSystemObject eso, CanvasObject dc, CanvasObject sc, int Capacity)
        {
            EventSystem = eso;
            DynamicCanvas = dc;
            StaticCanvas = sc;

            OtherCanvas = new List<CanvasObject>(Capacity);
        }

        public Canvas GetDynamicCanvas()
        {
            Canvas _ = DynamicCanvas.canvas;
            if (_ == null)
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
            return _;
        }

        public Canvas TryGetDynamicCanvas() => DynamicCanvas.canvas;

        public Canvas GetStaticCanvas()
        {
            Canvas _ = StaticCanvas.canvas;
            if (_ == null)
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
            return _;
        }

        public Canvas TryGetStaticCanvas() => StaticCanvas.canvas;

        public EventSystem GetEventSystem()
        {
            EventSystem _ = EventSystem.eventSystem;
            if (_ == null)
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
            return _;
        }

        public EventSystem TryGetEventSystem() => EventSystem.eventSystem;

        //OtheCanvas追加用
        public void AddCanvas(CanvasObject canvasObject)
        {
            OtherCanvas.Add(canvasObject);
        }

        public CanvasObject AddCanvas(Creator.CanvasType type, string name)
        {
            CanvasObject co = Creator.CreateCanvas(type, name);
            OtherCanvas.Add(co);
            return co;
        }
    }

    public enum CanvasType
    {
        Dymanic,
        Static
    };

    public enum UIType
    {
        Text,
        Button,
        Toggle,
        Slider,
        ScrollBar,
        DropDown,
        InputField,
        Panel,
        ScrollView
    };

    public enum SetOrCreateCanvasType
    {
        None,
        Dynamic,
        Static,
        Both,
    };
}