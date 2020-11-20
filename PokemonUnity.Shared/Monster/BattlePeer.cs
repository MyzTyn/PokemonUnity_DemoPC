﻿using System;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.Character;

namespace PokemonUnity.Monster
{
public partial class PokeBattle_NullBattlePeer : IPokeBattle_BattlePeer { 
  public void pbOnEnteringBattle(Combat.Battle battle,Monster.Pokemon pokemon) {
  }

  public int pbStorePokemon(Combat.Trainer player,Monster.Pokemon pokemon) {
    if (player.party.GetCount()<6) {
      player.party[player.party.GetCount()]=pokemon;
    }
    return -1;
  }

  public string pbGetStorageCreator() {
    return null;
  }

  public int pbCurrentBox() {
    return -1;
  }

  public string pbBoxName(int box) {
    return "";
  }
}
       
public partial class PokeBattle_RealBattlePeer {
  public int pbStorePokemon(Combat.Trainer player,Monster.Pokemon pokemon) {
    if (player.party.GetCount()<6) {
      player.party[player.party.GetCount()]=pokemon;
      return -1;
    } else {
      pokemon.Heal();
      //int oldcurbox=Game.GameData.PokemonStorage.currentBox;
      //int storedbox=Game.GameData.PokemonStorage.pbStoreCaught(pokemon);
      int oldcurbox=Game.GameData.Player.PC.ActiveBox;
      int? storedbox = Game.GameData.Player.PC.getIndexOfFirstEmpty();
      //if (storedbox<0) {
      if (!storedbox.HasValue) {
        //Game.UI.pbDisplayPaused(Game._INTL("Can't catch any more..."));
        return oldcurbox;
      } else {
        return storedbox.Value;
      }
    }
  }

  public string pbGetStorageCreator() {
    string creator=null;
    //if (Game.GameData != null && Game.GameData.Global.seenStorageCreator) {
    if (Game.GameData != null && Game.GameData.Player.IsCreator) {
      //creator=Game.GameData.PokemonStorage.pbGetStorageCreator();
      creator="someone"; //ToDo...
    }
    return creator;
  }

  public int pbCurrentBox() {
    //return Game.GameData.PokemonStorage.currentBox;
    return Game.GameData.Player.PC.ActiveBox;
  }

  public string pbBoxName(int box) {
   //return box<0 ? "" : Game.GameData.PokemonStorage[box].name;
   return box<0 ? "" : Game.GameData.Player.PC.BoxNames[box];
  }
}
    
public partial class PokeBattle_BattlePeer {
  public static PokeBattle_RealBattlePeer create() {
    return new PokeBattle_RealBattlePeer();
  }
}

    public interface IPokeBattle_BattlePeer
    {
        string pbBoxName(int box);
        int pbCurrentBox();
        string pbGetStorageCreator();
        void pbOnEnteringBattle(Combat.Battle battle, Monster.Pokemon pokemon);
        int pbStorePokemon(Combat.Trainer player, Monster.Pokemon pokemon);
    }
}