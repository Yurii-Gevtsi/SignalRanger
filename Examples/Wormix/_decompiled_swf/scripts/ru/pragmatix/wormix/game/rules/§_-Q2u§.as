package ru.pragmatix.wormix.game.rules
{
   import §_-5Y§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-63U§.§_-y2O§;
   import §_-82l§.§_-A2Y§;
   import §_-B2z§.§_-63i§;
   import §_-C2M§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.§_-D14§;
   import §_-P1B§.§_-b21§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-XA§;
   import §_-U1i§.§_-b1k§;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1b§.§_-OP§;
   import §_-ZP§.§_-H1H§;
   import §_-aM§.*;
   import §_-dS§.§_-D3a§;
   import §_-f1Q§.*;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-j1w§.§_-L1x§;
   import §_-l1g§.§_-L3Q§;
   import §_-m0§.§_-X1F§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import §_-sQ§.§_-V1n§;
   import §_-x2Q§.§_-dU§;
   import §_-zI§.§_-X1q§;
   import com.greensock.loading.core.*;
   import com.hurlant.crypto.tls.§_-21v§;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.ToggleButton;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontColor;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.ai.§_-12u§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.FreezingMissile;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.core.Starling;
   import starling.display.BlendMode;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.TextureSmoothing;
   import starling.utils.AssetManager;
   import starling.utils.Color;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   import starlingbuilder.engine.UIBuilder;
   
   use namespace gestouch_internal;
   
   public class §_-Q2u§
   {
      
      private static const §_-i1T§:int = 2;
      
      private static const §_-Fz§:int = 700;
      
      private static const §_-L1l§:int = 120;
      
      public function §_-Q2u§()
      {
         super();
         throw new Error("GameRulesUtil acn\'t be instantiated");
      }
      
      public static function §_-33X§(param1:UserProfile, param2:uint) : Vector.<UserProfile>
      {
         var _loc5_:UserProfile = null;
         var _loc6_:uint = 0;
         var _loc7_:Boolean = false;
         var _loc8_:uint = 0;
         var _loc3_:Vector.<UserProfile> = new Vector.<UserProfile>(0);
         var _loc4_:Array = param1.§_-n26§();
         if(_loc4_.length <= param2)
         {
            for each(_loc5_ in _loc4_)
            {
               _loc3_.push(_loc5_);
            }
         }
         else
         {
            _loc6_ = 0;
            _loc7_ = false;
            _loc8_ = 0;
            while(_loc8_ < _loc4_.length)
            {
               if(UserProfile(_loc4_[_loc8_]).equals(param1))
               {
                  _loc7_ = true;
               }
               if(!(!_loc7_ && _loc6_ == param2 - 1))
               {
                  var _loc9_:uint;
                  _loc3_[_loc9_ = _loc6_++] = _loc4_[_loc8_];
                  if(_loc6_ == param2)
                  {
                     break;
                  }
               }
               _loc8_++;
            }
         }
         return _loc3_;
      }
      
      public static function §_-x2K§(param1:UserProfile, param2:String, param3:uint, param4:§_-y1o§, param5:uint, param6:Vector.<UserProfile>) : §_-V23§
      {
         var _loc8_:UserProfile = null;
         var _loc9_:§_-01J§ = null;
         var _loc7_:§_-V23§ = new §_-V23§(param2,param3,param4,param5);
         for each(_loc8_ in param6)
         {
            _loc9_ = §_-L1x§.create(_loc8_,0,0);
            _loc9_.§_-81o§ = §_-73v§.§_-z1F§(param1);
            _loc7_.§_-D3t§(_loc9_);
            _loc7_.§_-uR§(param1);
         }
         return _loc7_;
      }
      
      public static function §_-T2R§(param1:Vector.<§_-V23§>, param2:§_-D3a§, param3:uint) : void
      {
         var _loc4_:Array = null;
         var _loc7_:Vector.<Vector.<§_-01J§>> = null;
         var _loc8_:§_-V23§ = null;
         _loc4_ = §_-X1j§.§_-H15§.§_-yw§(param2,param3);
         var _loc5_:Array = _loc4_[0];
         var _loc6_:Array = _loc4_[1];
         if(param1.length == 2)
         {
            §_-22M§(param1[0],param1[1],_loc5_,_loc6_);
         }
         else
         {
            _loc7_ = new Vector.<Vector.<§_-01J§>>();
            for each(_loc8_ in param1)
            {
               _loc7_.push(_loc8_.getUnits());
            }
            §_-M22§(_loc7_,_loc5_,_loc6_);
         }
      }
      
      public static function §_-C1b§(param1:§_-J1§, param2:§_-XA§) : §_-hK§
      {
         var _loc3_:§_-hK§ = new §_-hK§();
         _loc3_.result = param1;
         _loc3_.status = param2;
         _loc3_.§_-11J§ = §_-b1k§.§_-92C§;
         return _loc3_;
      }
      
      private static function §_-22M§(param1:§_-V23§, param2:§_-V23§, param3:Array, param4:Array) : void
      {
         var _loc10_:int = 0;
         var _loc13_:int = 0;
         var _loc5_:§_-V1n§ = null;
         var _loc6_:Vector.<§_-01J§> = param1.getUnits();
         var _loc7_:Vector.<§_-01J§> = param2.getUnits();
         setPosition(_loc6_[0],param3[0],_loc5_);
         var _loc8_:int = int.MAX_VALUE;
         var _loc9_:int = 0;
         _loc10_ = 1;
         while(_loc10_ < param3.length)
         {
            _loc13_ = Math.abs(§_-H3D§.distance(param3[0].x - param3[_loc10_].x,param3[0].y - param3[_loc10_].y) - §_-Fz§);
            if(_loc8_ > _loc13_)
            {
               _loc8_ = _loc13_;
               _loc9_ = _loc10_;
            }
            _loc10_++;
         }
         setPosition(_loc7_[0],param3[_loc9_],_loc5_);
         param3.splice(_loc9_,1);
         param3.shift();
         var _loc11_:Boolean = param1.getOwnerProfile().§_-e2A§() - param2.getOwnerProfile().§_-e2A§() > §_-L1l§;
         var _loc12_:Boolean = param1.getOwnerProfile().§_-e2A§() - param2.getOwnerProfile().§_-e2A§() > §_-L1l§;
         _loc10_ = 1;
         while(_loc10_ < Math.max(_loc6_.length,_loc7_.length))
         {
            if(_loc6_.length > _loc10_ || _loc7_.length > _loc10_)
            {
               if(_loc6_.length > _loc10_ && _loc7_.length > _loc10_)
               {
                  setPosition(_loc6_[_loc10_],param3.shift(),_loc5_);
                  setPosition(_loc7_[_loc10_],param3.shift(),_loc5_);
               }
               else if(_loc6_.length > _loc10_)
               {
                  setPosition(_loc6_[_loc10_],_loc11_ ? param4.shift() : param3.shift(),_loc5_);
               }
               else
               {
                  setPosition(_loc7_[_loc10_],_loc12_ ? param4.shift() : param3.shift(),_loc5_);
               }
            }
            _loc10_++;
         }
      }
      
      private static function §_-M22§(param1:Vector.<Vector.<§_-01J§>>, param2:Array, param3:Array) : void
      {
         var _loc6_:Vector.<§_-01J§> = null;
         var _loc7_:Array = null;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         var _loc4_:§_-V1n§ = new §_-V1n§(§_-X1j§.§_-12n§.§_-WN§().seed);
         var _loc5_:uint = param1[0].length;
         for each(_loc6_ in param1)
         {
            if(_loc6_.length > _loc5_)
            {
               _loc5_ = _loc6_.length;
            }
         }
         _loc7_ = param2.concat(param3);
         _loc8_ = 0;
         while(_loc8_ < _loc5_)
         {
            _loc9_ = 0;
            while(_loc9_ < param1.length)
            {
               if(_loc8_ < param1[_loc9_].length)
               {
                  setPosition(param1[_loc9_][_loc8_],_loc7_.shift(),_loc4_);
               }
               _loc9_++;
            }
            _loc8_++;
         }
      }
      
      private static function setPosition(param1:§_-01J§, param2:Point, param3:§_-V1n§ = null) : void
      {
         var findPos:Function;
         var checkDistance:int = 0;
         var distance:int = 0;
         var direction:int = 0;
         var unit:§_-01J§ = param1;
         var pos:Point = param2;
         var randomSeed:§_-V1n§ = param3;
         if(randomSeed)
         {
            findPos = function():Boolean
            {
               var _loc1_:Vector.<Point> = new <Point>[pos];
               var _loc2_:Point = pos;
               var _loc3_:* = 0;
               do
               {
                  _loc2_ = §_-12u§.§_-h1V§(_loc2_,direction,100);
                  if(_loc2_)
                  {
                     _loc1_.push(_loc2_);
                  }
               }
               while(Boolean(_loc2_) && _loc3_++ < checkDistance);
               var _loc4_:Point = _loc1_[_loc1_.length - 1];
               if(HitTest.unit(_loc4_.x + direction,_loc4_.y) || _loc3_ == checkDistance)
               {
                  pos = _loc1_.length > distance ? _loc1_[distance - 1] : _loc4_;
                  return true;
               }
               return false;
            };
            checkDistance = 100;
            distance = randomSeed.§_-Fh§(1,50);
            direction = int([§_-8K§.LEFT,§_-8K§.RIGHT][randomSeed.§_-Fh§(0,1)]);
            while(!HitTest.unit(pos.x,pos.y))
            {
               pos.y += 1;
            }
            --pos.y;
            if(!findPos())
            {
               direction = direction == §_-8K§.LEFT ? §_-8K§.RIGHT : §_-8K§.LEFT;
               findPos();
            }
         }
         unit.x = pos.x;
         unit.y = pos.y;
      }
      
      public static function §_-f1x§(param1:§_-hK§) : void
      {
         if(Application.userProfile.§_-u1a§() > 0)
         {
            param1.exp *= §_-i1T§;
            param1.money *= §_-i1T§;
         }
      }
   }
}

