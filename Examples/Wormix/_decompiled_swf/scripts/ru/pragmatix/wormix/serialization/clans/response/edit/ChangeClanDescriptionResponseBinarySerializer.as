package ru.pragmatix.wormix.serialization.clans.response.edit
{
   import §_-31W§.*;
   import §_-73d§.§_-pT§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-TF§.§_-q1j§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import feathers.controls.SimpleScrollBar;
   import feathers.layout.Direction;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.storage.StorageType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreated;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   public class ChangeClanDescriptionResponseBinarySerializer implements §_-D2L§
   {
      
      public function ChangeClanDescriptionResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ChangeClanDescriptionResponse = new ChangeClanDescriptionResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.CHANGE_CLAN_DESCRIPTION_RESPONSE;
      }
   }
}

