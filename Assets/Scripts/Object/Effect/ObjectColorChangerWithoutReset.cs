using UnityEngine;
using DG.Tweening;

public class ObjectColorChangerWithoutReset : ObjectColorChanger
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (isClicked) return;
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (isClicked) return;
        base.OnTriggerExit(other);
    }
}

