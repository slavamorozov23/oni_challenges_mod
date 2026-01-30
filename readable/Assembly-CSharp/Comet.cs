using System;
using System.Collections.Generic;
using FMOD.Studio;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000869 RID: 2153
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Comet")]
public class Comet : KMonoBehaviour, ISim33ms
{
	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06003B10 RID: 15120 RVA: 0x00149603 File Offset: 0x00147803
	public float ExplosionMass
	{
		get
		{
			return this.explosionMass;
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06003B11 RID: 15121 RVA: 0x0014960B File Offset: 0x0014780B
	public float AddTileMass
	{
		get
		{
			return this.addTileMass;
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06003B12 RID: 15122 RVA: 0x00149613 File Offset: 0x00147813
	public Vector3 TargetPosition
	{
		get
		{
			return this.anim.PositionIncludingOffset;
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06003B13 RID: 15123 RVA: 0x00149620 File Offset: 0x00147820
	// (set) Token: 0x06003B14 RID: 15124 RVA: 0x00149628 File Offset: 0x00147828
	public Vector2 Velocity
	{
		get
		{
			return this.velocity;
		}
		set
		{
			this.velocity = value;
		}
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x00149634 File Offset: 0x00147834
	private float GetVolume(GameObject gameObject)
	{
		float result = 1f;
		if (gameObject != null && this.selectable != null && this.selectable.IsSelected)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x00149672 File Offset: 0x00147872
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.remainingTileDamage = this.totalTileDamage;
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Meteor_LP", false);
		this.RandomizeVelocity();
	}

	// Token: 0x06003B17 RID: 15127 RVA: 0x001496B0 File Offset: 0x001478B0
	protected override void OnSpawn()
	{
		this.anim.Offset = this.offsetPosition;
		if (this.spawnWithOffset)
		{
			this.SetupOffset();
		}
		base.OnSpawn();
		this.RandomizeMassAndTemperature();
		this.StartLoopingSound();
		bool flag = this.offsetPosition.x != 0f || this.offsetPosition.y != 0f;
		this.selectable.enabled = !flag;
		this.typeID = base.GetComponent<KPrefabID>().PrefabTag;
		Components.Meteors.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06003B18 RID: 15128 RVA: 0x0014974F File Offset: 0x0014794F
	protected override void OnCleanUp()
	{
		Components.Meteors.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06003B19 RID: 15129 RVA: 0x00149768 File Offset: 0x00147968
	protected void SetupOffset()
	{
		Vector3 position = base.transform.GetPosition();
		Vector3 position2 = base.transform.GetPosition();
		position2.z = 0f;
		Vector3 vector = new Vector3(this.velocity.x, this.velocity.y, 0f);
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		float num = (float)(myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y;
		float f = Vector3.Angle(Vector3.up, -vector) * 0.017453292f;
		float d = Mathf.Abs(num / Mathf.Cos(f));
		Vector3 vector2 = position2 - vector.normalized * d;
		float num2 = (float)(myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
		if (vector2.x < (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters || vector2.x > num2)
		{
			float num3 = (vector.x < 0f) ? (num2 - position2.x) : (position2.x - (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters);
			f = Vector3.Angle((vector.x < 0f) ? Vector3.right : Vector3.left, -vector) * 0.017453292f;
			d = Mathf.Abs(num3 / Mathf.Cos(f));
		}
		Vector3 b = -vector.normalized * d;
		(position2 + b).z = position.z;
		this.offsetPosition = b;
		this.anim.Offset = this.offsetPosition;
	}

	// Token: 0x06003B1A RID: 15130 RVA: 0x0014992C File Offset: 0x00147B2C
	public virtual void RandomizeVelocity()
	{
		float num = UnityEngine.Random.Range(this.spawnAngle.x, this.spawnAngle.y);
		float f = num * 3.1415927f / 180f;
		float num2 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
		this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
		base.GetComponent<KBatchedAnimController>().Rotation = -num - 90f;
	}

	// Token: 0x06003B1B RID: 15131 RVA: 0x001499B0 File Offset: 0x00147BB0
	public void RandomizeMassAndTemperature()
	{
		float num = UnityEngine.Random.Range(this.massRange.x, this.massRange.y) * this.GetMassMultiplier();
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.Mass = num;
		component.Temperature = UnityEngine.Random.Range(this.temperatureRange.x, this.temperatureRange.y);
		if (this.addTiles > 0)
		{
			float num2 = UnityEngine.Random.Range(0.95f, 0.98f);
			this.explosionMass = num * (1f - num2);
			this.addTileMass = num * num2;
			return;
		}
		this.explosionMass = num;
		this.addTileMass = 0f;
	}

	// Token: 0x06003B1C RID: 15132 RVA: 0x00149A54 File Offset: 0x00147C54
	public float GetMassMultiplier()
	{
		float num = 1f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
		if (this.affectedByDifficulty && currentQualitySetting != null)
		{
			string id = currentQualitySetting.id;
			if (!(id == "Infrequent"))
			{
				if (!(id == "Intense"))
				{
					if (id == "Doomed")
					{
						num *= 0.5f;
					}
				}
				else
				{
					num *= 0.8f;
				}
			}
			else
			{
				num *= 1f;
			}
		}
		return num;
	}

	// Token: 0x06003B1D RID: 15133 RVA: 0x00149ACF File Offset: 0x00147CCF
	public int GetRandomNumOres()
	{
		return UnityEngine.Random.Range(this.explosionOreCount.x, this.explosionOreCount.y + 1);
	}

	// Token: 0x06003B1E RID: 15134 RVA: 0x00149AEE File Offset: 0x00147CEE
	public float GetRandomTemperatureForOres()
	{
		return UnityEngine.Random.Range(this.explosionTemperatureRange.x, this.explosionTemperatureRange.y);
	}

	// Token: 0x06003B1F RID: 15135 RVA: 0x00149B0C File Offset: 0x00147D0C
	[ContextMenu("Explode")]
	private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
	{
		int world = (int)Grid.WorldIdx[cell];
		this.PlayImpactSound(pos);
		Vector3 pos2 = pos;
		pos2.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
		if (this.explosionEffectHash != SpawnFXHashes.None)
		{
			Game.Instance.SpawnFX(this.explosionEffectHash, pos2, 0f);
		}
		Substance substance = element.substance;
		int randomNumOres = this.GetRandomNumOres();
		Vector2 vector = -this.velocity.normalized;
		Vector2 a = new Vector2(vector.y, -vector.x);
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)pos.x - 3, (int)pos.y - 3, 6, 6, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
			if (!(gameObject.GetComponent<MinionIdentity>() != null) && !(gameObject.GetComponent<CreatureBrain>() != null) && gameObject.GetDef<RobotAi.Def>() == null)
			{
				Vector2 vector2 = (gameObject.transform.GetPosition() - pos).normalized;
				vector2 += new Vector2(0f, 0.55f);
				vector2 *= 0.5f * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				if (GameComps.Gravities.Has(gameObject))
				{
					GameComps.Gravities.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, vector2);
			}
		}
		pooledList.Recycle();
		int num = this.splashRadius + 1;
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				int num2 = Grid.OffsetCell(cell, j, i);
				if (Grid.IsValidCellInWorld(num2, world) && !this.destroyedCells.Contains(num2))
				{
					float num3 = (1f - (float)Mathf.Abs(j) / (float)num) * (1f - (float)Mathf.Abs(i) / (float)num);
					if (num3 > 0f)
					{
						this.DamageTiles(num2, prev_cell, num3 * this.totalTileDamage * 0.5f);
					}
				}
			}
		}
		float mass = (randomNumOres > 0) ? (this.explosionMass / (float)randomNumOres) : 1f;
		float randomTemperatureForOres = this.GetRandomTemperatureForOres();
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		for (int k = 0; k < randomNumOres; k++)
		{
			Vector2 normalized = (vector + a * UnityEngine.Random.Range(-1f, 1f)).normalized;
			Vector3 v = normalized * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y);
			Vector3 vector3 = normalized.normalized * 0.75f;
			vector3 += new Vector3(0f, 0.55f, 0f);
			vector3 += pos;
			GameObject go = substance.SpawnResource(vector3, mass, randomTemperatureForOres, component.DiseaseIdx, component.DiseaseCount / (randomNumOres + this.addTiles), false, false, false);
			if (GameComps.Fallers.Has(go))
			{
				GameComps.Fallers.Remove(go);
			}
			GameComps.Fallers.Add(go, v);
		}
		if (this.addTiles > 0)
		{
			this.DepositTiles(cell, element, world, prev_cell, randomTemperatureForOres);
		}
		this.SpawnCraterPrefabs();
		if (this.OnImpact != null)
		{
			this.OnImpact();
		}
	}

	// Token: 0x06003B20 RID: 15136 RVA: 0x00149EF0 File Offset: 0x001480F0
	protected virtual void DepositTiles(int cell, Element element, int world, int prev_cell, float temperature)
	{
		float depthOfElement = (float)this.GetDepthOfElement(cell3, element, world);
		float num = 1f;
		float num2 = (depthOfElement - (float)this.addTilesMinHeight) / (float)(this.addTilesMaxHeight - this.addTilesMinHeight);
		if (!float.IsNaN(num2))
		{
			num -= num2;
		}
		int num3 = Mathf.Min(this.addTiles, Mathf.Clamp(Mathf.RoundToInt((float)this.addTiles * num), 1, this.addTiles));
		ListPool<int, Comet>.PooledList pooledList = ListPool<int, Comet>.Allocate();
		HashSetPool<int, Comet>.PooledHashSet pooledHashSet = HashSetPool<int, Comet>.Allocate();
		QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
		int num4 = -1;
		int num5 = 1;
		if (this.velocity.x < 0f)
		{
			num4 *= -1;
			num5 *= -1;
		}
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = prev_cell,
			depth = 0
		});
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = Grid.OffsetCell(prev_cell, new CellOffset(num4, 0)),
			depth = 0
		});
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = Grid.OffsetCell(prev_cell, new CellOffset(num5, 0)),
			depth = 0
		});
		Func<int, bool> condition = (int cell) => Grid.IsValidCellInWorld(cell, world) && !Grid.Solid[cell];
		GameUtil.FloodFillConditional(pooledQueue, condition, pooledHashSet, pooledList, 10);
		float mass = (num3 > 0) ? (this.addTileMass / (float)this.addTiles) : 1f;
		int disease_count = this.addDiseaseCount / num3;
		if (element.HasTag(GameTags.Unstable))
		{
			UnstableGroundManager component = World.Instance.GetComponent<UnstableGroundManager>();
			using (List<int>.Enumerator enumerator = pooledList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int cell2 = enumerator.Current;
					if (num3 <= 0)
					{
						break;
					}
					component.Spawn(cell2, element, mass, temperature, byte.MaxValue, 0);
					num3--;
				}
				goto IL_229;
			}
		}
		foreach (int gameCell in pooledList)
		{
			if (num3 <= 0)
			{
				break;
			}
			SimMessages.AddRemoveSubstance(gameCell, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, this.diseaseIdx, disease_count, true, -1);
			num3--;
		}
		IL_229:
		pooledList.Recycle();
		pooledHashSet.Recycle();
		pooledQueue.Recycle();
	}

	// Token: 0x06003B21 RID: 15137 RVA: 0x0014A158 File Offset: 0x00148358
	protected virtual void SpawnCraterPrefabs()
	{
		if (this.craterPrefabs != null && this.craterPrefabs.Length != 0)
		{
			GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab(this.craterPrefabs[UnityEngine.Random.Range(0, this.craterPrefabs.Length)]), Grid.CellToPos(Grid.PosToCell(this)));
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -19.5f);
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06003B22 RID: 15138 RVA: 0x0014A1E4 File Offset: 0x001483E4
	protected int GetDepthOfElement(int cell, Element element, int world)
	{
		int num = 0;
		int num2 = Grid.CellBelow(cell);
		while (Grid.IsValidCellInWorld(num2, world) && Grid.Element[num2] == element)
		{
			num++;
			num2 = Grid.CellBelow(num2);
		}
		return num;
	}

	// Token: 0x06003B23 RID: 15139 RVA: 0x0014A21C File Offset: 0x0014841C
	[ContextMenu("DamageTiles")]
	private float DamageTiles(int cell, int prev_cell, float input_damage)
	{
		GameObject gameObject = Grid.Objects[cell, 9];
		float num = 1f;
		bool flag = false;
		if (gameObject != null)
		{
			if (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Window))
			{
				num = this.windowDamageMultiplier;
			}
			else if (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Bunker))
			{
				num = this.bunkerDamageMultiplier;
				if (gameObject.GetComponent<Door>() != null)
				{
					Game.Instance.savedInfo.blockedCometWithBunkerDoor = true;
				}
			}
			SimCellOccupier component = gameObject.GetComponent<SimCellOccupier>();
			if (component != null && !component.doReplaceElement)
			{
				flag = true;
			}
		}
		Element element;
		if (flag)
		{
			element = gameObject.GetComponent<PrimaryElement>().Element;
		}
		else
		{
			element = Grid.Element[cell];
		}
		if (element.strength == 0f)
		{
			return 0f;
		}
		float num2 = input_damage * num / element.strength;
		this.PlayTileDamageSound(element, Grid.CellToPos(cell), gameObject);
		if (num2 == 0f)
		{
			return 0f;
		}
		float num3;
		if (flag)
		{
			BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
			float a = (float)component2.HitPoints / (float)component2.MaxHitPoints;
			float f = num2 * (float)component2.MaxHitPoints;
			component2.gameObject.BoxingTrigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = Mathf.RoundToInt(f),
				source = BUILDINGS.DAMAGESOURCES.COMET,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
			});
			num3 = Mathf.Min(a, num2);
		}
		else
		{
			num3 = WorldDamage.Instance.ApplyDamage(cell, num2, prev_cell, BUILDINGS.DAMAGESOURCES.COMET, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET);
		}
		this.destroyedCells.Add(cell);
		float num4 = num3 / num2;
		return input_damage * (1f - num4);
	}

	// Token: 0x06003B24 RID: 15140 RVA: 0x0014A3CC File Offset: 0x001485CC
	private void DamageThings(Vector3 pos, int cell, int damage, GameObject ignoreObject = null)
	{
		if (damage == 0 || !Grid.IsValidCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null && gameObject != ignoreObject)
		{
			BuildingHP component = gameObject.GetComponent<BuildingHP>();
			Building component2 = gameObject.GetComponent<Building>();
			if (component != null && !this.damagedEntities.Contains(gameObject))
			{
				float f = gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Bunker) ? ((float)damage * this.bunkerDamageMultiplier) : ((float)damage);
				if (component2 != null && component2.Def != null)
				{
					this.PlayBuildingDamageSound(component2.Def, Grid.CellToPos(cell), gameObject);
				}
				component.gameObject.BoxingTrigger(-794517298, new BuildingHP.DamageSourceInfo
				{
					damage = Mathf.RoundToInt(f),
					source = BUILDINGS.DAMAGESOURCES.COMET,
					popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
				});
				this.damagedEntities.Add(gameObject);
			}
		}
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)pos.x, (int)pos.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
			Health component3 = pickupable.GetComponent<Health>();
			if (component3 != null && !this.damagedEntities.Contains(pickupable.gameObject))
			{
				float amount = pickupable.KPrefabID.HasTag(GameTags.Bunker) ? ((float)damage * this.bunkerDamageMultiplier) : ((float)damage);
				component3.Damage(amount);
				this.damagedEntities.Add(pickupable.gameObject);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06003B25 RID: 15141 RVA: 0x0014A5B4 File Offset: 0x001487B4
	public float GetDistanceFromImpact()
	{
		float num = this.velocity.x / this.velocity.y;
		Vector3 position = base.transform.GetPosition();
		float num2 = 0f;
		while (num2 > -6f)
		{
			num2 -= 1f;
			num2 = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
			float x = num2 * num;
			Vector3 b = new Vector3(x, num2, 0f);
			int num3 = Grid.PosToCell(position + b);
			if (Grid.IsValidCell(num3) && Grid.Solid[num3])
			{
				return b.magnitude;
			}
		}
		return 6f;
	}

	// Token: 0x06003B26 RID: 15142 RVA: 0x0014A65D File Offset: 0x0014885D
	public float GetSoundDistance()
	{
		return this.GetDistanceFromImpact();
	}

	// Token: 0x06003B27 RID: 15143 RVA: 0x0014A668 File Offset: 0x00148868
	private void PlayTileDamageSound(Element element, Vector3 pos, GameObject tile_go)
	{
		string text = element.substance.GetMiningBreakSound();
		if (text == null)
		{
			if (element.HasTag(GameTags.RefinedMetal))
			{
				text = "RefinedMetal";
			}
			else if (element.HasTag(GameTags.Metal))
			{
				text = "RawMetal";
			}
			else
			{
				text = "Rock";
			}
		}
		text = "MeteorDamage_" + text;
		text = GlobalAssets.GetSound(text, false);
		if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
		{
			float volume = this.GetVolume(tile_go);
			KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos, false), volume);
		}
	}

	// Token: 0x06003B28 RID: 15144 RVA: 0x0014A704 File Offset: 0x00148904
	private void PlayBuildingDamageSound(BuildingDef def, Vector3 pos, GameObject building_go)
	{
		if (def != null)
		{
			string sound = GlobalAssets.GetSound(StringFormatter.Combine("MeteorDamage_Building_", def.AudioCategory), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("MeteorDamage_Building_Metal", false);
			}
			if (sound != null && CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, sound))
			{
				float volume = this.GetVolume(building_go);
				KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(pos, false), volume);
			}
		}
	}

	// Token: 0x06003B29 RID: 15145 RVA: 0x0014A780 File Offset: 0x00148980
	public void Sim33ms(float dt)
	{
		if (this.hasExploded)
		{
			return;
		}
		if (this.offsetPosition.y > 0f)
		{
			Vector3 b = new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Vector3 vector = this.offsetPosition + b;
			this.offsetPosition = vector;
			this.anim.Offset = this.offsetPosition;
		}
		else
		{
			if (this.anim.Offset != Vector3.zero)
			{
				this.anim.Offset = Vector3.zero;
			}
			if (!this.selectable.enabled)
			{
				this.selectable.enabled = true;
			}
			Vector2 vector2 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * -0.1f;
			Vector2 vector3 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * 1.1f;
			Vector3 position = base.transform.GetPosition();
			Vector3 vector4 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			int num = Grid.PosToCell(vector4);
			this.loopingSounds.UpdateVelocity(this.flyingSound, vector4 - position);
			Element element = ElementLoader.FindElementByHash(this.EXHAUST_ELEMENT);
			if (this.EXHAUST_ELEMENT != SimHashes.Void && Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				SimMessages.EmitMass(num, element.idx, dt * this.EXHAUST_RATE, element.defaultValues.temperature, this.diseaseIdx, Mathf.RoundToInt((float)this.addDiseaseCount * dt), -1);
			}
			if (vector4.x < vector2.x || vector3.x < vector4.x || vector4.y < vector2.y)
			{
				global::Util.KDestroyGameObject(base.gameObject);
			}
			int num2 = Grid.PosToCell(this);
			int num3 = Grid.PosToCell(this.previousPosition);
			if (num2 != num3)
			{
				if (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					this.remainingTileDamage = this.DamageTiles(num2, num3, this.remainingTileDamage);
					if (this.remainingTileDamage <= 0f)
					{
						this.Explode(position, num2, num3, component.Element);
						this.hasExploded = true;
						if (this.destroyOnExplode)
						{
							global::Util.KDestroyGameObject(base.gameObject);
						}
						return;
					}
				}
				else
				{
					GameObject ignoreObject = (this.ignoreObstacleForDamage.Get() == null) ? null : this.ignoreObstacleForDamage.Get().gameObject;
					this.DamageThings(position, num2, this.entityDamage, ignoreObject);
				}
			}
			if (this.canHitDuplicants && this.age > 0.25f && Grid.Objects[Grid.PosToCell(position), 0] != null)
			{
				base.transform.position = Grid.CellToPos(Grid.PosToCell(position));
				this.Explode(position, num2, num3, base.GetComponent<PrimaryElement>().Element);
				if (this.destroyOnExplode)
				{
					global::Util.KDestroyGameObject(base.gameObject);
				}
				return;
			}
			this.previousPosition = position;
			base.transform.SetPosition(vector4);
		}
		this.age += dt;
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x0014AAD0 File Offset: 0x00148CD0
	private void PlayImpactSound(Vector3 pos)
	{
		if (this.impactSound == null)
		{
			this.impactSound = "Meteor_Large_Impact";
		}
		this.loopingSounds.StopSound(this.flyingSound);
		string sound = GlobalAssets.GetSound(this.impactSound, false);
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			float volume = this.GetVolume(base.gameObject);
			pos.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x0014AB71 File Offset: 0x00148D71
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
		this.loopingSounds.UpdateFirstParameter(this.flyingSound, this.FLYING_SOUND_ID_PARAMETER, (float)this.flyingSoundID);
	}

	// Token: 0x06003B2C RID: 15148 RVA: 0x0014ABA4 File Offset: 0x00148DA4
	public void Explode()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		this.Explode(position, num, num, component.Element);
		this.hasExploded = true;
		if (this.destroyOnExplode)
		{
			global::Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x040023F2 RID: 9202
	public SimHashes EXHAUST_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x040023F3 RID: 9203
	public float EXHAUST_RATE = 50f;

	// Token: 0x040023F4 RID: 9204
	public Vector2 spawnVelocity = new Vector2(12f, 15f);

	// Token: 0x040023F5 RID: 9205
	public Vector2 spawnAngle = new Vector2(-100f, -80f);

	// Token: 0x040023F6 RID: 9206
	public Vector2 massRange;

	// Token: 0x040023F7 RID: 9207
	public Vector2 temperatureRange;

	// Token: 0x040023F8 RID: 9208
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x040023F9 RID: 9209
	public int splashRadius = 1;

	// Token: 0x040023FA RID: 9210
	public int addTiles;

	// Token: 0x040023FB RID: 9211
	public int addTilesMinHeight;

	// Token: 0x040023FC RID: 9212
	public int addTilesMaxHeight;

	// Token: 0x040023FD RID: 9213
	public int entityDamage = 1;

	// Token: 0x040023FE RID: 9214
	public float totalTileDamage = 0.2f;

	// Token: 0x040023FF RID: 9215
	protected float addTileMass;

	// Token: 0x04002400 RID: 9216
	public int addDiseaseCount;

	// Token: 0x04002401 RID: 9217
	public byte diseaseIdx = byte.MaxValue;

	// Token: 0x04002402 RID: 9218
	public Vector2 elementReplaceTileTemperatureRange = new Vector2(800f, 1000f);

	// Token: 0x04002403 RID: 9219
	public Vector2I explosionOreCount = new Vector2I(0, 0);

	// Token: 0x04002404 RID: 9220
	private float explosionMass;

	// Token: 0x04002405 RID: 9221
	public Vector2 explosionTemperatureRange = new Vector2(500f, 700f);

	// Token: 0x04002406 RID: 9222
	public Vector2 explosionSpeedRange = new Vector2(8f, 14f);

	// Token: 0x04002407 RID: 9223
	public float windowDamageMultiplier = 5f;

	// Token: 0x04002408 RID: 9224
	public float bunkerDamageMultiplier;

	// Token: 0x04002409 RID: 9225
	public string impactSound;

	// Token: 0x0400240A RID: 9226
	public string flyingSound;

	// Token: 0x0400240B RID: 9227
	public int flyingSoundID;

	// Token: 0x0400240C RID: 9228
	private HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

	// Token: 0x0400240D RID: 9229
	public bool affectedByDifficulty = true;

	// Token: 0x0400240E RID: 9230
	public bool Targeted;

	// Token: 0x0400240F RID: 9231
	[Serialize]
	protected Vector3 offsetPosition;

	// Token: 0x04002410 RID: 9232
	[Serialize]
	protected Vector2 velocity;

	// Token: 0x04002411 RID: 9233
	[Serialize]
	private float remainingTileDamage;

	// Token: 0x04002412 RID: 9234
	private Vector3 previousPosition;

	// Token: 0x04002413 RID: 9235
	private bool hasExploded;

	// Token: 0x04002414 RID: 9236
	public bool canHitDuplicants;

	// Token: 0x04002415 RID: 9237
	public string[] craterPrefabs;

	// Token: 0x04002416 RID: 9238
	public string[] lootOnDestroyedByMissile;

	// Token: 0x04002417 RID: 9239
	public bool destroyOnExplode = true;

	// Token: 0x04002418 RID: 9240
	public bool spawnWithOffset;

	// Token: 0x04002419 RID: 9241
	private float age;

	// Token: 0x0400241A RID: 9242
	public System.Action OnImpact;

	// Token: 0x0400241B RID: 9243
	public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();

	// Token: 0x0400241C RID: 9244
	[MyCmpGet]
	private KBatchedAnimController anim;

	// Token: 0x0400241D RID: 9245
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x0400241E RID: 9246
	public Tag typeID;

	// Token: 0x0400241F RID: 9247
	private LoopingSounds loopingSounds;

	// Token: 0x04002420 RID: 9248
	private List<GameObject> damagedEntities = new List<GameObject>();

	// Token: 0x04002421 RID: 9249
	private List<int> destroyedCells = new List<int>();

	// Token: 0x04002422 RID: 9250
	private const float MAX_DISTANCE_TEST = 6f;
}
