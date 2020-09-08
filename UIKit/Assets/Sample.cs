using UnityEngine;
using UIKit;
using System.Collections.Generic;

public class Sample : MonoBehaviour
{
    public List<Childs> Canvases;
    private void Start()
    {
        Creator.CreateButton(UIKitManager.Stock.TryGetDynamicCanvas(), "Sample");
    }
}
