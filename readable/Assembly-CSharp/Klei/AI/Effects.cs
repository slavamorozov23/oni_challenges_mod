using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200103E RID: 4158
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Effects")]
	public class Effects : KMonoBehaviour, ISaveLoadable, ISim1000ms
	{
		// Token: 0x060080E6 RID: 32998 RVA: 0x0033CBC7 File Offset: 0x0033ADC7
		protected override void OnPrefabInit()
		{
			this.autoRegisterSimRender = false;
		}

		// Token: 0x060080E7 RID: 32999 RVA: 0x0033CBD0 File Offset: 0x0033ADD0
		protected override void OnSpawn()
		{
			if (this.saveLoadImmunities != null)
			{
				foreach (Effects.SaveLoadImmunities saveLoadImmunities in this.saveLoadImmunities)
				{
					if (Db.Get().effects.Exists(saveLoadImmunities.effectID))
					{
						Effect effect = Db.Get().effects.Get(saveLoadImmunities.effectID);
						this.AddImmunity(effect, saveLoadImmunities.giverID, true);
					}
				}
			}
			if (this.saveLoadEffects != null)
			{
				foreach (Effects.SaveLoadEffect saveLoadEffect in this.saveLoadEffects)
				{
					if (Db.Get().effects.Exists(saveLoadEffect.id))
					{
						Effect newEffect = Db.Get().effects.Get(saveLoadEffect.id);
						EffectInstance effectInstance = this.Add(newEffect, true);
						if (effectInstance != null)
						{
							effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
						}
					}
				}
			}
			if (this.effectsThatExpire.Count > 0)
			{
				SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
			}
		}

		// Token: 0x060080E8 RID: 33000 RVA: 0x0033CCD4 File Offset: 0x0033AED4
		public EffectInstance Get(string effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.Id == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x060080E9 RID: 33001 RVA: 0x0033CD3C File Offset: 0x0033AF3C
		public EffectInstance Get(HashedString effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.IdHash == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x060080EA RID: 33002 RVA: 0x0033CDA4 File Offset: 0x0033AFA4
		public EffectInstance Get(Effect effect)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect == effect)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x060080EB RID: 33003 RVA: 0x0033CE00 File Offset: 0x0033B000
		public bool HasImmunityTo(Effect effect)
		{
			using (List<Effects.EffectImmunity>.Enumerator enumerator = this.effectImmunites.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060080EC RID: 33004 RVA: 0x0033CE5C File Offset: 0x0033B05C
		public EffectInstance Add(string effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x060080ED RID: 33005 RVA: 0x0033CE84 File Offset: 0x0033B084
		public EffectInstance Add(HashedString effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x060080EE RID: 33006 RVA: 0x0033CEAA File Offset: 0x0033B0AA
		public EffectInstance Add(Effect newEffect, bool should_save)
		{
			return this.Add(newEffect, should_save, null);
		}

		// Token: 0x060080EF RID: 33007 RVA: 0x0033CEB8 File Offset: 0x0033B0B8
		public EffectInstance Add(Effect newEffect, bool should_save, Func<string, object, string> resolveTooltipCallback = null)
		{
			if (this.HasImmunityTo(newEffect))
			{
				return null;
			}
			Traits component = base.GetComponent<Traits>();
			if (component != null && component.IsEffectIgnored(newEffect))
			{
				return null;
			}
			Attributes attributes = this.GetAttributes();
			EffectInstance effectInstance = this.Get(newEffect);
			if (!string.IsNullOrEmpty(newEffect.stompGroup))
			{
				for (int i = this.effects.Count - 1; i >= 0; i--)
				{
					if (this.effects[i] != effectInstance && !(this.effects[i].effect.stompGroup != newEffect.stompGroup) && this.effects[i].effect.stompPriority > newEffect.stompPriority)
					{
						return null;
					}
				}
				for (int j = this.effects.Count - 1; j >= 0; j--)
				{
					if (this.effects[j] != effectInstance && !(this.effects[j].effect.stompGroup != newEffect.stompGroup) && this.effects[j].effect.stompPriority <= newEffect.stompPriority)
					{
						this.Remove(this.effects[j].effect);
					}
				}
			}
			if (effectInstance == null)
			{
				effectInstance = new EffectInstance(base.gameObject, newEffect, should_save, resolveTooltipCallback);
				newEffect.AddTo(attributes);
				this.effects.Add(effectInstance);
				if (newEffect.duration > 0f)
				{
					this.effectsThatExpire.Add(effectInstance);
					if (this.effectsThatExpire.Count == 1)
					{
						SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
					}
				}
				if (newEffect.tag != null)
				{
					base.GetComponent<KPrefabID>().AddTag(newEffect.tag.Value, false);
				}
				base.Trigger(-1901442097, newEffect);
			}
			effectInstance.timeRemaining = newEffect.duration;
			return effectInstance;
		}

		// Token: 0x060080F0 RID: 33008 RVA: 0x0033D098 File Offset: 0x0033B298
		public void Remove(Effect effect)
		{
			this.Remove(effect.IdHash);
		}

		// Token: 0x060080F1 RID: 33009 RVA: 0x0033D0A8 File Offset: 0x0033B2A8
		public void Remove(HashedString effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.IdHash == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.IdHash == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					if (effect.tag != null)
					{
						base.GetComponent<KPrefabID>().RemoveTag(effect.tag.Value);
					}
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x060080F2 RID: 33010 RVA: 0x0033D204 File Offset: 0x0033B404
		public void Remove(string effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.Id == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.Id == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x060080F3 RID: 33011 RVA: 0x0033D338 File Offset: 0x0033B538
		public bool HasEffect(HashedString effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.IdHash == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060080F4 RID: 33012 RVA: 0x0033D39C File Offset: 0x0033B59C
		public bool HasEffect(string effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.Id == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060080F5 RID: 33013 RVA: 0x0033D400 File Offset: 0x0033B600
		public bool HasEffect(Effect effect)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060080F6 RID: 33014 RVA: 0x0033D45C File Offset: 0x0033B65C
		public void Sim1000ms(float dt)
		{
			for (int i = 0; i < this.effectsThatExpire.Count; i++)
			{
				EffectInstance effectInstance = this.effectsThatExpire[i];
				if (effectInstance.IsExpired())
				{
					this.Remove(effectInstance.effect);
				}
				effectInstance.timeRemaining -= dt;
			}
		}

		// Token: 0x060080F7 RID: 33015 RVA: 0x0033D4B0 File Offset: 0x0033B6B0
		public void AddImmunity(Effect effect, string giverID, bool shouldSave = true)
		{
			if (giverID != null)
			{
				foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
				{
					if (effectImmunity.giverID == giverID && effectImmunity.effect == effect)
					{
						return;
					}
				}
			}
			Effects.EffectImmunity effectImmunity2 = new Effects.EffectImmunity(effect, giverID, shouldSave);
			this.effectImmunites.Add(effectImmunity2);
			base.BoxingTrigger<Effects.EffectImmunity>(1152870979, effectImmunity2);
		}

		// Token: 0x060080F8 RID: 33016 RVA: 0x0033D53C File Offset: 0x0033B73C
		public void RemoveImmunity(Effect effect, string ID)
		{
			Effects.EffectImmunity effectImmunity = default(Effects.EffectImmunity);
			bool flag = false;
			foreach (Effects.EffectImmunity effectImmunity2 in this.effectImmunites)
			{
				if (effectImmunity2.effect == effect && (ID == null || ID == effectImmunity2.giverID))
				{
					effectImmunity = effectImmunity2;
					flag = true;
				}
			}
			if (flag)
			{
				this.effectImmunites.Remove(effectImmunity);
				base.BoxingTrigger<Effects.EffectImmunity>(964452195, effectImmunity);
			}
		}

		// Token: 0x060080F9 RID: 33017 RVA: 0x0033D5CC File Offset: 0x0033B7CC
		[OnSerializing]
		internal void OnSerializing()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.shouldSave)
				{
					Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
					{
						id = effectInstance.effect.Id,
						timeRemaining = effectInstance.timeRemaining,
						saved = true
					};
					list.Add(item);
				}
			}
			this.saveLoadEffects = list.ToArray();
			List<Effects.SaveLoadImmunities> list2 = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				if (effectImmunity.shouldSave)
				{
					Effect effect = effectImmunity.effect;
					Effects.SaveLoadImmunities item2 = new Effects.SaveLoadImmunities
					{
						effectID = effect.Id,
						giverID = effectImmunity.giverID,
						saved = true
					};
					list2.Add(item2);
				}
			}
			this.saveLoadImmunities = list2.ToArray();
		}

		// Token: 0x060080FA RID: 33018 RVA: 0x0033D708 File Offset: 0x0033B908
		public List<Effects.SaveLoadImmunities> GetAllImmunitiesForSerialization()
		{
			List<Effects.SaveLoadImmunities> list = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				Effect effect = effectImmunity.effect;
				Effects.SaveLoadImmunities item = new Effects.SaveLoadImmunities
				{
					effectID = effect.Id,
					giverID = effectImmunity.giverID,
					saved = effectImmunity.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060080FB RID: 33019 RVA: 0x0033D7A0 File Offset: 0x0033B9A0
		public List<Effects.SaveLoadEffect> GetAllEffectsForSerialization()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
				{
					id = effectInstance.effect.Id,
					timeRemaining = effectInstance.timeRemaining,
					saved = effectInstance.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060080FC RID: 33020 RVA: 0x0033D834 File Offset: 0x0033BA34
		public List<EffectInstance> GetTimeLimitedEffects()
		{
			return this.effectsThatExpire;
		}

		// Token: 0x060080FD RID: 33021 RVA: 0x0033D83C File Offset: 0x0033BA3C
		public void CopyEffects(Effects source)
		{
			foreach (EffectInstance effectInstance in source.effects)
			{
				this.Add(effectInstance.effect, effectInstance.shouldSave).timeRemaining = effectInstance.timeRemaining;
			}
			foreach (EffectInstance effectInstance2 in source.effectsThatExpire)
			{
				this.Add(effectInstance2.effect, effectInstance2.shouldSave).timeRemaining = effectInstance2.timeRemaining;
			}
		}

		// Token: 0x040061A3 RID: 24995
		[Serialize]
		private Effects.SaveLoadEffect[] saveLoadEffects;

		// Token: 0x040061A4 RID: 24996
		[Serialize]
		private Effects.SaveLoadImmunities[] saveLoadImmunities;

		// Token: 0x040061A5 RID: 24997
		private List<EffectInstance> effects = new List<EffectInstance>();

		// Token: 0x040061A6 RID: 24998
		private List<EffectInstance> effectsThatExpire = new List<EffectInstance>();

		// Token: 0x040061A7 RID: 24999
		private List<Effects.EffectImmunity> effectImmunites = new List<Effects.EffectImmunity>();

		// Token: 0x02002732 RID: 10034
		[Serializable]
		public struct EffectImmunity
		{
			// Token: 0x0600C81E RID: 51230 RVA: 0x004255FB File Offset: 0x004237FB
			public EffectImmunity(Effect e, string id, bool save = true)
			{
				this.giverID = id;
				this.effect = e;
				this.shouldSave = save;
			}

			// Token: 0x0400AE84 RID: 44676
			public string giverID;

			// Token: 0x0400AE85 RID: 44677
			public Effect effect;

			// Token: 0x0400AE86 RID: 44678
			public bool shouldSave;
		}

		// Token: 0x02002733 RID: 10035
		[Serializable]
		public struct SaveLoadImmunities
		{
			// Token: 0x0400AE87 RID: 44679
			public string giverID;

			// Token: 0x0400AE88 RID: 44680
			public string effectID;

			// Token: 0x0400AE89 RID: 44681
			public bool saved;
		}

		// Token: 0x02002734 RID: 10036
		[Serializable]
		public struct SaveLoadEffect
		{
			// Token: 0x0400AE8A RID: 44682
			public string id;

			// Token: 0x0400AE8B RID: 44683
			public float timeRemaining;

			// Token: 0x0400AE8C RID: 44684
			public bool saved;
		}
	}
}
