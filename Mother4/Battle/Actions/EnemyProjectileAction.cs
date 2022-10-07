using System;
using Mother4.Battle.Combatants;
using Mother4.Battle.PsiAnimation;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.Utility;

namespace Mother4.Battle.Actions
{
	internal class EnemyProjectileAction : BattleAction
	{
		private bool useCustomText;
		private string customText;
		public EnemyProjectileAction(ActionParams aparams) : base(aparams)
		{
			this.enemySender = (this.sender as EnemyCombatant);
			this.playerTarget = ((this.targets.Length > 0) ? (this.targets[0] as PlayerCombatant) : null);
			this.projectileName = ((aparams.data.Length > 0) ? ((string)aparams.data[0]) : "");
			this.targetDamage = ((aparams.data.Length > 1) ? ((int)aparams.data[1]) : 0);
			useCustomText = ((aparams.data.Length > 2) ? ((bool)aparams.data[2]) : false);
			customText = ((aparams.data.Length > 3) ? ((string)aparams.data[3]) : "");
			this.isReflected = BattleCalculator.CalculateReflection(this.sender, this.targets[0]);
			this.state = EnemyProjectileAction.State.Initialize;
			this.previousState = this.state;
		}

		private void ChangeState(EnemyProjectileAction.State newState)
		{
			this.previousState = this.state;
			this.state = newState;
		}

		private void DoAnimation(int index)
		{
			PsiElementList animation = PsiAnimations.Get(index);
			PsiAnimator psiAnimator = this.controller.InterfaceController.AddPsiAnimation(animation, this.sender, this.targets);
			psiAnimator.OnAnimationComplete += this.OnAnimationComplete;
			this.ChangeState(EnemyProjectileAction.State.WaitForUI);
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			if (this.state == EnemyProjectileAction.State.Initialize)
			{
				string message = "";

                string article = this.enemySender.Enemy.Article;
                string name = this.enemySender.Enemy.PlayerFriendlyName;
            //    string like = this.enemySender.Enemy.GetStringQualifiedName("thoughts");
            //    string subjective = this.enemySender.Enemy.GetStringQualifiedName("subjective");

				if (!useCustomText)
				{
					message = string.Format("{0}{1} flung {2}!",
						Capitalizer.Capitalize(article),
                        name, this.projectileName);
				}
				else
				{
					message = string.Format("{0}{1} {2}",
						Capitalizer.Capitalize(article),
                        name, this.customText);
				}

				this.controller.InterfaceController.OnTextboxComplete += this.OnTextboxComplete;
				this.controller.InterfaceController.ShowMessage(message, false);
				this.controller.InterfaceController.PreEnemyAttack.Play();
				this.ChangeState(EnemyProjectileAction.State.WaitForUI);
				return;
			}
			if (this.state == EnemyProjectileAction.State.AnimateThrow)
			{
				this.DoAnimation(4);
				return;
			}
			if (this.state == EnemyProjectileAction.State.ShowReflectMessage)
			{
				string message2 = string.Format("{0} hit it back!", Capitalizer.Capitalize(CharacterNames.GetName(this.playerTarget.Character)));
				this.controller.InterfaceController.ShowMessage(message2, false);
				this.controller.InterfaceController.ReflectSound.Play();
				this.ChangeState(EnemyProjectileAction.State.WaitForUI);
				return;
			}
			if (this.state == EnemyProjectileAction.State.AnimateReflect)
			{
				this.DoAnimation(5);
				return;
			}
			if (this.state == EnemyProjectileAction.State.AnimateHit)
			{
				this.DoAnimation(6);
				return;
			}
			if (this.state == EnemyProjectileAction.State.DamageNumbers)
			{
				if (this.isReflected)
				{
					int num = BattleCalculator.CalculateProjectileDamage(this.targetDamage, this.sender, this.sender);
					DamageNumber damageNumber = this.controller.InterfaceController.AddDamageNumber(this.sender, num);
					damageNumber.OnComplete += this.OnDamageNumberComplete;
					StatSet statChange = new StatSet
					{
						HP = -num
					};
					this.sender.AlterStats(statChange);
					this.controller.InterfaceController.BlinkEnemy(this.sender as EnemyCombatant, 3, 2);
					StatSet statChange2 = new StatSet
					{
						Meter = 0.2f
					};
					this.playerTarget.AlterStats(statChange2);
				}
				else
				{
					foreach (Combatant combatant in this.targets)
					{
						int num2 = BattleCalculator.CalculateProjectileDamage(this.targetDamage, this.sender, combatant);
						DamageNumber damageNumber2 = this.controller.InterfaceController.AddDamageNumber(combatant, num2);
						damageNumber2.OnComplete += this.OnDamageNumberComplete;
						StatSet statChange3 = new StatSet
						{
							HP = -num2
						};
						combatant.AlterStats(statChange3);
					}
				}
				this.ChangeState(EnemyProjectileAction.State.WaitForUI);
				return;
			}
			if (this.state == EnemyProjectileAction.State.Finish)
			{
				this.complete = true;
			}
		}

		private void OnDamageNumberComplete(DamageNumber sender)
		{
			sender.OnComplete -= this.OnDamageNumberComplete;
			this.ChangeState(EnemyProjectileAction.State.Finish);
		}

		private void OnTextboxComplete()
		{
			EnemyProjectileAction.State state = this.previousState;
			if (state == EnemyProjectileAction.State.ShowReflectMessage)
			{
				this.ChangeState(EnemyProjectileAction.State.AnimateReflect);
				return;
			}
			this.ChangeState(EnemyProjectileAction.State.AnimateThrow);
		}

		private void OnAnimationComplete(PsiAnimator anim)
		{
			anim.OnAnimationComplete -= this.OnAnimationComplete;
			switch (this.previousState)
			{
				case EnemyProjectileAction.State.AnimateThrow:
					this.ChangeState(this.isReflected ? EnemyProjectileAction.State.ShowReflectMessage : EnemyProjectileAction.State.AnimateHit);
					return;
				case EnemyProjectileAction.State.AnimateReflect:
					this.ChangeState(EnemyProjectileAction.State.AnimateHit);
					return;
				case EnemyProjectileAction.State.AnimateHit:
					this.ChangeState(EnemyProjectileAction.State.DamageNumbers);
					return;
				default:
					return;
			}
		}

		private const float ONE_GP = 0.013333334f;

		private const int ANIMINDEX_THROW = 4;

		private const int ANIMINDEX_HITBACK = 5;

		private const int ANIMINDEX_EXPLODE = 6;

		private EnemyProjectileAction.State state;

		private EnemyProjectileAction.State previousState;

		private EnemyCombatant enemySender;

		private PlayerCombatant playerTarget;

		private string projectileName;

		private bool isReflected;

		private int targetDamage;

		private enum State
		{
			Initialize,
			WaitForUI,
			AnimateThrow,
			AnimateReflect,
			AnimateHit,
			DamageNumbers,
			ShowReflectMessage,
			Finish
		}
	}
}
