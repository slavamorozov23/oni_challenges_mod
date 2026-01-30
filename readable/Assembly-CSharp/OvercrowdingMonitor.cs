using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008B1 RID: 2225
public class OvercrowdingMonitor : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>
{
	// Token: 0x06003D4E RID: 15694 RVA: 0x001562EC File Offset: 0x001544EC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<OvercrowdingMonitor.Instance, float>(OvercrowdingMonitor.UpdateState), UpdateRate.SIM_1000ms, true);
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x00156310 File Offset: 0x00154510
	private static int FetchCreaturePersonalSpace(KPrefabID creature)
	{
		int spaceRequiredPerCreature;
		if (OvercrowdingMonitor.personalSpaces.TryGetValue(creature.PrefabTag, out spaceRequiredPerCreature))
		{
			return spaceRequiredPerCreature;
		}
		OvercrowdingMonitor.Instance smi = creature.GetSMI<OvercrowdingMonitor.Instance>();
		if (smi != null)
		{
			spaceRequiredPerCreature = smi.def.spaceRequiredPerCreature;
		}
		OvercrowdingMonitor.personalSpaces[creature.PrefabTag] = spaceRequiredPerCreature;
		return spaceRequiredPerCreature;
	}

	// Token: 0x06003D50 RID: 15696 RVA: 0x0015635C File Offset: 0x0015455C
	private static int FetchEggPersonalSpace(KPrefabID egg)
	{
		int spaceRequiredPerCreature;
		if (OvercrowdingMonitor.personalSpaces.TryGetValue(egg.PrefabTag, out spaceRequiredPerCreature))
		{
			return spaceRequiredPerCreature;
		}
		IncubationMonitor.Instance smi = egg.GetSMI<IncubationMonitor.Instance>();
		if (OvercrowdingMonitor.personalSpaces.TryGetValue(smi.def.spawnedCreature, out spaceRequiredPerCreature))
		{
			OvercrowdingMonitor.personalSpaces[egg.PrefabTag] = spaceRequiredPerCreature;
			return spaceRequiredPerCreature;
		}
		GameObject prefab = Assets.GetPrefab(smi.def.spawnedCreature);
		if (prefab != null)
		{
			spaceRequiredPerCreature = prefab.GetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature;
		}
		OvercrowdingMonitor.personalSpaces[egg.PrefabTag] = spaceRequiredPerCreature;
		OvercrowdingMonitor.personalSpaces[smi.def.spawnedCreature] = spaceRequiredPerCreature;
		return spaceRequiredPerCreature;
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x00156400 File Offset: 0x00154600
	private static void UpdateState(OvercrowdingMonitor.Instance smi, float dt)
	{
		OvercrowdingMonitor.UpdateRegion(smi);
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return;
		}
		OvercrowdingMonitor.Occupancy occupancy = null;
		List<KPrefabID> creatures = null;
		List<KPrefabID> eggs = null;
		if (smi.IsFish)
		{
			if (smi.pond != null)
			{
				occupancy = smi.pond.occupancy;
				creatures = smi.pond.fishes;
				eggs = smi.pond.eggs;
			}
		}
		else if (smi.cavity != null)
		{
			occupancy = smi.cavity.occupancy;
			creatures = smi.cavity.creatures;
			eggs = smi.cavity.eggs;
		}
		if (occupancy != null && occupancy.dirty)
		{
			occupancy.Analyze(creatures, eggs);
		}
		if (smi.regionAnalysis.IsDirty)
		{
			smi.regionAnalysis.SetOccupancy(occupancy);
			smi.OnRegionAnalysisDirtied();
		}
		OvercrowdingMonitor.AlignTagsAndEffects(smi);
	}

