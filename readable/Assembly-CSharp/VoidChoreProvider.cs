using System;
using System.Collections.Generic;

// Token: 0x020004DC RID: 1244
public class VoidChoreProvider : ChoreProvider
{
	// Token: 0x06001AC5 RID: 6853 RVA: 0x000938E8 File Offset: 0x00091AE8
	public static void DestroyInstance()
	{
		VoidChoreProvider.Instance = null;
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000938F0 File Offset: 0x00091AF0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		VoidChoreProvider.Instance = this;
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x000938FE File Offset: 0x00091AFE
	public override void AddChore(Chore chore)
	{
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x00093900 File Offset: 0x00091B00
	public override void RemoveChore(Chore chore)
	{
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x00093902 File Offset: 0x00091B02
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
	}

	// Token: 0x04000F65 RID: 3941
	public static VoidChoreProvider Instance;
}
