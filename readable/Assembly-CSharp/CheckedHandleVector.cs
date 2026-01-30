using System;
using System.Collections.Generic;

// Token: 0x0200083F RID: 2111
public class CheckedHandleVector<T> where T : new()
{
	// Token: 0x06003992 RID: 14738 RVA: 0x00141970 File Offset: 0x0013FB70
	public CheckedHandleVector(int initial_size)
	{
		this.handleVector = new HandleVector<T>(initial_size);
		this.isFree = new List<bool>(initial_size);
		for (int i = 0; i < initial_size; i++)
		{
			this.isFree.Add(true);
		}
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x001419C0 File Offset: 0x0013FBC0
	public HandleVector<T>.Handle Add(T item, string debug_info)
	{
		HandleVector<T>.Handle result = this.handleVector.Add(item);
		if (result.index >= this.isFree.Count)
		{
			this.isFree.Add(false);
		}
		else
		{
			this.isFree[result.index] = false;
		}
		int i = this.handleVector.Items.Count;
		while (i > this.debugInfo.Count)
		{
			this.debugInfo.Add(null);
		}
		this.debugInfo[result.index] = debug_info;
		return result;
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x00141A50 File Offset: 0x0013FC50
	public T Release(HandleVector<T>.Handle handle)
	{
		if (this.isFree[handle.index])
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Tried to double free checked handle ",
				handle.index,
				"- Debug info:",
				this.debugInfo[handle.index]
			});
		}
		this.isFree[handle.index] = true;
		return this.handleVector.Release(handle);
	}

	// Token: 0x06003995 RID: 14741 RVA: 0x00141ACF File Offset: 0x0013FCCF
	public T Get(HandleVector<T>.Handle handle)
	{
		return this.handleVector.GetItem(handle);
	}

	// Token: 0x04002342 RID: 9026
	private HandleVector<T> handleVector;

	// Token: 0x04002343 RID: 9027
	private List<string> debugInfo = new List<string>();

	// Token: 0x04002344 RID: 9028
	private List<bool> isFree;
}