	// Token: 0x06003D52 RID: 15698 RVA: 0x001564C4 File Offset: 0x001546C4
	private static void AlignTagsAndEffects(OvercrowdingMonitor.Instance smi)
	{
		bool isConfined = smi.regionAnalysis.IsConfined;
		bool isOvercrowded = smi.regionAnalysis.IsOvercrowded;
		int num = smi.regionAnalysis.OvercrowdedModifier;
		bool isFutureOvercrowded = smi.regionAnalysis.IsFutureOvercrowded;
		num = (isOvercrowded ? num : 0);
		if (isOvercrowded)
		{
			smi.overcrowded.Modifier.SetValue((float)num);
		}
		bool flag = smi.kpid.HasTag(GameTags.Creatures.Overcrowded);
		bool flag2 = smi.kpid.HasTag(GameTags.Creatures.Expecting);
		bool flag3 = smi.kpid.HasTag(GameTags.Creatures.Confined);
		bool flag4 = smi.effects.HasEffect(smi.futureOvercrowded.Effect);
		if (flag != isOvercrowded)
		{
			smi.kpid.SetTag(GameTags.Creatures.Overcrowded, isOvercrowded);
		}
		bool flag5 = !smi.isBaby && !isFutureOvercrowded;
		if (flag2 != flag5)
		{
			smi.kpid.SetTag(GameTags.Creatures.Expecting, flag5);
		}
		if (flag3 != isConfined)
		{
			smi.kpid.SetTag(GameTags.Creatures.Confined, isConfined);
		}
		bool flag6 = isConfined;
		bool flag7 = isOvercrowded && !flag6;
		bool flag8 = isFutureOvercrowded && !flag6;
		if (flag6 != flag3)
		{
			smi.confined.instance = OvercrowdingMonitor.SetEffect(smi, smi.confined.Effect, flag6, smi.confined.Tooltip);
		}
		if (flag7 != flag)
		{
			smi.overcrowded.instance = OvercrowdingMonitor.SetEffect(smi, smi.overcrowded.Effect, flag7, smi.overcrowded.Tooltip);
		}
		if (flag8 != flag4)
		{
			smi.futureOvercrowded.instance = OvercrowdingMonitor.SetEffect(smi, smi.futureOvercrowded.Effect, flag8, smi.futureOvercrowded.Tooltip);
		}
	}

