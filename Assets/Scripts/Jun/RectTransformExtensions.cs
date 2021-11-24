using DG.Tweening;
using UnityEngine;

public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,

    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,

    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,

    StretchAll
}

public enum PivotPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottomCenter,
    BottomRight,
}

public static class RectTransformExtensions
{
    public static Tween SetAnchor(this RectTransform source, AnchorPresets allign, float offsetX = 0, float offsetY = 0, float duration = 1, Ease ease = Ease.OutCubic)
    {
        Vector2 anchorMin = Vector2.zero;
        Vector2 anchorMax = Vector2.zero;
        switch (allign)
        {
            case (AnchorPresets.TopLeft):
                {
                    anchorMin = new Vector2(0, 1);
                    anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.TopCenter):
                {
                    anchorMin = new Vector2(0.5f, 1);
                    anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.TopRight):
                {
                    anchorMin = new Vector2(1, 1);
                    anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.MiddleLeft):
                {
                    anchorMin = new Vector2(0, 0.5f);
                    anchorMax = new Vector2(0, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleCenter):
                {
                    anchorMin = new Vector2(0.5f, 0.5f);
                    anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleRight):
                {
                    anchorMin = new Vector2(1, 0.5f);
                    anchorMax = new Vector2(1, 0.5f);
                    break;
                }

            case (AnchorPresets.BottomLeft):
                {
                    anchorMin = new Vector2(0, 0);
                    anchorMax = new Vector2(0, 0);
                    break;
                }
            case (AnchorPresets.BottonCenter):
                {
                    anchorMin = new Vector2(0.5f, 0);
                    anchorMax = new Vector2(0.5f, 0);
                    break;
                }
            case (AnchorPresets.BottomRight):
                {
                    anchorMin = new Vector2(1, 0);
                    anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.HorStretchTop):
                {
                    anchorMin = new Vector2(0, 1);
                    anchorMax = new Vector2(1, 1);
                    break;
                }
            case (AnchorPresets.HorStretchMiddle):
                {
                    anchorMin = new Vector2(0, 0.5f);
                    anchorMax = new Vector2(1, 0.5f);
                    break;
                }
            case (AnchorPresets.HorStretchBottom):
                {
                    anchorMin = new Vector2(0, 0);
                    anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.VertStretchLeft):
                {
                    anchorMin = new Vector2(0, 0);
                    anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.VertStretchCenter):
                {
                    anchorMin = new Vector2(0.5f, 0);
                    anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.VertStretchRight):
                {
                    anchorMin = new Vector2(1, 0);
                    anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.StretchAll):
                {
                    anchorMin = new Vector2(0, 0);
                    anchorMax = new Vector2(1, 1);
                    break;
                }
        }


        source.DOAnchorMin(anchorMin, duration).SetEase(ease);
        source.DOAnchorMax(anchorMax, duration).SetEase(ease);

        return source.DOAnchorPos(new Vector2(offsetX, offsetY), duration).SetEase(ease);
    }

    public static Tween SetPivot(this RectTransform source, PivotPresets preset, float duration = 1, Ease ease = Ease.OutCubic)
    {
        Vector2 pivotVector2 = Vector2.zero;
        switch (preset)
        {
            case (PivotPresets.TopLeft):
                {
                    pivotVector2 = new Vector2(0, 1);
                    break;
                }
            case (PivotPresets.TopCenter):
                {
                    pivotVector2 = new Vector2(0.5f, 1);
                    break;
                }
            case (PivotPresets.TopRight):
                {
                    pivotVector2 = new Vector2(1, 1);
                    break;
                }

            case (PivotPresets.MiddleLeft):
                {
                    pivotVector2 = new Vector2(0, 0.5f);
                    break;
                }
            case (PivotPresets.MiddleCenter):
                {
                    pivotVector2 = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (PivotPresets.MiddleRight):
                {
                    pivotVector2 = new Vector2(1, 0.5f);
                    break;
                }

            case (PivotPresets.BottomLeft):
                {
                    pivotVector2 = new Vector2(0, 0);
                    break;
                }
            case (PivotPresets.BottomCenter):
                {
                    pivotVector2 = new Vector2(0.5f, 0);
                    break;
                }
            case (PivotPresets.BottomRight):
                {
                    pivotVector2 = new Vector2(1, 0);
                    break;
                }
        }
        return source.DOPivot(pivotVector2, duration).SetEase(ease);
    }
}