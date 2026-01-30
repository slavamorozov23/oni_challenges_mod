using System;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200103D RID: 4157
	[DebuggerDisplay("{effect.Id}")]
	public class EffectInstance : ModifierInstance<Effect>
	{
		// Token: 0x060080D9 RID: 32985 RVA: 0x0033C650 File Offset: 0x0033A850
		public EffectInstance(GameObject game_object, Effect effect, bool should_save, Func<string, object, string> resolveTooltipCallback = null) : base(game_object, effect)
		{
			this.effect = effect;
			this.shouldSave = should_save;
			this.DefineEffectImmunities();
			this.ApplyImmunities();
			this.ConfigureStatusItem(resolveTooltipCallback);
			if (effect.showInUI)
			{
				KSelectable component = base.gameObject.GetComponent<KSelectable>();
				if (!component.GetStatusItemGroup().HasStatusItem(this.statusItem))
				{
					component.AddStatusItem(this.statusItem, this);
				}
			}
			if (effect.triggerFloatingText && PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, effect.Name, game_object.transform, 1.5f, false);
			}
			if (effect.emote != null)
			{
				this.RegisterEmote(effect.emote, effect.emoteCooldown);
			}
			if (!string.IsNullOrEmpty(effect.emoteAnim))
			{
				this.RegisterEmote(effect.emoteAnim, effect.emoteCooldown);
			}
		}

		// Token: 0x060080DA RID: 32986 RVA: 0x0033C734 File Offset: 0x0033A934
		protected void DefineEffectImmunities()
		{
			if (this.immunityEffects == null && this.effect.immunityEffectsNames != null)
			{
				this.immunityEffects = new Effect[this.effect.immunityEffectsNames.Length];
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					this.immunityEffects[i] = Db.Get().effects.Get(this.effect.immunityEffectsNames[i]);
				}
			}
		}

		// Token: 0x060080DB RID: 32987 RVA: 0x0033C7A8 File Offset: 0x0033A9A8
		protected void ApplyImmunities()
		{
			if (base.gameObject != null && this.immunityEffects != null)
			{
				Effects component = base.gameObject.GetComponent<Effects>();
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					component.Remove(this.immunityEffects[i]);
					component.AddImmunity(this.immunityEffects[i], this.effect.IdHash.ToString(), false);
				}
			}
		}

		// Token: 0x060080DC RID: 32988 RVA: 0x0033C820 File Offset: 0x0033AA20
		protected void RemoveImmunities()
		{
			if (base.gameObject != null && this.immunityEffects != null)
			{
				Effects component = base.gameObject.GetComponent<Effects>();
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					component.RemoveImmunity(this.immunityEffects[i], this.effect.IdHash.ToString());
				}
			}
		}

		// Token: 0x060080DD RID: 32989 RVA: 0x0033C888 File Offset: 0x0033AA88
		public void RegisterEmote(string emoteAnim, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emoteAnim, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x060080DE RID: 32990 RVA: 0x0033C930 File Offset: 0x0033AB30
		public void RegisterEmote(Emote emote, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emote, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x060080DF RID: 32991 RVA: 0x0033C9DC File Offset: 0x0033ABDC
		private bool NotInATube(GameObject go, Navigator.ActiveTransition transition)
		{
			return transition.navGridTransition.start != NavType.Tube && transition.navGridTransition.end != NavType.Tube;
		}

		// Token: 0x060080E0 RID: 32992 RVA: 0x0033CA00 File Offset: 0x0033AC00
		public override void OnCleanUp()
		{
			if (this.statusItem != null)
			{
				base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.statusItem, false);
				this.statusItem = null;
			}
			if (this.reactable != null)
			{
				this.reactable.Cleanup();
				this.reactable = null;
			}
			this.RemoveImmunities();
		}

		// Token: 0x060080E1 RID: 32993 RVA: 0x0033CA54 File Offset: 0x0033AC54
		public float GetTimeRemaining()
		{
			return this.timeRemaining;
		}

		// Token: 0x060080E2 RID: 32994 RVA: 0x0033CA5C File Offset: 0x0033AC5C
		public bool IsExpired()
		{
			return this.effect.duration > 0f && this.timeRemaining <= 0f;
		}

		// Token: 0x060080E3 RID: 32995 RVA: 0x0033CA84 File Offset: 0x0033AC84
		private void ConfigureStatusItem(Func<string, object, string> resolveTooltipCallback)
		{
			StatusItem.IconType iconType = this.effect.isBad ? StatusItem.IconType.Exclamation : StatusItem.IconType.Info;
			if (!this.effect.customIcon.IsNullOrWhiteSpace())
			{
				iconType = StatusItem.IconType.Custom;
			}
			string id = this.effect.Id;
			string name = this.effect.Name;
			string description = this.effect.description;
			string customIcon = this.effect.customIcon;
			StatusItem.IconType icon_type = iconType;
			NotificationType notification_type = this.effect.isBad ? NotificationType.Bad : NotificationType.Neutral;
			bool allow_multiples = false;
			bool showStatusInWorld = this.effect.showStatusInWorld;
			this.statusItem = new StatusItem(id, name, description, customIcon, icon_type, notification_type, allow_multiples, OverlayModes.None.ID, 2, showStatusInWorld, null);
			this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveString);
			this.statusItem.resolveTooltipCallback = (resolveTooltipCallback ?? new Func<string, object, string>(this.ResolveTooltip));
		}

		// Token: 0x060080E4 RID: 32996 RVA: 0x0033CB48 File Offset: 0x0033AD48
		private string ResolveString(string str, object data)
		{
			return str;
		}

		// Token: 0x060080E5 RID: 32997 RVA: 0x0033CB4C File Offset: 0x0033AD4C
		public string ResolveTooltip(string str, object data)
		{
			string text = str;
			EffectInstance effectInstance = (EffectInstance)data;
			string text2 = Effect.CreateTooltip(effectInstance.effect, false, "\n    • ", true);
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			if (effectInstance.effect.duration > 0f)
			{
				text = text + "\n\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_REMAINING, GameUtil.GetFormattedCycles(this.GetTimeRemaining(), "F1", false));
			}
			return text;
		}

		// Token: 0x0400619D RID: 24989
		public Effect effect;

		// Token: 0x0400619E RID: 24990
		public bool shouldSave;

		// Token: 0x0400619F RID: 24991
		public StatusItem statusItem;

		// Token: 0x040061A0 RID: 24992
		public float timeRemaining;

		// Token: 0x040061A1 RID: 24993
		public EmoteReactable reactable;

		// Token: 0x040061A2 RID: 24994
		protected Effect[] immunityEffects;
	}
}
