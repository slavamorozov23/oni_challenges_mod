using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class CropTendingStates : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>
{
	// Token: 0x06000444 RID: 1092 RVA: 0x00023530 File Offset: 0x00021730
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findCrop;
		this.root.Exit(delegate(CropTendingStates.Instance smi)
		{
			this.UnreserveCrop(smi);
			if (!smi.tendedSucceeded)
			{
				this.RestoreSymbolsVisibility(smi);
			}
		});
		this.findCrop.Enter(delegate(CropTendingStates.Instance smi)
		{
			this.FindCrop(smi);
			if (smi.sm.targetCrop.Get(smi) == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			this.ReserverCrop(smi);
			smi.GoTo(this.moveToCrop);
		});
		GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State state = this.moveToCrop;
		string name = CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.NAME;
		string tooltip = CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).MoveTo((CropTendingStates.Instance smi) => smi.moveCell, this.tendCrop, this.behaviourcomplete, false).ParamTransition<GameObject>(this.targetCrop, this.behaviourcomplete, (CropTendingStates.Instance smi, GameObject p) => this.targetCrop.Get(smi) == null);
		GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State state2 = this.tendCrop.DefaultState(this.tendCrop.pre);
		string name2 = CREATURES.STATUSITEMS.DIVERGENT_TENDING.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.DIVERGENT_TENDING.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).ParamTransition<GameObject>(this.targetCrop, this.behaviourcomplete, (CropTendingStates.Instance smi, GameObject p) => this.targetCrop.Get(smi) == null).Enter(delegate(CropTendingStates.Instance smi)
		{
			smi.animSet = this.GetCropTendingAnimSet(smi);
			this.StoreSymbolsVisibility(smi);
		});
		this.tendCrop.pre.Face(this.targetCrop, 0f).PlayAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending_pre, KAnim.PlayMode.Once).OnAnimQueueComplete(this.tendCrop.tend);
		this.tendCrop.tend.Enter(delegate(CropTendingStates.Instance smi)
		{
			this.SetSymbolsVisibility(smi, false);
		}).QueueAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending, false, null).OnAnimQueueComplete(this.tendCrop.pst);
		this.tendCrop.pst.QueueAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending_pst, false, null).OnAnimQueueComplete(this.behaviourcomplete).Exit(delegate(CropTendingStates.Instance smi)
		{
			GameObject gameObject = smi.sm.targetCrop.Get(smi);
			if (gameObject != null)
			{
				if (smi.effect != null)
				{
					gameObject.GetComponent<Effects>().Add(smi.effect, true);
				}
				smi.tendedSucceeded = true;
				CropTendingStates.CropTendingEventData data = new CropTendingStates.CropTendingEventData
				{
					source = smi.gameObject,
					cropId = smi.sm.targetCrop.Get(smi).PrefabID()
				};
				smi.sm.targetCrop.Get(smi).Trigger(90606262, data);
				smi.Trigger(90606262, data);
			}
		});
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToTendCrops, false);
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x000237AC File Offset: 0x000219AC
	private CropTendingStates.AnimSet GetCropTendingAnimSet(CropTendingStates.Instance smi)
	{
		CropTendingStates.AnimSet result;
		if (smi.def.animSetOverrides.TryGetValue(this.targetCrop.Get(smi).PrefabID(), out result))
		{
			return result;
		}
		return CropTendingStates.defaultAnimSet;
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x000237E8 File Offset: 0x000219E8
	private void FindCrop(CropTendingStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Crop crop = null;
		int moveCell = Grid.InvalidCell;
		int num = 100;
		int num2 = -1;
		foreach (Crop crop2 in Components.Crops.GetWorldItems(smi.gameObject.GetMyWorldId(), false))
		{
			if (Vector2.SqrMagnitude(crop2.transform.position - smi.transform.position) <= 625f)
			{
				if (smi.effect != null)
				{
					Effects component2 = crop2.GetComponent<Effects>();
					if (component2 != null)
					{
						bool flag = false;
						for (int i = 0; i < smi.def.ignoreEffectGroup.Length; i++)
						{
							HashedString effect_id = smi.def.ignoreEffectGroup[i];
							if (component2.HasEffect(effect_id))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							continue;
						}
					}
				}
				KPrefabID component3 = crop2.GetComponent<KPrefabID>();
				if (!component3.HasTag(GameTags.FullyGrown) && !component3.HasTag(GameTags.Creatures.ReservedByCreature))
				{
					int num3;
					smi.def.interests.TryGetValue(crop2.PrefabID(), out num3);
					if (num3 >= num2)
					{
						bool flag2 = num3 > num2;
						int cell = Grid.PosToCell(crop2);
						int[] array = new int[]
						{
							Grid.CellLeft(cell),
							Grid.CellRight(cell)
						};
						if (component3.HasTag(GameTags.PlantedOnFloorVessel))
						{
							array = new int[]
							{
								Grid.CellLeft(cell),
								Grid.CellRight(cell),
								Grid.CellDownLeft(cell),
								Grid.CellDownRight(cell)
							};
						}
						int num4 = 100;
						int num5 = Grid.InvalidCell;
						for (int j = 0; j < array.Length; j++)
						{
							if (Grid.IsValidCell(array[j]))
							{
								int navigationCost = component.GetNavigationCost(array[j]);
								if (navigationCost != -1 && navigationCost < num4)
								{
									num4 = navigationCost;
									num5 = array[j];
								}
							}
						}
						if (num4 != -1 && num5 != Grid.InvalidCell && (flag2 || num4 < num))
						{
							moveCell = num5;
							num = num4;
							num2 = num3;
							crop = crop2;
						}
					}
				}
			}
		}
		GameObject value = (crop != null) ? crop.gameObject : null;
		smi.sm.targetCrop.Set(value, smi, false);
		smi.moveCell = moveCell;
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x00023A5C File Offset: 0x00021C5C
	private void ReserverCrop(CropTendingStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCrop.Get(smi);
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00023AA4 File Offset: 0x00021CA4
	private void UnreserveCrop(CropTendingStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCrop.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x00023AD8 File Offset: 0x00021CD8
	private void SetSymbolsVisibility(CropTendingStates.Instance smi, bool isVisible)
	{
		if (this.targetCrop.Get(smi) != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					foreach (string str in hide_symbols_after_pre)
					{
						component.SetSymbolVisiblity(str, isVisible);
					}
				}
			}
		}
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x00023B48 File Offset: 0x00021D48
	private void StoreSymbolsVisibility(CropTendingStates.Instance smi)
	{
		if (this.targetCrop.Get(smi) != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					smi.symbolStates = new bool[hide_symbols_after_pre.Length];
					for (int i = 0; i < hide_symbols_after_pre.Length; i++)
					{
						smi.symbolStates[i] = component.GetSymbolVisiblity(hide_symbols_after_pre[i]);
					}
				}
			}
		}
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00023BC8 File Offset: 0x00021DC8
	private void RestoreSymbolsVisibility(CropTendingStates.Instance smi)
	{
		if (this.targetCrop.Get(smi) != null && smi.symbolStates != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					for (int i = 0; i < hide_symbols_after_pre.Length; i++)
					{
						component.SetSymbolVisiblity(hide_symbols_after_pre[i], smi.symbolStates[i]);
					}
				}
			}
		}
	}

	// Token: 0x04000331 RID: 817
	private const int MAX_NAVIGATE_DISTANCE = 100;

	// Token: 0x04000332 RID: 818
	private const int MAX_SQR_EUCLIDEAN_DISTANCE = 625;

	// Token: 0x04000333 RID: 819
	private static CropTendingStates.AnimSet defaultAnimSet = new CropTendingStates.AnimSet
	{
		crop_tending_pre = "crop_tending_pre",
		crop_tending = "crop_tending_loop",
		crop_tending_pst = "crop_tending_pst"
	};

	// Token: 0x04000334 RID: 820
	public StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.TargetParameter targetCrop;

	// Token: 0x04000335 RID: 821
	private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State findCrop;

	// Token: 0x04000336 RID: 822
	private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State moveToCrop;

	// Token: 0x04000337 RID: 823
	private CropTendingStates.TendingStates tendCrop;

	// Token: 0x04000338 RID: 824
	private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State behaviourcomplete;

	// Token: 0x02001117 RID: 4375
	public class AnimSet
	{
		// Token: 0x040063FC RID: 25596
		public string crop_tending_pre;

		// Token: 0x040063FD RID: 25597
		public string crop_tending;

		// Token: 0x040063FE RID: 25598
		public string crop_tending_pst;

		// Token: 0x040063FF RID: 25599
		public string[] hide_symbols_after_pre;
	}

	// Token: 0x02001118 RID: 4376
	public class CropTendingEventData
	{
		// Token: 0x04006400 RID: 25600
		public GameObject source;

		// Token: 0x04006401 RID: 25601
		public Tag cropId;
	}

	// Token: 0x02001119 RID: 4377
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006402 RID: 25602
		public string effectId;

		// Token: 0x04006403 RID: 25603
		public HashedString[] ignoreEffectGroup;

		// Token: 0x04006404 RID: 25604
		public Dictionary<Tag, int> interests = new Dictionary<Tag, int>();

		// Token: 0x04006405 RID: 25605
		public Dictionary<Tag, CropTendingStates.AnimSet> animSetOverrides = new Dictionary<Tag, CropTendingStates.AnimSet>();
	}

	// Token: 0x0200111A RID: 4378
	public new class Instance : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.GameInstance
	{
		// Token: 0x060083BD RID: 33725 RVA: 0x00343D8C File Offset: 0x00341F8C
		public Instance(Chore<CropTendingStates.Instance> chore, CropTendingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToTendCrops);
			this.effect = Db.Get().effects.TryGet(base.smi.def.effectId);
		}

		// Token: 0x04006406 RID: 25606
		public Effect effect;

		// Token: 0x04006407 RID: 25607
		public int moveCell;

		// Token: 0x04006408 RID: 25608
		public CropTendingStates.AnimSet animSet;

		// Token: 0x04006409 RID: 25609
		public bool tendedSucceeded;

		// Token: 0x0400640A RID: 25610
		public bool[] symbolStates;
	}

	// Token: 0x0200111B RID: 4379
	public class TendingStates : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State
	{
		// Token: 0x0400640B RID: 25611
		public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pre;

		// Token: 0x0400640C RID: 25612
		public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State tend;

		// Token: 0x0400640D RID: 25613
		public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pst;
	}
}
