using MelonLoader;
using BTD_Mod_Helper;
using DeckOCards;
using HarmonyLib;
using Assets.Scripts.Unity.UI_New.InGame.Stats;
using Assets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;
using static Il2CppSystem.Runtime.Remoting.RemotingServices;
using BTD_Mod_Helper.Api;
using Assets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using System;
using System.Security.Cryptography;
using UnityEngine;
using Random = System.Random;
using Assets.Scripts.Models.Bloons;
using BTD_Mod_Helper.Api.Bloons;
using BTD_Mod_Helper.Api.Enums;
using Assets.Scripts.Simulation.Bloons;

[assembly: MelonInfo(typeof(DeckOCards.DeckOCards), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace DeckOCards;
public class SusDisplay : ModDisplay
{
    public override string BaseDisplay => Generic2dDisplay;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        Set2DTexture(node, "SusDisplay");
    }
}
class Global
{
    public static int card = 0;
    public static int card1 = 0;
    public static bool card1allowed = false;
    public static string carddesc = "No Card Active";
    public static string carddesc1 = "No Card Active";
}
public class DeckOCards : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<DeckOCards>("DeckOCards loaded!");
    }
    public override void OnRoundEnd()
    {
        Random rnd = new Random();
        int Randomnumber = rnd.Next(1, 9);
        int round = InGame.instance.bridge.GetCurrentRound();
        bool allowed = false;
        if (Global.card == 0)
        {
            allowed = true;
        }
        else if (Global.card1 == 0 && Global.card1allowed == true)
        {
            allowed = true;
        }
        if (allowed == true)
        {
            int setcard = 0;
            string setcarddesc = "No Card Active";
            switch (Randomnumber)
            {
                case 1:
                    //Money
                    if (round < 40)
                    {
                        setcard = 1;
                        setcarddesc = "Gain $250";
                    }
                    if (round < 60 && round > 39)
                    {
                        setcard = 2;
                        setcarddesc = "Gain $1000";
                    }
                    if (round > 59)
                    {
                        setcard = 3;
                        setcarddesc = "Gain $2500";
                    }
                    break;
                case 2:
                    if (round < 40)
                    {
                        setcard = 4;
                        setcarddesc = "Slow bloons by 50%";
                    }
                    if (round < 60 && round > 39)
                    {
                        setcard = 5;
                        setcarddesc = "Slow bloons by 75%";
                    }
                    if (round > 59)
                    {
                        setcard = 6;
                        setcarddesc = "Slow bloons by 90%";

                    }
                    break;
                case 3:
                    if (round < 40)
                    {
                        setcard = 7;
                        setcarddesc = "Deal 3 Damage to all bloons";
                    }
                    if (round < 60 && round > 39)
                    {
                        setcard = 8;
                        setcarddesc = "Deal 7 Damage to all bloons";
                    }
                    if (round > 59)
                    {
                        setcard = 9;
                        setcarddesc = "Deal 500 Damage to all bloons";

                    }
                    break;
                case 4:
                    if (round < 40)
                    {
                        setcard = 10;
                        setcarddesc = "Spawn 100 Green bloons";
                    }
                    if (round < 60 && round > 39)
                    {
                        setcard = 11;
                        setcarddesc = "Spawn 100 Purple bloons";
                    }
                    if (round > 59)
                    {
                        setcard = 12;
                        setcarddesc = "Spawn 50 MOABs";
                    }
                    break;
                case 5:
                    if (round < 40)
                    {
                        setcard = 13;
                        setcarddesc = "Move All Towers";
                    }
                    if (round < 60 && round > 39)
                    {
                        setcard = 14;
                        setcarddesc = "All towers are 10% slower";
                    }
                    if (round > 59)
                    {
                        setcard = 15;
                        setcarddesc = "All towers are 25% faster";
                    }
                    break;
                case 6:
                    if (round < 40)
                    {
                        setcard = 16;
                        setcarddesc = "Move forward 1 round";
                    }
                    if (round < 60 && round > 39)
                    {
                        setcard = 17;
                        setcarddesc = "Move forward 3 rounds";
                    }
                    if (round > 59)
                    {
                        setcard = 18;
                        setcarddesc = "Move forward 5 rounds";
                    }
                    break;
                case 7:
                    if (round < 40)
                    {
                        setcard = 19;
                        setcarddesc = "Move backwards 1 round";
                    }
                    if (round < 60 && round > 39)
                    {
                        setcard = 20;
                        setcarddesc = "Move backwards 2 rounds";
                    }
                    if (round > 59)
                    {
                        setcard = 21;
                        setcarddesc = "Move backwards 3 rounds";
                    }
                    break;
            }
            if(Global.card == 0)
            {
                Global.card = setcard;
                Global.carddesc = setcarddesc;
            }
            else
            {
                Global.card1 = setcard;
                Global.carddesc1 = setcarddesc;
            }
        }
        base.OnRoundEnd();
    }
}
[HarmonyPatch(typeof(RoundDisplay), nameof(RoundDisplay.OnUpdate))]
public sealed class Display
{
    public static string style = "{0:n0}: {3:P2}";
    [HarmonyPostfix]
    public static void Fix(ref RoundDisplay __instance)
    {
        if (Input.GetKey(KeyCode.F1))
        {
            if (Global.card != 0)
            {
                switch (Global.card)
                {
                    case 1:
                        InGame.instance.AddCash(250);
                        break;
                    case 2:
                        InGame.instance.AddCash(1000);
                        break;
                    case 3:
                        InGame.instance.AddCash(2500);
                        break;
                    case 4:
                        foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                        {
                            bloon.GetBloon().trackSpeedMultiplier = .5f;
                        }
                        break;
                    case 5:
                        foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                        {
                            bloon.GetBloon().trackSpeedMultiplier = .25f;
                        }
                        break;
                    case 6:
                        foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                        {
                            bloon.GetBloon().trackSpeedMultiplier = .1f;
                        }
                        break;
                    case 7:
                        foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                        {
                            bloon.GetBloon().ExecuteDamageTask(3.0f, null, true, false, true, null, false);
                        }
                        break;
                    case 8:
                        foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                        {
                            bloon.GetBloon().ExecuteDamageTask(7.0f, null, true, false, true, null, false);

                        }
                        break;
                    case 9:
                        foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                        {
                            bloon.GetBloon().ExecuteDamageTask(500.0f, null, true, false, true, null, false);
                        }
                        break;
                    case 10:
                        InGame.instance.SpawnBloons("Green", 50, 3);
                        break;
                    case 11:
                        InGame.instance.SpawnBloons("Purple", 50, 3);
                        break;
                    case 12:
                        InGame.instance.SpawnBloons("Moab", 50, 3);
                        break;
                    case 13:
                        foreach (var tower in InGame.instance.bridge.GetAllTowers())
                        {
                            Vector2 TowerPos = new Vector2(tower.tower.Node.position.X, tower.tower.Node.position.Y);
                            TowerPos.Set(TowerPos.x + UnityEngine.Random.RandomRange(-10f, 10f), TowerPos.y + UnityEngine.Random.RandomRange(-10f, 10f));
                            tower.tower.Node.position.X = TowerPos.x;
                            tower.tower.Node.position.Y = TowerPos.y;
                        }
                            break;
                    case 14:
                        foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                        {
                            bloon.GetBloon().trackSpeedMultiplier = 2f;
                        }
                        break;
                    case 15:
                        InGame.instance.SetRound((InGame.instance.bridge.GetCurrentRound() - 3));
                        break;
                    case 16:
                        InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() + 1);
                        break;
                    case 17:
                        InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() + 3);
                        break;
                    case 18:
                        InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() + 5);
                        break;
                    case 19:
                        InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() - 1);
                        break;
                    case 20:
                        InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() - 2);
                        break;
                    case 21:
                        InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() - 3);
                        break;
                }
                Global.carddesc = "No Card Active";
                Global.card = 0;
            }
        }
        if (Input.GetKey(KeyCode.F2))
        {
            if (Global.card1allowed == true)
            {
                if (Global.card1 != 0)
                {
                    switch (Global.card1)
                    {
                        case 1:
                            InGame.instance.AddCash(250);
                            break;
                        case 2:
                            InGame.instance.AddCash(1000);
                            break;
                        case 3:
                            InGame.instance.AddCash(2500);
                            break;
                        case 4:
                            foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                            {
                                bloon.GetBloon().trackSpeedMultiplier = .5f;
                            }
                            break;
                        case 5:
                            foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                            {
                                bloon.GetBloon().trackSpeedMultiplier = .25f;
                            }
                            break;
                        case 6:
                            foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                            {
                                bloon.GetBloon().trackSpeedMultiplier = .1f;
                            }
                            break;
                        case 7:
                            foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                            {
                                bloon.GetBloon().ExecuteDamageTask(3.0f, null, true, false, true, null, false);
                            }
                            break;
                        case 8:
                            foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                            {
                                bloon.GetBloon().ExecuteDamageTask(7.0f, null, true, false, true, null, false);

                            }
                            break;
                        case 9:
                            foreach (var bloon in InGame.instance.bridge.GetAllBloons())
                            {
                                bloon.GetBloon().ExecuteDamageTask(500.0f, null, true, false, true, null, false);
                            }
                            break;
                        case 10:
                            InGame.instance.SpawnBloons("Green", 100, 3);
                            break;
                        case 11:
                            InGame.instance.SpawnBloons("Purple", 100, 3);
                            break;
                        case 12:
                            InGame.instance.SpawnBloons("Moab", 50, 3);
                            break;
                        case 13:
                            foreach (var tower in InGame.instance.bridge.GetAllTowers())
                            {
                                Vector2 TowerPos = new Vector2(tower.tower.Node.position.X, tower.tower.Node.position.Y);
                                TowerPos.Set(TowerPos.x + UnityEngine.Random.RandomRange(-10f, 10f), TowerPos.y + UnityEngine.Random.RandomRange(-10f, 10f));
                                tower.tower.Node.position.X = TowerPos.x;
                                tower.tower.Node.position.Y = TowerPos.y;
                            }
                            break;
                        case 14:
                            foreach (var tower in InGame.instance.bridge.GetAllTowers())
                            {
                                tower.tower.towerModel.GetAttackModel().weapons[0].Rate *= .9f;
                            }
                            break;
                        case 15:
                            foreach (var tower in InGame.instance.bridge.GetAllTowers())
                            {
                                tower.tower.towerModel.GetAttackModel().weapons[0].Rate *= 1.25f;
                            }
                            break;
                        case 16:
                            InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() + 1);
                            break;
                        case 17:
                            InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() + 3);
                            break;
                        case 18:
                            InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() + 5);
                            break;
                        case 19:
                            InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() - 1);
                            break;
                        case 20:
                            InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() - 2);
                            break;
                        case 21:
                            InGame.instance.SetRound(InGame.instance.bridge.GetCurrentRound() - 3);
                            break;
                    }
                    Global.carddesc1 = "No Card Active";
                    Global.card1 = 0;
                }
            }
            else
            {
                if (InGame.instance.GetCash() > 9999)
                {
                    Global.card1allowed = true;
                    InGame.instance.AddCash(-10000);
                }
            }
        }
        __instance.text.text = $"{__instance.cachedRoundDisp}\n";
        __instance.text.text += Global.carddesc + "\n";
        if (Global.card1allowed == true)
        {
            __instance.text.text += Global.carddesc1 + "\n";
            __instance.text.text += "F1 and F2 to use";
        }
        else
        {
            __instance.text.text += "Slot 2 Cost: $10000 (F2)" + "\n";
            __instance.text.text += "F1 to use";
        }
    }
}
