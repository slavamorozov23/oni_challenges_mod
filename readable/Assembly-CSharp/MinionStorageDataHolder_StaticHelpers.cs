using System;

// Token: 0x02000609 RID: 1545
public static class MinionStorageDataHolder_StaticHelpers
{
	// Token: 0x0600240D RID: 9229 RVA: 0x000D0975 File Offset: 0x000CEB75
	public static void UpdateData<T>(this MinionStorageDataHolder dataHolderComponent, MinionStorageDataHolder.DataPackData data)
	{
		dataHolderComponent.Internal_UpdateData(typeof(T).ToString(), data);
	}

	// Token: 0x0600240E RID: 9230 RVA: 0x000D098D File Offset: 0x000CEB8D
	public static MinionStorageDataHolder.DataPack GetDataPack<T>(this MinionStorageDataHolder dataHolderComponent)
	{
		return dataHolderComponent.Internal_GetDataPack(typeof(T).ToString());
	}
}
