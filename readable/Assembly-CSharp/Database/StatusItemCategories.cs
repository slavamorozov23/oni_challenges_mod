using System;

namespace Database
{
	// Token: 0x02000F5D RID: 3933
	public class StatusItemCategories : ResourceSet<StatusItemCategory>
	{
		// Token: 0x06007CE4 RID: 31972 RVA: 0x003192B0 File Offset: 0x003174B0
		public StatusItemCategories(ResourceSet parent) : base("StatusItemCategories", parent)
		{
			this.Main = new StatusItemCategory("Main", this, "Main");
			this.Role = new StatusItemCategory("Role", this, "Role");
			this.Power = new StatusItemCategory("Power", this, "Power");
			this.Toilet = new StatusItemCategory("Toilet", this, "Toilet");
			this.Research = new StatusItemCategory("Research", this, "Research");
			this.Hitpoints = new StatusItemCategory("Hitpoints", this, "Hitpoints");
			this.Suffocation = new StatusItemCategory("Suffocation", this, "Suffocation");
			this.WoundEffects = new StatusItemCategory("WoundEffects", this, "WoundEffects");
			this.EntityReceptacle = new StatusItemCategory("EntityReceptacle", this, "EntityReceptacle");
			this.PreservationState = new StatusItemCategory("PreservationState", this, "PreservationState");
			this.PreservationTemperature = new StatusItemCategory("PreservationTemperature", this, "PreservationTemperature");
			this.PreservationAtmosphere = new StatusItemCategory("PreservationAtmosphere", this, "PreservationAtmosphere");
			this.ExhaustTemperature = new StatusItemCategory("ExhaustTemperature", this, "ExhaustTemperature");
			this.OperatingEnergy = new StatusItemCategory("OperatingEnergy", this, "OperatingEnergy");
			this.AccessControl = new StatusItemCategory("AccessControl", this, "AccessControl");
			this.RequiredRoom = new StatusItemCategory("RequiredRoom", this, "RequiredRoom");
			this.Yield = new StatusItemCategory("Yield", this, "Yield");
			this.Heat = new StatusItemCategory("Heat", this, "Heat");
			this.Stored = new StatusItemCategory("Stored", this, "Stored");
			this.Ownable = new StatusItemCategory("Ownable", this, "Ownable");
		}

		// Token: 0x04005BA5 RID: 23461
		public StatusItemCategory Main;

		// Token: 0x04005BA6 RID: 23462
		public StatusItemCategory Role;

		// Token: 0x04005BA7 RID: 23463
		public StatusItemCategory Power;

		// Token: 0x04005BA8 RID: 23464
		public StatusItemCategory Toilet;

		// Token: 0x04005BA9 RID: 23465
		public StatusItemCategory Research;

		// Token: 0x04005BAA RID: 23466
		public StatusItemCategory Hitpoints;

		// Token: 0x04005BAB RID: 23467
		public StatusItemCategory Suffocation;

		// Token: 0x04005BAC RID: 23468
		public StatusItemCategory WoundEffects;

		// Token: 0x04005BAD RID: 23469
		public StatusItemCategory EntityReceptacle;

		// Token: 0x04005BAE RID: 23470
		public StatusItemCategory PreservationState;

		// Token: 0x04005BAF RID: 23471
		public StatusItemCategory PreservationAtmosphere;

		// Token: 0x04005BB0 RID: 23472
		public StatusItemCategory PreservationTemperature;

		// Token: 0x04005BB1 RID: 23473
		public StatusItemCategory ExhaustTemperature;

		// Token: 0x04005BB2 RID: 23474
		public StatusItemCategory OperatingEnergy;

		// Token: 0x04005BB3 RID: 23475
		public StatusItemCategory AccessControl;

		// Token: 0x04005BB4 RID: 23476
		public StatusItemCategory RequiredRoom;

		// Token: 0x04005BB5 RID: 23477
		public StatusItemCategory Yield;

		// Token: 0x04005BB6 RID: 23478
		public StatusItemCategory Heat;

		// Token: 0x04005BB7 RID: 23479
		public StatusItemCategory Stored;

		// Token: 0x04005BB8 RID: 23480
		public StatusItemCategory Ownable;
	}
}
