package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-ou§;
   import §_-63U§.*;
   import §_-73d§.§_-V19§;
   import §_-82a§.*;
   import §_-82l§.§_-A2Y§;
   import §_-91B§.§_-QK§;
   import §_-CF§.§_-E19§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-b2K§;
   import §_-TF§.§_-F1B§;
   import §_-TF§.§_-q1j§;
   import §_-Ty§.§_-SC§;
   import §_-YF§.§_-q2v§;
   import §_-fo§.§_-r5§;
   import §_-g2r§.§_-x8§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u2J§.§_-T1K§;
   import §_-w2i§.§_-Zd§;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import com.hurlant.math.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.*;
   import feathers.data.*;
   import feathers.layout.*;
   import flash.display.MovieClip;
   import flash.display.Stage;
   import flash.geom.Point;
   import flash.text.TextFormatAlign;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.*;
   import ru.pragmatix.wormix.messages.clans.*;
   import ru.pragmatix.wormix.messages.clans.response.TopClansResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.phase.HealBolt;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   use namespace gestouch_internal;
   use namespace bi_internal;
   
   public class TopClansResponseBinarySerializer implements §_-D2L§
   {
      
      public function TopClansResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:TopClansResponse = new TopClansResponse();
         _loc3_.serviceResult = param1.readShort();
         var _loc4_:uint = param1.readUnsignedShort();
         var _loc5_:uint = 0;
         while(_loc5_ < _loc4_)
         {
            param1.readShort();
            _loc3_.topClans.push(§_-616§.§_-fk§(ClanTO).deserializeStructure(param1,null));
            _loc5_++;
         }
         _loc4_ = param1.readUnsignedShort();
         _loc5_ = 0;
         while(_loc5_ < _loc4_)
         {
            _loc3_.oldPlaces.push(param1.readInt());
            _loc5_++;
         }
         _loc3_.rating = param1.readInt();
         _loc3_.oldPlace = param1.readInt();
         _loc3_.playerClanPosition = param1.readInt();
         _loc3_.startSeasonDate = param1.readUnsignedInt();
         _loc3_.finishSeasonDate = param1.readUnsignedInt();
         _loc3_.seasonId = param1.readInt();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.TOP_CLANS_RESPONSE;
      }
   }
}

