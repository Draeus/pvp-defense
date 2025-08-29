using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;
using static TShockAPI.GetDataHandlers;

namespace PvpDefense
{
    [ApiVersion(2, 1)]
    public class PvpDefense : TerrariaPlugin
    {
        public override string Name => "PvPDefense";
        public override Version Version => new Version(1, 0);
        public override string Author => "Draeus7";
        public override string Description => "Adjusts PvP damage reduction based on defense.";

        private double defenseEffectiveness = 0.5;

        private bool critToggle = false;

        public static string path = Path.Combine(TShock.SavePath, "PvpDefense.json");

        public static Config Config = new Config();

        public PvpDefense(Main game) : base(game) { }

        public override void Initialize()
        {
            GeneralHooks.ReloadEvent += OnReload;

            if (File.Exists(path))
                Config = Config.Read();
            else
                Config.Write();
            defenseEffectiveness = Config.defenseEffectiveness;
            critToggle = Config.critToggle;
            GetDataHandlers.PlayerDamage += OnPlayerDamage;
        }

        private void OnReload(ReloadEventArgs e)
        {
            if (File.Exists(path))
                Config = Config.Read();
            else
                Config.Write();
            defenseEffectiveness = Config.defenseEffectiveness;
            critToggle = Config.critToggle;
            e.Player.SendMessage($"PvpDefense reloaded! Defense Eff: {defenseEffectiveness}, Crits {critToggle}", Color.Firebrick);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                GetDataHandlers.PlayerDamage -= OnPlayerDamage;
            base.Dispose(disposing);
        }

        //This function triggers when you take a hurt call in pvp, and it'll modify the damage and defense as desired.
        private void OnPlayerDamage(object sender, PlayerDamageEventArgs args)
        {
            //check if the attack is pvp and is valid
            TSPlayer target = TShock.Players[args.ID];
            TSPlayer attacker = args.Player;
            if (attacker == null || target == null || attacker == target) return;
            if (!attacker.TPlayer.hostile || !target.TPlayer.hostile) return;
            
            //calculate the proper input damage. This will be reduced when the client receives it by their defense value.
            int initialDamage = args.Damage;
            int defense = target.TPlayer.statDefense;
            bool isCrit = args.Critical;

            double vanillaDefenseEffectiveness = Main.GameMode switch
            {
                0 => 0.5,
                1 => 0.75,
                2 => 1.0,
                _ => 1.0
            };

            int vanillaDamageReduced = (int)(defense * vanillaDefenseEffectiveness);
            int damageReduced = (int)(defense * defenseEffectiveness);
            int adjustedDamage = Math.Max(1, initialDamage + vanillaDamageReduced - damageReduced);

            // Remove the vanilla hurt packet, set cleanup to true.
            args.Handled = true;

            // sending our own hurt packet
            Main.QueueMainThreadAction(() =>
            {
                NetMessage.SendPlayerHurt(
                    target.Index,
                    PlayerDeathReason.ByPlayer(attacker.TPlayer.whoAmI),
                    adjustedDamage,
                    attacker.TPlayer.direction,
                    critical: isCrit,
                    pvp: false,
                    10
                );

            });
            // optional log for debugging
            TShock.Log.ConsoleInfo(
                $"[PvP] {attacker.Name} → {target.Name}: Def {defense}, Eff {vanillaDamageReduced} -> {damageReduced}, Dmg {Math.Max(1, initialDamage - vanillaDamageReduced)} -> {Math.Max(1, adjustedDamage - vanillaDamageReduced)} Crit: {isCrit}"
            );
        }
    }
}
