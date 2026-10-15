package ru.pragmatix.wormix.messages.clans.request.login
{
   import §_-62§.*;
   import §_-931§.§_-02O§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-J2l§;
   import §_-Z1K§.§_-q24§;
   import §_-r1C§.§_-t1R§;
   import com.hurlant.math.*;
   import feathers.controls.Button;
   import feathers.layout.HorizontalAlign;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   use namespace bi_internal;
   
   public class LoginJoinClanRequest extends LoginClanBase
   {
      
      public var clanId:int;
      
      public var hostProfileId:int;
      
      public function LoginJoinClanRequest()
      {
         super();
      }
      
      override public function toString() : String
      {
         return "LoginJoinClanRequest{" + "protocolVersion=" + String(protocolVersion) + ",socialId=" + String(socialId) + ",profileId=" + String(profileId) + ",socialProfileId=" + String(socialProfileId) + ",name=" + String(name) + ",sessionKey=" + String(sessionKey) + ",clanId=" + String(this.clanId) + ",hostProfileId=" + String(this.hostProfileId) + "}";
      }
   }
}

