package ru.pragmatix.wormix.serialization.client
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-Tm§.§_-RF§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.util.der.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.List;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.client.LoginByProfileStringId;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   
   public class LoginByProfileStringIdBinarySerializer implements §_-D2L§
   {
      
      public function LoginByProfileStringIdBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 20;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc9_:ByteArray = null;
         var _loc3_:LoginByProfileStringId = LoginByProfileStringId(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.id);
         _loc5_.writeUTF(_loc3_.referrerId);
         _loc5_.writeUTF(_loc3_.authKey);
         _loc5_.writeInt(_loc3_.version);
         var _loc6_:uint = 0;
         var _loc7_:uint = _loc3_.ids.length;
         _loc5_.writeShort(_loc7_);
         _loc6_ = 0;
         while(_loc6_ < _loc7_)
         {
            _loc5_.writeUTF(_loc3_.ids[_loc6_]);
            _loc6_++;
         }
         _loc5_.writeByte(_loc3_.socialCode);
         _loc5_.writeShort(_loc3_.params.length);
         _loc6_ = 0;
         while(_loc6_ < _loc3_.params.length)
         {
            _loc5_.writeUTF(_loc3_.params[_loc6_]);
            _loc6_++;
         }
         var _loc8_:int = _loc3_.offlineBattles != null ? int(int(_loc3_.offlineBattles.length)) : 0;
         _loc5_.writeShort(_loc8_);
         if(_loc8_ > 0)
         {
            _loc6_ = 0;
            while(_loc6_ < _loc8_)
            {
               _loc9_ = new ByteArray();
               §_-616§.§_-fk§(EndBattleStructure).serializeStructure(_loc3_.offlineBattles[_loc6_],_loc9_,null);
               _loc5_.writeShort(_loc9_.length);
               _loc5_.writeBytes(_loc9_);
               _loc6_++;
            }
         }
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:LoginByProfileStringId = new LoginByProfileStringId();
         _loc3_.id = param1.readUTF();
         _loc3_.referrerId = param1.readUTF();
         _loc3_.authKey = param1.readUTF();
         _loc3_.version = param1.readInt();
         var _loc4_:uint = 0;
         _loc3_.ids = new Array();
         var _loc5_:uint = uint(param1.readShort());
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            _loc3_.ids[_loc4_] = param1.readUTF();
            _loc4_++;
         }
         _loc3_.socialCode = param1.readByte();
         _loc5_ = uint(param1.readShort());
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            _loc3_.params[_loc4_] = param1.readUTF();
            _loc4_++;
         }
         return _loc3_;
      }
   }
}

