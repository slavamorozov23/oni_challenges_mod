using System;
using STRINGS;

// Token: 0x0200090A RID: 2314
internal struct EffectorEntry
{
	// Token: 0x06004057 RID: 16471 RVA: 0x0016CC93 File Offset: 0x0016AE93
	public EffectorEntry(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x06004058 RID: 16472 RVA: 0x0016CCAC File Offset: 0x0016AEAC
	public override string ToString()
	{
		string arg = "";
		if (this.count > 1)
		{
			arg = string.Format(UI.OVERLAYS.DECOR.COUNT, this.count);
		}
		return string.Format(UI.OVERLAYS.DECOR.ENTRY, GameUtil.GetFormattedDecor(this.value, false), this.name, arg);
	}

	// Token: 0x040027F3 RID: 10227
	public string name;

	// Token: 0x040027F4 RID: 10228
	public int count;

	// Token: 0x040027F5 RID: 10229
	public float value;
}
