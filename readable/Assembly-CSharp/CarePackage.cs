using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000719 RID: 1817
public class CarePackage : StateMachineComponent<CarePackage.SMInstance>
{
	// Token: 0x06002D59 RID: 11609 RVA: 0x00106DDC File Offset: 0x00104FDC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.info != null)
		{
			this.SetAnimToInfo();
		}
		this.reactable = this.CreateReactable();
	}

	// Token: 0x06002D5A RID: 11610 RVA: 0x00106E0C File Offset: 0x0010500C
	public Reactable CreateReactable()
	{
		return new EmoteReactable(base.gameObject, "UpgradeFX", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 20f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Cheer);
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x00106E6D File Offset: 0x0010506D
	protected override void OnCleanUp()
	{
		this.reactable.Cleanup();
		base.OnCleanUp();
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x00106E80 File Offset: 0x00105080
	public void SetInfo(CarePackageInfo info)
	{
		this.info = info;
		this.SetAnimToInfo();
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x00106E8F File Offset: 0x0010508F
	public void SetFacade(string facadeID)
	{
		this.facadeID = facadeID;
		this.SetAnimToInfo();
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x00106EA0 File Offset: 0x001050A0
	private void SetAnimToInfo()
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Meter".ToTag()), base.gameObject, null);
		GameObject prefab = Assets.GetPrefab(this.info.id);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component3 = prefab.GetComponent<SymbolOverrideController>();
		KBatchedAnimController component4 = gameObject.GetComponent<KBatchedAnimController>();
		component4.transform.SetLocalPosition(Vector3.forward);
		component4.AnimFiles = component2.AnimFiles;
		component4.isMovable = true;
		component4.animWidth = component2.animWidth;
		component4.animHeight = component2.animHeight;
		if (component3 != null)
		{
			SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
			foreach (SymbolOverrideController.SymbolEntry symbolEntry in component3.GetSymbolOverrides)
			{
				symbolOverrideController.AddSymbolOverride(symbolEntry.targetSymbol, symbolEntry.sourceSymbol, 0);
			}
		}
		component4.initialAnim = component2.initialAnim;
		component4.initialMode = KAnim.PlayMode.Loop;
		if (!string.IsNullOrEmpty(this.facadeID))
		{
			component4.SwapAnims(new KAnimFile[]
			{
				Db.GetEquippableFacades().Get(this.facadeID).AnimFile
			});
			base.GetComponentsInChildren<KBatchedAnimController>()[1].SetSymbolVisiblity("object", false);
		}
		KBatchedAnimTracker component5 = gameObject.GetComponent<KBatchedAnimTracker>();
		component5.controller = component;
		component5.symbol = new HashedString("snapTO_object");
		component5.offset = new Vector3(0f, 0.5f, 0f);
		gameObject.SetActive(true);
		component.SetSymbolVisiblity("snapTO_object", false);
		new KAnimLink(component, component4);
	}

	// Token: 0x06002D5F RID: 11615 RVA: 0x00107040 File Offset: 0x00105240
	private void SpawnContents()
	{
		if (this.info == null)
		{
			global::Debug.LogWarning("CarePackage has no data to spawn from. Probably a save from before the CarePackage info data was serialized.");
			return;
		}
		GameObject gameObject = null;
		GameObject prefab = Assets.GetPrefab(this.info.id);
		Element element = ElementLoader.GetElement(this.info.id.ToTag());
		Vector3 position = base.transform.position + Vector3.up / 2f;
		if (element == null && prefab != null)
		{
			int num = 0;
			while ((float)num < this.info.quantity)
			{
				gameObject = Util.KInstantiate(prefab, position);
				if (gameObject != null)
				{
					if (!this.facadeID.IsNullOrWhiteSpace())
					{
						EquippableFacade.AddFacadeToEquippable(gameObject.GetComponent<Equippable>(), this.facadeID);
					}
					gameObject.SetActive(true);
				}
				num++;
			}
		}
		else if (element != null)
		{
			float quantity = this.info.quantity;
			gameObject = element.substance.SpawnResource(position, quantity, element.defaultValues.temperature, byte.MaxValue, 0, false, true, false);
		}
		else
		{
			global::Debug.LogWarning("Can't find spawnable thing from tag " + this.info.id);
		}
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x04001AF6 RID: 6902
	[Serialize]
	public CarePackageInfo info;

	// Token: 0x04001AF7 RID: 6903
	private string facadeID;

	// Token: 0x04001AF8 RID: 6904
	private Reactable reactable;

	// Token: 0x020015E0 RID: 5600
	public class SMInstance : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.GameInstance
	{
		// Token: 0x060094E9 RID: 38121 RVA: 0x0037A7E1 File Offset: 0x003789E1
		public SMInstance(CarePackage master) : base(master)
		{
		}

		// Token: 0x040072F1 RID: 29425
		public List<Chore> activeUseChores;
	}

	// Token: 0x020015E1 RID: 5601
	public class States : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage>
	{
		// Token: 0x060094EA RID: 38122 RVA: 0x0037A7EC File Offset: 0x003789EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.spawn;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.spawn.PlayAnim("portalbirth").OnAnimQueueComplete(this.open).ParamTransition<bool>(this.spawnedContents, this.pst, GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.IsTrue);
			this.open.PlayAnim("portalbirth_pst").QueueAnim("object_idle_loop", false, null).Exit(delegate(CarePackage.SMInstance smi)
			{
				smi.master.SpawnContents();
				this.spawnedContents.Set(true, smi, false);
			}).ScheduleGoTo(1f, this.pst);
			this.pst.PlayAnim("object_idle_pst").ScheduleGoTo(5f, this.destroy);
			this.destroy.Enter(delegate(CarePackage.SMInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x040072F2 RID: 29426
		public StateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.BoolParameter spawnedContents;

		// Token: 0x040072F3 RID: 29427
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State spawn;

		// Token: 0x040072F4 RID: 29428
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State open;

		// Token: 0x040072F5 RID: 29429
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State pst;

		// Token: 0x040072F6 RID: 29430
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State destroy;
	}
}
