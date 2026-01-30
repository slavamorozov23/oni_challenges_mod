using System;
using STRINGS;
using UnityEngine;

// Token: 0x020008CA RID: 2250
public class DeathLoot : GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>
{
	// Token: 0x06003E58 RID: 15960 RVA: 0x0015C697 File Offset: 0x0015A897
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x04002671 RID: 9841
	private StateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.BoolParameter WasLoopDropped;

	// Token: 0x020018E6 RID: 6374
	public class Loot
	{
		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x0600A0AD RID: 41133 RVA: 0x003AA49E File Offset: 0x003A869E
		// (set) Token: 0x0600A0AC RID: 41132 RVA: 0x003AA495 File Offset: 0x003A8695
		public Tag Id { get; private set; } = Tag.Invalid;

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x0600A0AF RID: 41135 RVA: 0x003AA4AF File Offset: 0x003A86AF
		// (set) Token: 0x0600A0AE RID: 41134 RVA: 0x003AA4A6 File Offset: 0x003A86A6
		public bool IsElement { get; private set; }

		// Token: 0x0600A0B0 RID: 41136 RVA: 0x003AA4B7 File Offset: 0x003A86B7
		public Loot(Tag tag)
		{
			this.Id = tag;
			this.IsElement = false;
			this.Quantity = 1f;
		}

		// Token: 0x0600A0B1 RID: 41137 RVA: 0x003AA4E3 File Offset: 0x003A86E3
		public Loot(SimHashes element, float quantity)
		{
			this.Id = element.CreateTag();
			this.IsElement = true;
			this.Quantity = quantity;
		}

		// Token: 0x04007C65 RID: 31845
		public float Quantity;
	}

	// Token: 0x020018E7 RID: 6375
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007C66 RID: 31846
		public DeathLoot.Loot[] loot;

		// Token: 0x04007C67 RID: 31847
		public CellOffset lootSpawnOffset;
	}

	// Token: 0x020018E8 RID: 6376
	public new class Instance : GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.GameInstance
	{
		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x0600A0B3 RID: 41139 RVA: 0x003AA518 File Offset: 0x003A8718
		public bool WasLoopDropped
		{
			get
			{
				return base.sm.WasLoopDropped.Get(base.smi);
			}
		}

		// Token: 0x0600A0B4 RID: 41140 RVA: 0x003AA530 File Offset: 0x003A8730
		public Instance(IStateMachineTarget master, DeathLoot.Def def) : base(master, def)
		{
			this.onDeathHandler = base.Subscribe(1623392196, new Action<object>(this.OnDeath));
		}

		// Token: 0x0600A0B5 RID: 41141 RVA: 0x003AA55E File Offset: 0x003A875E
		private void OnDeath(object obj)
		{
			if (!this.WasLoopDropped)
			{
				base.sm.WasLoopDropped.Set(true, this, false);
				this.CreateLoot();
			}
		}

		// Token: 0x0600A0B6 RID: 41142 RVA: 0x003AA584 File Offset: 0x003A8784
		public GameObject[] CreateLoot()
		{
			if (base.def.loot == null)
			{
				return null;
			}
			GameObject[] array = new GameObject[base.def.loot.Length];
			for (int i = 0; i < base.def.loot.Length; i++)
			{
				DeathLoot.Loot loot = base.def.loot[i];
				if (!(loot.Id == Tag.Invalid))
				{
					GameObject gameObject = Scenario.SpawnPrefab(this.GetLootSpawnCell(), 0, 0, loot.Id.ToString(), Grid.SceneLayer.Ore);
					gameObject.SetActive(true);
					Edible component = gameObject.GetComponent<Edible>();
					if (component)
					{
						ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
					}
					if (loot.IsElement)
					{
						PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
						if (component2 != null)
						{
							component2.Mass = loot.Quantity;
						}
					}
					array[i] = gameObject;
				}
			}
			return array;
		}

		// Token: 0x0600A0B7 RID: 41143 RVA: 0x003AA694 File Offset: 0x003A8894
		public int GetLootSpawnCell()
		{
			int num = Grid.PosToCell(base.gameObject);
			int num2 = Grid.OffsetCell(num, base.def.lootSpawnOffset);
			if (Grid.IsWorldValidCell(num2) && Grid.IsValidCellInWorld(num2, base.gameObject.GetMyWorldId()))
			{
				return num2;
			}
			return num;
		}

		// Token: 0x0600A0B8 RID: 41144 RVA: 0x003AA6DD File Offset: 0x003A88DD
		protected override void OnCleanUp()
		{
			base.Unsubscribe(ref this.onDeathHandler);
		}

		// Token: 0x04007C68 RID: 31848
		private int onDeathHandler = -1;
	}
}