	// Token: 0x06003D53 RID: 15699 RVA: 0x00156667 File Offset: 0x00154867
	private static EffectInstance SetEffect(OvercrowdingMonitor.Instance smi, Effect effect, bool set, Func<string, object, string> tooltip)
	{
		if (set)
		{
			return smi.effects.Add(effect, false, tooltip);
		}
		smi.effects.Remove(effect);
		return null;
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x00156688 File Offset: 0x00154888
	private static bool FetchIsFish(KPrefabID creature)
	{
		bool flag;
		if (OvercrowdingMonitor.isFish.TryGetValue(creature.PrefabTag, out flag))
		{
			return flag;
		}
		flag = (creature.GetDef<FishOvercrowdingMonitor.Def>() != null);
		OvercrowdingMonitor.isFish[creature.PrefabTag] = flag;
		return flag;
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x001566C8 File Offset: 0x001548C8
	private static bool FetchIsFishEgg(KPrefabID egg)
	{
		bool flag;
		if (OvercrowdingMonitor.isFish.TryGetValue(egg.PrefabTag, out flag))
		{
			return flag;
		}
		IncubationMonitor.Instance smi = egg.GetSMI<IncubationMonitor.Instance>();
		if (OvercrowdingMonitor.isFish.TryGetValue(smi.def.spawnedCreature, out flag))
		{
			OvercrowdingMonitor.isFish[egg.PrefabTag] = flag;
			return flag;
		}
		GameObject prefab = Assets.GetPrefab(smi.def.spawnedCreature);
		if (prefab != null)
		{
			flag = (prefab.GetDef<FishOvercrowdingMonitor.Def>() != null);
		}
		OvercrowdingMonitor.isFish[egg.PrefabTag] = flag;
		OvercrowdingMonitor.isFish[smi.def.spawnedCreature] = flag;
		return flag;
	}

	// Token: 0x06003D56 RID: 15702 RVA: 0x0015676C File Offset: 0x0015496C
	private static void UpdateRegion(OvercrowdingMonitor.Instance smi)
	{
		int cell = Grid.PosToCell(smi);
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
		if (cavityForCell != smi.cavity)
		{
			if (smi.cavity != null)
			{
				smi.RemoveFromCavity();
				Game.Instance.roomProber.UpdateRoom(cavityForCell);
				smi.regionAnalysis.ForceDirty();
			}
			smi.cavity = cavityForCell;
			if (smi.cavity != null)
			{
				smi.AddToCavity();
				Game.Instance.roomProber.UpdateRoom(smi.cavity);
				smi.regionAnalysis.ForceDirty();
			}
		}
		if (smi.IsFish)
		{
			FishOvercrowingManager.Pond pond = FishOvercrowingManager.Instance.GetPond(cell);
			if (pond != smi.pond)
			{
				if (smi.pond != null)
				{
					smi.regionAnalysis.ForceDirty();
				}
				smi.pond = pond;
				if (smi.pond != null)
				{
					smi.regionAnalysis.ForceDirty();
				}
			}
		}
	}

	// Token: 0x040025D6 RID: 9686
	public const float OVERCROWDED_FERTILITY_DEBUFF = -1f;

	// Token: 0x040025D7 RID: 9687
	public static readonly Tag[] CONFINEMENT_IMMUNITY_TAGS = new Tag[]
	{
		GameTags.Creatures.Burrowed,
		GameTags.Creatures.Digger
	};

	// Token: 0x040025D8 RID: 9688
	private static readonly Dictionary<Tag, int> personalSpaces = new Dictionary<Tag, int>();

	// Token: 0x040025D9 RID: 9689
	private static readonly Dictionary<Tag, bool> isFish = new Dictionary<Tag, bool>();

	// Token: 0x020018B2 RID: 6322
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007BB8 RID: 31672
		public int spaceRequiredPerCreature;
	}

	// Token: 0x020018B3 RID: 6323
	public class Occupancy
	{
		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06009FE8 RID: 40936 RVA: 0x003A805C File Offset: 0x003A625C
		public Dictionary<Tag, int> CritterCounts { get; } = new Dictionary<Tag, int>();

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06009FE9 RID: 40937 RVA: 0x003A8064 File Offset: 0x003A6264
		// (set) Token: 0x06009FEA RID: 40938 RVA: 0x003A806C File Offset: 0x003A626C
		public int OccupiedCellCount { get; private set; }

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06009FEB RID: 40939 RVA: 0x003A8075 File Offset: 0x003A6275
		// (set) Token: 0x06009FEC RID: 40940 RVA: 0x003A807D File Offset: 0x003A627D
		public int HatchedEggOccupiedCellCount { get; private set; }

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06009FED RID: 40941 RVA: 0x003A8086 File Offset: 0x003A6286
		// (set) Token: 0x06009FEE RID: 40942 RVA: 0x003A808E File Offset: 0x003A628E
		public int Generation { get; private set; }

		// Token: 0x06009FEF RID: 40943 RVA: 0x003A8098 File Offset: 0x003A6298
		public void Analyze(List<KPrefabID> creatures, List<KPrefabID> eggs)
		{
			DebugUtil.DevAssert(this.dirty, "Only incur Analyze overhead when dirty", null);
			this.CritterCounts.EnsureCapacity(creatures.Count);
			this.CritterCounts.Clear();
			this.OccupiedCellCount = 0;
			this.HatchedEggOccupiedCellCount = 0;
			creatures.RemoveAll(new Predicate<KPrefabID>(Util.IsNullOrDestroyed));
			foreach (KPrefabID kprefabID in creatures)
			{
				this.OccupiedCellCount += OvercrowdingMonitor.FetchCreaturePersonalSpace(kprefabID);
				int num;
				if (this.CritterCounts.TryGetValue(kprefabID.PrefabTag, out num))
				{
					this.CritterCounts[kprefabID.PrefabTag] = num + 1;
				}
				else
				{
					this.CritterCounts[kprefabID.PrefabTag] = 1;
				}
			}
			eggs.RemoveAll(new Predicate<KPrefabID>(Util.IsNullOrDestroyed));
			foreach (KPrefabID egg in eggs)
			{
				this.HatchedEggOccupiedCellCount += OvercrowdingMonitor.FetchEggPersonalSpace(egg);
			}
			int generation = this.Generation;
			this.Generation = generation + 1;
			this.dirty = false;
		}

		// Token: 0x04007BBD RID: 31677
		public bool dirty = true;
	}

	// Token: 0x020018B4 RID: 6324
	public struct RegionAnalysis
	{
		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06009FF1 RID: 40945 RVA: 0x003A820E File Offset: 0x003A640E
		public readonly bool IsDirty
		{
			get
			{
				return this.occupancy == null || this.occupancyGeneration != this.occupancy.Generation;
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06009FF2 RID: 40946 RVA: 0x003A8230 File Offset: 0x003A6430
		public readonly int CellCount
		{
			get
			{
				return this.smi.RegionSize;
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06009FF3 RID: 40947 RVA: 0x003A823D File Offset: 0x003A643D
		public readonly bool IsPond
		{
			get
			{
				return this.smi.IsInPond;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06009FF4 RID: 40948 RVA: 0x003A824A File Offset: 0x003A644A
		public readonly Dictionary<Tag, int> CritterCounts
		{
			get
			{
				if (this.occupancy == null)
				{
					return null;
				}
				return this.occupancy.CritterCounts;
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06009FF5 RID: 40949 RVA: 0x003A8261 File Offset: 0x003A6461
		public readonly int OccupiedCellCount
		{
			get
			{
				if (this.occupancy == null)
				{
					return 0;
				}
				return this.occupancy.OccupiedCellCount;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06009FF6 RID: 40950 RVA: 0x003A8278 File Offset: 0x003A6478
		public readonly int HatchedEggOccupiedCellCount
		{
			get
			{
				if (this.occupancy == null)
				{
					return 0;
				}
				return this.occupancy.HatchedEggOccupiedCellCount;
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06009FF7 RID: 40951 RVA: 0x003A828F File Offset: 0x003A648F
		public readonly bool IsOvercrowded
		{
			get
			{
				return !this.IsDegeneratePersonalSpace && this.UnoccupiedCellCount < 0;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06009FF8 RID: 40952 RVA: 0x003A82A4 File Offset: 0x003A64A4
		public readonly bool IsConfined
		{
			get
			{
				return !this.ConfinementImmunity && !this.IsDegeneratePersonalSpace && this.CellCount < this.PersonalSpace;
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06009FF9 RID: 40953 RVA: 0x003A82C6 File Offset: 0x003A64C6
		public readonly bool IsFutureOvercrowded
		{
			get
			{
				return !this.IsDegeneratePersonalSpace && this.FutureUnoccupiedCellCount < 0 && this.HatchedEggOccupiedCellCount > 0;
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06009FFA RID: 40954 RVA: 0x003A82E4 File Offset: 0x003A64E4
		public readonly int OvercrowdedModifier
		{
			get
			{
				if (!this.IsOvercrowded)
				{
					return 0;
				}
				return -this.OverOccupiedCritterCount;
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06009FFB RID: 40955 RVA: 0x003A82F7 File Offset: 0x003A64F7
		public readonly bool IsDegenerate
		{
			get
			{
				return this.CellCount <= 0;
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06009FFC RID: 40956 RVA: 0x003A8305 File Offset: 0x003A6505
		private readonly int PersonalSpace
		{
			get
			{
				return this.smi.def.spaceRequiredPerCreature;
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06009FFD RID: 40957 RVA: 0x003A8317 File Offset: 0x003A6517
		private readonly bool ConfinementImmunity
		{
			get
			{
				return this.smi.kpid.HasAnyTags(OvercrowdingMonitor.CONFINEMENT_IMMUNITY_TAGS);
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06009FFE RID: 40958 RVA: 0x003A832E File Offset: 0x003A652E
		private readonly int UnoccupiedCellCount
		{
			get
			{
				return this.CellCount - this.OccupiedCellCount;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06009FFF RID: 40959 RVA: 0x003A833D File Offset: 0x003A653D
		private readonly int OverOccupiedCellCount
		{
			get
			{
				return this.OccupiedCellCount - this.CellCount;
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x0600A000 RID: 40960 RVA: 0x003A834C File Offset: 0x003A654C
		private readonly int FutureUnoccupiedCellCount
		{
			get
			{
				return this.UnoccupiedCellCount - this.HatchedEggOccupiedCellCount;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x0600A001 RID: 40961 RVA: 0x003A835B File Offset: 0x003A655B
		private readonly bool IsDegeneratePersonalSpace
		{
			get
			{
				return this.PersonalSpace == 0;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x0600A002 RID: 40962 RVA: 0x003A8366 File Offset: 0x003A6566
		private readonly int OverOccupiedCritterCount
		{
			get
			{
				return this.ComputeOverOccupiedCritterCount(this.PersonalSpace);
			}
		}

		// Token: 0x0600A003 RID: 40963 RVA: 0x003A8374 File Offset: 0x003A6574
		public RegionAnalysis(OvercrowdingMonitor.Instance smi)
		{
			this.smi = smi;
			this.occupancy = null;
			this.occupancyGeneration = -1;
		}

		// Token: 0x0600A004 RID: 40964 RVA: 0x003A838B File Offset: 0x003A658B
		public void SetOccupancy(OvercrowdingMonitor.Occupancy occupancy)
		{
			this.occupancy = occupancy;
			this.occupancyGeneration = ((occupancy != null) ? occupancy.Generation : -1);
		}

		// Token: 0x0600A005 RID: 40965 RVA: 0x003A83A6 File Offset: 0x003A65A6
		public void ForceDirty()
		{
			this.occupancyGeneration = -1;
		}

		// Token: 0x0600A006 RID: 40966 RVA: 0x003A83B0 File Offset: 0x003A65B0
		public readonly string Substitute(string s)
		{
			LocString loc_string = this.IsPond ? ((this.CellCount == 0) ? CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION_AQUATIC.NO_CELLS : ((this.CellCount == 1) ? CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION_AQUATIC.SINGLE_CELL : CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION_AQUATIC.MULTIPLE_CELLS)) : ((this.CellCount == 0) ? CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION.NO_CELLS : ((this.CellCount == 1) ? CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION.SINGLE_CELL : CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION.MULTIPLE_CELLS));
			return s.Replace("{explanation}", loc_string).Replace("{contextCritterType}", this.smi.kpid.PrefabTag.ProperName()).Replace("{personalSpace}", string.Format("{0}", this.PersonalSpace)).Replace("{cellCount}", string.Format("{0}", this.CellCount)).Replace("{occupiedCellCount}", string.Format("{0}", this.OccupiedCellCount)).Replace("{unoccupiedCellCount}", string.Format("{0}", this.IsOvercrowded ? 0 : this.UnoccupiedCellCount)).Replace("{overOccupiedCellCount}", string.Format("{0}", this.IsOvercrowded ? this.OverOccupiedCellCount : 0)).Replace("{bullets}", this.BuildCritterOccupancies());
		}

		// Token: 0x0600A007 RID: 40967 RVA: 0x003A8504 File Offset: 0x003A6704
		private readonly int ComputeOverOccupiedCritterCount(int personalSpace)
		{
			if (personalSpace == 0)
			{
				return 0;
			}
			int num;
			return Math.DivRem(this.OverOccupiedCellCount, personalSpace, out num) + ((num == 0) ? 0 : 1);
		}

		// Token: 0x0600A008 RID: 40968 RVA: 0x003A852C File Offset: 0x003A672C
		private readonly string BuildCritterOccupancies()
		{
			if (this.CritterCounts == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
			ListPool<OvercrowdingMonitor.RegionAnalysis.CritterOccupancy, OvercrowdingMonitor.RegionAnalysis>.PooledList pooledList = ListPool<OvercrowdingMonitor.RegionAnalysis.CritterOccupancy, OvercrowdingMonitor.RegionAnalysis>.Allocate();
			pooledList.Capacity = this.CritterCounts.Count;
			foreach (Tag tag in this.CritterCounts.Keys)
			{
				int personalSpace = OvercrowdingMonitor.personalSpaces[tag];
				int num = this.ComputeOverOccupiedCritterCount(personalSpace);
				int num2 = this.CritterCounts[tag];
				bool canFix = num <= num2;
				num = Math.Min(this.CritterCounts[tag], num);
				pooledList.Add(new OvercrowdingMonitor.RegionAnalysis.CritterOccupancy
				{
					critterType = tag,
					overOccupancy = num,
					canFix = canFix
				});
			}
			Dictionary<Tag, int> capturedCritterCounts = this.CritterCounts;
			Dictionary<Tag, int> capturedPersonalSpaces = OvercrowdingMonitor.personalSpaces;
			pooledList.Sort(delegate(OvercrowdingMonitor.RegionAnalysis.CritterOccupancy a, OvercrowdingMonitor.RegionAnalysis.CritterOccupancy b)
			{
				int value2 = capturedCritterCounts[a.critterType] * capturedPersonalSpaces[a.critterType];
				return (capturedCritterCounts[b.critterType] * capturedPersonalSpaces[b.critterType]).CompareTo(value2);
			});
			foreach (OvercrowdingMonitor.RegionAnalysis.CritterOccupancy critterOccupancy in pooledList)
			{
				int num3 = this.CritterCounts[critterOccupancy.critterType];
				LocString locString = (num3 == 1) ? (critterOccupancy.canFix ? CREATURES.MODIFIERS.OVERCROWDED.BULLET.CAN_FIX.SINGULAR : CREATURES.MODIFIERS.OVERCROWDED.BULLET.CANNOT_FIX.SINGULAR) : (critterOccupancy.canFix ? CREATURES.MODIFIERS.OVERCROWDED.BULLET.CAN_FIX.MULTIPLE : CREATURES.MODIFIERS.OVERCROWDED.BULLET.CANNOT_FIX.MULTIPLE);
				int num4 = OvercrowdingMonitor.personalSpaces[critterOccupancy.critterType];
				int num5 = this.OccupiedCellCount - critterOccupancy.overOccupancy * num4;
				string value = locString.Replace("{critterType}", critterOccupancy.critterType.ProperName()).Replace("{critterCount}", string.Format("{0}", num3)).Replace("{personalSpace}", string.Format("{0}", num4)).Replace("{overOccupancy}", string.Format("{0}", critterOccupancy.overOccupancy)).Replace("{cellCountWithFix}", string.Format("{0}", num5));
				stringBuilder.AppendLine(value);
			}
			if (this.CritterCounts.Count > 0)
			{
				stringBuilder.Length--;
			}
			pooledList.Recycle();
			return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
		}

		// Token: 0x04007BBE RID: 31678
		private OvercrowdingMonitor.Occupancy occupancy;

		// Token: 0x04007BBF RID: 31679
		private int occupancyGeneration;

		// Token: 0x04007BC0 RID: 31680
		private readonly OvercrowdingMonitor.Instance smi;

		// Token: 0x0200298F RID: 10639
		private struct CritterOccupancy
		{
			// Token: 0x0400B7C4 RID: 47044
			public Tag critterType;

			// Token: 0x0400B7C5 RID: 47045
			public int overOccupancy;

			// Token: 0x0400B7C6 RID: 47046
			public bool canFix;
		}
	}

	// Token: 0x020018B5 RID: 6325
	public struct OvercrowdEffect
	{
		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x0600A009 RID: 40969 RVA: 0x003A87CC File Offset: 0x003A69CC
		// (set) Token: 0x0600A00A RID: 40970 RVA: 0x003A87D4 File Offset: 0x003A69D4
		public Effect Effect { readonly get; private set; }

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x0600A00B RID: 40971 RVA: 0x003A87DD File Offset: 0x003A69DD
		// (set) Token: 0x0600A00C RID: 40972 RVA: 0x003A87E5 File Offset: 0x003A69E5
		public AttributeModifier Modifier { readonly get; private set; }

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x0600A00D RID: 40973 RVA: 0x003A87EE File Offset: 0x003A69EE
		// (set) Token: 0x0600A00E RID: 40974 RVA: 0x003A87F6 File Offset: 0x003A69F6
		public Func<string, object, string> Tooltip { readonly get; private set; }

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x0600A00F RID: 40975 RVA: 0x003A87FF File Offset: 0x003A69FF
		// (set) Token: 0x0600A010 RID: 40976 RVA: 0x003A8807 File Offset: 0x003A6A07
		public string TooltipText { readonly get; private set; }

		// Token: 0x0600A011 RID: 40977 RVA: 0x003A8810 File Offset: 0x003A6A10
		public OvercrowdEffect(string id, string name, AttributeModifier modifier, Func<string> GenerateTooltip)
		{
			this.Effect = new Effect(id, name, string.Empty, 0f, true, false, true, null, -1f, 0f, null, "");
			this.instance = null;
			this.Modifier = modifier;
			this.Tooltip = null;
			this.Effect.Add(modifier);
			this.TooltipText = null;
			OvercrowdingMonitor.OvercrowdEffect capturedThis = this;
			this.Tooltip = delegate(string _tooltip, object untypedEffectInstance)
			{
				if (capturedThis.TooltipText == null)
				{
					EffectInstance effectInstance = (EffectInstance)untypedEffectInstance;
					capturedThis.TooltipText = effectInstance.ResolveTooltip(GenerateTooltip(), untypedEffectInstance);
				}
				return capturedThis.TooltipText;
			};
		}

		// Token: 0x0600A012 RID: 40978 RVA: 0x003A889C File Offset: 0x003A6A9C
		public OvercrowdEffect(string id, string name, AttributeModifier modifier, string tooltipFormat)
		{
			this = new OvercrowdingMonitor.OvercrowdEffect(id, name, modifier, () => tooltipFormat);
		}

		// Token: 0x0600A013 RID: 40979 RVA: 0x003A88CC File Offset: 0x003A6ACC
		public void ClearTooltip()
		{
			this.TooltipText = null;
		}

		// Token: 0x04007BC2 RID: 31682
		public EffectInstance instance;
	}

	// Token: 0x020018B6 RID: 6326
	public new class Instance : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>.GameInstance
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x0600A014 RID: 40980 RVA: 0x003A88D5 File Offset: 0x003A6AD5
		public int RegionSize
		{
			get
			{
				if (!this.IsFish)
				{
					if (this.cavity == null)
					{
						return 0;
					}
					return this.cavity.NumCells;
				}
				else
				{
					if (this.pond == null)
					{
						return 0;
					}
					return this.pond.cellCount;
				}
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x0600A015 RID: 40981 RVA: 0x003A890A File Offset: 0x003A6B0A
		public bool IsInPond
		{
			get
			{
				return this.pond != null;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x0600A016 RID: 40982 RVA: 0x003A8915 File Offset: 0x003A6B15
		public bool IsFish
		{
			get
			{
				return OvercrowdingMonitor.FetchIsFish(this.kpid);
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x0600A017 RID: 40983 RVA: 0x003A8922 File Offset: 0x003A6B22
		public bool IsEgg
		{
			get
			{
				return this.kpid.HasTag(GameTags.Egg);
			}
		}

		// Token: 0x0600A018 RID: 40984 RVA: 0x003A8934 File Offset: 0x003A6B34
		public Instance(IStateMachineTarget master, OvercrowdingMonitor.Def def) : base(master, def)
		{
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			this.isBaby = (def2 != null);
			this.futureOvercrowded = new OvercrowdingMonitor.OvercrowdEffect("FutureOvercrowded", CREATURES.MODIFIERS.OVERCROWDED.CRAMPED.NAME, new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.OVERCROWDED.CRAMPED.NAME, true, false, true), new Func<string>(this.<.ctor>g__FutureOvercrowdedTooltip|18_0));
			this.overcrowded = new OvercrowdingMonitor.OvercrowdEffect("Overcrowded", CREATURES.MODIFIERS.OVERCROWDED.CROWDED.NAME, new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 0f, CREATURES.MODIFIERS.OVERCROWDED.CROWDED.NAME, false, false, false), new Func<string>(this.<.ctor>g__OvercrowdedTooltip|18_1));
			this.confined = new OvercrowdingMonitor.OvercrowdEffect("Confined", CREATURES.MODIFIERS.OVERCROWDED.CONFINED.NAME, new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, CREATURES.MODIFIERS.OVERCROWDED.CONFINED.NAME, false, false, false), new Func<string>(this.<.ctor>g__ConfinedTooltip|18_2));
			this.confined.Effect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.OVERCROWDED.CONFINED.NAME, true, false, true));
			this.onRoomUpdatedHandle = base.Subscribe(144050788, new Action<object>(this.OnRoomUpdated));
			this.regionAnalysis = new OvercrowdingMonitor.RegionAnalysis(this);
			OvercrowdingMonitor.UpdateState(this, 0f);
		}

		// Token: 0x0600A019 RID: 40985 RVA: 0x003A8AD2 File Offset: 0x003A6CD2
		public void OnRegionAnalysisDirtied()
		{
			this.futureOvercrowded.ClearTooltip();
			this.overcrowded.ClearTooltip();
			this.confined.ClearTooltip();
		}

		// Token: 0x0600A01A RID: 40986 RVA: 0x003A8AF8 File Offset: 0x003A6CF8
		public void AddToCavity()
		{
			if (this.IsEgg)
			{
				this.cavity.eggs.Add(this.kpid);
				if (OvercrowdingMonitor.FetchIsFishEgg(this.kpid))
				{
					this.cavity.fish_eggs.Add(this.kpid);
				}
			}
			else
			{
				this.cavity.creatures.Add(this.kpid);
				if (this.IsFish)
				{
					this.cavity.fishes.Add(this.kpid);
				}
			}
			this.cavity.occupancy.dirty = true;
		}

		// Token: 0x0600A01B RID: 40987 RVA: 0x003A8B90 File Offset: 0x003A6D90
		public void RemoveFromCavity()
		{
			if (this.IsEgg)
			{
				this.cavity.RemoveFromCavity(this.kpid, this.cavity.eggs);
				if (OvercrowdingMonitor.FetchIsFishEgg(this.kpid))
				{
					this.cavity.RemoveFromCavity(this.kpid, this.cavity.fish_eggs);
				}
			}
			else
			{
				this.cavity.RemoveFromCavity(this.kpid, this.cavity.creatures);
				if (this.IsFish)
				{
					this.cavity.RemoveFromCavity(this.kpid, this.cavity.fishes);
				}
			}
			this.cavity.occupancy.dirty = true;
		}

		// Token: 0x0600A01C RID: 40988 RVA: 0x003A8C3D File Offset: 0x003A6E3D
		protected override void OnCleanUp()
		{
			base.Unsubscribe(ref this.onRoomUpdatedHandle);
			if (this.cavity == null)
			{
				return;
			}
			this.RemoveFromCavity();
		}

		// Token: 0x0600A01D RID: 40989 RVA: 0x003A8C5A File Offset: 0x003A6E5A
		public void OnRoomUpdated(object o)
		{
			if (o == null)
			{
				this.RoomRefreshUpdateCavity();
			}
		}

		// Token: 0x0600A01E RID: 40990 RVA: 0x003A8C65 File Offset: 0x003A6E65
		public void RoomRefreshUpdateCavity()
		{
			OvercrowdingMonitor.UpdateState(this, 0f);
		}

		// Token: 0x0600A01F RID: 40991 RVA: 0x003A8C72 File Offset: 0x003A6E72
		[CompilerGenerated]
		private string <.ctor>g__FutureOvercrowdedTooltip|18_0()
		{
			return this.regionAnalysis.Substitute(this.IsFish ? CREATURES.MODIFIERS.OVERCROWDED.CRAMPED.FISHTOOLTIP : CREATURES.MODIFIERS.OVERCROWDED.CRAMPED.TOOLTIP);
		}

		// Token: 0x0600A020 RID: 40992 RVA: 0x003A8C98 File Offset: 0x003A6E98
		[CompilerGenerated]
		private string <.ctor>g__OvercrowdedTooltip|18_1()
		{
			return this.regionAnalysis.Substitute(this.IsFish ? CREATURES.MODIFIERS.OVERCROWDED.CROWDED.FISHTOOLTIP : CREATURES.MODIFIERS.OVERCROWDED.CROWDED.TOOLTIP);
		}

		// Token: 0x0600A021 RID: 40993 RVA: 0x003A8CBE File Offset: 0x003A6EBE
		[CompilerGenerated]
		private string <.ctor>g__ConfinedTooltip|18_2()
		{
			return this.regionAnalysis.Substitute(this.regionAnalysis.IsDegenerate ? CREATURES.MODIFIERS.OVERCROWDED.CONFINED.TOOLTIP_NO_SUBSTITUTIONS : CREATURES.MODIFIERS.OVERCROWDED.CONFINED.TOOLTIP);
		}

		// Token: 0x04007BC6 RID: 31686
		public CavityInfo cavity;

		// Token: 0x04007BC7 RID: 31687
		public FishOvercrowingManager.Pond pond;

		// Token: 0x04007BC8 RID: 31688
		public bool isBaby;

		// Token: 0x04007BC9 RID: 31689
		public OvercrowdingMonitor.OvercrowdEffect futureOvercrowded;

		// Token: 0x04007BCA RID: 31690
		public OvercrowdingMonitor.OvercrowdEffect overcrowded;

		// Token: 0x04007BCB RID: 31691
		public OvercrowdingMonitor.OvercrowdEffect confined;

		// Token: 0x04007BCC RID: 31692
		public OvercrowdingMonitor.RegionAnalysis regionAnalysis;

		// Token: 0x04007BCD RID: 31693
		[MyCmpReq]
		public KPrefabID kpid;

		// Token: 0x04007BCE RID: 31694
		[MyCmpReq]
		public Effects effects;

		// Token: 0x04007BCF RID: 31695
		private int onRoomUpdatedHandle = -1;
	}
}
