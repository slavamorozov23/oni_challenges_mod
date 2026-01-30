using System;
using UnityEngine;

// Token: 0x0200081E RID: 2078
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TravelTubeBridge")]
public class TravelTubeBridge : KMonoBehaviour, ITravelTubePiece
{
	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x06003869 RID: 14441 RVA: 0x0013BC3E File Offset: 0x00139E3E
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x0600386A RID: 14442 RVA: 0x0013BC4C File Offset: 0x00139E4C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Grid.HasTube[Grid.PosToCell(this)] = true;
		Components.ITravelTubePieces.Add(this);
		base.Subscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate);
		base.Subscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate);
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x0013BCA0 File Offset: 0x00139EA0
	protected override void OnCleanUp()
	{
		base.Unsubscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate, false);
		Grid.HasTube[Grid.PosToCell(this)] = false;
		Components.ITravelTubePieces.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x0013BCF1 File Offset: 0x00139EF1
	private void OnBuildingBroken(object data)
	{
	}

	// Token: 0x0600386D RID: 14445 RVA: 0x0013BCF3 File Offset: 0x00139EF3
	private void OnBuildingFullyRepaired(object data)
	{
	}

	// Token: 0x04002247 RID: 8775
	private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>(delegate(TravelTubeBridge component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04002248 RID: 8776
	private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>(delegate(TravelTubeBridge component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
