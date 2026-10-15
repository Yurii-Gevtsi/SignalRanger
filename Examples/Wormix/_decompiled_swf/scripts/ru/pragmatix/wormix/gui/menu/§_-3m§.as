package ru.pragmatix.wormix.gui.menu
{
   import §_-11L§.*;
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-62§.*;
   import §_-63U§.§_-41y§;
   import §_-73I§.§_-q15§;
   import §_-931§.§_-02O§;
   import §_-A1K§.§_-TA§;
   import §_-C2t§.§_-92W§;
   import §_-C3f§.§_-4I§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-G16§.*;
   import §_-H2g§.§_-di§;
   import §_-K35§.§_-618§;
   import §_-T2V§.§_-13F§;
   import §_-Tm§.§_-RF§;
   import §_-W§.§_-Ap§;
   import §_-Y1Q§.§_-61Z§;
   import §_-Y1Q§.§_-aG§;
   import §_-YF§.*;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-Z2z§;
   import §_-j1w§.*;
   import §_-p18§.§_-83X§;
   import §_-q26§.*;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-wD§.*;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.pushnotifications.PushNotifications;
   import com.hurlant.math.BigInteger;
   import config.§_-K32§;
   import feathers.controls.Button;
   import feathers.controls.ToggleButton;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.events.FeathersEventType;
   import feathers.layout.VerticalLayout;
   import flash.display.SimpleButton;
   import flash.events.*;
   import flash.geom.Rectangle;
   import flash.utils.*;
   import org.as3commons.zip.*;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.flox.social.response.LoadFriendsResponse;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.clans.response.edit.CloseExitClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanAuditActionTO;
   import ru.pragmatix.wormix.messages.server.ProfilesResult;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.display.Canvas;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-3m§ extends §_-E2w§
   {
      
      private static const §_-vl§:String = "newField";
      
      protected var iconScale:Number;
      
      protected var §_-eY§:DisplayObject;
      
      protected var item:§_-W1r§;
      
      protected var §_-TN§:Image;
      
      private var §_-O1Q§:Number;
      
      private var §_-jZ§:Number;
      
      public function §_-3m§(param1:DisplayObject, param2:Boolean = false)
      {
         if(!(param1 is DisplayObjectContainer))
         {
            param1 = this.§_-l7§(param1);
         }
         super(param1);
         if(param2 && param1.width > 0 && param1.height > 0)
         {
            §_-nJ§ = new Quad(param1.width,param1.height,16777215);
            §_-nJ§.alpha = 0.5;
            §_-j2l§().addChild(§_-nJ§);
         }
         this.§_-O1Q§ = param1.width;
         this.§_-jZ§ = param1.height;
      }
      
      private function §_-l7§(param1:DisplayObject) : DisplayObjectContainer
      {
         var _loc2_:Sprite = null;
         _loc2_ = new Sprite();
         _loc2_.x = param1.x;
         _loc2_.y = param1.y;
         _loc2_.pivotX = param1.pivotX;
         _loc2_.pivotY = param1.pivotY;
         param1.parent.addChildAt(_loc2_,param1.parent.getChildIndex(param1));
         _loc2_.addChild(param1);
         _loc2_.scaleX = param1.scaleX;
         _loc2_.scaleX = param1.scaleX;
         _loc2_.width = param1.width;
         _loc2_.height = param1.height;
         param1.x = 0;
         param1.y = 0;
         param1.pivotX = 0;
         param1.pivotY = 0;
         return _loc2_;
      }
      
      override public function §_-tx§(param1:Boolean) : void
      {
         super.§_-tx§(param1);
         if(§_-nJ§)
         {
            §_-j2l§().setChildIndex(§_-nJ§,§_-j2l§().numChildren - 1);
         }
      }
      
      public function setItem(param1:§_-W1r§, param2:Number = 0.7, param3:Boolean = false) : void
      {
         var _loc4_:Boolean = false;
         var _loc5_:Number = NaN;
         var _loc6_:Texture = null;
         if(this.item == param1 && this.iconScale == param2)
         {
            return;
         }
         this.iconScale = param2;
         this.item = param1;
         if(this.§_-eY§)
         {
            §_-j2l§().removeChild(this.§_-eY§);
            this.§_-eY§ = null;
         }
         if(param1)
         {
            _loc4_ = param1 is §_-T1y§;
            if(param1 is CraftedEquipFamily)
            {
               _loc4_ = §_-G1p§.§_-93G§((param1 as CraftedEquipFamily).§_-46§()[0].§_-62n§()) != null;
               this.§_-eY§ = _loc4_ ? this.§_-Zj§(param1) : this.§_-D2M§(param1);
            }
            else if(param1 is §_-zF§)
            {
               this.§_-eY§ = this.§_-D2M§(param1);
            }
            else if(param1 is §_-T1y§)
            {
               this.§_-eY§ = this.§_-Zj§(param1);
            }
            else
            {
               this.§_-eY§ = §_-q15§.§_-71V§(param1);
            }
            if(!this.§_-eY§)
            {
               _loc6_ = Application.assetManager.getTexture(param1.§_-92P§());
               if(!_loc6_)
               {
                  return;
               }
               this.§_-eY§ = new Image(_loc6_);
            }
            this.§_-eY§.name = param1.§_-G3a§();
            _loc5_ = 0;
            if(_loc4_)
            {
               _loc5_ = §_-K32§.§_-a1q§(param1.§_-G3a§());
            }
            this.§_-o2W§(this.§_-eY§,getContainer(),_loc5_,_loc4_ && !(this.§_-eY§ is Image));
         }
      }
      
      public function §_-o1w§(param1:Boolean) : void
      {
         if(SystemUtil.isDesktop)
         {
            return;
         }
         if(this.§_-TN§)
         {
            this.§_-TN§.visible = param1;
            this.§_-TN§.parent.setChildIndex(this.§_-TN§,this.§_-TN§.parent.numChildren - 1);
         }
         else if(param1)
         {
            this.§_-H2Y§();
         }
      }
      
      private function §_-o2W§(param1:DisplayObject, param2:DisplayObject, param3:Number, param4:Boolean) : void
      {
         var _loc7_:Number = NaN;
         var _loc8_:Number = NaN;
         var _loc9_:Number = NaN;
         §_-j2l§().addChildAt(param1,0);
         var _loc5_:Rectangle = param1.getBounds(param1);
         var _loc6_:Rectangle = param1.getBounds(param1.parent);
         if(param4 && param3 == 0)
         {
            param1.rotation = §_-618§.§_-QU§;
         }
         else
         {
            param1.rotation = Math.PI * param3 / 180;
         }
         if(Boolean(param2.width) && Boolean(param2.height))
         {
            _loc8_ = this.§_-O1Q§ * this.iconScale / param1.width;
            _loc9_ = this.§_-jZ§ * this.iconScale / param1.height;
            _loc7_ = Math.min(_loc8_,_loc9_);
            param1.width *= _loc7_;
            param1.height *= _loc7_;
         }
         if(param4)
         {
            param1.x = -_loc6_.x * _loc7_ + (this.§_-O1Q§ - param1.width) * 0.5;
            param1.y = -_loc6_.y * _loc7_ + (this.§_-jZ§ - param1.height) * 0.5;
            param1.pivotX = 0;
            param1.pivotY = 0;
         }
         else
         {
            param1.x = this.§_-O1Q§ * 0.5;
            param1.y = this.§_-jZ§ * 0.5;
            param1.pivotX = _loc5_.x + _loc5_.width * 0.5;
            param1.pivotY = _loc5_.y + _loc5_.height * 0.5;
         }
         Pool.putRectangle(_loc5_);
         Pool.putRectangle(_loc6_);
      }
      
      private function §_-Zj§(param1:§_-W1r§) : DisplayObject
      {
         var _loc2_:§_-di§ = null;
         if(§_-q15§.§_-KH§(param1.§_-G3a§()))
         {
            _loc2_ = §_-q15§.§_-p1z§(param1.§_-G3a§());
            if(!_loc2_)
            {
               return null;
            }
            return _loc2_.display;
         }
         return §_-q15§.§_-W22§(param1.§_-G3a§(),0,0);
      }
      
      private function §_-D2M§(param1:§_-W1r§) : DisplayObject
      {
         var _loc2_:Texture = null;
         var _loc3_:§_-di§ = null;
         var _loc4_:DisplayObject = null;
         if(param1.§_-92P§())
         {
            _loc2_ = Application.assetManager.getTexture(param1.§_-92P§());
            if(_loc2_)
            {
               return new Image(_loc2_);
            }
         }
         if(§_-q15§.§_-Wj§(param1.§_-G3a§()))
         {
            _loc3_ = §_-q15§.§_-R26§(param1.§_-G3a§());
            if(!_loc3_)
            {
               return null;
            }
            _loc4_ = _loc3_.display;
            _loc4_.pivotX = (width - _loc4_.width) * 0.5;
            _loc4_.pivotY = (height - _loc4_.height) * 0.5;
            return _loc4_;
         }
         return §_-q15§.§_-L31§(param1.§_-G3a§());
      }
      
      private function §_-H2Y§() : void
      {
         this.§_-TN§ = new Image(Application.assetManager.getTexture(§_-vl§));
         this.§_-TN§.width = width * 0.65;
         this.§_-TN§.scaleY = this.§_-TN§.scaleX;
         this.§_-TN§.x = width * 0.45;
         this.§_-TN§.y = height * 0.85;
         §_-j2l§().addChild(this.§_-TN§);
      }
      
      public function getItem() : §_-W1r§
      {
         return this.item;
      }
      
      override public function dispose() : void
      {
         if(§_-nJ§)
         {
            §_-nJ§.dispose();
            §_-nJ§ = null;
         }
         if(this.§_-eY§)
         {
            if(this.§_-eY§.parent == this.getContainer())
            {
               §_-j2l§().removeChild(this.§_-eY§);
            }
            this.§_-eY§.dispose();
            this.§_-eY§ = null;
         }
         super.dispose();
      }
   }
}

