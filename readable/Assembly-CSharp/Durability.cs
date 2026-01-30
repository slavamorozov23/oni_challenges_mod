using System;
using Klei.CustomSettings;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000904 RID: 2308
[AddComponentMenu("KMonoBehaviour/scripts/Durability")]
public class Durability : KMonoBehaviour
{
	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x06004023 RID: 16419 RVA: 0x00169014 File Offset: 0x00167214
	// (set) Token: 0x06004024 RID: 16420 RVA: 0x0016901C File Offset: 0x0016721C
	public float TimeEquipped
	{
		get
		{
			return this.timeEquipped;
		}
		set
		{
			this.timeEquipped = value;
		}
	}

	// Token: 0x06004025 RID: 16421 RVA: 0x00169025 File Offset: 0x00167225
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Durability>(-1617557748, Durability.OnEquippedDelegate);
		base.Subscribe<Durability>(-170173755, Durability.OnUnequippedDelegate);
	}

	// Token: 0x06004026 RID: 16422 RVA: 0x00169050 File Offset: 0x00167250
	protected override void OnSpawn()
	{
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.Durability, base.gameObject);
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Durability);
		if (currentQualitySetting != null)
		{
			string id = currentQualitySetting.id;
			if (id == "Indestructible")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.INDESTRUCTIBLE_DURABILITY_MOD;
				return;
			}
			if (id == "Reinforced")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.REINFORCED_DURABILITY_MOD;
				return;
			}
			if (id == "Flimsy")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.FLIMSY_DURABILITY_MOD;
				return;
			}
			if (!(id == "Threadbare"))
			{
				return;
			}
			this.difficultySettingMod = EQUIPMENT.SUITS.THREADBARE_DURABILITY_MOD;
		}
	}

	// Token: 0x06004027 RID: 16423 RVA: 0x001690FC File Offset: 0x001672FC
	private void OnEquipped()
	{
		if (!this.isEquipped)
		{
			this.isEquipped = true;
			this.timeEquipped = GameClock.Instance.GetTimeInCycles();
		}
	}

	// Token: 0x06004028 RID: 16424 RVA: 0x00169120 File Offset: 0x00167320
	private void OnUnequipped()
	{
		if (this.isEquipped)
		{
			this.isEquipped = false;
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			this.DeltaDurability(num * this.durabilityLossPerCycle);
		}
	}

	// Token: 0x06004029 RID: 16425 RVA: 0x0016915C File Offset: 0x0016735C
	private void DeltaDurability(float delta)
	{
		delta *= this.difficultySettingMod;
		this.durability = Mathf.Clamp01(this.durability + delta);
	}

	// Token: 0x0600402A RID: 16426 RVA: 0x0016917C File Offset: 0x0016737C
	public void ConvertToWornObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.wornEquipmentPrefabID), Grid.SceneLayer.Ore, null, 0);
		gameObject.transform.SetPosition(base.transform.GetPosition());
		gameObject.GetComponent<PrimaryElement>().SetElement(base.GetComponent<PrimaryElement>().ElementID, false);
		gameObject.SetActive(true);
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component != null)
		{
			gameObject.GetComponent<RepairableEquipment>().facadeID = component.FacadeID;
		}
		Storage component2 = base.gameObject.GetComponent<Storage>();
		if (component2)
		{
			JetSuitTank component3 = base.gameObject.GetComponent<JetSuitTank>();
			if (component3)
			{
				component2.AddLiquid((component3.lastFuelUsed != SimHashes.Vacuum) ? component3.lastFuelUsed : SimHashes.Petroleum, component3.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			}
			component2.DropAll(false, false, default(Vector3), true, null);
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x0600402B RID: 16427 RVA: 0x0016927C File Offset: 0x0016747C
	public float GetDurability()
	{
		if (this.isEquipped)
		{
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			return this.durability - num * this.durabilityLossPerCycle;
		}
		return this.durability;
	}

	// Token: 0x0600402C RID: 16428 RVA: 0x001692B9 File Offset: 0x001674B9
	public bool IsWornOut()
	{
		return this.GetDurability() <= 0f;
	}

	// Token: 0x040027BC RID: 10172
	private static readonly EventSystem.IntraObjectHandler<Durability> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnEquipped();
	});

	// Token: 0x040027BD RID: 10173
	private static readonly EventSystem.IntraObjectHandler<Durability> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnUnequipped();
	});

	// Token: 0x040027BE RID: 10174
	[Serialize]
	private bool isEquipped;

	// Token: 0x040027BF RID: 10175
	[Serialize]
	private float timeEquipped;

	// Token: 0x040027C0 RID: 10176
	[Serialize]
	private float durability = 1f;

	// Token: 0x040027C1 RID: 10177
	public float durabilityLossPerCycle = -0.1f;

	// Token: 0x040027C2 RID: 10178
	public string wornEquipmentPrefabID;

	// Token: 0x040027C3 RID: 10179
	private float difficultySettingMod = 1f;
}
