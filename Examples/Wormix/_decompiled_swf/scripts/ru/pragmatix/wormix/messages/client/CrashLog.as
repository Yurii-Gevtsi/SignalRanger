package ru.pragmatix.wormix.messages.client
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.*;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-p2U§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-fH§;
   import §_-O2L§.*;
   import §_-T2h§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-OP§;
   import §_-aM§.*;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-Oi§;
   import §_-l2r§.§_-K1t§;
   import §_-w2W§.WardrobeRoomScreen;
   import §_-w2i§.§_-Ia§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.List;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.FreezingMissile;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   public class CrashLog
   {
      
      public var platform:String;
      
      public var userId:String;
      
      public var log:String;
      
      public function CrashLog()
      {
         super();
      }
      
      public function toString() : String
      {
         return "CrashLog{ " + "platform=" + this.platform + ",userId=" + this.userId + ",log=" + this.log + "}";
      }
   }
}

