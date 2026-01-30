using System;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000B42 RID: 2882
public class SelfChargingElectrobank : Electrobank
{
	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x060054CC RID: 21708 RVA: 0x001EF0F2 File Offset: 0x001ED2F2
	public float LifetimeRemaining
	{
		get
		{
			return this.lifetimeRemaining;
		}
	}

	// Token: 0x060054CD RID: 21709 RVA: 0x001EF0FC File Offset: 0x001ED2FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.selectable = base.GetComponent<KSelectable>();
		this.selectable.AddStatusItem(Db.Get().MiscStatusItems.ElectrobankSelfCharging, 60f);
		this.lifetimeStatus = this.selectable.AddStatusItem(Db.Get().MiscStatusItems.ElectrobankLifetimeRemaining, this);
		Components.SelfChargingElectrobanks.Add(base.gameObject.GetMyWorldId(), this);
		if (this.lifetimeRemaining <= 0f)
		{
			this.Delete();
		}
	}

	// Token: 0x060054CE RID: 21710 RVA: 0x001EF18C File Offset: 0x001ED38C
	[OnDeserialized]
	private void OnDeserialized()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component != null)
		{
			component.Mass = 20f;
		}
	}

	// Token: 0x060054CF RID: 21711 RVA: 0x001EF1B4 File Offset: 0x001ED3B4
	public override void Sim200ms(float dt)
	{
		base.Sim200ms(dt);
		if (this.lifetimeRemaining > 0f)
		{
			base.AddPower(dt * 60f);
			this.lifetimeRemaining -= dt;
			return;
		}
		this.Explode();
	}

	// Token: 0x060054D0 RID: 21712 RVA: 0x001EF1F0 File Offset: 0x001ED3F0
	public override void Explode()
	{
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, base.gameObject.transform.position, 0f);
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode", false), base.gameObject.transform.position, 1f);
		base.LaunchNearbyStuff();
		SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.position), SimHashes.NuclearWaste, CellEventLogger.Instance.ElementEmitted, 20f, 3000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.RoundToInt(10000000f), true, -1);
		if (base.transform.parent != null)
		{
			Storage component = base.transform.parent.GetComponent<Storage>();
			if (component != null)
			{
				Health component2 = component.GetComponent<Health>();
				if (component2 != null)
				{
					component2.Damage(500f);
				}
			}
		}
		this.Delete();
	}

	// Token: 0x060054D1 RID: 21713 RVA: 0x001EF2FC File Offset: 0x001ED4FC
	private void Delete()
	{
		if (!this.IsNullOrDestroyed() && !base.gameObject.IsNullOrDestroyed())
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x060054D2 RID: 21714 RVA: 0x001EF31E File Offset: 0x001ED51E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.SelfChargingElectrobanks.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x0400394E RID: 14670
	[Serialize]
	private float lifetimeRemaining = 90000f;

	// Token: 0x0400394F RID: 14671
	private KSelectable selectable;

	// Token: 0x04003950 RID: 14672
	private Guid lifetimeStatus = Guid.Empty;
}
