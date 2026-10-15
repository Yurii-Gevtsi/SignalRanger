package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-5Y§.§_-h2c§;
   import §_-82a§.*;
   import §_-82l§.§_-A2Y§;
   import §_-9§.§_-52A§;
   import §_-931§.*;
   import §_-B1y§.§_-F1P§;
   import §_-D3A§.*;
   import §_-L1H§.§_-b2K§;
   import §_-TF§.§_-q1j§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-hK§;
   import §_-Yk§.§_-m2w§;
   import §_-fo§.§_-A29§;
   import §_-k1u§.§_-Vr§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.§_-Dd§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u2J§.§_-T1K§;
   import feathers.controls.LayoutGroup;
   import flash.events.Event;
   import flash.system.ApplicationDomain;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.as3commons.zip.§_-G22§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.QuitClanResponse;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class QuitClanResponseBinarySerializer implements §_-D2L§
   {
      
      public function QuitClanResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:QuitClanResponse = new QuitClanResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.QUIT_FROM_CLAN_RESPONSE;
      }
   }
}

