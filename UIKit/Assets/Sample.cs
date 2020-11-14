using UnityEngine;
using UIKit;
using System.Collections.Generic;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        Creator.CreateButton(UIKitManager.Stock.TryGetDynamicCanvas(), "Sample");
    }
}
