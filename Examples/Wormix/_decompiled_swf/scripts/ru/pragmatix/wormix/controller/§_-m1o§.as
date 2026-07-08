package ru.pragmatix.wormix.controller
{
   import §_-02q§.§_-4s§;
   import §_-21p§.§_-4w§;
   import §_-23P§.§_-V2T§;
   import §_-23P§.§_-u1z§;
   import §_-31W§.§_-r26§;
   import §_-52L§.§_-Pk§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-zF§;
   import §_-82l§.§_-A2Y§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.*;
   import §_-CR§.§_-S2o§;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-H16§.*;
   import §_-L2F§.§_-D2p§;
   import §_-OJ§.§_-L3B§;
   import §_-OJ§.§_-sY§;
   import §_-R2S§.*;
   import §_-RE§.*;
   import §_-SP§.§_-M4§;
   import §_-TF§.*;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.*;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.§_-L29§;
   import §_-b1F§.§_-G4§;
   import §_-b1F§.§_-v2Q§;
   import §_-eG§.LevelUpDialog;
   import §_-g2r§.§_-03A§;
   import §_-j22§.§_-5g§;
   import §_-jF§.*;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Oi§;
   import §_-k2s§.§_-b0§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-e1k§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-m0§.§_-X1F§;
   import §_-m2s§.§_-R11§;
   import §_-nZ§.§_-6z§;
   import §_-o14§.*;
   import §_-p14§.*;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-sQ§.§_-V1n§;
   import §_-u2J§.§_-T1K§;
   import §_-w2i§.§_-52k§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-H31§;
   import §_-zI§.§_-X1q§;
   import com.distriqt.extension.pushnotifications.PushNotifications;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import com.worlize.websocket.*;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.VerticalLayoutData;
   import feathers.themes.FontColor;
   import flash.events.Event;
   import flash.net.URLRequest;
   import flash.net.URLVariables;
   import flash.text.TextField;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getDefinitionByName;
   import flash.utils.getTimer;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.rules.§_-Q2u§;
   import ru.pragmatix.wormix.gui.§_-gT§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import ru.pragmatix.wormix.weapons.interfaces.§_-o2d§;
   import starling.core.Starling;
   import starling.events.EnterFrameEvent;
   import starling.events.Event;
   import starling.events.KeyboardEvent;
   import starling.events.TouchEvent;
   import starling.utils.Color;
   
   public class §_-m1o§
   {
      
      public var §_-q1U§:§_-zF§ = null;
      
      public function §_-m1o§()
      {
         super();
      }
      
      public static function §_-73Y§(param1:§_-hK§) : void
      {
         var _loc5_:§_-e1k§ = null;
         var _loc2_:UserProfile = Application.userProfile;
         if(param1.reaction > 0)
         {
            _loc2_.§_-P2P§(param1.reaction);
            §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-S2§],[param1.reaction]);
         }
         if(param1.exp + param1.bonusExp > 0)
         {
            if(§_-33r§.§_-d8§(_loc2_,param1.exp + param1.bonusExp))
            {
               param1.§_-n2J§ = _loc2_.getLevel();
               param1.§_-wj§ = §_-H31§.§_-s2m§;
               param1.§_-LB§ = §_-H31§.§_-e§(param1.§_-n2J§);
            }
         }
         if(param1.rating != 0)
         {
            _loc2_.§_-E3L§(param1.rating);
         }
         var _loc3_:int = param1.money + param1.§_-dT§ + param1.§_-wj§;
         var _loc4_:int = param1.realMoney + param1.§_-LB§;
         if(_loc3_ != 0)
         {
            _loc2_.§_-oA§(_loc3_,false);
         }
         if(_loc4_ != 0)
         {
            _loc2_.§_-H3i§(_loc4_,false);
         }
         _loc2_.§_-W2K§();
         §_-f2M§(param1.weapons,param1.§_-Ff§,true);
         for each(_loc5_ in param1.reagents)
         {
            §_-Cq§.§_-Y22§(_loc5_.getId(),_loc5_.getCount());
         }
      }
      
      public static function §_-f2M§(param1:Vector.<§_-b0§>, param2:Vector.<§_-e1U§> = null, param3:Boolean = false) : void
      {
         var _loc5_:§_-b0§ = null;
         var _loc6_:§_-O2H§ = null;
         var _loc7_:int = 0;
         var _loc8_:§_-Oi§ = null;
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc12_:int = 0;
         var _loc13_:§_-e1U§ = null;
         var _loc14_:int = 0;
         var _loc4_:§_-2m§ = Application.userProfile.§_-pd§();
         for each(_loc5_ in param1)
         {
            _loc6_ = §_-G1p§.§_-I4§(_loc5_.getId());
            _loc7_ = _loc5_.getCount();
            if(_loc6_.isComplex())
            {
               _loc8_ = _loc4_.§_-d0§(_loc5_.getId());
               _loc9_ = (Boolean(_loc8_)) && _loc8_.isInfinite() ? int(_loc8_.getLevel()) : 0;
               _loc10_ = Boolean(_loc8_) && _loc8_.§_-n1u§() ? _loc9_ : int(_loc9_ + Math.abs(_loc7_));
               _loc4_.§_-Um§(_loc6_,-1,_loc6_.getMaxShots(),_loc10_);
            }
            else
            {
               _loc11_ = _loc4_.§_-d3§(_loc5_.getId());
               _loc12_ = _loc7_ == -1 || _loc11_ == -1 ? -1 : int(_loc7_ + _loc11_);
               _loc4_.§_-Um§(_loc6_,_loc12_,_loc6_.getMaxShots());
            }
         }
         if(param2)
         {
            for each(_loc13_ in param2)
            {
               _loc14_ = _loc13_.getCount() > 0 ? int(Math.round((_loc13_.getCount() - §_-72u§.§_-5L§()) / §_-72u§.§_-03d§)) : 0;
               §_-2t§.§_-E9§(_loc13_.getId(),true,param3,_loc14_);
            }
         }
         §_-6A§.§_-d2Q§(Application.userProfile);
      }
      
      public static function §_-L2L§() : void
      {
         if(Application.userProfile.getLevel() == 5 && Application.userProfile.§_-A1U§().length == 1)
         {
            Application.userProfile.build.§_-Fc§(§_-V2T§.§_-H2p§(-1).§_-d22§(),false);
         }
      }
      
      public static function §_-B3n§() : void
      {
         var _loc2_:§_-hK§ = null;
         var _loc1_:§_-O2i§ = §_-X1j§.§_-12n§.§_-WN§();
         if(Boolean(_loc1_ && _loc1_.missionIds && _loc1_.missionIds.length == 1) && Boolean(_loc1_.missionIds[0] == 0) && _loc1_.battleType == §_-81M§.§_-e1C§)
         {
            _loc2_ = §_-Q2u§.§_-C1b§(§_-J1§.§_-23m§,null);
            _loc2_.§_-11J§ = _loc1_.§_-11J§;
            _loc2_.money = §_-H31§.§_-nG§(§_-J1§.§_-23m§,_loc1_.§_-11J§);
            _loc2_.exp = §_-H31§.§_-70§(§_-J1§.§_-23m§,_loc1_.§_-11J§);
            §_-Q2u§.§_-f1x§(_loc2_);
            Application.userProfile.§_-oA§(-_loc2_.money);
            Application.userProfile.§_-K1U§(-_loc2_.exp);
         }
      }
      
      public function dispose() : void
      {
         this.§_-q1U§ = null;
      }
      
      public function §_-E2h§() : void
      {
         var _loc1_:LevelUpDialog = null;
         if(§_-X1j§.§_-12n§.§_-p1t§().§_-n2J§ > 0)
         {
            §_-L2L§();
            _loc1_ = new LevelUpDialog(§_-X1j§.§_-12n§.§_-p1t§());
            §_-52k§.single(_loc1_,§_-M4§.WINDOW_CLOSE,this.§_-L1z§);
            _loc1_.show();
         }
         else
         {
            §_-X1j§.§_-12n§.§_-g2c§();
            §_-X1j§.§_-61v§.§_-C1o§();
         }
      }
      
      private function §_-L1z§(param1:starling.events.Event) : void
      {
         §_-X1j§.§_-12n§.§_-g2c§();
         §_-X1j§.§_-61v§.§_-C1o§();
      }
      
      public function §_-2F§() : void
      {
         if(this.§_-q1U§ != null && this.§_-dL§(this.§_-q1U§,Application.userProfile))
         {
            Application.userProfile.§_-L§(this.§_-q1U§);
            this.§_-q1U§ = null;
         }
      }
      
      public function §_-dL§(param1:§_-zF§, param2:UserProfile) : Boolean
      {
         var _loc5_:§_-u1z§ = null;
         var _loc3_:Array = param2.§_-A1U§();
         var _loc4_:* = int(_loc3_.length);
         while(_loc4_--)
         {
            _loc5_ = §_-41c§.§_-H2p§(_loc3_[_loc4_]);
            if(_loc3_[_loc4_].getHat() == param1 && (!_loc5_ || _loc5_.§_-PG§() != param1.getId()))
            {
               return false;
            }
         }
         return true;
      }
   }
}

