using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200000D RID: 13
public class DragMe : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x06000034 RID: 52 RVA: 0x00002E8C File Offset: 0x0000108C
	public void OnBeginDrag(PointerEventData eventData)
	{
		Canvas canvas = DragMe.FindInParents<Canvas>(base.gameObject);
		if (canvas == null)
		{
			return;
		}
		this.m_DraggingIcon = UnityEngine.Object.Instantiate<GameObject>(base.gameObject, canvas.transform, false);
		GraphicRaycaster component = this.m_DraggingIcon.GetComponent<GraphicRaycaster>();
		if (component != null)
		{
			component.enabled = false;
		}
		this.m_DraggingIcon.name = "dragObj";
		this.m_DraggingIcon.transform.SetAsLastSibling();
		RectTransform component2 = this.m_DraggingIcon.GetComponent<RectTransform>();
		component2.pivot = Vector2.zero;
		component2.sizeDelta = base.GetComponent<RectTransform>().rect.size;
		this.x = this.m_DraggingIcon.transform.position.x;
		Canvas component3 = this.m_DraggingIcon.GetComponent<Canvas>();
		component3.overrideSorting = true;
		component3.sortingOrder = 99;
		if (this.dragOnSurfaces)
		{
			this.m_DraggingPlane = (base.transform as RectTransform);
		}
		else
		{
			this.m_DraggingPlane = (canvas.transform as RectTransform);
		}
		this.SetDraggedPosition(eventData);
		this.listener.OnBeginDrag(eventData.position);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002FA9 File Offset: 0x000011A9
	public void OnDrag(PointerEventData data)
	{
		if (this.m_DraggingIcon != null)
		{
			this.SetDraggedPosition(data);
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002FC0 File Offset: 0x000011C0
	private void SetDraggedPosition(PointerEventData data)
	{
		if (this.dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
		{
			this.m_DraggingPlane = (data.pointerEnter.transform as RectTransform);
		}
		RectTransform component = this.m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 position;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, data.position, data.pressEventCamera, out position))
		{
			position.x = this.x + 5f;
			position.y -= component.sizeDelta.y / 2f;
			component.position = position;
			component.rotation = this.m_DraggingPlane.rotation;
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0000307F File Offset: 0x0000127F
	public void OnEndDrag(PointerEventData eventData)
	{
		this.listener.OnEndDrag(eventData.position);
		if (this.m_DraggingIcon != null)
		{
			UnityEngine.Object.Destroy(this.m_DraggingIcon);
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x000030AC File Offset: 0x000012AC
	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return default(T);
		}
		T t = default(T);
		Transform parent = go.transform.parent;
		while (parent != null && t == null)
		{
			t = parent.gameObject.GetComponent<T>();
			parent = parent.parent;
		}
		return t;
	}

	// Token: 0x0400003B RID: 59
	public bool dragOnSurfaces = true;

	// Token: 0x0400003C RID: 60
	private GameObject m_DraggingIcon;

	// Token: 0x0400003D RID: 61
	private RectTransform m_DraggingPlane;

	// Token: 0x0400003E RID: 62
	private float x;

	// Token: 0x0400003F RID: 63
	public DragMe.IDragListener listener;

	// Token: 0x02001071 RID: 4209
	public interface IDragListener
	{
		// Token: 0x0600820F RID: 33295
		void OnBeginDrag(Vector2 position);

		// Token: 0x06008210 RID: 33296
		void OnEndDrag(Vector2 position);
	}
}
