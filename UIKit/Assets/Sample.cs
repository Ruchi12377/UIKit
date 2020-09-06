using UnityEngine;
using UIKit;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        Creator.CreateImage(UIKitManager.Stock.DynamicCanvas.canvas, "Sample");
    }
}
