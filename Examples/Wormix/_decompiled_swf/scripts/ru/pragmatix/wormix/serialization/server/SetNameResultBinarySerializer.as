package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-TA§;
   import §_-H2g§.§_-di§;
   import §_-l2r§.§_-K1t§;
   import §_-n1w§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import flash.text.TextFormatAlign;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.server.SetNameResult;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.weapons.ammo.AirCatAmmo;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.display.DisplayObject;
   
   public class SetNameResultBinarySerializer implements §_-D2L§
   {
      
      public function SetNameResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10103;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SetNameResult = new SetNameResult();
         _loc3_.result = param1.readShort();
         _loc3_.renameAct = param1.readByte();
         _loc3_.name = param1.readUTF();
         return _loc3_;
      }
   }
}

