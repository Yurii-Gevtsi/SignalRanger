package ru.pragmatix.wormix.gui.house
{
   import §_-02q§.§_-4s§;
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-23U§.*;
   import §_-62§.*;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.*;
   import §_-B2z§.§_-82u§;
   import §_-Bv§.§_-G3P§;
   import §_-C3f§.§_-yG§;
   import §_-DJ§.§_-J2l§;
   import §_-F39§.*;
   import §_-J31§.§_-R1h§;
   import §_-J31§.§_-e2J§;
   import §_-Ly§.§_-81L§;
   import §_-N8§.§_-o2w§;
   import §_-O2L§.*;
   import §_-OJ§.§_-32g§;
   import §_-OJ§.§_-L3B§;
   import §_-OJ§.§_-sY§;
   import §_-R2S§.*;
   import §_-T2h§.*;
   import §_-TF§.§_-Z1U§;
   import §_-TF§.§_-q1j§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.§_-q2v§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-dS§.§_-D3a§;
   import §_-j1w§.§_-L1x§;
   import §_-jF§.§_-PY§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-lh§.§_-z2A§;
   import §_-n1w§.*;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-u13§.MD2;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u13§.§_-F3§;
   import §_-u1m§.*;
   import §_-w2W§.WardrobeRoomScreen;
   import §_-w2i§.§_-Ia§;
   import §_-y1s§.§_-P9§;
   import com.distriqt.extension.googleidentity.GoogleIdentity;
   import com.distriqt.extension.pushnotifications.events.RegistrationEvent;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import com.hurlant.util.der.Sequence;
   import com.hurlant.util.der.§_-cz§;
   import com.hurlant.util.der.§_-l1b§;
   import config.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.List;
   import feathers.core.ITextRenderer;
   import feathers.core.PopUpManager;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.RelativePosition;
   import flash.display.DisplayObject;
   import flash.errors.IllegalOperationError;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getQualifiedClassName;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.structures.BossWormStructure;
   import ru.pragmatix.wormix.messages.structures.PreCalculatedPoints;
   import ru.pragmatix.wormix.messages.structures.RestrictionItemStructure;
   import ru.pragmatix.wormix.messages.structures.TurnStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.structures.*;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.LightningBullet;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class §_-t1t§ extends §_-PY§ implements §_-Z1U§
   {
      
      protected var §_-zP§:DisplayObjectContainer;
      
      protected var §_-mh§:ITextRenderer;
      
      protected var §_-u2I§:ITextRenderer;
      
      protected var display:DisplayObjectContainer;
      
      protected var icon:§_-P9§;
      
      public function §_-t1t§()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-z21§();
         this.§_-zP§ = this.§_-128§().getChildByName("countFields") as DisplayObjectContainer;
         this.§_-mh§ = this.§_-zP§.getChildByName("count") as ITextRenderer;
         this.§_-u2I§ = this.§_-zP§.getChildByName("countTotal") as ITextRenderer;
         §_-q1j§.register(this);
      }
      
      protected function §_-z21§() : void
      {
         this.display = §_-YQ§.§_-qM§(this.§_-v1y§(),§_-q2v§.§_-A2T§);
         addChild(this.display);
         width = this.display.width;
         height = this.display.height;
      }
      
      protected function §_-v1y§() : String
      {
         return §_-q2v§.§_-t2E§;
      }
      
      protected function §_-128§() : DisplayObjectContainer
      {
         return this.display;
      }
      
      protected function setCount(param1:int, param2:int) : void
      {
         if(param2 == 1)
         {
            this.§_-q20§();
         }
         else
         {
            this.§_-b17§(param1,param2);
            this.§_-mh§.text = param1.toString();
            this.§_-u2I§.text = "/".concat(param2);
         }
      }
      
      private function §_-b17§(param1:int, param2:int) : void
      {
         var _loc3_:uint = 0;
         var _loc4_:Number = MathUtil.§_-oL§(param1,param2);
         if(_loc4_ < 50)
         {
            _loc3_ = 13314593;
         }
         else if(_loc4_ >= 50 && _loc4_ < 100)
         {
            _loc3_ = 16750848;
         }
         else if(_loc4_ >= 100)
         {
            _loc3_ = 5216297;
         }
         §_-R2g§.§_-t8§(this.§_-mh§,_loc3_);
      }
      
      protected function §_-q20§() : void
      {
         this.§_-mh§.text = "";
         this.§_-u2I§.text = "";
      }
      
      public function getOrigin() : starling.display.DisplayObject
      {
         if(this.icon)
         {
            return this.icon.§_-d15§().getContainer();
         }
         return this.display;
      }
      
      public function §_-8I§() : starling.display.DisplayObject
      {
         return null;
      }
      
      public function §_-52e§() : Object
      {
         return new <String>[RelativePosition.BOTTOM,RelativePosition.RIGHT,RelativePosition.LEFT];
      }
      
      override public function dispose() : void
      {
         §_-q1j§.remove(this);
         this.§_-mh§ = §_-c1S§.dispose(this.§_-mh§ as starling.display.DisplayObject) as ITextRenderer;
         this.§_-u2I§ = §_-c1S§.dispose(this.§_-u2I§ as starling.display.DisplayObject) as ITextRenderer;
         this.§_-zP§ = null;
         this.display = null;
         super.dispose();
      }
   }
}

