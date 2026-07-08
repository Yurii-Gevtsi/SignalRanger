package ru.pragmatix.wormix.gui.menu
{
   import §_-62§.§_-G1p§;
   import §_-J31§.§_-R1h§;
   import §_-Y23§.*;
   import §_-Z1w§.§_-s1x§;
   import §_-b1F§.§_-v2Q§;
   import §_-j1w§.§_-L1x§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-u1T§.§_-33B§;
   import com.hurlant.math.*;
   import dragonBones.animation.WorldClock;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.messages.server.RepeatBossBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.scene.camera.SceneCamera;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import starling.animation.IAnimatable;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class §_-W1G§ extends Sprite implements IAnimatable
   {
      
      private static const §_-QP§:Number = 2;
      
      private static const §_-Y2w§:Number = 0;
      
      private static const §_-x15§:Number = -4.5;
      
      private static const §_-4C§:Number = 7.2;
      
      private var §_-P2r§:DisplayObject;
      
      private var §_-m1v§:DisplayObject;
      
      public function §_-W1G§(param1:DisplayObjectContainer)
      {
         super();
         addChild(param1);
         this.§_-P2r§ = param1.getChildByName("wheel1");
         this.§_-m1v§ = param1.getChildByName("wheel2");
         this.§_-P2r§.rotation = §_-Y2w§;
         this.§_-m1v§.rotation = §_-QP§;
         addEventListener(Event.REMOVED_FROM_STAGE,this.stop);
         addEventListener(Event.ADDED_TO_STAGE,this.run);
      }
      
      private function run(param1:Event = null) : void
      {
         if(!Starling.juggler.contains(this))
         {
            Starling.juggler.add(this);
         }
      }
      
      private function stop(param1:Event = null) : void
      {
         if(Starling.juggler.contains(this))
         {
            Starling.juggler.remove(this);
         }
      }
      
      public function advanceTime(param1:Number) : void
      {
         this.§_-P2r§.rotation += §_-x15§ * param1;
         this.§_-m1v§.rotation += §_-4C§ * param1;
      }
      
      override public function dispose() : void
      {
         this.§_-P2r§ = §_-c1S§.dispose(this.§_-P2r§);
         this.§_-m1v§ = §_-c1S§.dispose(this.§_-m1v§);
         this.stop();
         removeEventListener(Event.REMOVED_FROM_STAGE,this.stop);
         removeEventListener(Event.ADDED_TO_STAGE,this.run);
         super.dispose();
      }
   }
}

