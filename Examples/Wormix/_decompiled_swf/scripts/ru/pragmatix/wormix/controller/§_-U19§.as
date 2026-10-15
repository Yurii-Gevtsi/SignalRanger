package ru.pragmatix.wormix.controller
{
   import §_-12W§.*;
   import §_-31W§.§_-g16§;
   import §_-31W§.§_-r26§;
   import §_-31u§.*;
   import §_-62§.§_-zF§;
   import §_-63U§.§_-y2O§;
   import §_-82l§.§_-A2Y§;
   import §_-Af§.§_-d1B§;
   import §_-B1y§.§_-bs§;
   import §_-Bv§.§_-G3P§;
   import §_-C2M§.*;
   import §_-CR§.§_-72p§;
   import §_-D3A§.*;
   import §_-DJ§.*;
   import §_-G2H§.*;
   import §_-J31§.§_-e2J§;
   import §_-K35§.§_-99§;
   import §_-L1H§.§_-C3e§;
   import §_-N8§.§_-o2w§;
   import §_-TF§.§_-q1j§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-OP§;
   import §_-Y1b§.§_-jH§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-Z1w§.§_-w1D§;
   import §_-aM§.*;
   import §_-fo§.*;
   import §_-j1w§.§_-L1x§;
   import §_-j1y§.§_-i1b§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.*;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-sQ§.§_-H3D§;
   import §_-z1g§.§_-y1S§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-g1a§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.hurlant.util.der.*;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.MovieClip;
   import flash.display.Sprite;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.geom.ColorTransform;
   import flash.net.SharedObject;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.serialization.*;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.rules.§_-P2S§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.Bat;
   import ru.pragmatix.wormix.weapons.Girder;
   import ru.pragmatix.wormix.weapons.Shocker;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.filters.ColorMatrixFilter;
   import starling.text.TextField;
   import starling.textures.Texture;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-U19§
   {
      
      public static const §_-a2q§:uint = 1;
      
      public static const §_-uB§:uint = 20;
      
      public static const MASSIVE_DAMAGE:uint = 300;
      
      private var §_-C2§:§_-S2Y§;
      
      private var §_-PA§:uint = 0;
      
      private var §_-h1G§:Vector.<uint> = new Vector.<uint>();
      
      private var §_-K1w§:Array;
      
      private var §_-w2B§:Vector.<int> = new Vector.<int>();
      
      private var §_-fv§:int;
      
      private var §_-g1b§:int;
      
      private var §_-112§:int;
      
      private var §_-I3a§:int;
      
      public function §_-U19§()
      {
         super();
         this.§_-C2§ = new §_-S2Y§();
      }
      
      public static function §_-dM§(param1:§_-S2Y§, param2:Array, param3:uint) : Boolean
      {
         var _loc6_:§_-o2w§ = null;
         var _loc4_:Array = §_-w1D§.§_-YZ§(param3);
         var _loc5_:Boolean = true;
         if(_loc4_.length > 0)
         {
            for each(_loc6_ in _loc4_)
            {
               if(!param1.§_-Lb§(_loc6_.id) && param2.indexOf(_loc6_.id) == -1)
               {
                  _loc5_ = false;
                  break;
               }
            }
         }
         else
         {
            _loc5_ = false;
         }
         return _loc5_;
      }
      
      public function dispose() : void
      {
         this.§_-C2§ = null;
         this.§_-h1G§ = null;
         this.§_-w2B§ = null;
      }
      
      public function increment(param1:uint, param2:int = 1) : void
      {
         if(this.§_-B2c§())
         {
            this.§_-C2§.§_-Ae§(param1,param2);
         }
      }
      
      public function §_-7m§(param1:§_-01J§) : void
      {
         var _loc2_:§_-01J§ = null;
         if(this.§_-B2c§() && !§_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-C6§())
         {
            _loc2_ = §_-X1j§.§_-12n§.gameMaster.activeUnit;
            if(param1.squad.isLocal())
            {
               this.increment(§_-g16§.UNITS_LOST);
            }
            if(!_loc2_)
            {
               return;
            }
            if(_loc2_.squad.isLocal())
            {
               if(param1.squad.getTeamId() != _loc2_.squad.getTeamId())
               {
                  this.increment(§_-g16§.§_-71X§);
                  if(param1.§_-91i§)
                  {
                     this.increment(§_-r26§.§_-L2w§);
                     if(_loc2_.weapon is Bat || _loc2_.§_-81o§ is Bat || _loc2_.§_-81o§ is Shocker || _loc2_.weapon is Shocker)
                     {
                        this.increment(§_-r26§.§_-R1k§);
                     }
                     if(§_-X1j§.§_-12n§.gameMaster.§_-82g§() == 1)
                     {
                        this.increment(§_-r26§.§_-K1B§);
                     }
                  }
                  else if(param1.lastDamager != null)
                  {
                     if(param1.lastDamager.damageType == DamageType.FIRE)
                     {
                        this.increment(§_-r26§.§_-cV§);
                     }
                     if(param1.lastDamager.damageType == DamageType.CHEMICAL)
                     {
                        this.increment(§_-r26§.§_-21t§);
                     }
                     if(param1.lastDamager.§_-A3d§ && §_-H3D§.distance(param1.lastDamager.§_-w1U§ - param1.x,param1.lastDamager.§_-i2D§ - param1.y) > §_-P2S§.§_-NM§ && §_-H3D§.sign(param1.x - param1.lastDamager.§_-w1U§) != param1.lastDamager.§_-53a§)
                     {
                        this.increment(§_-r26§.§_-2x§);
                     }
                  }
                  if(_loc2_.isImmobile())
                  {
                     this.increment(§_-r26§.§_-RR§);
                  }
                  switch(param1.record.§_-P2e§().getId())
                  {
                     case §_-L1x§.§_-So§.getId():
                        this.increment(§_-g16§.KILLED_DRAGONS);
                        break;
                     case §_-L1x§.§_-Q1Q§.getId():
                        this.increment(§_-g16§.KILLED_BOTS);
                        break;
                     case §_-L1x§.§_-AU§.getId():
                        this.increment(§_-g16§.KILLED_BOXERS);
                        break;
                     case §_-L1x§.§_-l2G§.getId():
                        this.increment(§_-g16§.KILLED_DEMONS);
                        break;
                     case §_-L1x§.§_-I1h§.getId():
                        this.increment(§_-g16§.KILLED_RABBITS);
                        break;
                     case §_-L1x§.§_-p21§.getId():
                        if(param1.§_-OQ§())
                        {
                           this.increment(§_-g16§.KILLED_REVIVED);
                           this.increment(§_-r26§.§_-91c§);
                        }
                        else
                        {
                           this.increment(§_-g16§.KILLED_ZOMBIES);
                        }
                        break;
                     case §_-L1x§.§_-r2y§.getId():
                        this.increment(§_-g16§.KILLED_CATS);
                  }
               }
            }
         }
      }
      
      public function §_-63P§(param1:§_-P2S§) : void
      {
         var _loc2_:Object = null;
         if(this.§_-B2c§())
         {
            if(§_-X1j§.§_-12n§.gameMaster.activeSquad.isLocal())
            {
               if(param1.§_-T1s§ && param1.§_-b2m§ > 0)
               {
                  this.increment(§_-r26§.KAMIKAZE,param1.§_-b2m§);
               }
               if(param1.§_-b2m§ > 1)
               {
                  this.increment(§_-r26§.§_-B2b§);
               }
               if(param1.§_-G2P§ > 0 && §_-X1j§.§_-12n§.gameMaster.activeUnit.record.getId() == Application.userProfile.getId())
               {
                  this.increment(§_-g16§.SOLO_DAMAGE,param1.§_-G2P§);
               }
               else
               {
                  this.increment(§_-g16§.TEAM_DAMAGE,param1.§_-G2P§);
               }
               if(param1.§_-G2P§ > MASSIVE_DAMAGE)
               {
                  if(param1.§_-G2P§ < 400)
                  {
                     §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"massive damage hack"));
                  }
                  this.increment(§_-r26§.MASSIVE_DAMAGE);
               }
               if(param1.§_-G12§ > 0)
               {
                  this.increment(§_-g16§.SNIPER_SHOTS,param1.§_-G12§);
               }
               if(param1.§_-K3H§ > 0)
               {
                  this.increment(§_-g16§.WIND_SHOTS,param1.§_-K3H§);
               }
            }
            else
            {
               for each(_loc2_ in param1.§_-b2k§)
               {
                  if(§_-01J§(_loc2_.unit).record.getId() == Application.userProfile.getId())
                  {
                     this.increment(§_-g16§.SOLO_DAMAGE_TAKEN,_loc2_.damage);
                  }
                  else if(§_-01J§(_loc2_.unit).squad.isLocal())
                  {
                     this.increment(§_-g16§.TEAM_DAMAGE_TAKEN,_loc2_.damage);
                  }
               }
               if(param1.§_-K3E§)
               {
                  this.increment(§_-r26§.§_-Uo§);
               }
            }
         }
      }
      
      public function §_-x1L§() : void
      {
         var _loc4_:§_-01J§ = null;
         var _loc5_:§_-V23§ = null;
         this.§_-fv§ = 0;
         this.§_-g1b§ = 0;
         this.§_-112§ = 0;
         this.§_-I3a§ = 0;
         var _loc1_:Array = §_-X1j§.§_-12n§.gameMaster.§_-12G§();
         var _loc2_:§_-V23§ = §_-X1j§.§_-12n§.gameMaster.§_-vJ§(Application.userProfile);
         var _loc3_:uint = _loc2_.getTeamId();
         if(_loc2_ != null)
         {
            for each(_loc4_ in _loc2_.getUnits())
            {
               if(!_loc4_.disposed)
               {
                  this.§_-fv§ += _loc4_.hp;
               }
            }
            for each(_loc5_ in _loc1_)
            {
               if(_loc5_.getTeamId() != _loc3_)
               {
                  for each(_loc4_ in _loc5_.getUnits())
                  {
                     if(!_loc4_.disposed)
                     {
                        this.§_-g1b§ += _loc4_.hp;
                     }
                  }
               }
            }
            this.§_-112§ = _loc2_.§_-p2D§;
            this.§_-I3a§ = _loc2_.§_-Z2G§();
         }
      }
      
      public function §_-y8§(param1:§_-O2i§, param2:§_-hK§) : void
      {
         var _loc3_:§_-S2Y§ = null;
         var _loc4_:UserProfile = null;
         var _loc5_:Boolean = false;
         var _loc6_:Boolean = false;
         var _loc7_:uint = 0;
         var _loc8_:uint = 0;
         var _loc9_:Object = null;
         var _loc10_:§_-d1B§ = null;
         var _loc11_:§_-e2J§ = null;
         var _loc12_:uint = 0;
         var _loc13_:uint = 0;
         if(this.§_-B2c§())
         {
            _loc3_ = §_-X1j§.§_-k2F§.§_-Sy§();
            _loc4_ = §_-33r§.§_-t2t§(param1.§_-b2W§());
            _loc5_ = param2.result == §_-J1§.WINNER;
            _loc6_ = Boolean(_loc4_) && param1.isAllyFriendOrClanmate;
            this.§_-K1w§ = [];
            if(param1.battleType == §_-81M§.§_-Y1C§)
            {
               this.increment(§_-g16§.PVP_PLAYED);
               if(_loc5_)
               {
                  this.increment(§_-g16§.PVP_WON);
                  this.increment(§_-r26§.§_-GB§);
                  if(_loc6_)
                  {
                     this.increment(§_-r26§.§_-l2m§);
                  }
               }
               else if(param2.result == §_-J1§.DRAW_GAME)
               {
                  this.increment(§_-g16§.PVP_DRAWS);
               }
            }
            else if(param1.battleType == §_-81M§.§_-e1C§)
            {
               this.increment(§_-g16§.MISSIONS_PLAYED);
               if(_loc5_)
               {
                  this.increment(§_-g16§.MISSIONS_WON);
               }
            }
            else if(param1.battleType == §_-81M§.§_-K23§)
            {
               if(_loc5_)
               {
                  if(param1.scenario is §_-d1B§)
                  {
                     _loc10_ = §_-d1B§(param1.scenario);
                     for each(_loc11_ in _loc10_.§_-x1j§())
                     {
                        if(_loc11_.§_-h2j§ && !_loc3_.§_-Lb§(_loc11_.id))
                        {
                           this.§_-K1w§.push(_loc11_.id);
                        }
                     }
                  }
                  else if(param1.scenario is §_-jH§)
                  {
                     this.increment(§_-r26§.§_-U10§);
                     if(_loc6_)
                     {
                        this.increment(§_-r26§.§_-l2m§);
                     }
                  }
               }
            }
            if(_loc5_)
            {
               if(this.§_-fv§ > 0 && this.§_-fv§ <= §_-a2q§)
               {
                  this.increment(§_-r26§.§_-R1y§);
               }
               if(this.§_-g1b§ > 0 && this.§_-fv§ <= §_-uB§)
               {
                  this.increment(§_-r26§.§_-n2M§);
               }
               if(this.§_-112§ == 0 && this.§_-I3a§ > 1)
               {
                  this.increment(§_-r26§.§_-r1W§);
               }
            }
            _loc7_ = 0;
            for each(_loc8_ in this.§_-h1G§)
            {
               _loc7_ += _loc8_;
            }
            if(_loc7_ != this.§_-PA§)
            {
               §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"destroyed pixels hack"));
            }
            else
            {
               _loc12_ = this.§_-PA§ / Physics.SQMETER;
               if(_loc12_ > 0)
               {
                  this.increment(§_-r26§.§_-M29§,_loc12_);
               }
            }
            this.increment(§_-r26§.§_-l2C§,Math.min(this.§_-w2B§.length,4));
            if(param2.§_-dV§() > 0)
            {
               this.increment(§_-r26§.§_-61y§,param2.§_-dV§());
            }
            for each(_loc9_ in §_-w1D§.§_-QB§())
            {
               _loc13_ = uint(§_-o2w§(_loc9_.totalAchieve).id);
               if(§_-dM§(_loc3_,this.§_-K1w§,_loc9_.id) && !_loc3_.§_-Lb§(_loc13_))
               {
                  this.§_-K1w§.push(_loc13_);
               }
            }
         }
      }
      
      public function §_-x2H§() : Array
      {
         return this.§_-K1w§;
      }
      
      public function §_-L1k§(param1:int) : void
      {
         var _loc2_:int = 0;
         for each(_loc2_ in this.§_-w2B§)
         {
            if(_loc2_ == param1)
            {
               return;
            }
         }
         this.§_-w2B§[this.§_-w2B§.length] = param1;
      }
      
      public function §_-K3h§(param1:uint) : void
      {
         this.§_-PA§ += param1;
         this.§_-h1G§.push(param1);
      }
      
      public function get records() : §_-S2Y§
      {
         return this.§_-C2§;
      }
      
      public function §_-F25§() : void
      {
         var _loc1_:int = 0;
         while(_loc1_ < this.§_-C2§.ids.length)
         {
            _loc1_++;
         }
      }
      
      public function §_-B2c§() : Boolean
      {
         return Boolean(§_-X1j§.§_-12n§) && Boolean(§_-X1j§.§_-12n§.gameMaster) && §_-X1j§.§_-12n§.gameMaster.§_-T1r§();
      }
   }
}

