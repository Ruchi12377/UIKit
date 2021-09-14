using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace UIKit
{
    [Serializable]
    public readonly struct CanvasCache
    {
        private readonly EventSystem _eventSystem;
        private readonly List<Canvas> _canvases;

        public CanvasCache(EventSystem eventSystem,　List<Canvas> canvases)
        {
            _eventSystem = eventSystem;
            _canvases = canvases;
        }

        public EventSystem GetEventSystem()
        {
            Assert.IsNotNull(_eventSystem);
            return _eventSystem;
        }
        
        public List<Canvas> GetCanvases()
        {
            Assert.IsNotNull(_canvases);
            return _canvases;
        }
    }
}