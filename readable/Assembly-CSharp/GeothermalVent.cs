using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200076E RID: 1902
public class GeothermalVent : StateMachineComponent<GeothermalVent.StatesInstance>, ISim200ms, ISaveLoadable
{
	// Token: 0x0600304C RID: 12364 RVA: 0x00116C7C File Offset: 0x00114E7C
	public bool IsQuestEntombed()
	{
		return this.progress == GeothermalVent.QuestProgress.Entombed;
	}

	// Token: 0x0600304D RID: 12365 RVA: 0x00116C88 File Offset: 0x00114E88
	public void SetQuestComplete()
	{
		this.progress = GeothermalVent.QuestProgress.Complete;
		this.connectedToggler.showButton = true;
		base.GetComponent<InfoDescription>().description = BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + BUILDINGS.PREFABS.GEOTHERMALVENT.DESC;
		base.Trigger(-1514841199, null);
	}

	// Token: 0x0600304E RID: 12366 RVA: 0x00116CE0 File Offset: 0x00114EE0
	public static string GenerateName()
	{
		string text = "";
		for (int i = 0; i < 2; i++)
		{
			text += "0123456789"[UnityEngine.Random.Range(0, "0123456789".Length)].ToString();
		}
		return BUILDINGS.PREFABS.GEOTHERMALVENT.NAME_FMT.Replace("{ID}", text);
	}

