using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000477 RID: 1143
public class ListWithEvents<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060017F0 RID: 6128 RVA: 0x00087666 File Offset: 0x00085866
	public int Count
	{
		get
		{
			return this.internalList.Count;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060017F1 RID: 6129 RVA: 0x00087673 File Offset: 0x00085873
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.internalList).IsReadOnly;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060017F2 RID: 6130 RVA: 0x00087680 File Offset: 0x00085880
	// (remove) Token: 0x060017F3 RID: 6131 RVA: 0x000876B8 File Offset: 0x000858B8
	public event Action<T> onAdd;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060017F4 RID: 6132 RVA: 0x000876F0 File Offset: 0x000858F0
	// (remove) Token: 0x060017F5 RID: 6133 RVA: 0x00087728 File Offset: 0x00085928
	public event Action<T> onRemove;

	// Token: 0x1700006E RID: 110
	public T this[int index]
	{
		get
		{
			return this.internalList[index];
		}
		set
		{
			this.internalList[index] = value;
		}
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x0008777A File Offset: 0x0008597A
	public void Add(T item)
	{
		this.internalList.Add(item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x0008779C File Offset: 0x0008599C
	public void Insert(int index, T item)
	{
		this.internalList.Insert(index, item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x000877C0 File Offset: 0x000859C0
	public void RemoveAt(int index)
	{
		T obj = this.internalList[index];
		this.internalList.RemoveAt(index);
		if (this.onRemove != null)
		{
			this.onRemove(obj);
		}
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x000877FA File Offset: 0x000859FA
	public bool Remove(T item)
	{
		bool flag = this.internalList.Remove(item);
		if (flag && this.onRemove != null)
		{
			this.onRemove(item);
		}
		return flag;
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x0008781F File Offset: 0x00085A1F
	public void Clear()
	{
		while (this.Count > 0)
		{
			this.RemoveAt(0);
		}
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x00087833 File Offset: 0x00085A33
	public int IndexOf(T item)
	{
		return this.internalList.IndexOf(item);
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x00087841 File Offset: 0x00085A41
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.internalList.CopyTo(array, arrayIndex);
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x00087850 File Offset: 0x00085A50
	public bool Contains(T item)
	{
		return this.internalList.Contains(item);
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x0008785E File Offset: 0x00085A5E
	public IEnumerator<T> GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x00087870 File Offset: 0x00085A70
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x04000E20 RID: 3616
	private List<T> internalList = new List<T>();
}
