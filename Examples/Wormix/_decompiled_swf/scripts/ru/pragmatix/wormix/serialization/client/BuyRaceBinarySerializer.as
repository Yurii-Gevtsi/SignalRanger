package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-G1p§;
   import §_-CF§.§_-E19§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y23§.*;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-Nl§;
   import §_-j22§.§_-5g§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-z1g§.§_-522§;
   import §_-zI§.§_-8D§;
   import com.hurlant.crypto.tls.§_-e1A§;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import flash.display.MovieClip;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.client.BuyRace;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.GameObjectUtils;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class BuyRaceBinarySerializer implements §_-D2L§
   {
      
      public function BuyRaceBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 36;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:BuyRace = BuyRace(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.raceId);
         _loc5_.writeShort(_loc3_.moneyType.id);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:BuyRace = new BuyRace();
         _loc3_.raceId = param1.readShort();
         _loc3_.moneyType = §_-E19§.§_-H1M§(param1.readShort());
         return _loc3_;
      }
   }
}

