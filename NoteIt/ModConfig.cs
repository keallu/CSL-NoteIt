using System.Collections.Generic;

namespace NoteIt
{
    [ConfigurationPath("NoteItConfig.xml")]
    public class ModConfig
    {
        public bool ConfigUpdated { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public bool ShowAfterLoad { get; set; } = true;
        public float SaveInterval { get; set; } = 5.0f;
        public float SizeX { get; set; } = 480f;
        public float SizeY { get; set; } = 640f;
        public float Opacity { get; set; } = 1.0f;
        public float OpacityWhenHover { get; set; } = 1.0f;
        public int Tabs { get; set; } = 3;
        public float TextScale { get; set; } = 1.0f;
        public List<string> Notes { get; set; }

        private static ModConfig instance;

        public static ModConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Configuration<ModConfig>.Load();
                }

                return instance;
            }
        }

        public void Save()
        {
            Configuration<ModConfig>.Save();
            ConfigUpdated = true;
        }
    }
}