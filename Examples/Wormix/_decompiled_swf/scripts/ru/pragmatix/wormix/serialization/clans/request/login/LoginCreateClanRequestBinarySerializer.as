package ru.pragmatix.wormix.serialization.clans.request.login
{
   import §_-21p§.§_-4w§;
   import §_-62§.§_-zF§;
   import §_-F39§.EmojiShower;
   import §_-F39§.§_-GP§;
   import §_-L1H§.§_-b2K§;
   import §_-TF§.§_-F1B§;
   import §_-TF§.§_-q1j§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import feathers.core.ITextRenderer;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.GetJoinClansMainResponse;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   
   public class LoginCreateClanRequestBinarySerializer implements §_-D2L§
   {
      
      public function LoginCreateClanRequestBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:LoginCreateClanRequest = LoginCreateClanRequest(param1);
         var _loc4_:ByteArray = new ByteArray();
         _loc4_.writeUTF(_loc3_.clanName);
         var _loc5_:ByteArray = new ByteArray();
         SerializationUtils.writeUintVector(_loc5_,_loc3_.clanEmblem);
         _loc4_.writeShort(_loc5_.length);
         _loc4_.writeBytes(_loc5_,0,_loc5_.length);
         _loc4_.writeUTF(_loc3_.clanDescription);
         var _loc6_:§_-5g§ = new §_-5g§();
         _loc6_.§_-c2D§(this.getCommandId());
         _loc6_.setLength(_loc4_.length);
         _loc6_.write(param2);
         param2.writeBytes(_loc4_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.LOGIN_CREATE_REQUEST;
      }
   }
}

