using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C45 RID: 3141
public class UIPool<T> where T : MonoBehaviour
{
	// Token: 0x170006FB RID: 1787
	// (get) Token: 0x06005F0F RID: 24335 RVA: 0x0022C0B4 File Offset: 0x0022A2B4
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x170006FC RID: 1788
	// (get) Token: 0x06005F10 RID: 24336 RVA: 0x0022C0C1 File Offset: 0x0022A2C1
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x170006FD RID: 1789
	// (get) Token: 0x06005F11 RID: 24337 RVA: 0x0022C0CE File Offset: 0x0022A2CE
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x06005F12 RID: 24338 RVA: 0x0022C0DD File Offset: 0x0022A2DD
	public UIPool(T prefab)
	{
		this.prefab = prefab;
		this.freeElements = new Stack<T>();
		this.activeElements = new List<T>();
	}

	// Token: 0x06005F13 RID: 24339 RVA: 0x0022C104 File Offset: 0x0022A304
	public T GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		T t;
		if (this.freeElements.Count == 0)
		{
			t = Util.KInstantiateUI<T>(this.prefab.gameObject, instantiateParent, false);
		}
		else
		{
			t = this.freeElements.Pop();
			if (t.transform.parent != instantiateParent)
			{
				t.transform.SetParent((instantiateParent != null) ? instantiateParent.transform : null);
			}
		}
		if (t.gameObject.activeInHierarchy != forceActive)
		{
			t.gameObject.SetActive(forceActive);
		}
		this.activeElements.Add(t);
		return t;
	}

	// Token: 0x06005F14 RID: 24340 RVA: 0x0022C1AC File Offset: 0x0022A3AC
	public void ClearElement(T element)
	{
		if (!this.activeElements.Contains(element))
		{
			global::Debug.LogError(this.freeElements.Contains(element) ? "The element provided is already inactive" : "The element provided does not belong to this pool");
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.transform.SetParent(this.disabledElementParent);
		}
		element.gameObject.SetActive(false);
		this.freeElements.Push(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x06005F15 RID: 24341 RVA: 0x0022C238 File Offset: 0x0022A438
	public void ClearAll()
	{
		for (int i = this.activeElements.Count - 1; i >= 0; i--)
		{
			T t = this.activeElements[i];
			t.gameObject.SetActive(false);
			if (this.disabledElementParent != null)
			{
				t.transform.SetParent(this.disabledElementParent);
			}
			this.freeElements.Push(t);
		}
		this.activeElements.Clear();
	}

	// Token: 0x06005F16 RID: 24342 RVA: 0x0022C2B6 File Offset: 0x0022A4B6
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x06005F17 RID: 24343 RVA: 0x0022C2C4 File Offset: 0x0022A4C4
	public void DestroyAllActive()
	{
		foreach (T t in this.activeElements)
		{
			UnityEngine.Object.Destroy(t.gameObject);
		}
		this.activeElements.Clear();
	}

	// Token: 0x06005F18 RID: 24344 RVA: 0x0022C32C File Offset: 0x0022A52C
	public void DestroyAllFree()
	{
		foreach (T t in this.freeElements)
		{
			UnityEngine.Object.Destroy(t.gameObject);
		}
		this.freeElements.Clear();
	}

	// Token: 0x06005F19 RID: 24345 RVA: 0x0022C394 File Offset: 0x0022A594
	public void ForEachActiveElement(Action<T> predicate)
	{
		for (int i = 0; i < this.activeElements.Count; i++)
		{
			predicate(this.activeElements[i]);
		}
	}

	// Token: 0x06005F1A RID: 24346 RVA: 0x0022C3CC File Offset: 0x0022A5CC
	public void ForEachFreeElement(Action<T> predicate)
	{
		foreach (T obj in this.freeElements)
		{
			predicate(obj);
		}
	}

	// Token: 0x04003F75 RID: 16245
	private T prefab;

	// Token: 0x04003F76 RID: 16246
	private Stack<T> freeElements;

	// Token: 0x04003F77 RID: 16247
	private List<T> activeElements;

	// Token: 0x04003F78 RID: 16248
	public Transform disabledElementParent;
}