	// Token: 0x0600304F RID: 12367 RVA: 0x00116D38 File Offset: 0x00114F38
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.entombVulnerable.SetStatusItem(Db.Get().BuildingStatusItems.Entombed);
		base.GetComponent<PrimaryElement>().SetElement(SimHashes.Katairite, true);
		this.emitterInfo = default(GeothermalVent.EmitterInfo);
		this.emitterInfo.cell = Grid.PosToCell(base.gameObject) + Grid.WidthInCells * 3;
		this.emitterInfo.element = default(GeothermalVent.ElementInfo);
		this.emitterInfo.simHandle = -1;
		Components.GeothermalVents.Add(base.gameObject.GetMyWorldId(), this);
		if (this.progress == GeothermalVent.QuestProgress.Uninitialized)
		{
			if (Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId()).Count == 3)
			{
				this.progress = GeothermalVent.QuestProgress.Entombed;
			}
			else
			{
				this.progress = GeothermalVent.QuestProgress.Complete;
			}
		}
		if (this.progress == GeothermalVent.QuestProgress.Complete)
		{
			this.connectedToggler.showButton = true;
		}
		else
		{
			base.GetComponent<InfoDescription>().description = BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + BUILDINGS.PREFABS.GEOTHERMALVENT.BLOCKED_DESC;
			base.Trigger(-1514841199, null);
		}
		this.massMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.BAROMETER_SYMBOLS);
		UserNameable component = base.GetComponent<UserNameable>();
		if (component.savedName == "" || component.savedName == BUILDINGS.PREFABS.GEOTHERMALVENT.NAME)
		{
			component.SetName(GeothermalVent.GenerateName());
		}
		this.SimRegister();
		base.smi.StartSM();
	}

	// Token: 0x06003050 RID: 12368 RVA: 0x00116EC4 File Offset: 0x001150C4
	[OnDeserialized]
	internal void OnDeserializedMethod()
	{
		bool flag = false;
		for (int i = 0; i < this.availableMaterial.Count; i++)
		{
			GeothermalVent.ElementInfo elementInfo = this.availableMaterial[i];
			Element element = ElementLoader.FindElementByHash(elementInfo.elementHash);
			if (element == null)
			{
				element = ElementLoader.FindElementByHash(SimHashes.Steam);
				elementInfo.elementHash = SimHashes.Steam;
				elementInfo.isSolid = false;
			}
			elementInfo.elementIdx = element.idx;
			this.availableMaterial[i] = elementInfo;
		}
		if (flag)
		{
			global::Debug.LogWarning("Invalid geothermal vent content in save was converted to steam on load.");
		}
	}

	// Token: 0x06003051 RID: 12369 RVA: 0x00116F4C File Offset: 0x0011514C
	protected void SimRegister()
	{
		this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimBlockedCallback), true));
		this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimUnblockedCallback), true));
		SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(GeothermalVent.OnSimRegisteredCallback), this, "GeothermalVentElementEmitter").index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
	}

	// Token: 0x06003052 RID: 12370 RVA: 0x00116FF0 File Offset: 0x001151F0
	protected void OnSimBlockedCallback()
	{
		this.overpressure = true;
	}

	// Token: 0x06003053 RID: 12371 RVA: 0x00116FF9 File Offset: 0x001151F9
	protected void OnSimUnblockedCallback()
	{
		this.overpressure = false;
	}

	// Token: 0x06003054 RID: 12372 RVA: 0x00117002 File Offset: 0x00115202
	protected static void OnSimRegisteredCallback(int handle, object data)
	{
		((GeothermalVent)data).OnSimRegisteredImpl(handle);
	}

	// Token: 0x06003055 RID: 12373 RVA: 0x00117010 File Offset: 0x00115210
	protected void OnSimRegisteredImpl(int handle)
	{
		global::Debug.Assert(this.emitterInfo.simHandle == -1, "?! too many handles registered");
		this.emitterInfo.simHandle = handle;
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x00117036 File Offset: 0x00115236
	protected void SimUnregister()
	{
		if (Sim.IsValidHandle(this.emitterInfo.simHandle))
		{
			SimMessages.RemoveElementEmitter(-1, this.emitterInfo.simHandle);
		}
		this.emitterInfo.simHandle = -1;
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x00117067 File Offset: 0x00115267
	protected override void OnCleanUp()
	{
		Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
		Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
		Components.GeothermalVents.Remove(base.gameObject.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x06003058 RID: 12376 RVA: 0x001170A8 File Offset: 0x001152A8
	protected void OnMassEmitted(ushort element, float mass)
	{
		bool flag = false;
		for (int i = 0; i < this.availableMaterial.Count; i++)
		{
			if (this.availableMaterial[i].elementIdx == element)
			{
				GeothermalVent.ElementInfo elementInfo = this.availableMaterial[i];
				elementInfo.mass -= mass;
				flag |= (elementInfo.mass <= 0f);
				this.availableMaterial[i] = elementInfo;
				break;
			}
		}
		if (flag)
		{
			this.RecomputeEmissions();
		}
	}

	// Token: 0x06003059 RID: 12377 RVA: 0x00117128 File Offset: 0x00115328
	public void SpawnKeepsake()
	{
		GameObject keepsakePrefab = Assets.GetPrefab("keepsake_geothermalplant");
		if (keepsakePrefab != null)
		{
			base.GetComponent<KBatchedAnimController>().Play("pooped", KAnim.PlayMode.Once, 1f, 0f);
			GameScheduler.Instance.Schedule("UncorkPoopAnim", 1.5f, delegate(object data)
			{
				this.GetComponent<KBatchedAnimController>().Play("uncork", KAnim.PlayMode.Once, 1f, 0f);
			}, null, null);
			GameScheduler.Instance.Schedule("UncorkPoopFX", 2f, delegate(object data)
			{
				Game.Instance.SpawnFX(SpawnFXHashes.MissileExplosion, this.transform.GetPosition() + Vector3.up * 3f, 0f);
			}, null, null);
			GameScheduler.Instance.Schedule("SpawnGeothermalKeepsake", 3.75f, delegate(object data)
			{
				Vector3 position = this.transform.GetPosition();
				position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
				GameObject gameObject = Util.KInstantiate(keepsakePrefab, position);
				gameObject.SetActive(true);
				new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
			}, null, null);
		}
	}

	// Token: 0x0600305A RID: 12378 RVA: 0x001171F1 File Offset: 0x001153F1
	public bool IsOverPressure()
	{
		return this.overpressure;
	}

	// Token: 0x0600305B RID: 12379 RVA: 0x001171FC File Offset: 0x001153FC
	protected void RecomputeEmissions()
	{
		this.availableMaterial.Sort();
		while (this.availableMaterial.Count > 0 && this.availableMaterial[this.availableMaterial.Count - 1].mass <= 0f)
		{
			this.availableMaterial.RemoveAt(this.availableMaterial.Count - 1);
		}
		int num = 0;
		using (List<GeothermalVent.ElementInfo>.Enumerator enumerator = this.availableMaterial.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.isSolid)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			int num2 = UnityEngine.Random.Range(0, this.availableMaterial.Count);
			while (this.availableMaterial[num2].isSolid)
			{
				num2 = (num2 + 1) % this.availableMaterial.Count;
			}
			this.emitterInfo.element = this.availableMaterial[num2];
			this.emitterInfo.element.diseaseCount = (int)((float)this.availableMaterial[num2].diseaseCount * this.emitterInfo.element.mass / this.availableMaterial[num2].mass);
		}
		else
		{
			this.emitterInfo.element.elementIdx = 0;
			this.emitterInfo.element.mass = 0f;
		}
		this.emitterInfo.dirty = true;
	}

	// Token: 0x0600305C RID: 12380 RVA: 0x0011737C File Offset: 0x0011557C
	public void addMaterial(GeothermalVent.ElementInfo info)
	{
		this.availableMaterial.Add(info);
		this.recentMass = this.MaterialAvailable();
	}

	// Token: 0x0600305D RID: 12381 RVA: 0x00117398 File Offset: 0x00115598
	public bool HasMaterial()
	{
		bool flag = this.availableMaterial.Count != 0;
		if (flag != this.logicPorts.GetOutputValue("GEOTHERMAL_VENT_STATUS_PORT") > 0)
		{
			this.logicPorts.SendSignal("GEOTHERMAL_VENT_STATUS_PORT", flag ? 1 : 0);
		}
		return flag;
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x001173EC File Offset: 0x001155EC
	public float MaterialAvailable()
	{
		float num = 0f;
		foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
		{
			num += elementInfo.mass;
		}
		return num;
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x00117448 File Offset: 0x00115648
	public bool IsEntombed()
	{
		return this.entombVulnerable.GetEntombed;
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x00117455 File Offset: 0x00115655
	public bool CanVent()
	{
		return !this.HasMaterial() && !this.IsEntombed();
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x0011746A File Offset: 0x0011566A
	public bool IsVentConnected()
	{
		return !(this.connectedToggler == null) && this.connectedToggler.IsConnected;
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x00117488 File Offset: 0x00115688
	public void EmitSolidChunk()
	{
		int num = 0;
		foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
		{
			if (elementInfo.isSolid && elementInfo.mass > 0f)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return;
		}
		int num2 = UnityEngine.Random.Range(0, this.availableMaterial.Count);
		while (!this.availableMaterial[num2].isSolid)
		{
			num2 = (num2 + 1) % this.availableMaterial.Count;
		}
		GeothermalVent.ElementInfo elementInfo2 = this.availableMaterial[num2];
		if (ElementLoader.elements[(int)this.availableMaterial[num2].elementIdx] == null)
		{
			return;
		}
		bool flag = UnityEngine.Random.value >= 0.5f;
		float f = GeothermalVentConfig.INITIAL_DEBRIS_ANGLE.Get() * 3.1415927f / 180f;
		Vector2 normalized = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
		if (flag)
		{
			normalized.x = -normalized.x;
		}
		normalized = normalized.normalized;
		normalized * GeothermalVentConfig.INITIAL_DEBRIS_VELOCIOTY.Get();
		float num3 = Math.Min(GeothermalVentConfig.DEBRIS_MASS_KG.Get(), elementInfo2.mass);
		if (elementInfo2.mass - num3 < GeothermalVentConfig.DEBRIS_MASS_KG.min)
		{
			num3 = elementInfo2.mass;
		}
		if (num3 < 0.01f)
		{
			elementInfo2.mass = 0f;
			this.availableMaterial[num2] = elementInfo2;
			return;
		}
		int num4 = (int)((float)elementInfo2.diseaseCount * num3 / elementInfo2.mass);
		Vector3 vector = Grid.CellToPos(this.emitterInfo.cell, CellAlignment.Top, Grid.SceneLayer.BuildingFront);
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, vector, 0f);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MiniCometConfig.ID), vector);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(ElementLoader.elements[(int)elementInfo2.elementIdx].id, true);
		component.Mass = num3;
		component.Temperature = elementInfo2.temperature;
		MiniComet component2 = gameObject.GetComponent<MiniComet>();
		component2.diseaseIdx = elementInfo2.diseaseIdx;
		component2.addDiseaseCount = num4;
		gameObject.SetActive(true);
		elementInfo2.diseaseCount -= num4;
		elementInfo2.mass -= num3;
		this.availableMaterial[num2] = elementInfo2;
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x001176F0 File Offset: 0x001158F0
	public void Sim200ms(float dt)
	{
		if (dt > 0f)
		{
			this.unsafeSim200ms(dt);
		}
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x00117704 File Offset: 0x00115904
	private unsafe void unsafeSim200ms(float dt)
	{
		if (Sim.IsValidHandle(this.emitterInfo.simHandle))
		{
			if (this.emitterInfo.dirty)
			{
				SimMessages.ModifyElementEmitter(this.emitterInfo.simHandle, this.emitterInfo.cell, 1, ElementLoader.elements[(int)this.emitterInfo.element.elementIdx].id, 0.2f, Math.Min(3f, this.emitterInfo.element.mass), this.emitterInfo.element.temperature, 120f, this.emitterInfo.element.diseaseIdx, this.emitterInfo.element.diseaseCount);
				this.emitterInfo.dirty = false;
			}
			int handleIndex = Sim.GetHandleIndex(this.emitterInfo.simHandle);
			Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
			if (emittedMassInfo.mass > 0f)
			{
				this.OnMassEmitted(emittedMassInfo.elemIdx, emittedMassInfo.mass);
			}
		}
		this.massMeter.SetPositionPercent(this.MaterialAvailable() / this.recentMass);
	}

	// Token: 0x06003065 RID: 12389 RVA: 0x00117838 File Offset: 0x00115A38
	protected static bool HasProblem(GeothermalVent.StatesInstance smi)
	{
		return smi.master.IsEntombed() || smi.master.IsOverPressure();
	}

	// Token: 0x04001CC0 RID: 7360
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001CC1 RID: 7361
	[MyCmpAdd]
	private ConnectionManager connectedToggler;

	// Token: 0x04001CC2 RID: 7362
	[MyCmpAdd]
	private EntombVulnerable entombVulnerable;

	// Token: 0x04001CC3 RID: 7363
	[MyCmpReq]
	private LogicPorts logicPorts;

	// Token: 0x04001CC4 RID: 7364
	[Serialize]
	private float recentMass = 1f;

	// Token: 0x04001CC5 RID: 7365
	private MeterController massMeter;

	// Token: 0x04001CC6 RID: 7366
	[Serialize]
	private GeothermalVent.QuestProgress progress;

	// Token: 0x04001CC7 RID: 7367
	protected GeothermalVent.EmitterInfo emitterInfo;

	// Token: 0x04001CC8 RID: 7368
	[Serialize]
	protected List<GeothermalVent.ElementInfo> availableMaterial = new List<GeothermalVent.ElementInfo>();

	// Token: 0x04001CC9 RID: 7369
	protected bool overpressure;

	// Token: 0x04001CCA RID: 7370
	protected int debrisEmissionCell;

	// Token: 0x04001CCB RID: 7371
	private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x04001CCC RID: 7372
	private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x0200166A RID: 5738
	private enum QuestProgress
	{
		// Token: 0x040074D0 RID: 29904
		Uninitialized,
		// Token: 0x040074D1 RID: 29905
		Entombed,
		// Token: 0x040074D2 RID: 29906
		Complete
	}

	// Token: 0x0200166B RID: 5739
	public struct ElementInfo : IComparable
	{
		// Token: 0x06009729 RID: 38697 RVA: 0x00382FD9 File Offset: 0x003811D9
		public int CompareTo(object obj)
		{
			return -this.mass.CompareTo(((GeothermalVent.ElementInfo)obj).mass);
		}

		// Token: 0x040074D3 RID: 29907
		public bool isSolid;

		// Token: 0x040074D4 RID: 29908
		public SimHashes elementHash;

		// Token: 0x040074D5 RID: 29909
		public ushort elementIdx;

		// Token: 0x040074D6 RID: 29910
		public float mass;

		// Token: 0x040074D7 RID: 29911
		public float temperature;

		// Token: 0x040074D8 RID: 29912
		public byte diseaseIdx;

		// Token: 0x040074D9 RID: 29913
		public int diseaseCount;
	}

	// Token: 0x0200166C RID: 5740
	public struct EmitterInfo
	{
		// Token: 0x040074DA RID: 29914
		public int simHandle;

		// Token: 0x040074DB RID: 29915
		public int cell;

		// Token: 0x040074DC RID: 29916
		public GeothermalVent.ElementInfo element;

		// Token: 0x040074DD RID: 29917
		public bool dirty;
	}

	// Token: 0x0200166D RID: 5741
	public class States : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent>
	{
		// Token: 0x0600972A RID: 38698 RVA: 0x00382FF4 File Offset: 0x003811F4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EnterTransition(this.questEntombed, (GeothermalVent.StatesInstance smi) => smi.master.IsQuestEntombed()).EnterTransition(this.online, (GeothermalVent.StatesInstance smi) => !smi.master.IsQuestEntombed());
			this.questEntombed.PlayAnim("pooped").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentQuestBlockage, (GeothermalVent.StatesInstance smi) => smi.master).Transition(this.online, (GeothermalVent.StatesInstance smi) => smi.master.progress == GeothermalVent.QuestProgress.Complete, UpdateRate.SIM_200ms);
			this.online.PlayAnim("on", KAnim.PlayMode.Once).defaultState = this.online.identify;
			this.online.identify.EnterTransition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem)).EnterTransition(this.online.active, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && smi.master.HasMaterial()).EnterTransition(this.online.ready, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && smi.master.IsVentConnected()).EnterTransition(this.online.disconnected, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && !smi.master.IsVentConnected());
			this.online.active.defaultState = this.online.active.preVent;
			this.online.active.preVent.PlayAnim("working_pre").OnAnimQueueComplete(this.online.active.loopVent);
			this.online.active.loopVent.Enter(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Exit(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Transition(this.online.active.postVent, (GeothermalVent.StatesInstance smi) => !smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive.identify, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsVenting, (GeothermalVent.StatesInstance smi) => smi.master).Update(delegate(GeothermalVent.StatesInstance smi, float dt)
			{
				if (dt > 0f)
				{
					smi.master.RecomputeEmissions();
				}
			}, UpdateRate.SIM_4000ms, false).defaultState = this.online.active.loopVent.start;
			this.online.active.loopVent.start.PlayAnim("working1").OnAnimQueueComplete(this.online.active.loopVent.finish);
			this.online.active.loopVent.finish.Enter(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.EmitSolidChunk();
			}).PlayAnim("working2").OnAnimQueueComplete(this.online.active.loopVent.start);
			this.online.active.postVent.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.online.ready);
			this.online.ready.PlayAnim("on", KAnim.PlayMode.Once).Transition(this.online.active, (GeothermalVent.StatesInstance smi) => smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).Transition(this.online.disconnected, (GeothermalVent.StatesInstance smi) => !smi.master.IsVentConnected(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsReady, (GeothermalVent.StatesInstance smi) => smi.master);
			this.online.disconnected.PlayAnim("on", KAnim.PlayMode.Once).Transition(this.online.active, (GeothermalVent.StatesInstance smi) => smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).Transition(this.online.ready, (GeothermalVent.StatesInstance smi) => smi.master.IsVentConnected(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsDisconnected, (GeothermalVent.StatesInstance smi) => smi.master);
			this.online.inactive.PlayAnim("over_pressure", KAnim.PlayMode.Once).Transition(this.online.identify, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi), UpdateRate.SIM_200ms).defaultState = this.online.inactive.identify;
			this.online.inactive.identify.EnterTransition(this.online.inactive.entombed, (GeothermalVent.StatesInstance smi) => smi.master.IsEntombed()).EnterTransition(this.online.inactive.overpressure, (GeothermalVent.StatesInstance smi) => smi.master.IsOverPressure());
			this.online.inactive.entombed.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Entombed, null).Transition(this.online.inactive.identify, (GeothermalVent.StatesInstance smi) => !smi.master.IsEntombed(), UpdateRate.SIM_200ms);
			this.online.inactive.overpressure.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsOverpressure, null).EnterTransition(this.online.inactive.identify, (GeothermalVent.StatesInstance smi) => !smi.master.IsOverPressure());
		}

		// Token: 0x040074DE RID: 29918
		public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State questEntombed;

		// Token: 0x040074DF RID: 29919
		public GeothermalVent.States.OnlineStates online;

		// Token: 0x020028EF RID: 10479
		public class ActiveStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x0400B4CC RID: 46284
			public GeothermalVent.States.ActiveStates.LoopStates loopVent;

			// Token: 0x0400B4CD RID: 46285
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State preVent;

			// Token: 0x0400B4CE RID: 46286
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State postVent;

			// Token: 0x02003A48 RID: 14920
			public class LoopStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
			{
				// Token: 0x0400EB7B RID: 60283
				public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State start;

				// Token: 0x0400EB7C RID: 60284
				public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State finish;
			}
		}

		// Token: 0x020028F0 RID: 10480
		public class ProblemStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x0400B4CF RID: 46287
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;

			// Token: 0x0400B4D0 RID: 46288
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State entombed;

			// Token: 0x0400B4D1 RID: 46289
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State overpressure;
		}

		// Token: 0x020028F1 RID: 10481
		public class OnlineStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x0400B4D2 RID: 46290
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;

			// Token: 0x0400B4D3 RID: 46291
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State ready;

			// Token: 0x0400B4D4 RID: 46292
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State disconnected;

			// Token: 0x0400B4D5 RID: 46293
			public GeothermalVent.States.ActiveStates active;

			// Token: 0x0400B4D6 RID: 46294
			public GeothermalVent.States.ProblemStates inactive;
		}
	}

	// Token: 0x0200166E RID: 5742
	public class StatesInstance : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.GameInstance
	{
		// Token: 0x0600972C RID: 38700 RVA: 0x003836FF File Offset: 0x003818FF
		public StatesInstance(GeothermalVent smi) : base(smi)
		{
		}
	}
}
