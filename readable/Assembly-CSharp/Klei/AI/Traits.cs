using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200105A RID: 4186
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Traits")]
	public class Traits : KMonoBehaviour, ISaveLoadable
	{
		// Token: 0x0600819F RID: 33183 RVA: 0x0033FB52 File Offset: 0x0033DD52
		public List<string> GetTraitIds()
		{
			return this.TraitIds;
		}

		// Token: 0x060081A0 RID: 33184 RVA: 0x0033FB5A File Offset: 0x0033DD5A
		public void SetTraitIds(List<string> traits)
		{
			this.TraitIds = traits;
		}

		// Token: 0x060081A1 RID: 33185 RVA: 0x0033FB64 File Offset: 0x0033DD64
		protected override void OnSpawn()
		{
			foreach (string id in this.TraitIds)
			{
				if (Db.Get().traits.Exists(id))
				{
					Trait trait = Db.Get().traits.Get(id);
					if (Game.IsCorrectDlcActiveForCurrentSave(trait))
					{
						this.AddInternal(trait);
					}
				}
			}
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 15))
			{
				List<DUPLICANTSTATS.TraitVal> joytraits = DUPLICANTSTATS.JOYTRAITS;
				if (base.GetComponent<MinionIdentity>())
				{
					bool flag = true;
					foreach (DUPLICANTSTATS.TraitVal traitVal in joytraits)
					{
						if (this.HasTrait(traitVal.id))
						{
							flag = false;
						}
					}
					if (flag)
					{
						DUPLICANTSTATS.TraitVal random = joytraits.GetRandom<DUPLICANTSTATS.TraitVal>();
						Trait trait2 = Db.Get().traits.Get(random.id);
						this.Add(trait2);
					}
				}
			}
		}

		// Token: 0x060081A2 RID: 33186 RVA: 0x0033FC8C File Offset: 0x0033DE8C
		private void AddInternal(Trait trait)
		{
			if (!this.HasTrait(trait))
			{
				this.TraitList.Add(trait);
				trait.AddTo(this.GetAttributes());
				if (trait.OnAddTrait != null)
				{
					trait.OnAddTrait(base.gameObject);
				}
			}
		}

		// Token: 0x060081A3 RID: 33187 RVA: 0x0033FCC8 File Offset: 0x0033DEC8
		public void Add(Trait trait)
		{
			DebugUtil.Assert(base.IsInitialized() || base.GetComponent<Modifiers>().IsInitialized(), "Tried adding a trait on a prefab, use Modifiers.initialTraits instead!", trait.Name, base.gameObject.name);
			if (trait.ShouldSave)
			{
				this.TraitIds.Add(trait.Id);
			}
			this.AddInternal(trait);
		}

		// Token: 0x060081A4 RID: 33188 RVA: 0x0033FD28 File Offset: 0x0033DF28
		public bool HasTrait(string trait_id)
		{
			bool result = false;
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == trait_id)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060081A5 RID: 33189 RVA: 0x0033FD88 File Offset: 0x0033DF88
		public bool HasTrait(Trait trait)
		{
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == trait)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060081A6 RID: 33190 RVA: 0x0033FDE0 File Offset: 0x0033DFE0
		public void Clear()
		{
			while (this.TraitList.Count > 0)
			{
				this.Remove(this.TraitList[0]);
			}
		}

		// Token: 0x060081A7 RID: 33191 RVA: 0x0033FE04 File Offset: 0x0033E004
		public void Remove(Trait trait)
		{
			for (int i = 0; i < this.TraitList.Count; i++)
			{
				if (this.TraitList[i] == trait)
				{
					this.TraitList.RemoveAt(i);
					this.TraitIds.Remove(trait.Id);
					trait.RemoveFrom(this.GetAttributes());
					return;
				}
			}
		}

		// Token: 0x060081A8 RID: 33192 RVA: 0x0033FE64 File Offset: 0x0033E064
		public bool IsEffectIgnored(Effect effect)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.ignoredEffects != null && Array.IndexOf<string>(trait.ignoredEffects, effect.Id) != -1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060081A9 RID: 33193 RVA: 0x0033FED4 File Offset: 0x0033E0D4
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroup, out trait);
		}

		// Token: 0x060081AA RID: 33194 RVA: 0x0033FEEA File Offset: 0x0033E0EA
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup, out Trait disablingTrait)
		{
			return this.IsChoreGroupDisabled(choreGroup.IdHash, out disablingTrait);
		}

		// Token: 0x060081AB RID: 33195 RVA: 0x0033FEFC File Offset: 0x0033E0FC
		public bool IsChoreGroupDisabled(HashedString choreGroupId)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroupId, out trait);
		}

		// Token: 0x060081AC RID: 33196 RVA: 0x0033FF14 File Offset: 0x0033E114
		public bool IsChoreGroupDisabled(HashedString choreGroupId, out Trait disablingTrait)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == choreGroupId)
						{
							disablingTrait = trait;
							return true;
						}
					}
				}
			}
			disablingTrait = null;
			return false;
		}

		// Token: 0x04006212 RID: 25106
		public List<Trait> TraitList = new List<Trait>();

		// Token: 0x04006213 RID: 25107
		[Serialize]
		private List<string> TraitIds = new List<string>();
	}
}
