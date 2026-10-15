package ru.pragmatix.wormix.gui.menu
{
   import §_-21p§.§_-4w§;
   import §_-31W§.AwardCell;
   import §_-31W§.AwardListTab;
   import §_-9§.§_-52A§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-ts§;
   import §_-CF§.§_-p2U§;
   import §_-Cl§.*;
   import §_-DJ§.§_-J2l§;
   import §_-H2g§.§_-di§;
   import §_-H3§.*;
   import §_-K35§.§_-L1O§;
   import §_-Ly§.§_-81L§;
   import §_-P1z§.*;
   import §_-S1N§.*;
   import §_-TF§.§_-Ud§;
   import §_-TF§.§_-y2n§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-b1k§;
   import §_-YF§.*;
   import §_-aM§.*;
   import §_-g2r§.§_-03A§;
   import §_-j1w§.§_-L1x§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-p18§.§_-vd§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-z1g§.*;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.events.LoaderEvent;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ToggleButton;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.HorizontalLayoutData;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalLayoutData;
   import flash.errors.IllegalOperationError;
   import flash.net.URLLoader;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.clearTimeout;
   import flash.utils.setTimeout;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.client.GetProfiles;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.pvp.messages.client.PvpDropPlayer;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.weapons.ammo.feats.IFrameFeat;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class §_-51d§ extends §_-E2w§
   {
      
      private var panel:DisplayObject;
      
      private var items:Array = [];
      
      public function §_-51d§(param1:DisplayObjectContainer)
      {
         super(param1);
         this.panel = getChildByName("panel");
      }
      
      public function §_-L2n§(param1:String, param2:String, param3:Function) : Button
      {
         var _loc4_:Button = getChildByName(param1) as Button;
         if(param2)
         {
            _loc4_.label = param2;
         }
         _loc4_.addEventListener(Event.TRIGGERED,param3);
         this.items.push(_loc4_);
         return _loc4_;
      }
      
      public function §_-IR§(param1:int, param2:int) : void
      {
         if(param1 + container.width >= §_-81L§.stageWidth)
         {
            container.x = §_-81L§.stageWidth - container.width - 5;
         }
         else
         {
            container.x = param1 - 5;
         }
         if(param2 + container.height >= §_-81L§.stageHeight)
         {
            container.y = §_-81L§.stageHeight - container.height - 5;
         }
         else
         {
            container.y = param2 - 5;
         }
         Starling.current.stage.addChild(container);
      }
      
      public function hide() : void
      {
         Starling.current.stage.removeChild(container);
      }
      
      public function §_-x18§() : void
      {
         this.hide();
      }
      
      public function §_-51R§() : DisplayObject
      {
         return container;
      }
      
      override public function dispose() : void
      {
         this.items = null;
         this.panel = null;
         super.dispose();
      }
   }
}

