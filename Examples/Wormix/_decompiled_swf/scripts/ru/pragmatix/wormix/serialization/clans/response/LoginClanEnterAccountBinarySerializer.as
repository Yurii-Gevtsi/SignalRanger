package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-A1K§.§_-7u§;
   import §_-A2U§.*;
   import §_-Af§.§_-k1I§;
   import §_-B1y§.*;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-j15§;
   import §_-G16§.§_-33h§;
   import §_-H2g§.§_-di§;
   import §_-J31§.§_-R1h§;
   import §_-U1i§.§_-b1k§;
   import §_-YF§.§_-q2v§;
   import §_-gG§.§_-M2S§;
   import §_-hv§.*;
   import §_-j1w§.§_-L1x§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s1Q§.Artifacts;
   import §_-w2i§.§_-Ia§;
   import com.distriqt.extension.cloudstorage.CloudStorage;
   import feathers.controls.Button;
   import feathers.controls.Scroller;
   import feathers.layout.Direction;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.Touch;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.StaticCharge;
   import starling.animation.Transitions;
   import starling.animation.Tween;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class LoginClanEnterAccountBinarySerializer implements §_-D2L§
   {
      
      public function LoginClanEnterAccountBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc4_:uint = 0;
         var _loc5_:uint = 0;
         var _loc3_:LoginClanEnterAccount = new LoginClanEnterAccount();
         _loc3_.serviceResult = param1.readShort();
         _loc3_.clan = SerializationUtils.readStructure(param1 as ByteArray,ClanTO) as ClanTO;
         _loc5_ = param1.readUnsignedShort();
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            param1.readShort();
            _loc3_.chat.push(§_-616§.§_-fk§(ClanChatMessageTO).deserializeStructure(param1,null));
            _loc4_++;
         }
         _loc5_ = param1.readUnsignedShort();
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            param1.readShort();
            _loc3_.newsBoard.push(§_-616§.§_-fk§(ClanNewsTO).deserializeStructure(param1,null));
            _loc4_++;
         }
         _loc3_.startSeasonDate = param1.readInt();
         _loc3_.finishSeasonDate = param1.readInt();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.LOGIN_RESPONSE;
      }
   }
}

