using Mother4.SOMETHING;
using System;

namespace Mother4.Psi
{
	public struct DefensivePsi : IPsi
	{
		public DefensivePsi(OffensePsi ability)
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