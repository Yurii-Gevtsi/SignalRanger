package ru.pragmatix.wormix.messages.client
{
   import §_-12W§.§_-X2w§;
   import §_-12W§.§_-l23§;
   import §_-21p§.§_-4w§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-D3A§.*;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-TF§.§_-q1j§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-T2N§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-sQ§.§_-V1n§;
   import com.greensock.loading.*;
   import com.greensock.loading.core.LoaderItem;
   import feathers.controls.LayoutGroup;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.Image;
   import starling.events.Event;
   
   public class RepeatLevelUpAwardRequest
   {
      
      public var level:int;
      
      public var adEventId:String;
      
      public function RepeatLevelUpAwardRequest(param1:int, param2:String = "")
      {
         super();
         this.level = param1;
         this.adEventId = param2;
      }
      
      public function toString() : String
      {
         return "RepeatLevelUpAwardRequest{" + "level=" + this.level + ",adEventId=" + this.adEventId + "}";
      }
   }
}

