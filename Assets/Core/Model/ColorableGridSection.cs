using System;

namespace NotTimeTravel.Core.Model
{
    [Serializable]
    public class ColorableGridSection
    {
        public string colorScheme;
        public string secondaryColorScheme;
        public string backgroundColorScheme;
        public int originX;
        public int originY;
        public int targetX;
        public int targetY;
    }
}