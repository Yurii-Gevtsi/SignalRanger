package ru.pragmatix.wormix.gui
{
   import §_-02q§.§_-11O§;
   import §_-533§.§_-c2b§;
   import §_-5P§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-zF§;
   import §_-91A§.*;
   import §_-A1K§.§_-TA§;
   import §_-A1m§.*;
   import §_-Cl§.*;
   import §_-H16§.*;
   import §_-Ly§.§_-81L§;
   import §_-Ly§.§_-I2K§;
   import §_-U1Y§.*;
   import §_-Vu§.MD5;
   import §_-YF§.§_-q2v§;
   import §_-b1F§.§_-G4§;
   import §_-fo§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-Zd§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.display.Display;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.loading.core.*;
   import com.hurlant.math.*;
   import config.§_-vR§;
   import feathers.controls.Button;
   import feathers.controls.Drawers;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ToggleButton;
   import feathers.core.FeathersControl;
   import feathers.core.IFeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.events.FeathersEventType;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.RelativePosition;
   import flash.display.BitmapData;
   import flash.events.Event;
   import flash.net.URLRequest;
   import flash.net.URLVariables;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.getDefinitionByName;
   import org.swiftsuspenders.Injector;
   import org.swiftsuspenders.§_-71M§;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.client.MobileLogin;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.ReconnectToBattle;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.textures.Texture;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   
   public class §_-z2G§ extends Drawers
   {
      
      private var §_-P2L§:Dictionary = new Dictionary();
      
      private var controls:Dictionary = new Dictionary();
      
      public function §_-z2G§(param1:IFeathersControl = null)
      {
         super(param1);
      }
      
      override protected function openOrCloseRightDrawer() : void
      {
         super.openOrCloseRightDrawer();
         §_-h2B§.play(§_-81L§.§_-o1Z§);
      }
      
      override protected function openOrCloseLeftDrawer() : void
      {
         super.openOrCloseLeftDrawer();
         §_-h2B§.play(§_-81L§.§_-o1Z§);
      }
      
      override protected function openOrCloseTopDrawer() : void
      {
         super.openOrCloseTopDrawer();
         §_-h2B§.play(§_-81L§.§_-o1Z§);
      }
      
      override protected function openOrCloseBottomDrawer() : void
      {
         super.openOrCloseBottomDrawer();
         §_-h2B§.play(§_-81L§.§_-o1Z§);
      }
      
      override public function set leftDrawer(param1:IFeathersControl) : void
      {
         if(_leftDrawer)
         {
            _leftDrawer.removeFromParent(true);
         }
         _isDraggingLeftDrawer = false;
         super.leftDrawer = param1;
         isLeftDrawerOpen = false;
      }
      
      override public function set rightDrawer(param1:IFeathersControl) : void
      {
         if(_rightDrawer)
         {
            _rightDrawer.removeFromParent(true);
         }
         _isDraggingRightDrawer = false;
         super.rightDrawer = param1;
         isRightDrawerOpen = false;
      }
      
      public function §_-K3Q§(param1:IFeathersControl, param2:ToggleButton = null, param3:int = 0, param4:int = 0) : void
      {
         var _loc5_:IFeathersControl = null;
         if(param1)
         {
            _loc5_ = this.§_-bS§(param1,param3);
         }
         else
         {
            _loc5_ = null;
         }
         this.leftDrawer = _loc5_;
         this.§_-D3f§(param2,RelativePosition.LEFT,param4);
      }
      
      public function §_-ZK§(param1:IFeathersControl, param2:ToggleButton = null, param3:int = 0, param4:int = 0) : void
      {
         var _loc5_:IFeathersControl = null;
         if(param1)
         {
            _loc5_ = this.§_-bS§(param1,0,param3);
         }
         else
         {
            _loc5_ = null;
         }
         this.rightDrawer = _loc5_;
         this.§_-D3f§(param2,RelativePosition.RIGHT,param4);
      }
      
      private function §_-bS§(param1:IFeathersControl, param2:Number = 0, param3:Number = 0) : IFeathersControl
      {
         var _loc4_:LayoutGroup = new LayoutGroup();
         var _loc5_:HorizontalLayout = new HorizontalLayout();
         _loc5_.paddingLeft = param2;
         _loc5_.paddingRight = param3;
         _loc4_.layout = _loc5_;
         if(param1 is DisplayObject)
         {
            _loc4_.addChild(param1 as DisplayObject);
         }
         return _loc4_;
      }
      
      private function §_-D3f§(param1:ToggleButton, param2:String, param3:Number = 0) : void
      {
         var _loc4_:ToggleButton = this.controls[param2];
         if(_loc4_)
         {
            _loc4_.removeEventListener(starling.events.Event.TRIGGERED,this.§_-a8§);
            _loc4_.removeFromParent();
         }
         if(param1)
         {
            addChild(param1);
            this.controls[param2] = param1;
            this.§_-P2L§[param2] = param3;
            param1.addEventListener(starling.events.Event.TRIGGERED,this.§_-a8§);
            switch(param2)
            {
               case RelativePosition.LEFT:
                  param1.x = param3;
                  break;
               case RelativePosition.RIGHT:
                  param1.x = Starling.current.stage.stageWidth;
            }
         }
      }
      
      private function §_-a8§(param1:starling.events.Event) : void
      {
         switch(param1.currentTarget)
         {
            case this.controls[RelativePosition.LEFT]:
               toggleLeftDrawer();
               break;
            case this.controls[RelativePosition.RIGHT]:
               toggleRightDrawer();
         }
      }
      
      override protected function leftDrawerOpenOrCloseTween_onUpdate() : void
      {
         super.leftDrawerOpenOrCloseTween_onUpdate();
         var _loc1_:DisplayObject = this.controls[RelativePosition.LEFT] as DisplayObject;
         if(_loc1_)
         {
            _loc1_.x = Math.max(leftDrawer.bounds.right,this.§_-P2L§[RelativePosition.LEFT]);
         }
      }
      
      override protected function rightDrawerOpenOrCloseTween_onUpdate() : void
      {
         super.leftDrawerOpenOrCloseTween_onUpdate();
         var _loc1_:DisplayObject = this.controls[RelativePosition.RIGHT] as DisplayObject;
         if(_loc1_)
         {
            _loc1_.x = Math.min(rightDrawer.bounds.left,this.§_-P2L§[RelativePosition.RIGHT]);
         }
      }
   }
}

