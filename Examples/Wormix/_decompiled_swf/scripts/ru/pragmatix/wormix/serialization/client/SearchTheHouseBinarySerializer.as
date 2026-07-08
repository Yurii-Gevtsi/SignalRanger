package ru.pragmatix.wormix.serialization.client
{
   import §_-31N§.§_-42G§;
   import §_-62§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-GQ§.§_-31v§;
   import §_-H3§.*;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-P2i§.§_-b7§;
   import §_-R2I§.*;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-m1g§.§_-K2W§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-82X§;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.SearchTheHouse;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.DisplayObject;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class SearchTheHouseBinarySerializer implements §_-D2L§
   {
      
      public function SearchTheHouseBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 81;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SearchTheHouse = SearchTheHouse(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.sessionKey);
         _loc5_.writeUnsignedInt(_loc3_.friendId);
         _loc5_.writeByte(_loc3_.keyNum);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:SearchTheHouse = new SearchTheHouse();
         _loc4_.sessionKey = _loc3_.readUTF();
         _loc4_.friendId = _loc3_.readUnsignedInt();
         _loc4_.keyNum = _loc3_.readByte();
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER) + " SearchTheHouse",§_-fH§.§_-L2I§);
            return null;
         }
         return _loc4_;
      }
   }
}

