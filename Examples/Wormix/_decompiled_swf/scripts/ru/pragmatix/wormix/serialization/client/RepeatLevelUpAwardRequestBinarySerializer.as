package ru.pragmatix.wormix.serialization.client
{
   import §_-21p§.§_-4w§;
   import §_-5Y§.§_-Ma§;
   import §_-C2M§.DesktopTutorialPanel;
   import §_-C2M§.MobileTutorialPanel;
   import §_-C2M§.§_-o2f§;
   import §_-CR§.§_-02k§;
   import §_-F39§.§_-Ey§;
   import §_-K35§.§_-S18§;
   import §_-j22§.§_-5g§;
   import §_-p24§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import feathers.controls.ImageLoader;
   import feathers.controls.LayoutGroup;
   import flash.display.*;
   import flash.events.*;
   import flash.utils.*;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.gui.§_-fT§;
   import ru.pragmatix.wormix.messages.client.RepeatLevelUpAwardRequest;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.weapons.Girder;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   
   public class RepeatLevelUpAwardRequestBinarySerializer implements §_-D2L§
   {
      
      public function RepeatLevelUpAwardRequestBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 141;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:RepeatLevelUpAwardRequest = RepeatLevelUpAwardRequest(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.level);
         _loc5_.writeUTF(_loc3_.adEventId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
   }
}

