using System;

// Token: 0x02000BFC RID: 3068
public class TravelTubeUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr
{
	// Token: 0x06005C29 RID: 23593 RVA: 0x00215983 File Offset: 0x00213B83
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005C2A RID: 23594 RVA: 0x0021598B File Offset: 0x00213B8B
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.AddLink(cell1, cell2);
	}

	// Token: 0x06005C2B RID: 23595 RVA: 0x0021599E File Offset: 0x00213B9E
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.RemoveLink(cell1, cell2);
	}

	// Token: 0x06005C2C RID: 23596 RVA: 0x002159B1 File Offset: 0x00213BB1
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.travelTubeSystem;
	}
}
