using System;
using Mother4.Battle.Combatants;
using Mother4.Battle.UI;
using Mother4.Data;

namespace Mother4.Battle.Actions
{
	internal class PlayerDecisionAction : DecisionAction
	{
		public PlayerDecisionAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (this.sender as PlayerCombatant);
			this.character = this.combatant.Character;
			if (aparams.data != null)
			{
				if (aparams.data.Length > 0)
				{
					this.isGroovy = (bool)aparams.data[0];
				}
				if (aparams.data.Length > 1)
				{
					this.isFromUndo = (bool)aparams.data[1];
				}
			}
			this.state = PlayerDecisionAction.State.Initialize;
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case PlayerDecisionAction.State.Initialize:
			{
				bool flag = this.HandleStatusEffects(false);
				Console.WriteLine("{0} is deciding what to do!", Enum.GetName(typeof(CharacterType), this.character));
				this.controller.InterfaceController.OnInteractionComplete += this.InteractionComplete;
				if (!flag)
				{
					this.controller.InterfaceController.BeginPlayerInteraction(this.character);
					this.controller.InterfaceController.ShowButtonBar();
					this.controller.InterfaceController.PopCard(this.combatant.ID, 28);
					this.controller.InterfaceController.AllowUndo = (this.controller.CanRevert() && !this.isGroovy);
					this.state = PlayerDecisionAction.State.WaitForUI;
					return;
				}
				if (this.isFromUndo)
				{
					this.controller.RevertDecision();
				}
				this.state = PlayerDecisionAction.State.Finish;
				return;
			}
			case PlayerDecisionAction.State.WaitForUI:
				break;
			case PlayerDecisionAction.State.Finish:
				this.controller.InterfaceController.OnInteractionComplete -= this.InteractionComplete;
				this.controller.InterfaceController.EndPlayerInteraction();
				if (!this.isGroovy)
				{
					this.controller.InterfaceController.PopCard(this.combatant.ID, 0);
				}
				Console.WriteLine("Finished deciding.");
				this.complete = true;
				break;
			default:
				return;
			}
		}

		public void InteractionComplete(SelectionState selectionState)
		{
			Console.WriteLine(" Type: {0}", selectionState.Type);
			if (selectionState.AttackIndex >= 0)
			{
				Console.WriteLine(" Attack Index: {0}", selectionState.AttackIndex);
			}
			if (selectionState.ItemIndex >= 0)
			{
				Console.WriteLine(" Item Index: {0}", selectionState.ItemIndex);
			}
			if (selectionState.Targets != null)
			{
				Console.Write(" Targets: ");
				for (int i = 0; i < selectionState.Targets.Length; i++)
				{
					Console.Write("{0}, ", selectionState.Targets[i].ToString());
				}
				Console.WriteLine();
			}
			int priority = this.isGroovy ? 2147483646 : this.combatant.Stats.Speed;
			bool flag = false;
			ActionParams? actionParams = null;
			StatusEffectInstance[] statusEffects = this.sender.GetStatusEffects();
			if (selectionState.Type == SelectionState.SelectionType.Undo)
			{
				this.controller.RevertDecision();
				flag = true;
			}
			else if (statusEffects.Length > 0)
			{
				flag = this.HandleStatusEffects(true);
			}
			if (!flag)
			{
				switch (selectionState.Type)
				{
				case SelectionState.SelectionType.Bash:
					actionParams = new ActionParams?(new ActionParams
					{
						actionType = typeof(PlayerBashAction),
						controller = this.controller,
						sender = this.combatant,
						priority = priority,
						targets = selectionState.Targets
					});
					break;
				case SelectionState.SelectionType.PSI:
					
					actionParams = new ActionParams?(new ActionParams
					{
						actionType = typeof(PlayerPsiAction),
						controller = this.controller,
						sender = this.combatant,
						priority = priority,
						targets = selectionState.Targets,
						data = new object[]
						{
							selectionState.Wrapper,
							selectionState.PsiLevel,
							selectionState.Psi
						}
					});
					break;
				case SelectionState.SelectionType.Talk:
					actionParams = new ActionParams?(new ActionParams
					{
						actionType = typeof(FloydTalkAction),
						controller = this.controller,
						sender = this.combatant,
						priority = priority,
						targets = selectionState.Targets
					});
					break;
				case SelectionState.SelectionType.Guard:
					actionParams = new ActionParams?(new ActionParams
					{
						actionType = typeof(MessageAction),
						controller = this.controller,
						sender = this.combatant,
						priority = priority,
						targets = selectionState.Targets,
						data = new object[]
						{
							CharacterNames.GetName((this.sender as PlayerCombatant).Character) + " braces for impact!",
							false
						}
					});
					break;
				}
			}
			if (actionParams != null)
			{
				BattleAction instance = BattleAction.GetInstance(actionParams.Value);
				this.controller.AddAction(instance);
				if (this.isGroovy)
				{
					instance.OnActionComplete += this.OnGroovyBonusActionComplete;
				}
			}
			this.state = PlayerDecisionAction.State.Finish;
		}

		private void OnGroovyBonusActionComplete(BattleAction action)
		{
			this.controller.InterfaceController.SetCardGroovy(this.combatant.ID, false);
			this.combatant.AlterStats(new StatSet
			{
				Meter = -this.combatant.Stats.Meter
			});
		}

		private bool HandleStatusEffects(bool addActions)
		{
			bool flag = false;
			StatusEffectInstance[] statusEffects = this.sender.GetStatusEffects();
			foreach (StatusEffectInstance statusEffectInstance in statusEffects)
			{
				flag |= (statusEffectInstance.Type == StatusEffect.Talking || statusEffectInstance.Type == StatusEffect.Diamondized || statusEffectInstance.Type == StatusEffect.Unconscious);
				if (addActions)
				{
					ActionParams aparams = new ActionParams
					{
						actionType = StatusEffectActions.Get(statusEffectInstance.Type),
						controller = this.controller,
						sender = this.sender,
						targets = this.sender.SavedTargets,
						priority = this.sender.Stats.Speed,
						data = new object[]
						{
							statusEffectInstance
						}
					};
					this.controller.AddAction(BattleAction.GetInstance(aparams));
				}
			}
			return flag;
		}

		private const int CARD_POP_HEIGHT = 28;

		private PlayerCombatant combatant;

		private CharacterType character;

		private PlayerDecisionAction.State state;

		private bool isGroovy;

		private bool isFromUndo;

		private enum State
		{
			Initialize,
			WaitForUI,
			Finish
		}
	}
}
