package ru.pragmatix.wormix.gui
{
   import §_-5P§.§_-u2H§;
   import §_-A1K§.§_-TA§;
   import §_-J3L§.§_-K2i§;
   import §_-J3b§.DailyBonusPanel;
   import §_-J3b§.PresentBonusPanel;
   import §_-J3b§.§_-A1M§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-YF§.§_-G2U§;
   import §_-p18§.§_-83X§;
   import §_-p18§.§_-V14§;
   import §_-r1C§.§_-U2p§;
   import §_-z1g§.§_-4l§;
   import com.hurlant.math.*;
   import feathers.controls.text.BitmapFontTextRenderer;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.events.FeathersEventType;
   import feathers.text.BitmapFontTextFormat;
   import flash.events.Event;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.UfoTargetPointer;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.messages.structures.LoginAwardStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.text.TextField;
   
   use namespace bi_internal;
   
   public class §_-F2n§
   {
      
      public function §_-F2n§()
      {
         super();
      }
      
      public static function §_-r1w§() : void
      {
         var _loc1_:§_-E1B§ = §_-E1B§.instance;
         var _loc2_:§_-u2H§ = §_-X1j§.§_-p1c§;
         if(!_loc2_.§_-922§())
         {
         }
         var _loc3_:LoginAwardStructure = _loc2_.§_-F33§();
         if(!_loc3_)
         {
         }
         var _loc4_:§_-d2F§ = _loc2_.§_-J3m§();
         if(!_loc4_)
         {
         }
         var _loc5_:§_-d2F§ = _loc2_.§_-82p§();
         if(_loc5_)
         {
            _loc1_.§_-w1A§(new PresentBonusPanel(_loc5_));
         }
         var _loc6_:§_-d2F§ = _loc2_.§_-Ml§();
         if(_loc6_)
         {
            _loc1_.§_-w1A§(new PresentBonusPanel(_loc6_));
         }
         var _loc7_:§_-d2F§ = _loc2_.§_-c1Y§();
         if(_loc7_)
         {
            _loc1_.§_-w1A§(new PresentBonusPanel(_loc7_));
         }
         var _loc8_:§_-4l§ = _loc2_.§_-5J§();
         if(_loc8_)
         {
            _loc1_.§_-w1A§(new DailyBonusPanel(_loc8_));
         }
         var _loc9_:§_-d2F§ = _loc2_.§_-ba§();
         if(_loc9_)
         {
            _loc1_.§_-w1A§(new §_-A1M§(_loc9_));
         }
      }
      
      public static function §_-h1w§() : void
      {
         §_-E1B§.instance.start();
      }
   }
}

