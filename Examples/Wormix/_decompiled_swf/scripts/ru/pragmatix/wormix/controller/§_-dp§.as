package ru.pragmatix.wormix.controller
{
   import §_-12W§.*;
   import §_-12a§.§_-G2l§;
   import §_-21p§.§_-4w§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-zF§;
   import §_-73I§.*;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-Bv§.§_-G3P§;
   import §_-C3f§.*;
   import §_-Cl§.§_-L2z§;
   import §_-DJ§.§_-J2l§;
   import §_-F3k§.§_-d1U§;
   import §_-G16§.*;
   import §_-H2g§.§_-di§;
   import §_-L2F§.§_-I1q§;
   import §_-L2F§.§_-t2Z§;
   import §_-Ly§.§_-81L§;
   import §_-OJ§.§_-L3B§;
   import §_-R2I§.PvpTab;
   import §_-S1N§.§_-C14§;
   import §_-S1N§.§_-RO§;
   import §_-U1i§.§_-b1k§;
   import §_-Y1b§.§_-u1B§;
   import §_-a19§.*;
   import §_-b1F§.§_-G4§;
   import §_-f1G§.§_-kf§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-Vr§;
   import §_-k2s§.§_-Go§;
   import §_-k2s§.§_-b0§;
   import §_-l1K§.§_-cZ§;
   import §_-n1w§.*;
   import §_-nE§.DowngradeCraftWeaponScreen;
   import §_-nE§.InfoCraftWeaponScreen;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.UpgradeCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-p18§.§_-83X§;
   import §_-p24§.MissionTab;
   import §_-r1C§.§_-h2B§;
   import §_-u13§.MAC;
   import §_-u13§.MD2;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u13§.§_-F3§;
   import §_-y1s§.*;
   import §_-z1g§.§_-lp§;
   import §_-zI§.§_-sI§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.crypto.*;
   import com.hurlant.util.der.Sequence;
   import com.hurlant.util.der.§_-cz§;
   import com.hurlant.util.der.§_-l1b§;
   import config.§_-V2w§;
   import feathers.controls.Panel;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.core.ITextRenderer;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import flash.display.DisplayObject;
   import flash.errors.IllegalOperationError;
   import flash.events.*;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.utils.*;
   import org.as3commons.zip.*;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.CommandState;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.*;
   import ru.pragmatix.wormix.gui.menu.arena.boss.BossTypesTab;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.*;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.GetFriendsForBoss;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.rewards.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.model.user.§_-uH§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.KeyboardEvent;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   use namespace gestouch_internal;
   
   public class §_-dp§
   {
      
      private static const §_-j2x§:uint = §_-Y15§.all.length;
      
      private static const §_-g2s§:int = 200;
      
      private static const §_-SO§:int = 500;
      
      private static const §_-5A§:uint = 5;
      
      private static const §_-r14§:uint = 5;
      
      private static const §_-N2I§:uint = 40;
      
      private static const §_-H2i§:uint = 20;
      
      private var §_-d2g§:§_-u1B§;
      
      private var §_-82H§:uint;
      
      private var §_-fK§:uint;
      
      public function §_-dp§()
      {
         super();
         this.§_-d2g§ = new §_-u1B§();
      }
      
      public function §_-D3J§() : void
      {
         this.§_-d2g§.§_-f1E§();
      }
      
      public function §_-R2f§(param1:UserProfile) : Boolean
      {
         var _loc2_:uint = 0;
         var _loc3_:uint = 0;
         var _loc5_:UserProfile = null;
         _loc2_ = _loc3_ = uint(param1.getLevel());
         var _loc4_:int = 0;
         for each(_loc5_ in param1.§_-A1U§())
         {
            _loc4_ += _loc5_.getLevel() - param1.getLevel();
         }
         _loc4_ = Math.ceil(_loc4_ / param1.§_-A1U§().length);
         if(_loc4_ > 0)
         {
            _loc3_ += _loc4_;
         }
         if(_loc2_ != this.§_-fK§ || _loc3_ != this.§_-82H§)
         {
            this.§_-fK§ = _loc2_;
            this.§_-82H§ = _loc3_;
            return true;
         }
         return false;
      }
      
      public function §_-K3C§() : Vector.<UserProfile>
      {
         var _loc1_:Vector.<UserProfile> = new Vector.<UserProfile>(0);
         this.§_-R2f§(Application.userProfile);
         var _loc2_:Vector.<§_-Uv§> = this.§_-22g§();
         var _loc3_:uint = 0;
         while(_loc3_ < §_-j2x§)
         {
            _loc1_.push(this.§_-c6§(_loc2_[_loc3_],§_-Y15§.all[_loc3_],null));
            _loc3_++;
         }
         return _loc1_;
      }
      
      public function §_-T1T§(param1:§_-b1k§) : UserProfile
      {
         var _loc2_:Array = null;
         _loc2_ = this.§_-3e§(param1);
         var _loc3_:uint = uint(_loc2_[int(Math.random() * _loc2_.length)]);
         return this.§_-c6§(this.§_-22g§()[_loc3_],§_-Y15§.all[_loc3_],null);
      }
      
      public function §_-82j§(param1:§_-b1k§) : Vector.<uint>
      {
         var _loc2_:Array = null;
         var _loc3_:uint = 0;
         _loc2_ = this.§_-3e§(param1);
         _loc3_ = uint(Application.userProfile.getLevel());
         var _loc4_:uint = §_-Y15§.all[_loc2_[0]].§_-82j§(_loc3_)[0];
         var _loc5_:uint = §_-Y15§.all[_loc2_[_loc2_.length - 1]].§_-82j§(_loc3_)[1];
         return new <uint>[_loc4_,_loc5_];
      }
      
      private function §_-3e§(param1:§_-b1k§) : Array
      {
         var _loc2_:Array = null;
         switch(param1.value)
         {
            case §_-b1k§.§_-98§.value:
               _loc2_ = [0,1];
               break;
            case §_-b1k§.§_-U1h§.value:
               _loc2_ = [4,5];
               break;
            case §_-b1k§.§_-92C§.value:
            default:
               _loc2_ = [2,3];
         }
         return _loc2_;
      }
      
      private function §_-22g§() : Vector.<§_-Uv§>
      {
         var _loc4_:§_-t2Z§ = null;
         var _loc5_:uint = 0;
         var _loc6_:§_-Y15§ = null;
         var _loc7_:uint = 0;
         var _loc8_:uint = 0;
         var _loc9_:int = 0;
         var _loc10_:Vector.<uint> = null;
         var _loc11_:uint = 0;
         var _loc12_:int = 0;
         var _loc1_:Vector.<§_-Uv§> = new Vector.<§_-Uv§>();
         var _loc2_:uint = §_-5A§ + §_-5A§ * this.§_-fK§;
         var _loc3_:uint = uint.MAX_VALUE;
         for each(_loc4_ in §_-I1q§.§_-x9§())
         {
            if(this.§_-fK§ >= _loc4_.§_-D0§() && this.§_-fK§ <= _loc4_.§_-01i§() && _loc4_.§_-rQ§() <= _loc3_)
            {
               _loc3_ = _loc4_.§_-rQ§();
            }
         }
         _loc5_ = 0;
         while(_loc5_ < §_-j2x§)
         {
            _loc6_ = §_-Y15§.all[_loc5_];
            _loc7_ = MathUtil.§_-na§(_loc6_.§_-S1c§(),_loc2_);
            _loc8_ = _loc6_.§_-72R§(this.§_-fK§);
            _loc9_ = _loc7_ - _loc3_ * (_loc8_ - 1);
            _loc10_ = new Vector.<uint>();
            _loc11_ = 0;
            while(_loc11_ < _loc8_)
            {
               if(_loc11_ != _loc8_ - 1)
               {
                  _loc12_ = MathUtil.random(_loc3_,_loc9_);
               }
               else
               {
                  _loc12_ = _loc9_;
               }
               _loc9_ = _loc9_ - _loc12_ + _loc3_;
               _loc10_[_loc11_] = _loc12_;
               _loc11_++;
            }
            _loc1_.push(new §_-Uv§(_loc7_,_loc10_));
            _loc5_++;
         }
         return _loc1_;
      }
      
      private function §_-X1G§(param1:uint) : §_-t2Z§
      {
         var _loc3_:§_-t2Z§ = null;
         var _loc4_:uint = 0;
         var _loc5_:uint = 0;
         var _loc6_:§_-t2Z§ = null;
         var _loc7_:uint = 0;
         var _loc2_:Vector.<§_-t2Z§> = new Vector.<§_-t2Z§>();
         for each(_loc3_ in §_-I1q§.§_-x9§())
         {
            if(_loc3_.isAvailable(this.§_-fK§,param1))
            {
               _loc2_.push(_loc3_);
            }
         }
         _loc4_ = _loc2_.length / 2;
         while(_loc4_ > 0)
         {
            _loc5_ = _loc4_;
            while(_loc5_ < _loc2_.length)
            {
               _loc6_ = _loc2_[_loc5_];
               _loc7_ = _loc5_;
               while(_loc7_ >= _loc4_ && _loc2_[_loc7_ - _loc4_].§_-rQ§() > _loc6_.§_-rQ§())
               {
                  _loc2_[_loc7_] = _loc2_[_loc7_ - _loc4_];
                  _loc7_ -= _loc4_;
               }
               _loc2_[_loc7_] = _loc6_;
               _loc5_++;
            }
            _loc4_ = Math.round(_loc4_ / 2.2);
         }
         _loc2_ = _loc2_.reverse();
         return _loc2_[§_-X1j§.randomSeed.§_-Fh§(0,_loc2_.length > 5 ? Math.min(5,_loc2_.length - 1) : _loc2_.length - 1)];
      }
      
      private function §_-c6§(param1:§_-Uv§, param2:§_-Y15§, param3:Vector.<UserProfile>) : UserProfile
      {
         var _loc4_:uint = 0;
         var _loc6_:UserProfile = null;
         var _loc8_:uint = 0;
         var _loc9_:§_-t2Z§ = null;
         var _loc10_:§_-zF§ = null;
         var _loc5_:uint = param1.§_-I2F§().length;
         var _loc7_:Array = [];
         this.§_-D3J§();
         _loc8_ = 0;
         while(_loc8_ < _loc5_)
         {
            _loc4_ = param1.§_-I2F§()[_loc8_];
            _loc9_ = this.§_-X1G§(_loc4_);
            _loc4_ -= _loc9_.§_-rQ§();
            _loc6_ = new UserProfile();
            _loc6_.§_-t0§(this.§_-d2g§.§_-cU§(_loc9_.§_-PG§(),_loc7_,param3));
            this.§_-M2E§(_loc6_,param2);
            this.§_-Q2W§(_loc6_,_loc9_,_loc4_,param2);
            this.§_-C3h§(_loc6_,_loc9_);
            _loc10_ = §_-G1p§.§_-12J§(_loc9_.§_-PG§(),_loc6_.§_-P2e§());
            if(_loc10_)
            {
               _loc6_.§_-L§(_loc10_);
            }
            this.§_-Zh§(_loc6_,_loc9_,_loc4_);
            _loc7_.push(_loc6_);
            _loc8_++;
         }
         _loc4_ = uint.MIN_VALUE;
         _loc8_ = 0;
         while(_loc8_ < _loc5_)
         {
            if(param1.§_-I2F§()[_loc8_] > _loc4_)
            {
               _loc4_ = param1.§_-I2F§()[_loc8_];
               _loc6_ = _loc7_[_loc8_];
            }
            _loc8_++;
         }
         §_-F2L§.§_-CT§(_loc7_);
         _loc6_.§_-I1N§(_loc6_.getCharName());
         _loc6_.§_-fR§(_loc7_);
         return _loc6_;
      }
      
      private function §_-Q2W§(param1:UserProfile, param2:§_-t2Z§, param3:uint, param4:§_-Y15§) : void
      {
         var _loc5_:Vector.<§_-g1S§> = param2.§_-F1W§();
         var _loc6_:§_-g1S§ = _loc5_[MathUtil.random(0,_loc5_.length - 1)];
         var _loc7_:uint = uint(param1.getLevel());
         var _loc8_:§_-uH§ = new §_-uH§(_loc7_);
         switch(_loc6_)
         {
            case §_-g1S§.§_-jl§:
               _loc8_.§_-eQ§(_loc8_.§_-VY§);
               break;
            case §_-g1S§.DEFENSIVE:
               _loc8_.§_-eQ§(_loc8_.§_-VY§ * 0.2);
               break;
            case §_-g1S§.§_-31m§:
               _loc8_.§_-eQ§(_loc8_.§_-VY§ * 0.5);
               break;
            case §_-g1S§.§_-q1w§:
               _loc8_.§_-eQ§(MathUtil.random(_loc8_.§_-VY§ * 0.2,_loc8_.§_-VY§));
               break;
            default:
               _loc8_.§_-eQ§(_loc8_.§_-VY§ * 0.5);
         }
         var _loc9_:int = int(_loc8_.attack);
         var _loc10_:int = int(_loc8_.§_-41C§);
         _loc9_ += this.§_-63p§(_loc9_,param3,param4) + this.§_-b1K§(param4);
         _loc10_ += this.§_-63p§(_loc10_,param3,param4) + this.§_-b1K§(param4);
         if(_loc9_ < 0 || _loc9_ > int.MAX_VALUE / 10)
         {
            _loc9_ = 0;
         }
         if(_loc10_ < 0 || _loc10_ > int.MAX_VALUE / 10)
         {
            _loc10_ = 0;
         }
         param1.§_-eQ§(_loc9_);
         param1.§_-q1H§(_loc10_);
         var _loc11_:int = Math.min(30,_loc7_);
         var _loc12_:int = (§_-SO§ - §_-g2s§) / 30;
         var _loc13_:int = §_-g2s§ + _loc12_ * _loc11_;
         param1.§_-q28§(_loc13_ + MathUtil.random(-5,5));
      }
      
      private function §_-63p§(param1:int, param2:int, param3:§_-Y15§) : int
      {
         var _loc4_:Number = NaN;
         var _loc5_:int = 0;
         if(param3.§_-12q§() >= §_-Y15§.MEDIUM.§_-12q§() && param2 > §_-5A§)
         {
            _loc4_ = Math.min(§_-N2I§,param2 / §_-5A§ * 2);
            _loc5_ = MathUtil.§_-na§(_loc4_,Math.abs(param1));
            return MathUtil.random(_loc5_ * 0.9,_loc5_);
         }
         return 0;
      }
      
      private function §_-b1K§(param1:§_-Y15§) : int
      {
         var _loc2_:int = this.§_-82H§ - §_-r14§;
         if(_loc2_ > 0)
         {
            return 0;
         }
         return MathUtil.random(_loc2_ - 1,_loc2_ + 1) * 2 * param1.§_-b1K§();
      }
      
      private function §_-M2E§(param1:UserProfile, param2:§_-Y15§) : void
      {
         var _loc3_:uint = Math.max(1,Math.ceil(param2.§_-V2S§() * this.§_-82H§) + param2.§_-e1Z§());
         if(this.§_-82H§ > 6)
         {
            _loc3_ += MathUtil.random(-1,1);
         }
         param1.§_-N1n§(_loc3_);
      }
      
      private function §_-C3h§(param1:UserProfile, param2:§_-t2Z§) : void
      {
         var _loc3_:Vector.<§_-L1x§> = param2.§_-A37§();
         var _loc4_:§_-B2i§ = _loc3_[MathUtil.random(0,_loc3_.length - 1)].§_-wz§();
         param1.§_-s1o§(_loc4_);
      }
      
      private function §_-Zh§(param1:UserProfile, param2:§_-t2Z§, param3:uint) : void
      {
         var _loc4_:§_-de§ = null;
         var _loc5_:uint = 0;
         var _loc6_:uint = 0;
         param1.aiArsenal = new §_-A1l§(null,param1.getLevel());
         for each(_loc4_ in param2.§_-72b§())
         {
            param1.aiArsenal.§_-t1j§(_loc4_,param1.getLevel());
         }
         if(param3 >= §_-H2i§)
         {
            _loc5_ = Math.min(3,param3 / §_-H2i§);
            _loc6_ = 0;
            while(_loc6_ < _loc5_)
            {
               param1.aiArsenal.§_-t1j§(§_-d1U§.§_-f1s§(),param1.getLevel());
               _loc6_++;
            }
         }
      }
   }
}

