using ColossalFramework.UI;
using UnityEngine;

namespace NoteIt
{
    public class UIUtils
    {
        public static UILabel CreateMenuPanelTitle(UIComponent parent, string title)
        {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.name = "Title";
            label.text = title;
            label.textAlignment = UIHorizontalAlignment.Center;
            label.relativePosition = new Vector3(parent.width / 2f - label.width / 2f, 11f);

            return label;
        }

        public static UIButton CreateMenuPanelCloseButton(UIComponent parent)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = "CloseButton";
            button.relativePosition = new Vector3(parent.width - 37f, 2f);

            button.normalBgSprite = "buttonclose";
            button.hoveredBgSprite = "buttonclosehover";
            button.pressedBgSprite = "buttonclosepressed";

            button.eventClick += (component, eventParam) =>
            {
                parent.Hide();
            };

            return button;
        }

        public static UIDragHandle CreateMenuPanelDragHandle(UIComponent parent)
        {
            UIDragHandle dragHandle = parent.AddUIComponent<UIDragHandle>();
            dragHandle.name = "DragHandle";
            dragHandle.width = parent.width - 40f;
            dragHandle.height = 40f;
            dragHandle.relativePosition = Vector3.zero;
            dragHandle.target = parent;

            return dragHandle;
        }

        public static UITabstrip CreateTabStrip(UIComponent parent)
        {
            UITabstrip tabstrip = parent.AddUIComponent<UITabstrip>();
            tabstrip.name = "TabStrip";

            tabstrip.width = parent.width - 40f;
            tabstrip.relativePosition = new Vector3(20f, 50f);

            return tabstrip;
        }

        public static UITabContainer CreateTabContainer(UIComponent parent)
        {
            UITabContainer tabContainer = parent.AddUIComponent<UITabContainer>();
            tabContainer.name = "TabContainer";

            tabContainer.width = parent.width - 40f;
            tabContainer.height = parent.height - 120f;
            tabContainer.relativePosition = new Vector3(20f, 100f);

            return tabContainer;
        }

        public static UIButton CreateTabButton(UIComponent parent)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = "TabButton";

            button.height = 26f;
            button.width = 146f;

            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
            button.textVerticalAlignment = UIVerticalAlignment.Middle;

            button.normalBgSprite = "GenericTab";
            button.disabledBgSprite = "GenericTabDisabled";
            button.focusedBgSprite = "GenericTabFocused";
            button.hoveredBgSprite = "GenericTabHovered";
            button.pressedBgSprite = "GenericTabPressed";

            button.textColor = new Color32(255, 255, 255, 255);
            button.disabledTextColor = new Color32(111, 111, 111, 255);
            button.focusedTextColor = new Color32(16, 16, 16, 255);
            button.hoveredTextColor = new Color32(255, 255, 255, 255);
            button.pressedTextColor = new Color32(255, 255, 255, 255);

            return button;
        }

        public static UITextField CreateMultilineTextBox(UIComponent parent)
        {
            UITextField textField = parent.AddUIComponent<UITextField>();
            textField.name = "TextField";

            textField.width = parent.width;
            textField.height = parent.height;
            textField.relativePosition = Vector3.zero;

            textField.multiline = true;
            textField.selectOnFocus = true;
            textField.selectionSprite = "EmptySprite";
            textField.selectionBackgroundColor = new Color32(233, 201, 148, 255);
            textField.hoveredBgSprite = "TextFieldPanelHovered";
            textField.focusedBgSprite = "TextFieldPanel";
            textField.builtinKeyNavigation = true;

            textField.padding = new RectOffset(10, 10, 10, 10);

            textField.verticalAlignment = UIVerticalAlignment.Top;
            textField.horizontalAlignment = UIHorizontalAlignment.Left;

            return textField;
        }
    }
}