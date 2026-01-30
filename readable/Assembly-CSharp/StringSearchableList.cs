using System;
using System.Collections.Generic;

// Token: 0x02000482 RID: 1154
public class StringSearchableList<T>
{
	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06001863 RID: 6243 RVA: 0x0008834D File Offset: 0x0008654D
	// (set) Token: 0x06001864 RID: 6244 RVA: 0x00088355 File Offset: 0x00086555
	public bool didUseFilter { get; private set; }

	// Token: 0x06001865 RID: 6245 RVA: 0x0008835E File Offset: 0x0008655E
	public StringSearchableList(List<T> allValues, StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.allValues = allValues;
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.filteredValues = new List<T>();
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x0008838A File Offset: 0x0008658A
	public StringSearchableList(StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.allValues = new List<T>();
		this.filteredValues = new List<T>();
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x000883BC File Offset: 0x000865BC
	public void Refilter()
	{
		if (StringSearchableListUtil.ShouldUseFilter(this.filter))
		{
			this.filteredValues.Clear();
			foreach (T t in this.allValues)
			{
				if (!this.shouldFilterOutFn(t, this.filter))
				{
					this.filteredValues.Add(t);
				}
			}
			this.didUseFilter = true;
			return;
		}
		if (this.filteredValues.Count != this.allValues.Count)
		{
			this.filteredValues.Clear();
			this.filteredValues.AddRange(this.allValues);
		}
		this.didUseFilter = false;
	}

	// Token: 0x04000E30 RID: 3632
	public string filter = "";

	// Token: 0x04000E31 RID: 3633
	public List<T> allValues;

	// Token: 0x04000E32 RID: 3634
	public List<T> filteredValues;

	// Token: 0x04000E34 RID: 3636
	public readonly StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn;

	// Token: 0x02001296 RID: 4758
	// (Invoke) Token: 0x060088AC RID: 34988
	public delegate bool ShouldFilterOutFn(T candidateValue, in string filter);
}
