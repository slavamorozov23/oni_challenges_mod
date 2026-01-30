using System;

namespace FoodRehydrator
{
	// Token: 0x02000F02 RID: 3842
	public class ResourceRequirementMonitor : KMonoBehaviour
	{
		// Token: 0x06007B9F RID: 31647 RVA: 0x00300540 File Offset: 0x002FE740
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Storage[] components = base.GetComponents<Storage>();
			DebugUtil.DevAssert(components.Length == 2, "Incorrect number of storages on foodrehydrator", null);
			this.packages = components[0];
			this.water = components[1];
			base.Subscribe<ResourceRequirementMonitor>(-1697596308, ResourceRequirementMonitor.OnStorageChangedDelegate);
		}

		// Token: 0x06007BA0 RID: 31648 RVA: 0x0030058E File Offset: 0x002FE78E
		protected float GetAvailableWater()
		{
			return this.water.GetMassAvailable(GameTags.Water);
		}

		// Token: 0x06007BA1 RID: 31649 RVA: 0x003005A0 File Offset: 0x002FE7A0
		protected bool HasSufficientResources()
		{
			return this.packages.items.Count > 0 && this.GetAvailableWater() > 1f;
		}

		// Token: 0x06007BA2 RID: 31650 RVA: 0x003005C4 File Offset: 0x002FE7C4
		protected void OnStorageChanged(object _)
		{
			this.operational.SetFlag(ResourceRequirementMonitor.flag, this.HasSufficientResources());
		}

		// Token: 0x0400563E RID: 22078
		[MyCmpReq]
		private Operational operational;

		// Token: 0x0400563F RID: 22079
		private Storage packages;

		// Token: 0x04005640 RID: 22080
		private Storage water;

		// Token: 0x04005641 RID: 22081
		private static readonly Operational.Flag flag = new Operational.Flag("HasSufficientResources", Operational.Flag.Type.Requirement);

		// Token: 0x04005642 RID: 22082
		private static readonly EventSystem.IntraObjectHandler<ResourceRequirementMonitor> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ResourceRequirementMonitor>(delegate(ResourceRequirementMonitor component, object data)
		{
			component.OnStorageChanged(data);
		});
	}
}
