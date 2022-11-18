using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BakeryBash
{
	public class UI : Entity
	{
		public static Color TextOutlineColor = Calc.HexToColor("a22b00");
		MTexture backgroundTexture;
		public NavigationList<UIElement> Children;
		public SelectableUIElement SelectedItem;
		public bool Focused;
		public UI(Vector2 position, MTexture backgroundTexture)
		{
			Position = position;
			Children = new();
			this.backgroundTexture = backgroundTexture;
		}

		public void AddElement(UIElement element)
		{
			Children.Add(element);
			element.Parent = this;

			if (SelectedItem == null)
				if (Children.Where((c) => c is SelectableUIElement element).Any())
				{
					SelectedItem = (SelectableUIElement)Children.First((c => c is SelectableUIElement));
					SelectedItem.Enter();
				}
		}

		public void RemoveElement(UIElement element)
		{
			Children.Remove(element);
		}

		public void SelectNext()
		{
			UIElement elem;
			do elem = Children.MoveNext; while (Children.Current is not SelectableUIElement);
			SelectedItem.Leave();
			SelectedItem = (SelectableUIElement)elem;
			SelectedItem.Enter();
		}

		public void SelectPrevious()
		{
			UIElement elem;
			do elem = Children.MovePrevious; while (Children.Current is not SelectableUIElement);
			SelectedItem.Leave();
			SelectedItem = (SelectableUIElement)elem;
			SelectedItem.Enter();
		}

		public override void Update()
		{
			base.Update();
			foreach (var item in Children)
			{
				item.Update();
			}

		}
		public override void Render()
		{
			base.Render();
			backgroundTexture?.DrawCentered(Position);
			foreach (var item in Children)
			{
				item.Render();
			}
		}

	}
}