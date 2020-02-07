using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator
{
    public class CharacterCreator : BaseScript
    {
        private static MainMenu Main { get; set; }

        public CharacterCreator()
        {
            OpenMenu();
        }

        private async void OpenMenu()
        {
            Main = new MainMenu();
            var mainMenu = Main.GetMenu();
            Tick += Main.OnTick;
            
            uint model = (uint)GetHashKey("mp_m_freemode_01");

            if (!HasModelLoaded(model))
            {
                RequestModel(model);
                while (!HasModelLoaded(model))
                {
                    await BaseScript.Delay(0);
                }
            }

            int maxHealth = Game.PlayerPed.MaxHealth;
            int maxArmour = Game.Player.MaxArmor;
            int health = Game.PlayerPed.Health;
            int armour = Game.PlayerPed.Armor;

            SetPlayerModel(Game.Player.Handle, model);

            Game.Player.MaxArmor = maxArmour;
            Game.PlayerPed.MaxHealth = maxHealth;
            Game.PlayerPed.Health = health;
            Game.PlayerPed.Armor = armour;

            ClearPedDecorations(Game.PlayerPed.Handle);
            ClearPedFacialDecorations(Game.PlayerPed.Handle);
            SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
            SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
            SetPedEyeColor(Game.PlayerPed.Handle, 0);
            ClearAllPedProps(Game.PlayerPed.Handle);

            MakeCreateCharacterMenu.Main(male: true);
        }
    }
}