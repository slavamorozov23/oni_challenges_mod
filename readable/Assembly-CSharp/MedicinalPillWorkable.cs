using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020009FD RID: 2557
[AddComponentMenu("KMonoBehaviour/Workable/MedicinalPillWorkable")]
public class MedicinalPillWorkable : Workable, IConsumableUIItem
{
	// Token: 0x06004AAE RID: 19118 RVA: 0x001B0580 File Offset: 0x001AE780
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		this.showProgressBar = false;
		this.synchronizeAnims = false;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		this.CreateChore();
	}

	// Token: 0x06004AAF RID: 19119 RVA: 0x001B05E0 File Offset: 0x001AE7E0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			EffectInstance effectInstance = component.Get(this.pill.info.effect);
			if (effectInstance != null)
			{
				effectInstance.timeRemaining = effectInstance.effect.duration;
			}
			else
			{
				component.Add(this.pill.info.effect, true);
			}
		}
		Sicknesses sicknesses = worker.GetSicknesses();
		foreach (string id in this.pill.info.curedSicknesses)
		{
			SicknessInstance sicknessInstance = sicknesses.Get(id);
			if (sicknessInstance != null)
			{
				Game.Instance.savedInfo.curedDisease = true;
				sicknessInstance.Cure();
			}
		}
		foreach (string effect_id in this.pill.info.curedEffects)
		{
			if (component.HasEffect(effect_id))
			{
				Game.Instance.savedInfo.curedDisease = true;
				component.Remove(effect_id);
			}
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x06004AB0 RID: 19120 RVA: 0x001B073C File Offset: 0x001AE93C
	private void CreateChore()
	{
		new TakeMedicineChore(this);
	}

	// Token: 0x06004AB1 RID: 19121 RVA: 0x001B0748 File Offset: 0x001AE948
	public bool CanBeTakenBy(GameObject consumer)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = consumer.GetComponent<Effects>();
			if (component == null || component.HasEffect(this.pill.info.effect))
			{
				return false;
			}
		}
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.Booster)
		{
			return true;
		}
		Sicknesses sicknesses = consumer.GetSicknesses();
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.CureAny && sicknesses.Count > 0)
		{
			return true;
		}
		foreach (SicknessInstance sicknessInstance in sicknesses)
		{
			if (this.pill.info.curedSicknesses.Contains(sicknessInstance.modifier.Id))
			{
				return true;
			}
		}
		Effects component2 = consumer.GetComponent<Effects>();
		for (int i = 0; i < this.pill.info.curedEffects.Count; i++)
		{
			string effect_id = this.pill.info.curedEffects[i];
			if (component2.HasEffect(effect_id))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x06004AB2 RID: 19122 RVA: 0x001B0880 File Offset: 0x001AEA80
	public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x06004AB3 RID: 19123 RVA: 0x001B089B File Offset: 0x001AEA9B
	public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x06004AB4 RID: 19124 RVA: 0x001B08A3 File Offset: 0x001AEAA3
	public int MajorOrder
	{
		get
		{
			return (int)(this.pill.info.medicineType + 1000);
		}
	}

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06004AB5 RID: 19125 RVA: 0x001B08BB File Offset: 0x001AEABB
	public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x06004AB6 RID: 19126 RVA: 0x001B08BE File Offset: 0x001AEABE
	public bool Display
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0400317D RID: 12669
	public MedicinalPill pill;
}
