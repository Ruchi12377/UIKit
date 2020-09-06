using UnityEngine;
using UIKit;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        Creator.CreateButton(UIKitManager.Stock.TryGetDynamicCanvas(), "Sample");
    }
}
