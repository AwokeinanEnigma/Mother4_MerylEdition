using System;
using Carbine.Audio;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Mother4.Battle.Combatants;
using Mother4.Data;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	internal class BattleWinAction : BattleAction
	{
		public BattleWinAction(ActionParams aparams) : base(aparams)
		{
			this.state = BattleWinAction.State.Initialization;
			this.levelup = new LevelUpBuilder(null);
		}

		private void ChangeState(BattleWinAction.State state)
		{
			if (this.state != BattleWinAction.State.WaitForUI && this.state != BattleWinAction.State.WaitForTimer)
			{
				this.previousState = this.state;
			}
			this.state = state;
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			bool flag;
			do
			{
				flag = false;
				if (this.state == BattleWinAction.State.Initialization)
				{
					Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
					foreach (Combatant combatant in factionCombatants)
					{
						if (!combatant.HasStatusEffect(StatusEffect.Unconscious))
						{
							this.controller.InterfaceController.PopCard(combatant.ID, 28);
						}
						if (combatant.Stats.Meter >= 1f)
						{
							StatSet statChange = new StatSet
							{
								Meter = 1f - combatant.Stats.Meter - 0.013333334f
							};
							combatant.AlterStats(statChange);
							this.controller.InterfaceController.SetCardGroovy(combatant.ID, false);
						}
					}
					this.controller.SetFinalStatSets();
					this.controller.InterfaceController.RemoveAllModifiers();
					AudioManager.Instance.BGM.Stop();
					this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
					this.ChangeState(BattleWinAction.State.YouWon);
				}
				else if (this.state == BattleWinAction.State.YouWon)
				{
					int num = 0;
					int num2 = 180;
					string message = string.Format("[t:0,{0}][p:{1}] ", num, num2);
					this.controller.InterfaceController.ShowMessage(message, false);
					this.controller.InterfaceController.PlayWinBGM(num);
					this.controller.InterfaceController.RemoveTalkers();
					this.controller.InterfaceController.SetLetterboxing(0f);
					this.ChangeState(BattleWinAction.State.WaitForUI);
					flag = true;
				}
				else if (this.state == BattleWinAction.State.ItemDrop)
				{
					this.ChangeState(BattleWinAction.State.Experience);
					flag = true;
				}
				else if (this.state == BattleWinAction.State.Experience)
				{
					this.controller.InterfaceController.ShowMessage("@Travis and friends gained 261088 EXP.", true);
					this.ChangeState(BattleWinAction.State.WaitForUI);
				}
				else if (this.state == BattleWinAction.State.LevelUp)
				{
					this.controller.InterfaceController.StopWinBGM();
					this.controller.InterfaceController.PlayLevelUpBGM();
					this.controller.InterfaceController.ShowMessage(this.levelup.GetLevelUpString(), true);
					this.ChangeState(BattleWinAction.State.WaitForUI);
				}
				else if (this.state == BattleWinAction.State.WaitForTimer)
				{
					if (this.timer == 0U && this.previousState == BattleWinAction.State.LevelUp)
					{
						this.controller.InterfaceController.EndLevelUpBGM();
					}
					this.timer += 1U;
					if (this.timer > BattleWinAction.BATTLE_END_DELAY)
					{
						this.ChangeState(BattleWinAction.State.Done);
						this.timer = 0U;
						flag = true;
					}
				}
				else if (this.state == BattleWinAction.State.Done)
				{
					this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
					ITransition transition = new ColorFadeTransition(1f, Color.Black);
					transition.Blocking = true;
					SceneManager.Instance.Transition = transition;
					SceneManager.Instance.Pop();
					this.complete = true;
				}
			}
			while (flag);
		}

		private void InteractionComplete()
		{
			switch (this.previousState)
			{
			case BattleWinAction.State.YouWon:
				this.ChangeState(BattleWinAction.State.ItemDrop);
				return;
			case BattleWinAction.State.ItemDrop:
				this.ChangeState(BattleWinAction.State.Experience);
				return;
			case BattleWinAction.State.Experience:
				this.ChangeState(BattleWinAction.State.Done);
				return;
			case BattleWinAction.State.LevelUp:
				this.ChangeState(BattleWinAction.State.WaitForTimer);
				return;
			default:
				return;
			}
		}

		private const int CARD_POP_HEIGHT = 28;

		private static uint BATTLE_END_DELAY = 180U;

		private BattleWinAction.State previousState;

		private BattleWinAction.State state;

		private LevelUpBuilder levelup;

		private uint timer;

		private enum State
		{
			Initialization,
			WaitForUI,
			YouWon,
			ItemDrop,
			Experience,
			LevelUp,
			WaitForTimer,
			Done
		}
	}
}
