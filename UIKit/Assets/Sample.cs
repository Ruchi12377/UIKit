using UnityEngine;
using UIKit;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        Creator.CreateImage(UIKitManager.Instance.Stock.DynamicCanvas.canvas, "Sample");
    }
}
