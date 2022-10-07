using System;
using System.Collections.Generic;
using Mother4.Battle;
using Mother4.Battle.Actions;

namespace Mother4.Data
{
	internal class StatusEffectActions
	{
		public static Type Get(StatusEffect effect)
		{
			Type result = null;
			if (StatusEffectActions.types.ContainsKey(effect))
			{
				result = StatusEffectActions.types[effect];
			}
			return result;
		}

		private static Dictionary<StatusEffect, Type> types = new Dictionary<StatusEffect, Type>
		{
			{
				StatusEffect.Talking,
				typeof(TalkStatusEffectAction)
			},
			{
				StatusEffect.DisablePSI,
				typeof(DisablePSI)
			}			
		};
	}
}
