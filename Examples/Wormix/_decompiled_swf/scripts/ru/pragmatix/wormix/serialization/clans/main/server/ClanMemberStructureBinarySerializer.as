package ru.pragmatix.wormix.serialization.clans.main.server
{
   import §_-H16§.*;
   import §_-H2g§.§_-di§;
   import §_-l2r§.§_-K1t§;
   import §_-rM§.§_-02j§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.request.ListClansRequest;
   import ru.pragmatix.wormix.messages.clans.response.ListClansResponse;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.DisplayObject;
   
   public class ClanMemberStructureBinarySerializer implements §_-02j§
   {
      
      public function ClanMemberStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc5_:ByteArray = null;
         var _loc3_:ClanMemberStructure = new ClanMemberStructure();
         _loc3_.rank = param1.readShort();
         _loc3_.clanId = param1.readInt();
         _loc3_.clanName = param1.readUTF();
         var _temp_1:* = param1.readShort();
         var _loc4_:int = param1.readShort();
         if(_loc4_)
         {
            _loc5_ = new ByteArray();
            param1.readBytes(_loc5_,0,_loc4_);
            SerializationUtils.readUintVector(_loc5_,_loc3_.clanEmblem);
         }
         _loc3_.clanRating = param1.readInt();
         _loc3_.clanSeasonRating = param1.readInt();
         _loc3_.reviewState = param1.readShort();
         _loc3_.prevSeasonTopPlace = param1.readInt();
         return _loc3_;
      }
   }
}

