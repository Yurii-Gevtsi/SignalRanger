package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-5Y§.*;
   import §_-82a§.SwitchedRenderTextures;
   import §_-B1y§.*;
   import §_-B2z§.§_-82u§;
   import §_-F39§.§_-a2W§;
   import §_-n1w§.*;
   import §_-sQ§.§_-H3D§;
   import flash.errors.IllegalOperationError;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.PromoteRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   public class PromoteRankInClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function PromoteRankInClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:PromoteRankInClanRequest = PromoteRankInClanRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.profileId);
         _loc3_.writeShort(_loc2_.targetRank);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.PROMOTE_IN_RANK_REQUEST;
      }
   }
}

