using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
// ReSharper disable InvertIf

namespace UIKit
{
    public struct CanvasObject
    {
        public Canvas Canvas;
        public CanvasScaler CanvasScaler;
        public GraphicRaycaster GraphicRaycaster;

        public CanvasObject(Canvas c, CanvasScaler cs, GraphicRaycaster gr)
        {
            Canvas = c;
            CanvasScaler = cs;
            GraphicRaycaster = gr;
        }

        //Canvasの生成&代入
        public void Init(CanvasType type, string name)
        {
            this = Creator.CreateCanvas(type, name);
        }

        //Canvasの代入
        public void Init(Canvas canvas)
        {
            CanvasObject co = default;
            try
            {
                co = new CanvasObject
                {
                    Canvas = canvas,
                    CanvasScaler = canvas.gameObject.GetComponent<CanvasScaler>(),
                    GraphicRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>()
                };
            }
            catch
            {
                this = default;
            }
            this = co;
        }
    }

    public struct EventSystemObject
    {
        public readonly EventSystem EventSystem;
        public StandaloneInputModule StandaloneInputModule;

        public EventSystemObject(EventSystem es, StandaloneInputModule sim)
        {
            EventSystem = es;
            StandaloneInputModule = sim;
        }

        //EventSystemの生成&代入
        public void Init()
        {
            this = Creator.CreateEventSystem();
        }
    }

    public readonly struct CanvasStock
    {
        private readonly EventSystemObject _eventSystem;
        private readonly CanvasObject _dynamicCanvas;
        private readonly CanvasObject _staticCanvas;
        private readonly List<CanvasObject> _otherCanvas;

        public CanvasStock(EventSystemObject eso, CanvasObject dc, CanvasObject sc, int capacity)
        {
            _eventSystem = eso;
            _dynamicCanvas = dc;
            _staticCanvas = sc;

            _otherCanvas = new List<CanvasObject>(capacity);
        }
        
        public CanvasStock(EventSystemObject eso, CanvasObject dc, CanvasObject sc,List<CanvasObject> oc)
        {
            _eventSystem = eso;
            _dynamicCanvas = dc;
            _staticCanvas = sc;

            _otherCanvas = oc;
        }

        public Canvas GetDynamicCanvas()
        {
            var _ = _dynamicCanvas.Canvas;
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

        public Canvas TryGetDynamicCanvas() => _dynamicCanvas.Canvas;

        public Canvas GetStaticCanvas()
        {
            var _ = _staticCanvas.Canvas;
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

        public Canvas TryGetStaticCanvas() => _staticCanvas.Canvas;

        public EventSystem GetEventSystem()
        {
            var _ = _eventSystem.EventSystem;
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

        public EventSystem TryGetEventSystem() => _eventSystem.EventSystem;

        //OtherCanvas追加用
        public void AddCanvas(CanvasObject canvasObject)
        {
            _otherCanvas.Add(canvasObject);
        }

        public CanvasObject AddCanvas(CanvasType type, string name)
        {
            var co = Creator.CreateCanvas(type, name);
            _otherCanvas.Add(co);
            return co;
        }
    }

    [Serializable]
    public struct Childs
    {
        public int nest;
        public Transform transform;
    }

    public enum CanvasType
    {
        Dynamic,
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