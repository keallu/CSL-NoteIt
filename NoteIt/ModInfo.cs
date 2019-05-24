using ICities;
using System;

namespace NoteIt
{
    public class ModInfo : IUserMod
    {
        public string Name => "Note It!";
        public string Description => "Manage small notes within the game.";

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group;
            bool selected;
            float selectedValue;
            float result;

            group = helper.AddGroup(Name);

            selected = ModConfig.Instance.ShowAfterLoad;
            group.AddCheckbox("Show after Load", selected, sel =>
            {
                ModConfig.Instance.ShowAfterLoad = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.SaveInterval;
            group.AddTextfield("Save Interval (in seconds)", selectedValue.ToString(), sel =>
            {
                float.TryParse(sel, out result);
                ModConfig.Instance.SaveInterval = result;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.SizeX;
            group.AddSlider("Panel Width", 160f, 1920f, 160f, selectedValue, sel =>
            {
                ModConfig.Instance.SizeX = (int)sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.SizeY;
            group.AddSlider("Panel Height", 160f, 1920f, 160f, selectedValue, sel =>
            {
                ModConfig.Instance.SizeY = (int)sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.Opacity;
            group.AddSlider("Opacity", 0.0f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.Opacity = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.OpacityWhenHover;
            group.AddSlider("Opacity When Hover", 0.0f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.OpacityWhenHover = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.TextScale;
            group.AddSlider("Text Scale", 0.0f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.TextScale = sel;
                ModConfig.Instance.Save();
            });
        }
    }
}