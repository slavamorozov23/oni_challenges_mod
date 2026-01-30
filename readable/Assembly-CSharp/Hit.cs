using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000866 RID: 2150
public class Hit
{
	// Token: 0x06003B05 RID: 15109 RVA: 0x0014928C File Offset: 0x0014748C
	public Hit(AttackProperties properties, GameObject target)
	{
		this.properties = properties;
		this.target = target;
		this.DeliverHit();
	}

	// Token: 0x06003B06 RID: 15110 RVA: 0x001492A8 File Offset: 0x001474A8
	private float rollDamage()
	{
		return (float)Mathf.RoundToInt(UnityEngine.Random.Range(this.properties.base_damage_min, this.properties.base_damage_max));
	}

	// Token: 0x06003B07 RID: 15111 RVA: 0x001492CC File Offset: 0x001474CC
	private void DeliverHit()
	{
		Health component = this.target.GetComponent<Health>();
		if (!component)
		{
			return;
		}
		this.target.Trigger(-787691065, this.properties.attacker.GetComponent<FactionAlignment>());
		float num = this.rollDamage();
		AttackableBase component2 = this.target.GetComponent<AttackableBase>();
		num *= 1f + component2.GetDamageMultiplier();
		component.Damage(num);
		if (this.properties.effects == null)
		{
			return;
		}
		Effects component3 = this.target.GetComponent<Effects>();
		if (component3)
		{
			foreach (AttackEffect attackEffect in this.properties.effects)
			{
				if (UnityEngine.Random.Range(0f, 100f) < attackEffect.effectProbability * 100f)
				{
					component3.Add(attackEffect.effectID, true);
				}
			}
		}
	}

	// Token: 0x040023EE RID: 9198
	private AttackProperties properties;

	// Token: 0x040023EF RID: 9199
	private GameObject target;
}
