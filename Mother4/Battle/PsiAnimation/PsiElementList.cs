using System;
using System.Collections.Generic;

namespace Mother4.Battle.PsiAnimation
{
	internal class PsiElementList
	{
		public bool HasElements
		{
			get
			{
				return this.elementCounter < this.elements.Count;
			}
		}

		public PsiElementList(List<PsiElement> elements)
		{
			this.elements = new List<PsiElement>(elements);
			this.elements.Sort(new Comparison<PsiElement>(PsiElementList.CompareElements));
		}

		private static int CompareElements(PsiElement x, PsiElement y)
		{
			return y.Timestamp - x.Timestamp;
		}

		public List<PsiElement> GetElementsAtTime(int timestamp)
		{
			List<PsiElement> list = this.elements.FindAll((PsiElement x) => x.Timestamp == timestamp);
			this.elementCounter += list.Count;
			return list;
		}

		private List<PsiElement> elements;

		private int elementCounter;
	}
}
