package ru.pragmatix.wormix.weapons
{
   import §_-31u§.§_-l1n§;
   import §_-62§.§_-G1p§;
   import §_-63U§.§_-412§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-E19§;
   import §_-CR§.§_-72p§;
   import §_-K35§.§_-99§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-b1F§.§_-G4§;
   import §_-g1P§.§_-z1n§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-u1m§.§_-e1l§;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.brokenLine.BrokenLine;
   import ru.pragmatix.wormix.fx.particle.ParticleEffect;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.GameObjectUtils;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.boss.emperor.EmperorTerracottaWarrior;
   import ru.pragmatix.wormix.weapons.ammo.boss.scientist.ScientistGenerator;
   import starling.events.Event;
   import starling.utils.Pool;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class LaserGun extends §_-e1l§
   {
      
      private static const §_-73q§:Array = [§_-z1n§,§_-l1n§,§_-412§,Mine,ScientistGenerator,EmperorTerracottaWarrior];
      
      private static const MAX_LENGTH:uint = 1000;
      
      private static const §_-s2Y§:§_-TA§ = new §_-TA§(32);
      
      private static const §_-Vc§:§_-TA§ = new §_-TA§(4);
      
      private static const §_-52U§:uint = 6;
      
      private static const §_-v2L§:uint = 5;
      
      private static const §_-J3r§:Number = §_-v2L§ << 1;
      
      private var §_-zx§:BrokenLine;
      
      private var §_-IH§:ParticleEffect;
      
      private var sound:§_-V1R§;
      
      private var burning:Boolean = false;
      
      private var bullets:§_-TA§ = new §_-TA§(§_-s2Y§.value);
      
      private var armorReduction:uint = 0;
      
      private var rayColor:uint;
      
      private var §_-v26§:uint;
      
      private var dummy:§_-F3o§;
      
      private var §_-125§:Vector.<§_-01J§>;
      
      private var §_-XE§:Vector.<§_-F3o§>;
      
      private var §_-xA§:Point;
      
      private var §_-U26§:Point;
      
      public function LaserGun()
      {
         var _loc1_:Number = MathUtil.PI_HALF - MathUtil.PI_ONE_SIX;
         super(MathUtil.PI_ONE_360,_loc1_);
         §_-z1W§(new §_-W2f§(§_-W2f§.§_-G3n§,MathUtil.PI_HALF,_loc1_));
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         if(param1["burning"])
         {
            this.burning = param1["burning"].value;
         }
         if(param1["bullets"])
         {
            this.bullets.value = param1["bullets"].value;
         }
         if(param1["armorReduction"])
         {
            this.armorReduction = param1["armorReduction"].value;
         }
         this.rayColor = param1["rayColor"];
         super.§_-z2Y§(param1);
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
         this.§_-xA§ = Pool.getPoint();
      }
      
      override public function detach() : void
      {
         super.detach();
         if(Boolean(this.dummy) && !this.dummy.disposed)
         {
            this.dummy.disposed = true;
         }
         this.dummy = null;
         if(this.sound)
         {
            this.sound.stop();
            this.sound = null;
         }
         if(this.§_-zx§ != null)
         {
            this.§_-zx§.removeFromParent(true);
            this.§_-zx§ = null;
            this.§_-IH§.removeFromParent(true);
            this.§_-IH§ = null;
         }
         if(this.§_-xA§)
         {
            Pool.putPoint(this.§_-xA§);
            this.§_-xA§ = null;
         }
         if(this.§_-U26§)
         {
            Pool.putPoint(this.§_-U26§);
            this.§_-U26§ = null;
         }
      }
      
      override protected function shoot() : void
      {
         super.shoot();
         this.sound = §_-h2B§.play(§_-d27§.§_-F1A§,true);
         this.dummy = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
      }
      
      override protected function §_-s2E§() : void
      {
         super.§_-s2E§();
         if(this.§_-U26§)
         {
            this.§_-g19§();
         }
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:Point = null;
         var _loc6_:§_-01J§ = null;
         var _loc7_:§_-F3o§ = null;
         var _loc8_:uint = 0;
         var _loc9_:BurningOil = null;
         if(owner != null)
         {
            super.onFrame(param1);
            _loc2_ = owner;
            getBarrelPoint(angle,this.§_-xA§);
            _loc3_ = paramSet.damage.value;
            this.§_-U26§ = HitTest.castRayVec(this.§_-xA§.x,this.§_-xA§.y,angle,MAX_LENGTH);
            if(this.§_-U26§)
            {
               if(frames.value % §_-52U§ == 0)
               {
                  §_-G4§.§_-Ee§(_loc2_,this.§_-U26§.x,this.§_-U26§.y,null,_loc3_,paramSet.damageRadius.value,paramSet.explosionRadius.value,false);
                  Effects.explodeParticles(this.§_-U26§.x,this.§_-U26§.y,4);
                  if(this.§_-v26§ != frames.value)
                  {
                     this.§_-g19§();
                  }
               }
            }
            else
            {
               this.§_-U26§ = Pool.getPoint();
               this.§_-U26§.setTo(this.§_-xA§.x + Math.cos(angle) * MAX_LENGTH,this.§_-xA§.y + Math.sin(angle) * MAX_LENGTH);
            }
            if(frames.value % §_-v2L§ == 0)
            {
               _loc4_ = _loc2_.direction;
               if(frames.value % 10 == 0 || !this.§_-125§ || this.§_-125§.length == 0)
               {
                  this.§_-125§ = §_-X1j§.§_-12n§.gameMaster.§_-D3n§();
               }
               else
               {
                  GameObjectUtils.spliceDisposedUnits(this.§_-125§);
               }
               _loc5_ = this.§_-U26§;
               for each(_loc6_ in this.§_-125§)
               {
                  if(Boolean(_loc2_ && _loc6_ != _loc2_ && !_loc6_.disposed && _loc5_) && Boolean(this.§_-xA§) && MathUtil.§_-G1o§(this.§_-xA§.x,this.§_-xA§.y,_loc5_.x,_loc5_.y,_loc6_.x,_loc6_.y,28))
                  {
                     §_-G4§.§_-Ee§(_loc2_,_loc6_.x,_loc6_.y,_loc6_ as §_-F3o§,_loc3_,0,0,false);
                     if(Boolean(_loc2_) && Boolean(this.burning) && frames.value % §_-J3r§ == 0)
                     {
                        _loc8_ = 10;
                        _loc9_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BURNING_OIL) as BurningOil;
                        _loc9_.initAmmo(AmmosFactory.BURNING_OIL_SKIN,_loc2_,Pool.getPoint(_loc6_.x,_loc6_.y + 15),null,-MathUtil.PI_HALF + MathUtil.PI_QUARTER * _loc2_.direction,4);
                        _loc9_.setDuration(_loc8_);
                     }
                     if(!_loc6_.isImmobile())
                     {
                        UnitPhysicModel(_loc6_.getPhysicModel()).rolling = _loc4_ * §_-Vc§.value;
                     }
                  }
               }
               if(frames.value % 10 == 0 || !this.§_-XE§ || this.§_-125§.length == 0)
               {
                  this.§_-XE§ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§(§_-73q§);
               }
               else
               {
                  GameObjectUtils.spliceDisposedObjects(this.§_-XE§);
               }
               for each(_loc7_ in this.§_-XE§)
               {
                  if(Boolean(_loc2_ && _loc5_) && Boolean(this.§_-xA§) && MathUtil.§_-G1o§(this.§_-xA§.x,this.§_-xA§.y,_loc5_.x,_loc5_.y,_loc7_.x,_loc7_.y,15))
                  {
                     §_-G4§.§_-Ee§(_loc2_,_loc7_.x - _loc2_.direction * 5,_loc7_.y + 5,_loc7_,_loc3_,0,0,false);
                  }
               }
               ++§_-a2H§.value;
            }
            if(frames.value <= 1)
            {
               this.§_-g19§();
            }
            var _temp_17:* = frames;
            ++frames.value;
            if(§_-a2H§.value == this.bullets.value)
            {
               this.detach();
            }
         }
      }
      
      private function §_-g19§() : void
      {
         if(owner)
         {
            if(!this.§_-zx§)
            {
               this.§_-zx§ = new BrokenLine(4,this.rayColor);
               §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(this.§_-zx§,SortingLayer.AMMO_LAYER);
            }
            if(!this.§_-IH§)
            {
               this.§_-IH§ = Effects.createParticleEffect(ParticleEffectType.LASER);
            }
            getBarrelPoint(angle,this.§_-xA§);
            this.§_-zx§.clear();
            this.§_-zx§.beginDraw();
            this.§_-zx§.drawLine(this.§_-xA§,this.§_-U26§);
            this.§_-zx§.endDraw();
            this.§_-IH§.emitterX = this.§_-U26§.x;
            this.§_-IH§.emitterY = this.§_-U26§.y;
            this.§_-v26§ = frames.value;
         }
      }
   }
}

