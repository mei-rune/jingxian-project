using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace jingxian.ui
{
    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

	[Component(typeof(IToolbarFactory),
        typeof(ToolbarFactory),
		ToolbarFactory.ToolbarFactoryId,
        jingxian.ui.Constants.BundleId)]
    public class ToolbarFactory: IToolbarFactory
	{
        public const string ToolbarFactoryId = "jingxian.ui.toolbarFactory";
		private readonly IActionService m_ActionService;
		private readonly IIconResourceService m_IconResourceService;

		private readonly Dictionary<string, List<IActionHandler>> m_ItemRegister =
				new Dictionary<string, List<IActionHandler>>();


        public ToolbarFactory(IActionService actionService, IIconResourceService iconService)
		{
			m_ActionService = actionService;
			m_IconResourceService = iconService;
		}

		public void InitializeWidget(IToolbarPart toolbarPart)
		{
			InitializeToolbarPart(toolbarPart);
			ToolStripEx strip = CreateDockToolStrip(toolbarPart);
			strip.Name = toolbarPart.Id;
			toolbarPart.Widget = strip;
		}

		public T CreateToolbar<T>(object context) where T: ToolStrip, new()
		{
			T strip = new T();
			ICollection<IActionHandler> actions = m_ActionService.CreateActionHandlers(context);
			FillStrip(strip, actions, ToolbarType.Menu, false);
			return strip;
		}

		public ToolStripItem[] GetItems(object context)
		{
			List<ToolStripItem> itemList = new List<ToolStripItem>();

			foreach (IActionHandler ah in m_ActionService.CreateActionHandlers(context))
			{
				itemList.Add(CreateItem(ah, ToolbarType.Menu, context));
			}

			ToolStripItem[] items = new ToolStripItem[0];

			if (itemList.Count > 0)
			{
				items = new ToolStripItem[itemList.Count];
				itemList.CopyTo(items);
			}

			return items;
		}

		private void InitializeToolbarPart(IToolbarPart toolbarPart)
		{
			//TODO: should we avoid recreating all the action handlers?
			toolbarPart.Clear();
			foreach (IActionHandler actionHandler in m_ActionService.CreateActionHandlers(toolbarPart.TypeId))
			{
				toolbarPart.Add(actionHandler);
			}
		}

		private ToolStripEx CreateDockToolStrip(IToolbarPart toolbarPart)
		{
            ToolStripEx dockToolStrip = new ToolStripEx(toolbarPart);
			FillStrip(dockToolStrip, toolbarPart, toolbarPart.ToolbarType, toolbarPart.IgnoreParents);
			// @todo Not supported by Mono - if we want to allow item reorder for toolbars, then we must also be able to save that order or it's just experimental.
			//dockToolStrip.AllowItemReorder = true;
			//dockToolStrip.Configure(toolbarPart);
			return dockToolStrip;
		}

		private void FillStrip(ToolStrip strip, ICollection<IActionHandler> actions, ToolbarType type, bool ignoreParents)
		{

			Dictionary<string, List<IActionHandler>> itemRegister =
				new Dictionary<string, List<IActionHandler>>();

			if (actions.Count > 0)
			{
				if (ignoreParents)
				{
					foreach (IActionHandler ah in actions)
					{
						if (ah != null)
						{
							if (!string.IsNullOrEmpty(ah.RetargetId) && ah.ActionType.Equals(ActionHandlerType.Standard))
							{
								// do nothing, because then this action will be encapsulated by an RetargetActionHandler
							}
							else
							{
								strip.Items.Add(CreateItem(ah, type));
							}
						}
					}
				}
				else
				{
					foreach (IActionHandler ah in actions)
					{
						if (ah != null)
						{
							List<IActionHandler> children;
							if (itemRegister.TryGetValue(ah.Parent, out children))
							{
								children.Add(ah);
							}
							else
							{
								children = new List<IActionHandler>();
								children.Add(ah);
								itemRegister.Add(ah.Parent, children);
							}
						}
					}

					SetUpItemCollection(itemRegister, string.Empty, strip.Items, type);
					ApplyVisualStyle(strip, type);
				}

				//the following Block removes all previously added items, sorts them and adds them again.
				//TODO:better add them in the right order in the first run (see big if statement above) -PJR- 

				List<ToolStripItem> itemList = new List<ToolStripItem>();
				foreach (ToolStripItem item in strip.Items)
				{
					itemList.Add(item);
				}
				//itemList.Sort(CompareToolStripItemsBySortOrder);
				strip.Items.Clear();
				strip.Items.AddRange(itemList.ToArray());
			}
		}

		private static int CompareToolStripItemsBySortOrder(ToolStripItem first, ToolStripItem second)
		{
			return ((first.Tag as IActionHandler).OrderIndex).CompareTo((second.Tag as IActionHandler).OrderIndex);
		}

		private static void ApplyVisualStyle(ToolStrip strip, ToolbarType type)
		{
			if (type == ToolbarType.Menu)
			{
				//hide grip to avoid ability of moving the toolbar
				strip.GripStyle = ToolStripGripStyle.Hidden;
				//stretch the toolbar accross the whole available space of its parent container
				strip.Stretch = true;
			}
			else
			{
				//TODO set normal toolbar specific style
			}
		}

		private void SetUpItemCollection(Dictionary<string, List<IActionHandler>> register,
																		string key,
																		ToolStripItemCollection collection,
																		ToolbarType type)
		{
			List<IActionHandler> actions;
			if (register.TryGetValue(key, out actions))
			{
				List<ToolStripItem> dropDownItems = new List<ToolStripItem>(actions.Count);
				foreach (IActionHandler ah in actions)
				{
					// TODO ND: Changed
					ToolStripItem item = CreateItem(ah, type);
					dropDownItems.Add(item);
					// TODO ND: Changed	
					if (item is ToolStripDropDownItem)
					{
						SetUpItemCollection(register, ah.Id, ((ToolStripDropDownItem) item).DropDownItems, type);
					}
				}

				//sort by OrderIndex and add to the collection
				dropDownItems.Sort(CompareToolStripItemsBySortOrder);
				collection.AddRange(dropDownItems.ToArray());

				register.Remove(key);
			}
			else
			{
				register.Remove(key);
			}
		}

		private ToolStripItem CreateItem(IActionHandler actionHandler, ToolbarType type)
		{
			return CreateItem(actionHandler, type, null);
		}

		private ToolStripItem CreateItem(IActionHandler actionHandler, ToolbarType type, object contextObject)
		{
			ToolStripItem item;

			switch (actionHandler.Style)
			{
				case ActionStyle.MenuItem:
					item = new ToolStripMenuItem();
					break;
				case ActionStyle.Push:
					item = new ToolStripMenuItem();
					item.Click += RunAction;
					break;
				case ActionStyle.Radio:
					//TODO: create Radio button ...
					item = new ToolStripMenuItem(); //HACK
					break;
				case ActionStyle.Toggle:
					//TODO: create Checkbox ...
					item = new ToolStripMenuItem(); //HACK
					((ToolStripMenuItem) item).CheckOnClick = true;
					break;
				case ActionStyle.Pulldown:
					//TODO: create ListBox ...
					item = new ToolStripMenuItem(); //HACK
					break;
				case ActionStyle.Separator:
					item = new ToolStripSeparator();
					break;
				default:
					item = new ToolStripMenuItem();
					return item;
			}

			item.Tag = actionHandler;
			item.Text = actionHandler.Label;
			if (!string.IsNullOrEmpty(actionHandler.IconId))
			{
				item.Image = m_IconResourceService.GetBitmap(actionHandler.IconId);
			}

			switch (type)
			{
				case ToolbarType.Menu:
					{
						item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
						break;
					}
				case ToolbarType.Toolbar:
					{
						item.DisplayStyle = ToolStripItemDisplayStyle.Image;
						break;
					}
				default:
					{
						item.DisplayStyle = ToolStripItemDisplayStyle.Image;
						break;
					}
			}

			return item;
		}

		private void RunAction(object sender, EventArgs e)
		{

			#region debug msg
			if (Log.IsDebugEnabled)
			{
				Log.Debug("RunAction called.");  //NON-NLS-1
			}
			#endregion

			IActionHandler aH = ((ToolStripItem) sender).Tag as IActionHandler;

			#region debug assertions
			Debug.Assert(aH != null, "ToolStripItem's Tag must be an ActionHandler!");
			Debug.Assert(aH.Enabled, "ActionHandler must be enabled to run.");
			#endregion

			if (aH.Enabled)
			{
				aH.Run( e );
			}
		}


		private log4net.ILog m_Log;

        private log4net.ILog Log
        {
            get
            {
                if (m_Log == null)
                {
                    m_Log = log4net.LogManager.GetLogger(GetType());
                }
                return m_Log;
            }
        }
	}
}
