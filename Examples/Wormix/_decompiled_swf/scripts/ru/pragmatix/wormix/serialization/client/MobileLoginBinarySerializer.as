package ru.pragmatix.wormix.serialization.client
{
   import §_-21p§.§_-4w§;
   import §_-Bv§.§_-G3P§;
   import §_-DJ§.§_-J2l§;
   import §_-TF§.§_-P19§;
   import §_-TF§.§_-q1j§;
   import §_-YF§.§_-q2v§;
   import §_-j22§.§_-5g§;
   import §_-p24§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-y1s§.§_-P9§;
   import com.hurlant.util.der.*;
   import flash.display.BitmapData;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.setInterval;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.messages.client.MobileLogin;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class MobileLoginBinarySerializer implements §_-D2L§
   {
      
      public function MobileLoginBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc8_:ByteArray = null;
         var _loc3_:MobileLogin = MobileLogin(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.platformId);
         _loc5_.writeInt(_loc3_.version);
         _loc5_.writeUTF(_loc3_.deviceId);
         _loc5_.writeUTF(_loc3_.authKey);
         var _loc6_:uint = 0;
         _loc5_.writeShort(_loc3_.params.length);
         _loc6_ = 0;
         while(_loc6_ < _loc3_.params.length)
         {
            _loc5_.writeUTF(_loc3_.params[_loc6_]);
            _loc6_++;
         }
         var _loc7_:int = _loc3_.offlineBattles != null ? int(int(_loc3_.offlineBattles.length)) : 0;
         _loc5_.writeShort(_loc7_);
         if(_loc7_ > 0)
         {
            _loc6_ = 0;
            while(_loc6_ < _loc7_)
            {
               _loc8_ = new ByteArray();
               §_-616§.§_-fk§(EndBattleStructure).serializeStructure(_loc3_.offlineBattles[_loc6_],_loc8_,null);
               _loc5_.writeShort(_loc8_.length);
               _loc5_.writeBytes(_loc8_);
               _loc6_++;
            }
         }
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

