using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001058 RID: 4184
	public class TraitUtil
	{
		// Token: 0x06008190 RID: 33168 RVA: 0x0033F816 File Offset: 0x0033DA16
		public static System.Action CreateDisabledTaskTrait(string id, string name, string desc, string disabled_chore_group, bool is_valid_starter_trait)
		{
			return delegate()
			{
				ChoreGroup[] disabled_chore_groups = new ChoreGroup[]
				{
					Db.Get().ChoreGroups.Get(disabled_chore_group)
				};
				Db.Get().CreateTrait(id, name, desc, null, true, disabled_chore_groups, false, is_valid_starter_trait);
			};
		}

		// Token: 0x06008191 RID: 33169 RVA: 0x0033F84C File Offset: 0x0033DA4C
		public static System.Action CreateTrait(string id, string name, string desc, string attributeId, float delta, string[] chore_groups, bool positiveTrait = false)
		{
			return delegate()
			{
				List<ChoreGroup> list = new List<ChoreGroup>();
				foreach (string id2 in chore_groups)
				{
					list.Add(Db.Get().ChoreGroups.Get(id2));
				}
				Db.Get().CreateTrait(id, name, desc, null, true, list.ToArray(), positiveTrait, true).Add(new AttributeModifier(attributeId, delta, name, false, false, true));
			};
		}

		// Token: 0x06008192 RID: 33170 RVA: 0x0033F8A0 File Offset: 0x0033DAA0
		public static System.Action CreateAttributeEffectTrait(string id, string name, string desc, string attributeId, float delta, string attributeId2, float delta2, bool positiveTrait = false)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
				trait.Add(new AttributeModifier(attributeId, delta, name, false, false, true));
				trait.Add(new AttributeModifier(attributeId2, delta2, name, false, false, true));
			};
		}

		// Token: 0x06008193 RID: 33171 RVA: 0x0033F8F9 File Offset: 0x0033DAF9
		public static System.Action CreateAttributeEffectTrait(string id, string name, string desc, string[] attributeIds, float[] deltas, bool positiveTrait = false)
		{
			return delegate()
			{
				global::Debug.Assert(attributeIds.Length == deltas.Length, "CreateAttributeEffectTrait must have an equal number of attributeIds and deltas");
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
				for (int i = 0; i < attributeIds.Length; i++)
				{
					trait.Add(new AttributeModifier(attributeIds[i], deltas[i], name, false, false, true));
				}
			};
		}

		// Token: 0x06008194 RID: 33172 RVA: 0x0033F938 File Offset: 0x0033DB38
		public static System.Action CreateAttributeEffectTrait(string id, string name, string desc, string attributeId, float delta, bool positiveTrait = false, Action<GameObject> on_add = null, bool is_valid_starter_trait = true)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, is_valid_starter_trait);
				trait.Add(new AttributeModifier(attributeId, delta, name, false, false, true));
				trait.OnAddTrait = on_add;
			};
		}

		// Token: 0x06008195 RID: 33173 RVA: 0x0033F991 File Offset: 0x0033DB91
		public static System.Action CreateEffectModifierTrait(string id, string name, string desc, string[] ignoredEffects, bool positiveTrait = false)
		{
			return delegate()
			{
				Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true).AddIgnoredEffects(ignoredEffects);
			};
		}

		// Token: 0x06008196 RID: 33174 RVA: 0x0033F9C7 File Offset: 0x0033DBC7
		public static System.Action CreateNamedTrait(string id, string name, string desc, bool positiveTrait = false)
		{
			return delegate()
			{
				Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
			};
		}

		// Token: 0x06008197 RID: 33175 RVA: 0x0033F9F8 File Offset: 0x0033DBF8
		public static System.Action CreateTrait(string id, string name, string desc, Action<GameObject> on_add, ChoreGroup[] disabled_chore_groups = null, bool positiveTrait = false, Func<string> extendedDescFn = null)
		{
			return TraitUtil.CreateTrait(id, name, desc, on_add, null, null, disabled_chore_groups, positiveTrait, extendedDescFn);
		}

		// Token: 0x06008198 RID: 33176 RVA: 0x0033FA18 File Offset: 0x0033DC18
		public static System.Action CreateTrait(string id, string name, string desc, Action<GameObject> on_add, string[] requiredDlcIds, string[] forbiddenDlcIds = null, ChoreGroup[] disabled_chore_groups = null, bool positiveTrait = false, Func<string> extendedDescFn = null)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, disabled_chore_groups, positiveTrait, true, requiredDlcIds, forbiddenDlcIds);
				trait.OnAddTrait = on_add;
				if (extendedDescFn != null)
				{
					Trait trait2 = trait;
					trait2.ExtendedTooltip = (Func<string>)Delegate.Combine(trait2.ExtendedTooltip, extendedDescFn);
				}
			};
		}

		// Token: 0x06008199 RID: 33177 RVA: 0x0033FA79 File Offset: 0x0033DC79
		public static System.Action CreateComponentTrait<T>(string id, string name, string desc, bool positiveTrait = false, Func<string> extendedDescFn = null) where T : KMonoBehaviour
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
				trait.OnAddTrait = delegate(GameObject go)
				{
					go.FindOrAddUnityComponent<T>();
				};
				if (extendedDescFn != null)
				{
					Trait trait2 = trait;
					trait2.ExtendedTooltip = (Func<string>)Delegate.Combine(trait2.ExtendedTooltip, extendedDescFn);
				}
			};
		}

		// Token: 0x0600819A RID: 33178 RVA: 0x0033FAAF File Offset: 0x0033DCAF
		public static System.Action CreateSkillGrantingTrait(string id, string name, string desc, string skillId)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
				trait.TooltipCB = (() => string.Format(DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_DESC, desc, SkillWidget.SkillPerksString(Db.Get().Skills.Get(skillId))));
				trait.OnAddTrait = delegate(GameObject go)
				{
					MinionResume component = go.GetComponent<MinionResume>();
					if (component != null)
					{
						component.GrantSkill(skillId);
					}
				};
			};
		}

		// Token: 0x0600819B RID: 33179 RVA: 0x0033FAE0 File Offset: 0x0033DCE0
		public static string GetSkillGrantingTraitNameById(string id)
		{
			string result = "";
			StringEntry stringEntry;
			if (Strings.TryGet("STRINGS.DUPLICANTS.TRAITS.GRANTSKILL_" + id.ToUpper() + ".NAME", out stringEntry))
			{
				result = stringEntry.String;
			}
			return result;
		}

		// Token: 0x0600819C RID: 33180 RVA: 0x0033FB19 File Offset: 0x0033DD19
		public static System.Action CreateBionicUpgradeTrait(string id, string effectsDescription)
		{
			return delegate()
			{
				string name = Strings.Get("STRINGS.DUPLICANTS.TRAITS." + id.ToUpper() + ".NAME");
				string desc = Strings.Get("STRINGS.DUPLICANTS.TRAITS." + id.ToUpper() + ".DESC");
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
				trait.TooltipCB = (() => desc + "\n\n" + effectsDescription);
				trait.NameCB = (() => name);
				string shortDescTooltip = Strings.Get("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC_TOOLTIP");
				trait.ShortDescTooltipCB = (() => shortDescTooltip + "\n\n" + effectsDescription);
			};
		}
	}
}
