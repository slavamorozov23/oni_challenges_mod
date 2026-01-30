using System;
using KSerialization;

// Token: 0x02000649 RID: 1609
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class SubstanceSource : KMonoBehaviour
{
	// Token: 0x0600272F RID: 10031 RVA: 0x000E15CA File Offset: 0x000DF7CA
	protected override void OnPrefabInit()
	{
		this.pickupable.SetWorkTime(SubstanceSource.MaxPickupTime);
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x000E15DC File Offset: 0x000DF7DC
	protected override void OnSpawn()
	{
		this.pickupable.SetWorkTime(10f);
	}

	// Token: 0x06002731 RID: 10033
	protected abstract CellOffset[] GetOffsetGroup();

	// Token: 0x06002732 RID: 10034
	protected abstract IChunkManager GetChunkManager();

	// Token: 0x06002733 RID: 10035 RVA: 0x000E15EE File Offset: 0x000DF7EE
	public SimHashes GetElementID()
	{
		return this.primaryElement.ElementID;
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x000E15FC File Offset: 0x000DF7FC
	public Tag GetElementTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.tag;
		}
		return result;
	}

	// Token: 0x06002735 RID: 10037 RVA: 0x000E164C File Offset: 0x000DF84C
	public Tag GetMaterialCategoryTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.GetMaterialCategoryTag();
		}
		return result;
	}

	// Token: 0x0400172B RID: 5931
	private bool enableRefresh;

	// Token: 0x0400172C RID: 5932
	private static readonly float MaxPickupTime = 8f;

	// Token: 0x0400172D RID: 5933
	[MyCmpReq]
	public Pickupable pickupable;

	// Token: 0x0400172E RID: 5934
	[MyCmpReq]
	private PrimaryElement primaryElement;
}
