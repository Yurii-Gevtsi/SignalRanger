package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-12W§.*;
   import §_-31W§.§_-Y1v§;
   import §_-5Y§.§_-h2c§;
   import §_-73I§.*;
   import §_-82a§.*;
   import §_-82l§.§_-A2Y§;
   import §_-A1K§.§_-7u§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-DJ§.§_-fH§;
   import §_-G2H§.*;
   import §_-L1H§.§_-b2K§;
   import §_-P2i§.§_-b7§;
   import §_-U1i§.§_-b1k§;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-Y1O§.*;
   import §_-a19§.§_-Y15§;
   import §_-g2r§.§_-x8§;
   import §_-gG§.§_-M2S§;
   import §_-hv§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.§_-Dd§;
   import §_-p18§.§_-83X§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u2J§.§_-T1K§;
   import §_-w2i§.§_-Ia§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.loading.*;
   import flash.display.Bitmap;
   import flash.events.*;
   import flash.geom.ColorTransform;
   import flash.net.SharedObject;
   import flash.utils.*;
   import org.as3commons.zip.*;
   import org.swiftsuspenders.*;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.*;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.ListClansResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.ShowSystemMessage;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.StaticCharge;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ListClansResponseBinarySerializer implements §_-D2L§
   {
      
      public function ListClansResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ListClansResponse = new ListClansResponse();
         _loc3_.serviceResult = param1.readShort();
         var _loc4_:uint = param1.readUnsignedShort();
         var _loc5_:uint = 0;
         while(_loc5_ < _loc4_)
         {
            param1.readShort();
            _loc3_.clans.push(§_-616§.§_-fk§(ClanTO).deserializeStructure(param1,null));
            _loc5_++;
         }
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.LIST_CLANS_RESPONSE;
      }
   }
}

