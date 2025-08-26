using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    public Camera miniMapCamera;
    public RawImage miniMapImage;
    public RectTransform mapIndicator;
    public LayerMask interactableLayer;

    [Header("Settings")]
    public Color highlightColor = Color.yellow;
    public float highlightDuration = 2f;
    public float indicatorMoveSpeed = 10f;

    private Vector3? targetPosition;
    private GameObject lastHighlighted;

    void Update()
    {
        // 平滑移动指示器
        if (targetPosition.HasValue && mapIndicator != null)
        {
            Vector2 targetScreenPos = WorldToMiniMapPosition(targetPosition.Value);
            mapIndicator.anchoredPosition = Vector2.Lerp(
                mapIndicator.anchoredPosition,
                targetScreenPos,
                indicatorMoveSpeed * Time.deltaTime
            );

            // 检查是否到达目标位置
            if (Vector2.Distance(mapIndicator.anchoredPosition, targetScreenPos) < 5f)
            {
                targetPosition = null;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform rectTransform = miniMapImage.rectTransform;
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            // 转换为UV坐标 (0-1)
            Vector2 normalizedPoint = Rect.PointToNormalized(
                rectTransform.rect,
                new Vector2(
                    localPoint.x + rectTransform.rect.width / 2,
                    localPoint.y + rectTransform.rect.height / 2
                )
            );

            // 创建从小地图相机发出的射线
            Ray ray = miniMapCamera.ViewportPointToRay(new Vector3(normalizedPoint.x, normalizedPoint.y, 0));
            RaycastHit hit;

            // 检测点击位置
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                HandleObjectClick(hit.collider.gameObject, hit.point);
            }
            else
            {
                // 如果没击中物体，则使用地面位置
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (groundPlane.Raycast(ray, out distance))
                {
                    Vector3 worldPoint = ray.GetPoint(distance);
                    HandleGroundClick(worldPoint);
                }
            }
        }
    }

    private void HandleObjectClick(GameObject clickedObject, Vector3 position)
    {
        // 移除之前的高亮
        if (lastHighlighted != null)
        {
            var previousRenderer = lastHighlighted.GetComponent<Renderer>();
            if (previousRenderer != null)
            {
                previousRenderer.material.SetColor("_EmissionColor", Color.black);
            }
        }

        // 高亮当前物体
        var renderer = clickedObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.SetColor("_EmissionColor", highlightColor);
            renderer.material.EnableKeyword("_EMISSION");
        }

        // 显示UI指示器
        targetPosition = position;
        if (mapIndicator != null)
        {
            mapIndicator.anchoredPosition = WorldToMiniMapPosition(position);
        }

        // 记录并设置定时取消高亮
        lastHighlighted = clickedObject;
        CancelInvoke("RemoveHighlight");
        Invoke("RemoveHighlight", highlightDuration);

        Debug.Log($"Clicked on: {clickedObject.name}");
    }

    private void HandleGroundClick(Vector3 position)
    {
        targetPosition = position;
        if (mapIndicator != null)
        {
            mapIndicator.anchoredPosition = WorldToMiniMapPosition(position);
        }

        Debug.Log($"Clicked at position: {position}");
    }

    private void RemoveHighlight()
    {
        if (lastHighlighted != null)
        {
            var renderer = lastHighlighted.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.SetColor("_EmissionColor", Color.black);
            }
            lastHighlighted = null;
        }
    }

    private Vector2 WorldToMiniMapPosition(Vector3 worldPosition)
    {
        // 将世界坐标转换为小地图上的屏幕坐标
        Vector3 viewportPoint = miniMapCamera.WorldToViewportPoint(worldPosition);
        Rect mapRect = miniMapImage.rectTransform.rect;

        return new Vector2(
            (viewportPoint.x * mapRect.width) - (mapRect.width / 2),
            (viewportPoint.y * mapRect.height) - (mapRect.height / 2)
        );
    }

    // 在编辑器中可视化点击位置
    void OnDrawGizmosSelected()
    {
        if (targetPosition.HasValue)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPosition.Value, 1f);
            Gizmos.DrawWireSphere(targetPosition.Value, 1.5f);
        }
    }
}