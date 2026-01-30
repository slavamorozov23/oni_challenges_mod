using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020005EC RID: 1516
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InOrbitRequired")]
public class InOrbitRequired : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06002312 RID: 8978 RVA: 0x000CB93C File Offset: 0x000C9B3C
	protected override void OnSpawn()
	{
		WorldContainer myWorld = this.GetMyWorld();
		this.craftModuleInterface = myWorld.GetComponent<CraftModuleInterface>();
		base.OnSpawn();
		bool newInOrbit = this.craftModuleInterface.HasTag(GameTags.RocketNotOnGround);
		this.UpdateFlag(newInOrbit);
		this.craftModuleInterface.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x000CB997 File Offset: 0x000C9B97
	protected override void OnCleanUp()
	{
		if (this.craftModuleInterface != null)
		{
			this.craftModuleInterface.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000CB9C4 File Offset: 0x000C9BC4
	private void OnTagsChanged(object data)
	{
		TagChangedEventData value = ((Boxed<TagChangedEventData>)data).value;
		if (value.tag == GameTags.RocketNotOnGround)
		{
			this.UpdateFlag(value.added);
		}
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000CB9FB File Offset: 0x000C9BFB
	private void UpdateFlag(bool newInOrbit)
	{
		this.operational.SetFlag(InOrbitRequired.inOrbitFlag, newInOrbit);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InOrbitRequired, !newInOrbit, this);
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x000CBA2E File Offset: 0x000C9C2E
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.IN_ORBIT_REQUIRED, UI.BUILDINGEFFECTS.TOOLTIPS.IN_ORBIT_REQUIRED, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04001485 RID: 5253
	[MyCmpReq]
	private Building building;

	// Token: 0x04001486 RID: 5254
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001487 RID: 5255
	public static readonly Operational.Flag inOrbitFlag = new Operational.Flag("in_orbit", Operational.Flag.Type.Requirement);

	// Token: 0x04001488 RID: 5256
	private CraftModuleInterface craftModuleInterface;
}
