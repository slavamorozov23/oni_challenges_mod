using System;

// Token: 0x02000C10 RID: 3088
public struct UtilityNetworkGridNode : IEquatable<UtilityNetworkGridNode>
{
	// Token: 0x06005CC1 RID: 23745 RVA: 0x00218E5B File Offset: 0x0021705B
	public bool Equals(UtilityNetworkGridNode other)
	{
		return this.connections == other.connections && this.networkIdx == other.networkIdx;
	}

	// Token: 0x06005CC2 RID: 23746 RVA: 0x00218E7C File Offset: 0x0021707C
	public override bool Equals(object obj)
	{
		return ((UtilityNetworkGridNode)obj).Equals(this);
	}

	// Token: 0x06005CC3 RID: 23747 RVA: 0x00218E9D File Offset: 0x0021709D
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06005CC4 RID: 23748 RVA: 0x00218EAF File Offset: 0x002170AF
	public static bool operator ==(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return x.Equals(y);
	}

	// Token: 0x06005CC5 RID: 23749 RVA: 0x00218EB9 File Offset: 0x002170B9
	public static bool operator !=(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return !x.Equals(y);
	}

	// Token: 0x04003DBD RID: 15805
	public UtilityConnections connections;

	// Token: 0x04003DBE RID: 15806
	public int networkIdx;

	// Token: 0x04003DBF RID: 15807
	public const int InvalidNetworkIdx = -1;
}
