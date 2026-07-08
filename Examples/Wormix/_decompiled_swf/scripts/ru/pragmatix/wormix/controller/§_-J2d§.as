package ru.pragmatix.wormix.controller
{
   import §_-02q§.§_-E2p§;
   import §_-11L§.Archdemon;
   import §_-11L§.Hacker;
   import §_-11L§.Paladin;
   import §_-11L§.Phantoms;
   import §_-11L§.Thieves;
   import §_-11L§.§_-P25§;
   import §_-2k§.*;
   import §_-31W§.§_-r26§;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-P14§;
   import §_-62§.§_-o6§;
   import §_-63U§.§_-y2O§;
   import §_-82a§.SwitchedRenderTextures;
   import §_-82a§.§_-yb§;
   import §_-91B§.§_-QK§;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-c1G§;
   import §_-C2M§.*;
   import §_-C3f§.§_-1Y§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F2M§.*;
   import §_-F2Q§.GestureEvent;
   import §_-H3§.*;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-L2F§.§_-D2p§;
   import §_-OJ§.*;
   import §_-RE§.*;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1O§.§_-42h§;
   import §_-Y1b§.*;
   import §_-Yk§.*;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-b1R§.§_-Bx§;
   import §_-b1R§.§_-C3P§;
   import §_-f1G§.§_-ol§;
   import §_-fo§.§_-21L§;
   import §_-fo§.§_-A29§;
   import §_-hv§.*;
   import §_-j1w§.§_-B2i§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-Oi§;
   import §_-k2s§.§_-b0§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-iI§;
   import §_-k2s§.§_-uU§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-m1g§.§_-K2W§;
   import §_-m2s§.*;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-s1Q§.Artifacts;
   import §_-s1s§.§_-62d§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-sQ§.§_-V1n§;
   import §_-z1g§.*;
   import com.distriqt.extension.pushnotifications.events.PushNotificationGroupEvent;
   import config.§_-V2w§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.IFeathersControl;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.LayoutBoundsResult;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import feathers.layout.ViewPortBounds;
   import flash.events.StatusEvent;
   import flash.events.TimerEvent;
   import flash.external.ExtensionContext;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.system.Capabilities;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import flash.utils.getDefinitionByName;
   import ru.pragmatix.flox.gui.controls.Qtip;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.*;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.AssembleStuff;
   import ru.pragmatix.wormix.messages.client.DowngradeWeapon;
   import ru.pragmatix.wormix.messages.client.GetRating;
   import ru.pragmatix.wormix.messages.client.UpgradeWeapon;
   import ru.pragmatix.wormix.messages.server.AssembleStuffResult;
   import ru.pragmatix.wormix.messages.server.DowngradeWeaponResult;
   import ru.pragmatix.wormix.messages.server.UpgradeWeaponResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-WU§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.phase.HealBolt;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-J2d§
   {
      
      public static const §_-73s§:§_-TA§ = new §_-TA§(5);
      
      public static const §_-xa§:Number = 0.8;
      
      public function §_-J2d§()
      {
         super();
      }
      
      private static function §_-a1x§(param1:§_-63i§, param2:§_-E19§, param3:uint, param4:Function) : void
      {
         var _loc5_:AssembleStuffResult = null;
         var _loc7_:UserProfile = null;
         var _loc8_:CraftedEquipFamily = null;
         var _loc9_:int = 0;
         var _loc10_:Boolean = false;
         var _loc11_:Vector.<§_-F1D§> = null;
         var _loc12_:§_-F1D§ = null;
         var _loc13_:§_-QK§ = null;
         var _loc14_:int = 0;
         _loc5_ = AssembleStuffResult(param1.message);
         var _loc6_:int = _loc5_.equipId;
         if(_loc5_.result == AssembleStuffResult.SUCCESS)
         {
            _loc7_ = Application.userProfile;
            _loc8_ = §_-G1p§.§_-713§()[_loc5_.familyId];
            _loc9_ = §_-2t§.§_-r1j§(_loc5_.familyId);
            _loc10_ = _loc9_ == -1;
            if(_loc9_ != -1)
            {
               _loc7_.§_-XT§().removeItem(_loc9_);
            }
            _loc11_ = _loc9_ == -1 || !_loc8_.§_-b2Z§() ? _loc8_.§_-h1o§() : _loc8_.§_-b2Z§();
            for each(_loc12_ in _loc11_)
            {
               if(§_-V2w§.§_-q1F§(_loc12_.id) || §_-V2w§.§_-Q2a§(_loc12_.id))
               {
                  if(_loc9_ == -1)
                  {
                     _loc7_.§_-XT§().removeItem(_loc12_.id);
                  }
               }
               else
               {
                  §_-Cq§.§_-n1v§(_loc12_.id,_loc12_.quantity);
               }
            }
            _loc13_ = §_-2t§.§_-HJ§(_loc6_);
            if(_loc13_.getType() == §_-z2l§.LEGENDARY)
            {
               §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-92j§],[1]);
            }
            if(param2.id == §_-E19§.§_-51D§.id)
            {
               _loc14_ = _loc10_ ? int(_loc8_.getPrice().realPrice) : int(_loc8_.§_-e18§().realPrice);
               _loc14_ = _loc14_ + param3;
               _loc7_.§_-H3i§(-_loc14_);
               §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-b22§],[_loc14_]);
            }
            else
            {
               _loc14_ = _loc10_ ? int(_loc8_.getPrice().§_-R1T§) : int(_loc8_.§_-e18§().§_-R1T§);
               _loc7_.§_-oA§(-_loc14_);
               §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-T20§],[_loc14_]);
            }
            §_-2t§.§_-E9§(_loc6_);
            §_-h2B§.play(§_-d27§.§_-M15§);
         }
         if(param4 != null)
         {
            SystemUtil.executeWhenApplicationIsActive(param4,_loc5_.result,_loc6_);
         }
         §_-J2l§.§_-R25§.hide();
      }
      
      public function §_-u11§(param1:§_-6S§, param2:uint, param3:Function) : void
      {
         var _loc4_:UpgradeWeapon = new UpgradeWeapon(param1.§_-HS§.getId());
         §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc4_,UpgradeWeaponResult,this.§_-41s§,this.§_-F2r§,param1,param2,param3);
      }
      
      private function §_-41s§(param1:§_-63i§, param2:§_-6S§, param3:uint, param4:Function) : void
      {
         var _loc7_:§_-IE§ = null;
         var _loc8_:int = 0;
         var _loc9_:§_-Oi§ = null;
         var _loc10_:§_-o6§ = null;
         var _loc5_:UpgradeWeaponResult = UpgradeWeaponResult(param1.message);
         var _loc6_:Boolean = _loc5_.result == UpgradeWeaponResult.SUCCESS && param2.§_-HS§.getId() == _loc5_.recipeId;
         if(_loc6_)
         {
            _loc7_ = §_-Y2u§.§_-v2G§(_loc5_.recipeId);
            Application.userProfile.§_-pd§().removeItem(_loc7_.prevUpgradeId,-1);
            §_-Cq§.§_-G38§(_loc7_.upgradeId,Application.userProfile);
            _loc8_ = _loc7_.getId();
            _loc9_ = Application.userProfile.§_-h1v§().§_-d0§(_loc8_);
            if(_loc9_)
            {
               _loc9_.setCount(1);
            }
            if(param3 != 0)
            {
               Application.userProfile.§_-H3i§(-param3);
               §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-b22§],[param3]);
            }
            for each(_loc10_ in param2.§_-HS§.reagents)
            {
               Application.userProfile.§_-E3H§().removeItem(_loc10_.id,_loc10_.amount);
            }
            §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-41k§],[1]);
            §_-h2B§.play(§_-d27§.§_-M15§);
            §_-N1V§.postController.§_-3G§(param2.weapon);
         }
         param4(_loc6_);
      }
      
      private function §_-F2r§(param1:Object, param2:§_-6S§, param3:uint, param4:Function) : void
      {
         param4(false);
      }
      
      public function §_-n27§(param1:§_-6S§, param2:Function) : void
      {
         var _loc3_:DowngradeWeapon = new DowngradeWeapon(param1.§_-HS§.getId());
         §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc3_,DowngradeWeaponResult,this.§_-o2n§,this.§_-8a§,param1,param2);
      }
      
      private function §_-o2n§(param1:§_-63i§, param2:§_-6S§, param3:Function) : void
      {
         var _loc6_:§_-IE§ = null;
         var _loc7_:int = 0;
         var _loc8_:§_-Oi§ = null;
         var _loc9_:§_-o6§ = null;
         var _loc10_:uint = 0;
         var _loc4_:DowngradeWeaponResult = DowngradeWeaponResult(param1.message);
         var _loc5_:Boolean = _loc4_.result == DowngradeWeaponResult.SUCCESS && param2.§_-HS§.getId() == _loc4_.recipeId;
         if(_loc5_)
         {
            _loc6_ = §_-Y2u§.§_-v2G§(_loc4_.recipeId);
            Application.userProfile.§_-pd§().removeItem(_loc6_.upgradeId,-1);
            §_-Cq§.§_-G38§(_loc6_.prevUpgradeId,Application.userProfile);
            _loc7_ = _loc6_.getId();
            _loc8_ = Application.userProfile.§_-h1v§().§_-d0§(_loc7_);
            if(_loc8_)
            {
               _loc8_.setCount(-1);
            }
            Application.userProfile.§_-H3i§(-§_-73s§.value);
            §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-b22§],[§_-73s§.value]);
            for each(_loc9_ in _loc6_.reagents)
            {
               _loc10_ = uint(_loc9_.§_-I1k§);
               if(_loc10_ != 0)
               {
                  §_-Cq§.§_-Y22§(_loc9_.id,_loc10_);
               }
            }
            §_-h2B§.play(§_-d27§.§_-t1u§);
         }
         param3(_loc5_);
      }
      
      private function §_-8a§(param1:*, param2:§_-6S§, param3:Function) : void
      {
         param3(false);
      }
      
      public function §_-L2D§(param1:UserProfile, param2:§_-IE§) : int
      {
         var _loc5_:§_-o6§ = null;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         for each(_loc5_ in param2.reagents)
         {
            _loc4_ = param1.§_-E3H§().§_-d3§(_loc5_.id) - _loc5_.amount;
            if(_loc4_ < 0)
            {
               _loc3_ += Math.abs(_loc4_) * §_-Y2u§.§_-k14§(_loc5_.id).§_-91e§();
            }
         }
         return int(Math.ceil(_loc3_ / 100));
      }
      
      public function §_-hJ§(param1:int, param2:§_-E19§, param3:uint, param4:Function) : void
      {
         var _loc5_:AssembleStuff = new AssembleStuff(param1,param2.id);
         §_-J2l§.§_-R25§.show(§_-d21§.§_-vH§,§_-J2g§.§_-i1N§,_loc5_,AssembleStuffResult,§_-a1x§,this.§_-63o§,param2,param3,param4);
      }
      
      private function §_-63o§(param1:*, param2:§_-E19§, param3:uint, param4:Function) : void
      {
         param4(1,null);
         §_-J2l§.§_-R25§.hide();
      }
   }
}

