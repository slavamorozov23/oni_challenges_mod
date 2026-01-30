using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007FA RID: 2042
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduit")]
public class SolidConduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr
{
	// Token: 0x060036CA RID: 14026 RVA: 0x00134EC8 File Offset: 0x001330C8
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x060036CB RID: 14027 RVA: 0x00134EDE File Offset: 0x001330DE
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x00134EED File Offset: 0x001330ED
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.solidConduitSystem;
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x00134EF9 File Offset: 0x001330F9
	public UtilityNetwork GetNetwork()
	{
		return this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x00134F0C File Offset: 0x0013310C
	public static SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x060036CF RID: 14031 RVA: 0x00134F18 File Offset: 0x00133118
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x060036D0 RID: 14032 RVA: 0x00134F25 File Offset: 0x00133125
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Conveyor, this);
	}

	// Token: 0x060036D1 RID: 14033 RVA: 0x00134F58 File Offset: 0x00133158
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(this);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			SolidConduit.GetFlowManager().EmptyConduit(cell);
		}
		base.OnCleanUp();
	}

	// Token: 0x04002143 RID: 8515
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x04002144 RID: 8516
	private System.Action firstFrameCallback;
}
