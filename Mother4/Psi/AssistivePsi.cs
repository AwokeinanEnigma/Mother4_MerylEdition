using Mother4.SOMETHING;
using System;

namespace Mother4.Psi
{
	public struct AssistivePsi : IPsi
	{
		public AssistivePsi(PSIBase ability)
		{
			_aux = ability;
		}
		public AssistivePsi(AssistivePsi ability)
		{
			_aux = ability.aux;
		}


		private PSIBase _aux;
		public PSIBase aux
		{
			get { return _aux; }
			set
			{
				_aux = aux;
			}
		}
	}
}