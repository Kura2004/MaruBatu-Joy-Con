using UnityEngine;
using DG.Tweening;

public class ObjectColorChangerWithoutReset : ObjectColorChanger
{
    //protected override void OnMouseEnter()
    //{
    //    if (!isClicked && !GameStateManager.Instance.IsRotating)
    //    {
    //        base.OnMouseEnter();
    //    }
    //}

    //protected override void OnMouseExit()
    //{
    //    if (!isClicked)
    //    {
    //        base.OnMouseExit();
    //    }
    //}

    //public override void OnMouseDown()
    //{
    //    if (!MouseInteractionWithTurnManager.IsInteractionBlocked() && !isClicked)
    //        base.OnMouseDown();
    //}

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

