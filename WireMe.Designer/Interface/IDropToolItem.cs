using System.Windows;
using WireMe.Designer.Model;

namespace WireMe.Designer.Interface
{
	public interface IDropToolItem
	{
		void DropToolItem(ToolItem item, Point? point);
	}
}