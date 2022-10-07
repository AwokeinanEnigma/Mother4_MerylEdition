using System;
using System.Linq;
//using Mother4.AUX;
using Mother4.Battle.Combatants;
using Mother4.Battle.PsiAnimation;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.Psi;
using Mother4.SOMETHING;

namespace Mother4.Battle.Actions
{
	internal class PlayerPsiAction : BattleAction
	{
		public PSIBase aux;
		public PlayerPsiAction(ActionParams aparams) : base(aparams)
		{
			Console.WriteLine("CONSTRUCTOR");
			this.combatant = (this.sender as PlayerCombatant);
			IPsi psi = (aparams.data.Length > 0) ? ((IPsi)aparams.data[0]) : null;
			this.psi = psi;
			this.psiLevel = ((aparams.data.Length > 1) ? ((int)aparams.data[1]) : 0);
			aux = ((aparams.data.Length > 1) ? ((PSIBase)aparams.data[2]) : null);
			
			this.state = PlayerPsiAction.State.Initialize;
		}

		private void RemoveInvalidTargets()
		{
			Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.EnemyTeam, true);
			bool[] array = new bool[this.targets.Length];
			int num = this.targets.Length;
			for (int i = 0; i < this.targets.Length; i++)
			{
				Combatant combatant = this.targets[i];
				if (!this.controller.CombatantController.IsIdValid(combatant.ID))
				{
					array[i] = true;
					num--;
					foreach (Combatant combatant2 in factionCombatants)
					{
						if (!this.targets.Contains(combatant2))
						{
							this.targets[i] = combatant2;
							array[i] = false;
							num++;
							break;
						}
					}
				}
			}
			Combatant[] array3 = new Combatant[num];
			int k = 0;
			int num2 = 0;
			while (k < this.targets.Length)
			{
				if (!array[k])
				{
					array3[num2] = this.targets[k];
					num2++;
				}
				k++;
			}
			this.targets = array3;
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			if (this.state == PlayerPsiAction.State.Initialize)
			{
				foreach (StatusEffectInstance statusEffectInstance in combatant.GetStatusEffects())
				{
					if (statusEffectInstance.Type == StatusEffect.DisablePSI)
					{
						string msg = string.Format("{0} tried {1} {2}!", CharacterNames.GetName(this.combatant.Character), this.psi.aux.QualifiedName, PsiLetters.Get(this.psiLevel));
						this.controller.InterfaceController.ShowMessage(msg, false);
						this.controller.InterfaceController.ShowMessage("...But it failed!", false);
						this.complete = true;

						return;
					}
				}

				this.RemoveInvalidTargets();
				aux.Initialize(combatant, controller.InterfaceController, this, targets);
				return;
			}
			if (this.state == PlayerPsiAction.State.Animate)
			{
				aux.Animate(combatant, controller.InterfaceController, this, targets);


				return;
			}
			if (this.state == PlayerPsiAction.State.DamageNumbers)
			{
				aux.Act(targets, combatant, controller.InterfaceController, this);
				/*
				foreach (Combatant combatant in this.targets)
				{
					//todo:
					//lifeup breaks the game because its effect is null
					//int num = BattleCalculator.CalculatePsiDamage(this.psi.Effect[this.psiLevel][0], this.psi.Effect[this.psiLevel][1], this.sender, combatant);
					DamageNumber damageNumber = this.controller.InterfaceController.AddDamageNumber(combatant, 32);
					damageNumber.OnComplete += this.OnDamageNumberComplete;
					StatSet statChange = new StatSet
					{
						HP = -32
					};
					combatant.AlterStats(statChange);
					if (combatant as EnemyCombatant != null)
					{
						this.controller.InterfaceController.BlinkEnemy(combatant as EnemyCombatant, 3, 2);
					}
				}
				StatSet statChange2 = new StatSet
				{
					PP = -this.psi.aux.AUCost,
					Meter = 0.026666667f
				};
				this.sender.AlterStats(statChange2);*/
				//this.state = PlayerPsiAction.State.WaitForUI;
				return;
			}
			if (this.state == PlayerPsiAction.State.Finish)
			{
				aux.Finish(targets, combatant, controller.InterfaceController, this);
			}
		}

		private void OnDamageNumberComplete(DamageNumber sender)
		{
			sender.OnComplete -= this.OnDamageNumberComplete;
			this.state = PlayerPsiAction.State.Finish;
		}

		public void Finish()
        {
			this.complete = true;

		}



		private const float ONE_GP = 0.013333334f;

		private const int CARD_POP_HEIGHT = 12;

		private const int DAMAGE_NUMBER_WAIT = 70;

		private const int PSI_INDEX = 0;

		private const int PSI_LEVEL_INDEX = 1;

		public PlayerPsiAction.State state;

		private PlayerCombatant combatant;

		public IPsi psi;

		public int psiLevel;

		public enum State
		{
			Initialize,
			Animate,
			WaitForUI,
			DamageNumbers,
			Finish
		}
	}
}
