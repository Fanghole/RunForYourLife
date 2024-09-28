using BepInEx.Configuration;

namespace RunForYourLife
{
    internal class RFYLConfig
    {
        public static ConfigEntry<float> baseStamina;

        public static ConfigEntry<int> damageMagnitude;

        public static ConfigEntry<float> damageInterval;

        public static ConfigEntry<bool> isFatal;

        public static void Setup()
        {
            baseStamina = Plugin.Instance.Config.Bind<float>("Stamina", "BaseStamina", 0.9f, "How much stamina the player has before taking damage.");
            damageMagnitude = Plugin.Instance.Config.Bind<int>("Damage", "DamageMagnitude", 9, "The amount of damage taken per tick of damage.");
            damageInterval = Plugin.Instance.Config.Bind<float>("Damage", "DamageFrequency", 0.1f, "How much stamina taken to cause one tick of damage.");
            isFatal = Plugin.Instance.Config.Bind<bool>("Damage", "IsFatal", true, "Whether you can die from oversprinting. Otherwise it'll stop when hitting critical health.");
        }
    }
}
