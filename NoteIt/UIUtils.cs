using ColossalFramework.UI;
using UnityEngine;

namespace NoteIt
{
    public class UIUtils
    {
        public static UIFont GetUIFont(string name)
        {
            UIFont[] fonts = Resources.FindObjectsOfTypeAll<UIFont>();

            foreach (UIFont font in fonts)
            {
                if (font.name.CompareTo(name) == 0)
                {
                    return font;
                }
            }

            return null;
        }

        public static UIPanel CreatePanel(UIComponent parent, string name)
        {
            UIPanel panel = parent.AddUIComponent<UIPanel>();
            panel.name = name;

            return panel;
        }

        public static UIScrollablePanel CreateScrollablePanel(UIComponent parent, string name)
        {
            UIScrollablePanel scrollablePanel = parent.AddUIComponent<UIScrollablePanel>();
            scrollablePanel.name = name;

            return scrollablePanel;
        }

        public static UIScrollbar CreateScrollbar(UIComponent parent, string name)
        {
            UIScrollbar scrollbar = parent.AddUIComponent<UIScrollbar>();
            scrollbar.name = name;

            return scrollbar;
        }

        public static UISlicedSprite CreateSlicedSprite(UIComponent parent, string name)
        {
            UISlicedSprite slicedSprite = parent.AddUIComponent<UISlicedSprite>();
            slicedSprite.name = name;

            return slicedSprite;
        }

        public static UILabel CreateLabel(UIComponent parent, string name, string text)
        {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.name = name;
            label.text = text;

            return label;
        }

        public static UITextField CreateTextField(UIComponent parent, string name, string text)
        {
            UITextField textField = parent.AddUIComponent<UITextField>();
            textField.name = name;
            textField.text = text;

            textField.multiline = false;
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

        public static UITextField CreateMultilineTextField(UIComponent parent, string name, string text)
        {
            UITextField textField = parent.AddUIComponent<UITextField>();
            textField.name = name;
            textField.text = text;

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

        public static UIButton CreateButton(UIComponent parent, string name, string text)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = name;
            button.text = text;

            button.hoveredTextColor = new Color32(7, 132, 255, 255);
            button.pressedTextColor = new Color32(30, 44, 44, 255);
            button.focusedTextColor = new Color32(255, 255, 255, 255);

            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
            button.textVerticalAlignment = UIVerticalAlignment.Middle;

            button.normalBgSprite = "ButtonMenu";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.disabledBgSprite = "ButtonMenuDisabled";

            return button;
        }

        public static UIButton CreateSmallButton(UIComponent parent, string name, string text)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = name;
            button.text = text;

            button.hoveredTextColor = new Color32(7, 132, 255, 255);
            button.pressedTextColor = new Color32(30, 44, 44, 255);
            button.focusedTextColor = new Color32(255, 255, 255, 255);

            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
            button.textVerticalAlignment = UIVerticalAlignment.Middle;

            button.normalBgSprite = "ButtonSmall";
            button.hoveredBgSprite = "ButtonSmallMenuHovered";
            button.pressedBgSprite = "ButtonSmallPressed";
            button.disabledBgSprite = "ButtonSmallDisabled";

            return button;
        }

        public static UILabel CreateMenuPanelTitle(UIComponent parent, string title)
        {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.name = "Title";
            label.zOrder = 1;
            label.text = title;
            label.textAlignment = UIHorizontalAlignment.Center;

            return label;
        }

        public static UIButton CreateMenuPanelLocationButton(UIComponent parent)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = "LocationButton";
            button.zOrder = 1;

            button.normalBgSprite = "LocationMarkerActiveNormal";
            button.hoveredBgSprite = "LocationMarkerActiveHovered";
            button.pressedBgSprite = "LocationMarkerActivePressed";

            return button;
        }

        public static UIButton CreateMenuPanelCloseButton(UIComponent parent)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = "CloseButton";
            button.zOrder = 1;

