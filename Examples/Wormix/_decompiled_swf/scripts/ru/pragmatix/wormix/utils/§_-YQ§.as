package ru.pragmatix.wormix.utils
{
   import §_-12W§.§_-221§;
   import §_-5P§.§_-H1u§;
   import §_-62§.*;
   import §_-73I§.*;
   import §_-9§.§_-52A§;
   import §_-931§.§_-02O§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.Cookie;
   import §_-B1y§.§_-F1P§;
   import §_-Bv§.§_-G3P§;
   import §_-CF§.§_-E19§;
   import §_-CR§.§_-S2o§;
   import §_-Cl§.§_-K3v§;
   import §_-Cl§.§_-i28§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-K3O§;
   import §_-DJ§.§_-T22§;
   import §_-DJ§.§_-T2G§;
   import §_-F39§.*;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-Tm§.§_-W1Y§;
   import §_-W§.*;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-C2D§;
   import §_-YF§.§_-q2v§;
   import §_-b1F§.§_-G4§;
   import §_-dS§.§_-D3a§;
   import §_-gG§.§_-Z2z§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-n1w§.*;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-r1C§.§_-h2B§;
   import §_-u13§.MD2;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u13§.§_-F3§;
   import §_-w2W§.§_-21w§;
   import §_-x2Q§.§_-dU§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import com.hurlant.util.der.Sequence;
   import com.hurlant.util.der.§_-cz§;
   import com.hurlant.util.der.§_-l1b§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.Label;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ToggleButton;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.core.PopUpManager;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import flash.events.Event;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.getDefinitionByName;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.ReconnectToBattle;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.CrossbowBolt;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class §_-YQ§
   {
      
      public static const §_-41D§:String = "icon_fuzy";
      
      public static const §_-N2x§:String = "icon_ruby";
      
      public function §_-YQ§()
      {
         super();
      }
      
      public static function removeAllPopUps() : void
      {
         var _loc2_:DisplayObject = null;
         var _loc1_:DisplayObjectContainer = Application.§_-i1N§;
         while(true)
         {
            _loc2_ = _loc1_.getChildAt(_loc1_.numChildren - 1);
            if(!(_loc2_.name != "ShopMenuBase" && PopUpManager.isPopUp(_loc2_)))
            {
               break;
            }
            PopUpManager.removePopUp(_loc2_);
         }
      }
      
      public static function §_-g27§() : Boolean
      {
         if(§_-T2G§.§_-D1y§())
         {
            return true;
         }
         var _loc1_:DisplayObjectContainer = Application.§_-i1N§;
         var _loc2_:DisplayObject = _loc1_.getChildAt(_loc1_.numChildren - 1);
         return PopUpManager.isPopUp(_loc2_);
      }
      
      public static function §_-A3J§() : DisplayObject
      {
         var _loc1_:DisplayObjectContainer = Application.§_-i1N§;
         var _loc2_:DisplayObject = _loc1_.getChildAt(_loc1_.numChildren - 1);
         if(PopUpManager.isPopUp(_loc2_))
         {
            return _loc2_;
         }
         return null;
      }
      
      public static function §_-l2B§(param1:DisplayObject, param2:Boolean, param3:Function) : void
      {
         var _loc4_:DisplayObjectContainer = Application.§_-i1N§;
         if(PopUpManager.isPopUp(param1) && !PopUpManager.isTopLevelPopUp(param1) && _loc4_.contains(param1))
         {
            PopUpManager.removePopUp(param1);
            PopUpManager.addPopUp(param1,true,param2,param3);
         }
      }
      
      public static function §_-33S§() : void
      {
         if(§_-T2G§.§_-D1y§())
         {
            §_-T2G§.§_-wr§();
            return;
         }
         var _loc1_:DisplayObject = §_-A3J§();
         if(_loc1_)
         {
            if(_loc1_ is §_-K3O§)
            {
               (_loc1_ as §_-K3O§).hide();
            }
            else
            {
               PopUpManager.removePopUp(_loc1_,true);
            }
         }
      }
      
      public static function §_-z2y§(param1:Button, param2:String, param3:String = "text", param4:Boolean = true) : void
      {
         var _loc5_:String = param4 ? §_-s2a§.§_-K3t§(param2) : param2;
         ((param1.defaultSkin as DisplayObjectContainer).getChildByName(param3) as ITextRenderer).text = _loc5_;
         ((param1.downSkin as DisplayObjectContainer).getChildByName(param3) as ITextRenderer).text = _loc5_;
      }
      
      public static function §_-S1U§(param1:DisplayObject, param2:§_-02O§, param3:String = "text") : void
      {
         var _loc4_:ITextRenderer = null;
         var _loc5_:Button = null;
         if(!param1 || !param2)
         {
            return;
         }
         if(param1 is Button)
         {
            _loc5_ = param1 as Button;
            if(Boolean(_loc5_.defaultSkin) && Boolean(_loc5_.defaultSkin is DisplayObjectContainer) && Boolean((_loc5_.defaultSkin as DisplayObjectContainer).getChildByName(param3)))
            {
               §_-q1E§(_loc5_,param2,param3);
               _loc5_.label = "";
            }
            else
            {
               _loc5_.label = §_-jU§(param2);
            }
         }
         else if(param1 is DisplayObjectContainer)
         {
            _loc4_ = (param1 as DisplayObjectContainer).getChildByName(param3) as ITextRenderer;
         }
         else
         {
            _loc4_ = param1 as ITextRenderer;
         }
         if(_loc4_)
         {
            §_-JO§(_loc4_,param2);
         }
      }
      
      private static function §_-q1E§(param1:Button, param2:§_-02O§, param3:String = "text") : void
      {
         §_-ef§(param1.defaultSkin,param2,param3);
         §_-ef§(param1.upSkin,param2,param3);
         §_-ef§(param1.downSkin,param2,param3);
         §_-ef§(param1.disabledSkin,param2,param3);
         if(param1 is ToggleButton)
         {
            §_-ef§((param1 as ToggleButton).selectedUpSkin,param2,param3);
            §_-ef§((param1 as ToggleButton).selectedDownSkin,param2,param3);
            §_-ef§((param1 as ToggleButton).selectedHoverSkin,param2,param3);
         }
      }
      
      private static function §_-ef§(param1:DisplayObject, param2:§_-02O§, param3:String = "text") : void
      {
         if(param1)
         {
            §_-JO§((param1 as DisplayObjectContainer).getChildByName(param3) as ITextRenderer,param2);
         }
      }
      
      public static function §_-JO§(param1:ITextRenderer, param2:§_-02O§) : void
      {
         if(Boolean(param1) && Boolean(param2))
         {
            param1.text = §_-jU§(param2);
            if(param2.format)
            {
               §_-F1s§(param1,param2.format);
            }
         }
      }
      
      private static function §_-jU§(param1:§_-02O§) : String
      {
         var _loc2_:Object = null;
         var _loc3_:Object = null;
         var _loc4_:String = null;
         if(param1.text)
         {
            return param1.text;
         }
         if(param1.stringId)
         {
            return §_-52A§.§_-K3t§(param1.stringId);
         }
         if(param1.template)
         {
            _loc2_ = {};
            _loc3_ = §_-K2r§(param1.template,param1.§_-73o§);
            for(_loc4_ in _loc3_)
            {
               _loc2_[_loc4_] = §_-52A§.§_-K3t§(_loc3_[_loc4_]);
            }
            return §_-F1P§.format(param1.template,_loc2_);
         }
         return "";
      }
      
      private static function §_-F1s§(param1:ITextRenderer, param2:TextFormat) : void
      {
         §_-R2g§.§_-g1H§(param1,param2.font,Number(param2.size),uint(param2.color),param2.align);
      }
      
      private static function §_-K2r§(param1:String, param2:Object) : Object
      {
         var _loc4_:String = null;
         var _loc3_:Object = {};
         for(_loc4_ in param2)
         {
            if(param1.indexOf("{" + _loc4_ + "}") != -1)
            {
               _loc3_[_loc4_] = param2[_loc4_];
            }
         }
         return _loc3_;
      }
      
      public static function §_-qM§(param1:String, param2:String = "ui", param3:Object = null) : Sprite
      {
         var uiList:XMLList = null;
         var xmlToRestore:XML = null;
         var className:String = param1;
         var source:String = param2;
         var binder:Object = param3;
         §§push(§§newactivation());
         var _loc6_:int = 0;
         var _loc7_:* = §§checkfilter(Application.assetManager.getXml(source).object);
         var _loc5_:XMLList = new XMLList("");
         while(§§hasnext(_loc7_,_loc6_))
         {
            var _temp_1:* = §§nextvalue(_loc6_,_loc7_);
            var _loc8_:* = §§nextvalue(_loc6_,_loc7_);
            with(_loc8_)
            {
               
               if(@name == className.toString())
               {
                  _loc5_[_loc6_] = _loc8_;
               }
            }
         }
         §§pop().§§slot[4] = _loc5_;
         if(!uiList || uiList.length() == 0)
         {
            return null;
         }
         xmlToRestore = uiList[0].copy();
         return §_-K3v§.restore(xmlToRestore,false,binder);
      }
      
      public static function §_-L1A§(param1:String) : DisplayObject
      {
         var uiList:XMLList = null;
         var xmlToRestore:XML = null;
         var className:String = param1;
         §§push(§§newactivation());
         var _loc4_:int = 0;
         var _loc5_:* = §§checkfilter(Application.assetManager.getXml(§_-q2v§.§_-iW§).object);
         var _loc3_:XMLList = new XMLList("");
         while(§§hasnext(_loc5_,_loc4_))
         {
            var _loc6_:* = §§nextvalue(_loc4_,_loc5_);
            var _temp_2:* = _loc6_;
            with(_loc6_)
            {
               
               if(@commonSource == className.toString())
               {
                  _loc3_[_loc4_] = _loc6_;
               }
            }
         }
         §§pop().§§slot[2] = _loc3_;
         if(!uiList || uiList.length() == 0)
         {
            return null;
         }
         xmlToRestore = uiList[0].copy();
         return §_-i28§.restore(xmlToRestore);
      }
      
      public static function §_-71H§() : void
      {
         var _loc1_:§_-T22§ = null;
         if(§_-J1H§())
         {
            return;
         }
         if(§_-N1V§.§_-42k§.isBankOpened)
         {
            §_-N1V§.§_-42k§.§_-O1W§();
         }
         else if(§_-X1j§.§_-zU§.§_-831§)
         {
            §_-X1j§.§_-zU§.§_-O1W§();
         }
         else if(§_-S2o§.§_-53s§())
         {
            §_-S2o§.§_-x6§();
         }
         else if(§_-YQ§.§_-g27§())
         {
            §_-YQ§.§_-33S§();
         }
         else if(§_-X1j§.§_-12n§.inBattle)
         {
            if(§_-X1j§.§_-12n§.isLearning && §_-X1j§.§_-12n§.§_-WN§().missionIds[0] == -1)
            {
               return;
            }
            §_-X1j§.§_-12n§.scene.gameInterface.§_-u1k§();
         }
         else
         {
            _loc1_ = Application.§_-E1k§.activeScreen as §_-T22§;
            if(_loc1_)
            {
               _loc1_.§_-X2O§();
            }
         }
      }
      
      private static function §_-J1H§() : Boolean
      {
         return Application.§_-E1k§ && Application.§_-E1k§.§_-w18§(§_-21w§.§_-lv§) || §_-J2l§.§_-u2b§.§_-h1C§() || §_-J2l§.§_-73c§.§_-h1C§() || §_-J2l§.§_-R25§.§_-h1C§() || §_-J2l§.§_-Q2L§.§_-h1C§() || §_-J2l§.§_-j28§.§_-h1C§();
      }
      
      public static function §_-42Z§(param1:§_-E19§, param2:Image) : void
      {
         var _loc3_:Texture = null;
         var _loc4_:Number = NaN;
         if(param1 == §_-E19§.§_-i2d§)
         {
            _loc3_ = Application.assetManager.getTexture(§_-41D§);
         }
         else if(param1 == §_-E19§.§_-51D§)
         {
            _loc3_ = Application.assetManager.getTexture(§_-N2x§);
         }
         if(Boolean(_loc3_) && Boolean(param2))
         {
            _loc4_ = param2.scaleX;
            param2.texture = _loc3_;
            param2.readjustSize();
            param2.scaleX = param2.scaleY = _loc4_;
         }
      }
      
      public static function §_-o1M§(param1:§_-E19§, param2:ImageLoader) : void
      {
         var _loc3_:Texture = null;
         if(param1 == §_-E19§.§_-i2d§)
         {
            _loc3_ = Application.assetManager.getTexture(§_-41D§);
         }
         else if(param1 == §_-E19§.§_-51D§)
         {
            _loc3_ = Application.assetManager.getTexture(§_-N2x§);
         }
         if(Boolean(_loc3_) && Boolean(param2))
         {
            param2.scaleMode = ScaleMode.SHOW_ALL;
            param2.maintainAspectRatio = true;
            param2.verticalAlign = VerticalAlign.MIDDLE;
            param2.horizontalAlign = HorizontalAlign.CENTER;
            param2.source = _loc3_;
         }
      }
      
      public static function §_-l21§(param1:§_-E19§) : Image
      {
         var _loc2_:Texture = null;
         if(param1 == §_-E19§.§_-i2d§)
         {
            _loc2_ = Application.assetManager.getTexture(§_-41D§);
         }
         else if(param1 == §_-E19§.§_-51D§)
         {
            _loc2_ = Application.assetManager.getTexture(§_-N2x§);
         }
         return new Image(_loc2_);
      }
      
      public static function §_-dH§(param1:Object) : void
      {
         if(param1 is Image)
         {
            (param1 as Image).readjustSize(-1,-1);
         }
      }
      
      public static function §_-pL§(param1:int, param2:Label) : void
      {
         var originalText:String = null;
         var length:int = 0;
         var i:int = 0;
         var maxWidth:int = param1;
         var label:Label = param2;
         try
         {
            label.validate();
            if(label.width > maxWidth)
            {
               originalText = label.text;
               length = originalText.length;
               i = 0;
               while(i < length)
               {
                  label.text = originalText.substring(0,i);
                  label.validate();
                  if(label.width > maxWidth)
                  {
                     label.text = originalText.substring(0,i - 1) + "...";
                     break;
                  }
                  i++;
               }
            }
         }
         catch(e:Error)
         {
         }
      }
      
      public static function §_-W2H§(param1:DisplayObject, param2:DisplayObjectContainer, param3:String = "showAll") : void
      {
         var _loc4_:Rectangle = param2.getBounds(param2);
         var _loc5_:Rectangle = param1.bounds;
         var _loc6_:Rectangle = RectangleUtil.fit(_loc5_,_loc4_,param3);
         param1.x = _loc6_.x;
         param1.y = _loc6_.y;
         param1.width = _loc6_.width;
         param1.height = _loc6_.height;
         Pool.putRectangle(_loc6_);
         Pool.putRectangle(_loc4_);
      }
      
      public static function getClip(param1:String, param2:DisplayObjectContainer) : DisplayObject
      {
         if(param2 == null)
         {
            return null;
         }
         var _loc3_:int = param1.indexOf(".");
         if(_loc3_ < 0)
         {
            return param2.getChildByName(param1);
         }
         var _loc4_:String = param1.substring(0,_loc3_);
         return getClip(param1.substr(_loc3_ + 1),param2.getChildByName(_loc4_) as DisplayObjectContainer);
      }
      
      public static function §_-Xv§(param1:FeathersControl, param2:Boolean) : void
      {
         param1.visible = param2;
         param1.includeInLayout = param2;
      }
   }
}

