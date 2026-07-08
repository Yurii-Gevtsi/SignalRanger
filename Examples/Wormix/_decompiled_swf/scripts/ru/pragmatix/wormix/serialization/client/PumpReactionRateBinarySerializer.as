package ru.pragmatix.wormix.serialization.client
{
   import §_-31N§.§_-Yy§;
   import §_-82a§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-83E§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-fH§;
   import §_-b1F§.§_-G4§;
   import §_-j22§.§_-5g§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-z1g§.§_-5c§;
   import feathers.controls.text.BitmapFontTextRenderer;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.text.BitmapFontTextFormat;
   import flash.errors.IllegalOperationError;
   import flash.events.IEventDispatcher;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.client.PumpReactionRate;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   import starling.text.TextField;
   
   public class PumpReactionRateBinarySerializer implements §_-D2L§
   {
      
      public function PumpReactionRateBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 83;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:PumpReactionRate = PumpReactionRate(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.friendId);
         _loc5_.writeUTF(_loc3_.myName);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:PumpReactionRate = new PumpReactionRate();
         _loc4_.friendId = _loc3_.readUnsignedInt();
         _loc4_.myName = _loc3_.readUTF();
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER) + " PumpReactionRate",§_-fH§.§_-L2I§);
            return null;
         }
         return _loc4_;
      }
   }
}

