package ru.pragmatix.wormix.serialization.client
{
   import §_-2k§.*;
   import §_-51M§.Cat;
   import §_-62§.§_-T1y§;
   import §_-A1K§.§_-7u§;
   import §_-A2U§.§_-A23§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-RE§.§_-qW§;
   import §_-Y1b§.§_-22N§;
   import §_-b1R§.§_-C3P§;
   import §_-g2e§.§_-q2Z§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.hurlant.crypto.tls.*;
   import com.mesmotronic.ane.AndroidFullScreen;
   import feathers.controls.Button;
   import feathers.core.IFeathersControl;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.LayoutBoundsResult;
   import feathers.layout.ViewPortBounds;
   import flash.desktop.NativeApplication;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanEmblemResponse;
   import ru.pragmatix.wormix.messages.clans.response.treasury.DonateClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.GetFriendListPage;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class GetFriendListPageBinarySerializer implements §_-D2L§
   {
      
      public function GetFriendListPageBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 18;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetFriendListPage = GetFriendListPage(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.pageIndex);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetFriendListPage = new GetFriendListPage();
         _loc3_.pageIndex = param1.readInt();
         return _loc3_;
      }
   }
}

