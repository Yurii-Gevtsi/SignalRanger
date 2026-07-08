package ru.pragmatix.wormix.game.rules
{
   import §_-21p§.§_-4w§;
   import §_-51p§.§_-P1R§;
   import §_-51p§.§_-W2e§;
   import §_-5Y§.*;
   import §_-62§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-92W§;
   import §_-CF§.§_-E19§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-H2g§.§_-di§;
   import §_-HD§.§_-N1J§;
   import §_-J31§.§_-e2J§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-O2L§.§_-C3d§;
   import §_-O2L§.§_-X1T§;
   import §_-RE§.§_-52p§;
   import §_-TF§.*;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-g1P§.§_-L38§;
   import §_-hO§.§_-v24§;
   import §_-jF§.*;
   import §_-k1u§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.*;
   import §_-w2i§.*;
   import §_-x22§.§_-Cb§;
   import §_-x22§.§_-G30§;
   import §_-x22§.§_-M2T§;
   import §_-x22§.§_-XR§;
   import §_-x22§.§_-xY§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import §_-z1g§.§_-y1S§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import com.hurlant.util.der.*;
   import com.worlize.websocket.§_-x2j§;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ScrollBarDisplayMode;
   import feathers.controls.ScrollInteractionMode;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.events.*;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-3p§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.DesktopShopPropertiesListItem;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.DesktopShopnfoRequirementRenderer;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.Disconnect;
   import ru.pragmatix.wormix.messages.server.GetFriendsForBossResult;
   import ru.pragmatix.wormix.messages.server.SelectStuffResult;
   import ru.pragmatix.wormix.messages.structures.SelectStuffResultStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.PvpRetryCommandRequestClient;
   import ru.pragmatix.wormix.social.*;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.LightningBullet;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   use namespace bi_internal;
   
   public class §_-nS§
   {
      
      private static const §_-B2H§:uint = 40;
      
      private static const §_-F1a§:uint = 10;
      
      private static var §_-03K§:uint = 0;
      
      private static const §_-s1W§:Array = [{
         "klass":§_-Cb§,
         "difference":§_-Cb§.§_-33v§ * 1.5
      },{
         "klass":§_-XR§,
         "difference":§_-XR§.§_-33v§ * 1.8
      },{
         "klass":§_-G30§,
         "difference":§_-G30§.§_-33v§ * 1.8
      }];
      
      public function §_-nS§()
      {
         super();
      }
      
      public static function reset() : void
      {
         §_-03K§ = 0;
      }
      
      public static function §_-R2A§(param1:§_-3p§, param2:Scene) : void
      {
         var _loc3_:Point = §_-bU§(param1,param2);
         if(_loc3_)
         {
            ++§_-03K§;
            new §_-W2e§(_loc3_);
            Pool.putPoint(_loc3_);
         }
      }
      
      public static function §_-ar§(param1:§_-3p§, param2:Scene) : void
      {
         var _loc5_:Point = null;
         var _loc3_:Vector.<int> = §_-X1j§.§_-12n§.§_-WN§().reagentsForBattle;
         if(_loc3_.length == 0)
         {
            return;
         }
         var _loc4_:uint = uint(param1.activeUnit.record.getLevel());
         if(_loc3_.length > 0 && §_-X1j§.randomSeed.§_-Fh§(1,100) < _loc4_)
         {
            _loc5_ = §_-bU§(param1,param2);
            if(_loc5_)
            {
               new §_-P1R§(_loc5_,_loc3_.shift());
               Pool.putPoint(_loc5_);
            }
         }
      }
      
      private static function §_-bU§(param1:§_-3p§, param2:Scene) : Point
      {
         return param1.activeSquad.isAi() ? §_-630§(param1,param2) : §_-Nu§(param1,param2);
      }
      
      public static function §_-U2Z§(param1:§_-3p§, param2:Scene) : Boolean
      {
         var _loc4_:Point = null;
         var _loc5_:Boolean = false;
         if(§_-03K§ >= §_-B2H§ || §_-V5§() >= §_-F1a§)
         {
            return false;
         }
         var _loc3_:Class = §_-Ct§(param1);
         if(_loc3_ != null)
         {
            _loc4_ = param1.activeSquad.isAi() ? §_-630§(param1,param2) : §_-Nu§(param1,param2);
            _loc5_ = _loc4_ != null;
            if(_loc5_)
            {
               new _loc3_(_loc4_);
               Pool.putPoint(_loc4_);
            }
            ++§_-03K§;
            return _loc5_;
         }
         return false;
      }
      
      public static function §_-p1p§(param1:§_-3p§, param2:Scene) : Boolean
      {
         var _loc4_:Point = null;
         var _loc5_:Boolean = false;
         if(§_-03K§ >= §_-B2H§ || §_-V5§() >= §_-F1a§)
         {
            return false;
         }
         var _loc3_:Class = §_-R1l§(param1);
         if(_loc3_ != null)
         {
            _loc4_ = §_-Nu§(param1,param2);
            _loc5_ = _loc4_ != null;
            if(_loc5_)
            {
               new _loc3_(_loc4_);
               Pool.putPoint(_loc4_);
            }
            ++§_-03K§;
            return _loc5_;
         }
         return false;
      }
      
      private static function §_-V5§() : uint
      {
         var count:uint = 0;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-a2S§().forEach(function(param1:§_-F3o§, ... rest):void
         {
            if(param1 is §_-xY§)
            {
               ++count;
            }
         });
         return count;
      }
      
      private static function §_-X2t§() : uint
      {
         var _loc2_:§_-xY§ = null;
         var _loc1_:uint = 0;
         for each(_loc2_ in §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([§_-xY§]))
         {
            _loc1_ += _loc2_.§_-F22§();
         }
         return _loc1_;
      }
      
      private static function §_-Ct§(param1:§_-3p§) : Class
      {
         var hpOnMap:uint;
         var Medkit:Class;
         var ourHP:int = 0;
         var enemyHP:int = 0;
         var diff:int = 0;
         var seed:int = 0;
         var l:int = 0;
         var threshold:int = 0;
         var gm:§_-3p§ = param1;
         ourHP = gm.activeUnit.hp;
         gm.§_-b2W§().forEach(function(param1:§_-01J§, ... rest):void
         {
            ourHP += param1.hp;
         });
         enemyHP = 0;
         gm.§_-83I§().forEach(function(param1:§_-01J§, ... rest):void
         {
            enemyHP += param1.hp;
         });
         hpOnMap = §_-X2t§() * 0.8;
         diff = Math.min(enemyHP - ourHP - hpOnMap,gm.activeUnit.record.§_-N23§() - gm.activeUnit.hp - hpOnMap);
         Medkit = null;
         §_-s1W§.every(function(param1:Object, ... rest):Boolean
         {
            if(param1.difference < diff)
            {
               Medkit = param1.klass;
               return true;
            }
            return false;
         });
         if(Medkit != null)
         {
            seed = §_-X1j§.randomSeed.§_-Fh§(1,100);
            l = gm.activeUnit.record.getLevel();
            threshold = gm.activeSquad.isAi() ? int(Math.min(3 * l,70)) : int(Math.max(100 - 3 * l,30));
            return seed < threshold ? Medkit : null;
         }
         return §_-X1j§.randomSeed.getBoolean() ? §_-M2T§ : null;
      }
      
      private static function §_-R1l§(param1:§_-3p§) : Class
      {
         var unit:§_-01J§ = null;
         var enemyHP:uint = 0;
         var hpOnMap:uint = 0;
         var diff:int = 0;
         var Medkit:Class = null;
         var seed:int = 0;
         var threshold:int = 0;
         var miniThreshold:int = 0;
         var gm:§_-3p§ = param1;
         var ourHP:uint = 0;
         for each(unit in gm.§_-b2W§(null,false,true))
         {
            ourHP += unit.hp;
         }
         enemyHP = 0;
         for each(unit in gm.§_-83I§(null))
         {
            enemyHP += unit.hp;
         }
         hpOnMap = §_-X2t§() * 0.9;
         diff = Math.min(enemyHP - ourHP - hpOnMap,gm.activeUnit.record.§_-N23§() - gm.activeUnit.hp - hpOnMap);
         Medkit = null;
         §_-s1W§.every(function(param1:Object, ... rest):Boolean
         {
            if(param1.difference < diff)
            {
               Medkit = param1.klass;
               return true;
            }
            return false;
         });
         seed = §_-X1j§.randomSeed.§_-Fh§(1,100);
         threshold = 8;
         miniThreshold = 35;
         if(Medkit != null && seed < threshold)
         {
            return Medkit;
         }
         return seed < miniThreshold ? §_-M2T§ : null;
      }
      
      private static function §_-630§(param1:§_-3p§, param2:Scene) : Point
      {
         var _loc3_:int = param1.activeUnit.x;
         var _loc4_:int = param1.activeUnit.y;
         while(!param2.hitTestPoint(_loc3_,_loc4_ - 20))
         {
            _loc4_ -= 20;
            if(_loc4_ <= 30)
            {
               break;
            }
         }
         return Pool.getPoint(_loc3_,_loc4_);
      }
      
      private static function §_-Nu§(param1:§_-3p§, param2:Scene) : Point
      {
         var _loc7_:uint = 0;
         var _loc8_:uint = 0;
         var _loc9_:uint = 0;
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc12_:int = 0;
         var _loc3_:Boolean = false;
         var _loc4_:* = 0;
         var _loc5_:uint = 30;
         var _loc6_:Vector.<§_-F3o§> = param2.gameObjectProcessor.cache.§_-MM§([§_-01J§,§_-L38§]);
         while(_loc4_++ < _loc5_ && !_loc3_)
         {
            _loc7_ = 20;
            _loc8_ = uint(param2.height >> 1);
            _loc9_ = param2.height - 100;
            _loc10_ = param1.activeUnit.x + §_-X1j§.randomSeed.§_-Fh§(-750,750);
            if(Physics.getGameObjectsInZone(_loc10_ - 50,-30,_loc10_ + 50,param2.height + 30,_loc6_).length > 0)
            {
               break;
            }
            _loc11_ = -1;
            _loc12_ = 10;
            while(_loc12_ < _loc8_)
            {
               if(!HitTest.point(_loc10_,_loc12_))
               {
                  _loc11_ = _loc12_;
                  break;
               }
               _loc12_ += _loc7_;
            }
            if(_loc11_ == -1)
            {
               break;
            }
            _loc12_ = _loc11_ + _loc7_;
            while(_loc12_ < _loc9_)
            {
               if(HitTest.point(_loc10_,_loc12_) && HitTest.circle(_loc10_,_loc12_,true,20))
               {
                  _loc3_ = true;
                  break;
               }
               _loc12_ += _loc7_;
            }
         }
         return _loc3_ ? Pool.getPoint(_loc10_,_loc12_ - _loc7_) : null;
      }
   }
}

