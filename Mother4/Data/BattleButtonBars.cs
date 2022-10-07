using System;
using System.Collections.Generic;
using Mother4.Battle.UI;
using Mother4.Psi;

namespace Mother4.Data
{
	internal static class BattleButtonBars
	{
		public static ButtonBar.Action[] GetActions(CharacterType character, bool showRun, bool lockedPsi = false)
		{
			List<ButtonBar.Action> list = new List<ButtonBar.Action>();
			list.Add(ButtonBar.Action.Bash);
			if (character != CharacterType.Floyd)
			{
				if (character != CharacterType.Zack && PsiManager.Instance.CharacterHasPsi(character))
				{
					list.Add(ButtonBar.Action.Psi);
				}
			}
			else
			{
				list.Add(ButtonBar.Action.Talk);
			}
			if (lockedPsi == true) {
				list.Remove(ButtonBar.Action.Psi);
			}

			list.Add(ButtonBar.Action.Items);
			list.Add(ButtonBar.Action.Guard);
			if (showRun)
			{
				list.Add(ButtonBar.Action.Run);
			}
			return list.ToArray();
		}
	}
}
