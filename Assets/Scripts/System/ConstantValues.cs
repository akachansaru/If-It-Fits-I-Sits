using System.Collections;

public static class ConstantValues
{
    public static class Tags
    {
        public static string Player { get; } = "Player";
    }

    public static class Layers
    {
        public static int Default { get; } = 0;
        public static int UI { get; } = 5;
    }

    public static class LayerMasks
    {
        public static int Default { get; } = 1 << 0;
        public static int UI { get; } = 1 << 5;
    }

    public static class Scenes
    {
        public static string MainMenu { get; } = "MainMenu";
    }

    public static class Animations
    {
       
    }

    // Will need to put prefabs into the Resources folder to use these paths to instantiate
    public static class Prefabs
    {
        private static string PrefabPath { get; } = "Prefabs";    
    }
}