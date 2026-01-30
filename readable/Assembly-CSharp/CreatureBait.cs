using System;
using KSerialization;

// Token: 0x02000CE4 RID: 3300
[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureBait : StateMachineComponent<CreatureBait.StatesInstance>
{
	// Token: 0x060065E7 RID: 26087 RVA: 0x0026627F File Offset: 0x0026447F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060065E8 RID: 26088 RVA: 0x00266288 File Offset: 0x00264488
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Tag[] constructionElements = base.GetComponent<Deconstructable>().constructionElements;
		this.baitElement = ((constructionElements.Length > 1) ? constructionElements[1] : constructionElements[0]);
		base.gameObject.GetSMI<Lure.Instance>().SetActiveLures(new Tag[]
		{
			this.baitElement
		});
		base.smi.StartSM();
	}

	// Token: 0x04004579 RID: 17785
	[Serialize]
	public Tag baitElement;

	// Token: 0x02001F17 RID: 7959
	public class StatesInstance : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GameInstance
	{
		// Token: 0x0600B561 RID: 46433 RVA: 0x003EDDFA File Offset: 0x003EBFFA
		public StatesInstance(CreatureBait master) : base(master)
		{
		}
	}

	// Token: 0x02001F18 RID: 7960
	public class States : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait>
	{
		// Token: 0x0600B562 RID: 46434 RVA: 0x003EDE04 File Offset: 0x003EC004
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Baited, null).Enter(delegate(CreatureBait.StatesInstance smi)
			{
				KAnim.Build build = ElementLoader.FindElementByName(smi.master.baitElement.ToString()).substance.anim.GetData().build;
				KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
				HashedString target_symbol = "snapTo_bait";
				smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(target_symbol, symbol, 0);
			}).TagTransition(GameTags.LureUsed, this.destroy, false);
			this.destroy.PlayAnim("use").EventHandler(GameHashes.AnimQueueComplete, delegate(CreatureBait.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x0400918E RID: 37262
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State idle;

		// Token: 0x0400918F RID: 37263
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State destroy;
	}
}
