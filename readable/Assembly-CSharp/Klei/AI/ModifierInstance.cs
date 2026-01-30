using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001050 RID: 4176
	public class ModifierInstance<ModifierType> : IStateMachineTarget
	{
		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x0600814E RID: 33102 RVA: 0x0033EA24 File Offset: 0x0033CC24
		// (set) Token: 0x0600814F RID: 33103 RVA: 0x0033EA2C File Offset: 0x0033CC2C
		public GameObject gameObject { get; private set; }

		// Token: 0x06008150 RID: 33104 RVA: 0x0033EA35 File Offset: 0x0033CC35
		public ModifierInstance(GameObject game_object, ModifierType modifier)
		{
			this.gameObject = game_object;
			this.modifier = modifier;
		}

		// Token: 0x06008151 RID: 33105 RVA: 0x0033EA4B File Offset: 0x0033CC4B
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x06008152 RID: 33106 RVA: 0x0033EA58 File Offset: 0x0033CC58
		public int Subscribe(int hash, Action<object> handler)
		{
			return this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler);
		}

		// Token: 0x06008153 RID: 33107 RVA: 0x0033EA6C File Offset: 0x0033CC6C
		public int Subscribe(int hash, Action<object, object> handler, object context)
		{
			return this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler, context);
		}

		// Token: 0x06008154 RID: 33108 RVA: 0x0033EA81 File Offset: 0x0033CC81
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(hash, handler);
		}

		// Token: 0x06008155 RID: 33109 RVA: 0x0033EA95 File Offset: 0x0033CC95
		public void Unsubscribe(int id)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(id);
		}

		// Token: 0x06008156 RID: 33110 RVA: 0x0033EAA8 File Offset: 0x0033CCA8
		public void Trigger(int hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06008157 RID: 33111 RVA: 0x0033EABC File Offset: 0x0033CCBC
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06008158 RID: 33112 RVA: 0x0033EAC9 File Offset: 0x0033CCC9
		public bool isNull
		{
			get
			{
				return this.gameObject == null;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06008159 RID: 33113 RVA: 0x0033EAD7 File Offset: 0x0033CCD7
		public string name
		{
			get
			{
				return this.gameObject.name;
			}
		}

		// Token: 0x0600815A RID: 33114 RVA: 0x0033EAE4 File Offset: 0x0033CCE4
		public virtual void OnCleanUp()
		{
		}

		// Token: 0x040061F3 RID: 25075
		public ModifierType modifier;
	}
}
