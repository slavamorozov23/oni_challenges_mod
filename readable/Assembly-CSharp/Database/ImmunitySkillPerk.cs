using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02000FAD RID: 4013
	public class ImmunitySkillPerk : SkillPerk
	{
		// Token: 0x06007E38 RID: 32312 RVA: 0x003229DC File Offset: 0x00320BDC
		public ImmunitySkillPerk(string id, string nameOfEffectToBecomeImmuneTo) : base(id, "", null, null, delegate(MinionResume identity)
		{
		}, null, false)
		{
			Effect effect = Db.Get().effects.Get(nameOfEffectToBecomeImmuneTo);
			this.Name = GameUtil.SafeStringFormat(UI.ROLES_SCREEN.PERKS.IMMUNITY, new object[]
			{
				effect.Name
			});
			base.OnApply = delegate(MinionResume identity)
			{
				Effects component = identity.GetComponent<Effects>();
				if (component != null)
				{
					component.AddImmunity(effect, id, false);
				}
			};
			base.OnRemove = delegate(MinionResume identity)
			{
				Effects component = identity.GetComponent<Effects>();
				if (component != null)
				{
					component.RemoveImmunity(effect, id);
				}
			};
		}
	}
}