            button.normalBgSprite = "buttonclose";
            button.hoveredBgSprite = "buttonclosehover";
            button.pressedBgSprite = "buttonclosepressed";

            button.eventClick += (component, eventParam) =>
            {
                if (!eventParam.used)
                {
                    parent.Hide();

                    eventParam.Use();
                }                
            };

            return button;
        }

        public static UIDragHandle CreateMenuPanelDragHandle(UIComponent parent)
        {
            UIDragHandle dragHandle = parent.AddUIComponent<UIDragHandle>();
            dragHandle.name = "DragHandle";
            dragHandle.zOrder = 2;

            dragHandle.target = parent;

            return dragHandle;
        }

        public static UITabstrip CreateTabStrip(UIComponent parent)
        {
            UITabstrip tabstrip = parent.AddUIComponent<UITabstrip>();
            tabstrip.name = "TabStrip";
            tabstrip.clipChildren = true;

            return tabstrip;
        }

        public static UITabContainer CreateTabContainer(UIComponent parent)
        {
            UITabContainer tabContainer = parent.AddUIComponent<UITabContainer>();
            tabContainer.name = "TabContainer";

            return tabContainer;
        }

        public static UIButton CreateTabButton(UIComponent parent)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = "TabButton";

            button.height = 26f;
            button.width = 52f;

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

        public static UIPanel CreateLinePanel(UIComponent parent, string name, bool alternate)
        {
            UIPanel panel = parent.AddUIComponent<UIPanel>();
            panel.name = name;
            panel.backgroundSprite = "InfoviewPanel";
            panel.color = alternate ? new Color32(56, 61, 63, 255) : new Color32(49, 52, 58, 255);
            panel.height = 45f;
            panel.eventMouseEnter += (component, eventParam) =>
            {
                panel.color = new Color32(73, 78, 87, 255);
            };
            panel.eventMouseLeave += (component, eventParam) =>
            {
                panel.color = alternate ? new Color32(56, 61, 63, 255) : new Color32(49, 52, 58, 255);
            };

            return panel;
        }

        public static UILabel CreateLineLabel(UIComponent parent, string name, string text)
        {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.name = name;
            label.text = text;
            label.textScale = 1f;
            label.textColor = new Color32(185, 221, 254, 255);
            label.textAlignment = UIHorizontalAlignment.Left;
            label.verticalAlignment = UIVerticalAlignment.Top;

            return label;
        }

        public static UITextField CreateLineTextField(UIComponent parent, string name, string text)
        {
            UITextField textField = parent.AddUIComponent<UITextField>();
            textField.name = name;
            textField.text = text;
            textField.textScale = 1f;
            textField.textColor = new Color32(185, 221, 254, 255);

            textField.multiline = false;
            textField.selectOnFocus = true;
            textField.selectionSprite = "EmptySprite";
            textField.selectionBackgroundColor = new Color32(233, 201, 148, 255);
            textField.hoveredBgSprite = "TextFieldPanelHovered";
            textField.focusedBgSprite = "TextFieldPanel";
            textField.builtinKeyNavigation = true;

            textField.verticalAlignment = UIVerticalAlignment.Middle;
            textField.horizontalAlignment = UIHorizontalAlignment.Left;

            return textField;
        }

        public static UIButton CreateLineFocusButton(UIComponent parent, string name)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = name;
            button.height = 28f;
            button.width = 28f;

            button.normalBgSprite = "LocationMarkerNormal";
            button.hoveredBgSprite = "LocationMarkerHovered";
            button.pressedBgSprite = "LocationMarkerPressed";

            return button;
        }

        public static UIButton CreateLineDeleteButton(UIComponent parent, string name)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = name;
            button.height = 28f;
            button.width = 28f;

            button.normalBgSprite = "DeleteLineButton";
            button.hoveredBgSprite = "DeleteLineButtonHover";
            button.pressedBgSprite = "DeleteLineButtonPressed";

            return button;
        }
    }
}