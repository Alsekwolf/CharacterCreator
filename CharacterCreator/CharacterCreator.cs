using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator
{
    public class CharacterCreator : BaseScript
    {

        public CharacterCreator()
        {
            OpenMenu();
        }

        private void OpenMenu()
        {
            int maxHealth = Game.PlayerPed.MaxHealth;
            int maxArmour = Game.Player.MaxArmor;
            int health = Game.PlayerPed.Health;
            int armour = Game.PlayerPed.Armor;

            Game.Player.MaxArmor = maxArmour;
            Game.PlayerPed.MaxHealth = maxHealth;
            Game.PlayerPed.Health = health;
            Game.PlayerPed.Armor = armour;
            
            var instance = new MakeCreateCharacterMenu();
            instance.SetupMain(male: true);
        }
    }
}