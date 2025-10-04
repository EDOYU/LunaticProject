using UnityEngine;
using UnityEngine.EventSystems;

public class 地图拖拽 : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform 限制范围;
    private RectTransform 目标;
    private Vector2 指针偏移量;

    private void Awake()
    {
        限制范围=transform.parent.GetComponent<RectTransform>();
        目标 = GetComponent<RectTransform>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!大地图系统.是可以点击地图事件)return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            目标.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint);
        指针偏移量 = 目标.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!大地图系统.是可以点击地图事件)return;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            目标.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint))
        {
            Vector2 targetPos = localPoint + 指针偏移量;
            targetPos = 范围限制(targetPos);
            目标.anchoredPosition = targetPos;
        }
    }

    private Vector2 范围限制(Vector2 t)
    {
        float dragHalfW  = 目标.rect.width  * 0.5f;
        float dragHalfH  = 目标.rect.height * 0.5f;
        float limitHalfW = 限制范围.rect.width  * 0.5f;
        float limitHalfH = 限制范围.rect.height * 0.5f;
        Vector2 中心 = 限制范围.anchoredPosition;

        float minX = 中心.x + limitHalfW - dragHalfW;
        float maxX = 中心.x - limitHalfW + dragHalfW;
        float minY = 中心.y + limitHalfH - dragHalfH;
        float maxY = 中心.y - limitHalfH + dragHalfH;

        t.x = Mathf.Clamp(t.x, minX, maxX);
        t.y = Mathf.Clamp(t.y, minY, maxY);
        return t;
    }
}
