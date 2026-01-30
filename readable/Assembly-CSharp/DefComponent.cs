using System;
using UnityEngine;

// Token: 0x0200052B RID: 1323
[Serializable]
public class DefComponent<T> where T : Component
{
	// Token: 0x06001C96 RID: 7318 RVA: 0x0009CDCC File Offset: 0x0009AFCC
	public DefComponent(T cmp)
	{
		this.cmp = cmp;
	}

	// Token: 0x06001C97 RID: 7319 RVA: 0x0009CDDC File Offset: 0x0009AFDC
	public T Get(StateMachine.Instance smi)
	{
		T[] components = this.cmp.GetComponents<T>();
		int num = 0;
		while (num < components.Length && !(components[num] == this.cmp))
		{
			num++;
		}
		return smi.gameObject.GetComponents<T>()[num];
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x0009CE37 File Offset: 0x0009B037
	public static implicit operator DefComponent<T>(T cmp)
	{
		return new DefComponent<T>(cmp);
	}

	// Token: 0x040010D9 RID: 4313
	[SerializeField]
	private T cmp;
}
