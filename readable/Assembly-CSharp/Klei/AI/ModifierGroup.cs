using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x0200104F RID: 4175
	public class ModifierGroup<T> : Resource
	{
		// Token: 0x06008149 RID: 33097 RVA: 0x0033E9D4 File Offset: 0x0033CBD4
		public IEnumerator<T> GetEnumerator()
		{
			return this.modifiers.GetEnumerator();
		}

		// Token: 0x17000922 RID: 2338
		public T this[int idx]
		{
			get
			{
				return this.modifiers[idx];
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x0600814B RID: 33099 RVA: 0x0033E9F4 File Offset: 0x0033CBF4
		public int Count
		{
			get
			{
				return this.modifiers.Count;
			}
		}

		// Token: 0x0600814C RID: 33100 RVA: 0x0033EA01 File Offset: 0x0033CC01
		public ModifierGroup(string id, string name) : base(id, name)
		{
		}

		// Token: 0x0600814D RID: 33101 RVA: 0x0033EA16 File Offset: 0x0033CC16
		public void Add(T modifier)
		{
			this.modifiers.Add(modifier);
		}

		// Token: 0x040061F1 RID: 25073
		public List<T> modifiers = new List<T>();
	}
}
