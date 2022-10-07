using System;
using Mother4.Utility;

namespace Mother4.Battle.Actions
{
	internal class DisablePSI : StatusEffectAction
	{
		public DisablePSI(ActionParams aparams) : base(aparams)
		{
			if (this.effect.TurnsRemaining > 1)
			{
				this.message = string.Format("@{0} can't feel their PSI!", new object[]
				{
					this.senderName
				});
			}
			else
			{
				this.message = string.Format("@{0} felt their PSI return to them!", new object[]
				{
					this.senderName
				});
			}
			this.actionStartSound = this.controller.InterfaceController.PrePsiSound;
		}
	}
}
