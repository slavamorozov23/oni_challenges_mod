using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200090E RID: 2318
public class Electrobank : KMonoBehaviour, ISim1000ms, ISim200ms, IConsumableUIItem, IGameObjectEffectDescriptor
{
	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x06004061 RID: 16481 RVA: 0x0016CE21 File Offset: 0x0016B021
	// (set) Token: 0x06004060 RID: 16480 RVA: 0x0016CE18 File Offset: 0x0016B018
	public string ID { get; private set; }

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x06004062 RID: 16482 RVA: 0x0016CE29 File Offset: 0x0016B029
	public bool IsFullyCharged
	{
		get
		{
			return this.charge == Electrobank.capacity;
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x06004063 RID: 16483 RVA: 0x0016CE38 File Offset: 0x0016B038
	public float Charge
	{
		get
		{
			return this.charge;
		}
	}

	// Token: 0x06004064 RID: 16484 RVA: 0x0016CE40 File Offset: 0x0016B040
	protected override void OnPrefabInit()
	{
		this.ID = base.gameObject.PrefabID().ToString();
		base.Subscribe(748399584, new Action<object>(this.OnCraft));
		base.OnPrefabInit();
	}

	// Token: 0x06004065 RID: 16485 RVA: 0x0016CE8C File Offset: 0x0016B08C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(856640610, new Action<object>(this.ClearHealthBar));
		Components.Electrobanks.Add(base.gameObject.GetMyWorldId(), this);
		this.radiationEmitter = base.GetComponent<RadiationEmitter>();
		this.UpdateRadiationEmitter();
	}

	// Token: 0x06004066 RID: 16486 RVA: 0x0016CEDF File Offset: 0x0016B0DF
	private void OnCraft(object data)
	{
		WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountProduced(this.Charge);
	}

	// Token: 0x06004067 RID: 16487 RVA: 0x0016CEF4 File Offset: 0x0016B0F4
	private void UpdateRadiationEmitter()
	{
		if (this.radiationEmitter == null)
		{
			return;
		}
		bool flag = this.timeSincePowerDrawn < 0.5f;
		this.radiationEmitter.emitRads = (flag ? this.radioactivityTuning : 0f);
		this.radiationEmitter.Refresh();
	}

	// Token: 0x06004068 RID: 16488 RVA: 0x0016CF44 File Offset: 0x0016B144
	private static GameObject Replace(GameObject electrobank, Tag replacement, bool dropFromStorage = false)
	{
		Vector3 position = electrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(replacement), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(electrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = electrobank.GetComponent<Pickupable>().storage;
		if (storage != null)
		{
			storage.Remove(electrobank, true);
		}
		electrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x06004069 RID: 16489 RVA: 0x0016CFC9 File Offset: 0x0016B1C9
	public static GameObject ReplaceEmptyWithCharged(GameObject EmptyElectrobank, bool dropFromStorage = false)
	{
		return Electrobank.Replace(EmptyElectrobank, "Electrobank", dropFromStorage);
	}

	// Token: 0x0600406A RID: 16490 RVA: 0x0016CFDC File Offset: 0x0016B1DC
	public static GameObject ReplaceChargedWithEmpty(GameObject ChargedElectrobank, bool dropFromStorage = false)
	{
		return Electrobank.Replace(ChargedElectrobank, "EmptyElectrobank", dropFromStorage);
	}

	// Token: 0x0600406B RID: 16491 RVA: 0x0016CFEF File Offset: 0x0016B1EF
	public static GameObject ReplaceEmptyWithGarbage(GameObject ChargedElectrobank, bool dropFromStorage = false)
	{
		return Electrobank.Replace(ChargedElectrobank, "GarbageElectrobank", dropFromStorage);
	}

	// Token: 0x0600406C RID: 16492 RVA: 0x0016D004 File Offset: 0x0016B204
	public float AddPower(float joules)
	{
		if (joules < 0f)
		{
			joules = 0f;
		}
		float num = Mathf.Min(joules, Electrobank.capacity - this.charge);
		this.charge += num;
		return num;
	}

	// Token: 0x0600406D RID: 16493 RVA: 0x0016D044 File Offset: 0x0016B244
	public float RemovePower(float joules, bool dropWhenEmpty)
	{
		float num = Mathf.Min(this.charge, joules);
		this.charge -= num;
		if (this.charge <= 0f)
		{
			this.OnEmpty(dropWhenEmpty);
		}
		if (num > 0f)
		{
			this.timeSincePowerDrawn = 0f;
		}
		return num;
	}

	// Token: 0x0600406E RID: 16494 RVA: 0x0016D094 File Offset: 0x0016B294
	protected virtual void OnEmpty(bool dropWhenEmpty)
	{
		if (this.rechargeable)
		{
			Electrobank.ReplaceChargedWithEmpty(base.gameObject, dropWhenEmpty);
			return;
		}
		if (!this.keepEmpty)
		{
			if (this.pickupable.storage != null)
			{
				this.pickupable.storage.Remove(base.gameObject, true);
			}
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x0600406F RID: 16495 RVA: 0x0016D0F4 File Offset: 0x0016B2F4
	public void FullyCharge()
	{
		this.charge = Electrobank.capacity;
	}

	// Token: 0x06004070 RID: 16496 RVA: 0x0016D104 File Offset: 0x0016B304
	public virtual void Explode()
	{
		int num = Grid.PosToCell(base.gameObject.transform.position);
		float num2 = Grid.Temperature[num];
		num2 += this.charge / (Grid.Mass[num] * Grid.Element[num].specificHeatCapacity);
		num2 = Mathf.Clamp(num2, 1f, 9999f);
		SimMessages.ReplaceElement(num, Grid.Element[num].id, CellEventLogger.Instance.SandBoxTool, Grid.Mass[num], num2, Grid.DiseaseIdx[num], Grid.DiseaseCount[num], -1);
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, base.gameObject.transform.position, 0f);
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode", false), base.gameObject.transform.position, 1f);
		if (this.rechargeable)
		{
			Electrobank.ReplaceEmptyWithGarbage(base.gameObject, false);
			return;
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x06004071 RID: 16497 RVA: 0x0016D210 File Offset: 0x0016B410
	protected void LaunchNearbyStuff()
	{
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		Vector3 position = base.transform.position;
		GameScenePartitioner.Instance.GatherEntries((int)position.x - 3, (int)position.y - 3, 6, 6, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
			if (!(gameObject.GetComponent<MinionIdentity>() != null) && !(gameObject.GetComponent<CreatureBrain>() != null) && gameObject.GetDef<RobotAi.Def>() == null)
			{
				Vector2 vector = gameObject.transform.GetPosition() - position;
				vector = vector.normalized;
				vector *= (float)UnityEngine.Random.Range(4, 6);
				vector.y += (float)UnityEngine.Random.Range(2, 4);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				if (GameComps.Gravities.Has(gameObject))
				{
					GameComps.Gravities.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, vector);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06004072 RID: 16498 RVA: 0x0016D360 File Offset: 0x0016B560
	public void Sim1000ms(float dt)
	{
		if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
		{
			return;
		}
		this.EvaluateWaterDamage(dt);
		this.UpdateHealthBar();
	}

	// Token: 0x06004073 RID: 16499 RVA: 0x0016D387 File Offset: 0x0016B587
	public virtual void Sim200ms(float dt)
	{
		this.UpdateRadiationEmitter();
		this.timeSincePowerDrawn = Mathf.Min(this.timeSincePowerDrawn + dt, 10f);
	}

	// Token: 0x06004074 RID: 16500 RVA: 0x0016D3A8 File Offset: 0x0016B5A8
	private void EvaluateWaterDamage(float dt)
	{
		if (Grid.IsValidCell(this.pickupable.cachedCell) && Grid.Element[this.pickupable.cachedCell].HasTag(GameTags.AnyWater) && UnityEngine.Random.Range(1, 101) > 75)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.POWER_BANK_WATER_DAMAGE, base.transform, 1.5f, false);
			this.Damage(UnityEngine.Random.Range(0f, dt));
		}
	}

	// Token: 0x06004075 RID: 16501 RVA: 0x0016D42C File Offset: 0x0016B62C
	public void Damage(float amount)
	{
		Game.Instance.SpawnFX(SpawnFXHashes.ElectrobankDamage, Grid.PosToCell(base.gameObject), 0f);
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_sparks_short", false), base.gameObject.transform.position, 1f);
		this.currentHealth -= amount;
		if (this.healthBar == null)
		{
			this.CreateHealthBar();
		}
		this.healthBar.Update();
		this.lastDamageTime = Time.time;
		if (this.currentHealth <= 0f)
		{
			this.Explode();
		}
	}

	// Token: 0x06004076 RID: 16502 RVA: 0x0016D4C8 File Offset: 0x0016B6C8
	protected override void OnCleanUp()
	{
		this.ClearHealthBar(null);
		Components.Electrobanks.Remove(base.gameObject.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x06004077 RID: 16503 RVA: 0x0016D4ED File Offset: 0x0016B6ED
	public void CreateHealthBar()
	{
		this.healthBar = ProgressBar.CreateProgressBar(base.gameObject, () => this.currentHealth / 10f);
		this.healthBar.SetVisibility(true);
		this.healthBar.barColor = Util.ColorFromHex("CC3333");
	}

	// Token: 0x06004078 RID: 16504 RVA: 0x0016D52D File Offset: 0x0016B72D
	public void UpdateHealthBar()
	{
		if (this.healthBar != null && Time.time - this.lastDamageTime > 5f)
		{
			this.ClearHealthBar(null);
		}
	}

	// Token: 0x06004079 RID: 16505 RVA: 0x0016D557 File Offset: 0x0016B757
	public void ClearHealthBar(object _ = null)
	{
		if (this.healthBar != null)
		{
			Util.KDestroyGameObject(this.healthBar);
			this.healthBar = null;
		}
	}

	// Token: 0x0600407A RID: 16506 RVA: 0x0016D57C File Offset: 0x0016B77C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELECTROBANKS, GameUtil.GetFormattedJoules(this.Charge, "F1", GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELECTROBANKS, GameUtil.GetFormattedJoules(this.Charge, "F1", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x0600407B RID: 16507 RVA: 0x0016D5E8 File Offset: 0x0016B7E8
	public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x0600407C RID: 16508 RVA: 0x0016D603 File Offset: 0x0016B803
	public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x0600407D RID: 16509 RVA: 0x0016D60B File Offset: 0x0016B80B
	public int MajorOrder
	{
		get
		{
			return 500;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x0600407E RID: 16510 RVA: 0x0016D612 File Offset: 0x0016B812
	public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x0600407F RID: 16511 RVA: 0x0016D615 File Offset: 0x0016B815
	public bool Display
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04002804 RID: 10244
	private static float capacity = 120000f;

	// Token: 0x04002805 RID: 10245
	[Serialize]
	private float charge = Electrobank.capacity;

	// Token: 0x04002806 RID: 10246
	private const float MAX_HEALTH = 10f;

	// Token: 0x04002807 RID: 10247
	[Serialize]
	private float currentHealth = 10f;

	// Token: 0x04002808 RID: 10248
	[Serialize]
	private float timeSincePowerDrawn = 0.5f;

	// Token: 0x04002809 RID: 10249
	private const float RADIATION_EMITTER_TIMEOUT = 0.5f;

	// Token: 0x0400280A RID: 10250
	public float radioactivityTuning;

	// Token: 0x0400280B RID: 10251
	private RadiationEmitter radiationEmitter;

	// Token: 0x0400280C RID: 10252
	private float lastDamageTime;

	// Token: 0x0400280D RID: 10253
	public ProgressBar healthBar;

	// Token: 0x0400280E RID: 10254
	public bool rechargeable;

	// Token: 0x0400280F RID: 10255
	public bool keepEmpty;

	// Token: 0x04002810 RID: 10256
	[MyCmpGet]
	private Pickupable pickupable;
}
