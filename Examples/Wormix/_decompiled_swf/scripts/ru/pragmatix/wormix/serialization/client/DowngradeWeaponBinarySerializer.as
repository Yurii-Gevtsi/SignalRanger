package ru.pragmatix.wormix.serialization.client
{
   import §_-31W§.*;
   import §_-62§.*;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-Ly§.§_-81L§;
   import §_-ZB§.§_-A10§;
   import §_-fo§.*;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.§_-Z25§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-sQ§.§_-H3D§;
   import §_-z20§.§_-Tn§;
   import com.hurlant.crypto.tls.*;
   import com.mesmotronic.ane.AndroidFullScreen;
   import flash.desktop.NativeApplication;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.DowngradeWeapon;
   import ru.pragmatix.wormix.messages.client.GetFriendListPage;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.MolotovProjectile;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class DowngradeWeaponBinarySerializer implements §_-D2L§
   {
      
      public function DowngradeWeaponBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 88;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:DowngradeWeapon = DowngradeWeapon(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.recipeId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
   }
}

