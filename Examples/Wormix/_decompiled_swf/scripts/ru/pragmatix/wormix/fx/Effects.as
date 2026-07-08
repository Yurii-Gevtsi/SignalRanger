package ru.pragmatix.wormix.fx
{
   import §_-73I§.§_-K2s§;
   import §_-G16§.§_-K2m§;
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-YF§.§_-q2v§;
   import §_-hO§.§_-03k§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import com.funkypandagame.stardustplayer.project.ProjectValueObject;
   import com.greensock.TimelineMax;
   import com.greensock.TweenLite;
   import com.greensock.TweenMax;
   import com.greensock.easing.Back;
   import com.greensock.easing.Elastic;
   import com.greensock.easing.Linear;
   import dragonBones.animation.WorldClock;
   import feathers.core.ITextRenderer;
   import flash.geom.ColorTransform;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-D8§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.particle.ParticleEffect;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.fx.trail.ITrail;
   import ru.pragmatix.wormix.fx.trail.ParticleTrail;
   import ru.pragmatix.wormix.fx.trail.ShadowTrail;
   import ru.pragmatix.wormix.fx.trail.StardustParticleTrail;
   import ru.pragmatix.wormix.fx.trail.VertexTrail;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import stardust.StardustCustomSimLoaderLoader;
   import stardust.StardustSimPlayersManager;
   import stardust.player.AbstractSimPlayer;
   import stardust.player.MovableSimPlayer;
   import stardust.player.StationarySimPlayer;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.extensions.ColorArgb;
   import starling.extensions.ParticleAura;
   import starling.textures.Texture;
   
   public class Effects
   {
      
      private static var stationaryPlayerPool:§_-K2m§;
      
      private static var movablePlayerPool:§_-K2m§;
      
      private static var frontParticleCanvas:Sprite;
      
      private static var backParticleCanvas:Sprite;
      
      private static var simLoaderDict:Dictionary;
      
      private static var simPlayersManager:StardustSimPlayersManager;
      
      public static const DISABLED:Boolean = false;
      
      public static const DAMAGE_COLOR:String = "0xFD2A19";
      
      public static const FUEL_COLOR:String = "0x22CEFF";
      
      public static const FUEL_SHADOW:String = "0x082439";
      
      public static const HEAL_COLOR:String = "0x82D808";
      
      public static const XP_COLOR:String = "0xFFF043";
      
      public static const XP_SHADOW:String = "0x5D2300";
      
      public static const ATTACK_BONUS_COLOR:String = "0xFF7200";
      
      public static const ATTACK_BONUS_SHADOW:String = "0x691400";
      
      public static const CRITICAL_TIME_COLOR:String = "0xFF7200";
      
      public static const CRITICAL_TIME_SHADOW:String = "0x691400";
      
      public static const MISC_COLOR:String = "0xFFFFFF";
      
      public static const MISC_SHADOW:String = "0x272727";
      
      public static const COLOR_GREY9:String = "0x999999";
      
      public static const TEXT_COLOR_DAMAGE_RED:String = "0xFD2A19";
      
      public static const TEXT_COLOR_GREEN:String = "0x99CC99";
      
      public static const TEXT_COLOR_RED:String = "0xCD5C5C";
      
      public static const TEXT_COLOR_ORANGE:String = "0xFF9900";
      
      public static const TEXT_COLOR_GOLD:String = "0xD4A017";
      
      public static const TEXT_COLOR_BLUE:String = "0x4682B4";
      
      public static const TEXT_COLOR_SHADOW_BLACK:String = "0x460005";
      
      public static const AURA_WIND:String = "WindAura";
      
      public static const AURA_SOUL:String = "SoulAura";
      
      public static const AURA_RAGE:String = "RageAura";
      
      public static const SLOW:String = "SlowEffect";
      
      public static const KAMIKAZE:String = "KamikazeBuff";
      
      public static const DARK_GREEN_TRANSFORM:ColorTransform = new ColorTransform(1,1,1,1,-60,0,-50,0);
      
      public static const DARK_RED_TRANSFORM:ColorTransform = new ColorTransform(1,1,1,1,0,-60,-80,0);
      
      public static const RED_TRANSFORM:ColorTransform = new ColorTransform(1,1,1,1,120,30,40,0);
      
      public static const GREEN_TRANSFORM:ColorTransform = new ColorTransform(1,1,1,1,20,90,40,0);
      
      public static const GRAY_TRANSFORM:ColorTransform = new ColorTransform(1,1,1,1,-50,-50,-50,0);
      
      public static const NORMAL_TRANSFORM:ColorTransform = new ColorTransform(1,1,1,1,0,0,0,0);
      
      private static const ITEM_ICON_CACHE_SIZE:uint = 82;
      
      private static const TEXT_FIELD_OFFSET:Number = 4;
      
      public static const CROSS_ARROW_NAME:String = "arrow";
      
      public static const BOOMERANG_DEBUFF:String = "BoomerangDebuffEffect";
      
      private static var effectObjectsPool:EffectsPool = EffectsPool.getInstance();
      
      private static var improvedFxEnabled:Boolean = true;
      
      public function Effects()
      {
         super();
      }
      
      public static function initBeforeBattle() : void
      {
         if(improvedFxEnabled)
         {
            if(!simPlayersManager)
            {
               simPlayersManager = new StardustSimPlayersManager();
            }
            if(!stationaryPlayerPool)
            {
               stationaryPlayerPool = new §_-K2m§();
               stationaryPlayerPool.initialize(100,5,function():StationarySimPlayer
               {
                  return new StationarySimPlayer();
               });
            }
            if(!movablePlayerPool)
            {
               movablePlayerPool = new §_-K2m§();
               movablePlayerPool.initialize(100,5,function():MovableSimPlayer
               {
                  return new MovableSimPlayer();
               });
            }
            if(!simLoaderDict)
            {
               simLoaderDict = new Dictionary();
               simLoaderDict[EffectType.EXPLOSION_DRILL.name] = createSimLoader("particle_explosion_small");
               simLoaderDict[EffectType.EXPLOSION_MEDIUM.name] = createSimLoader("particle_explosion_normal");
               simLoaderDict[EffectType.EXPLOSION_LARGE.name] = createSimLoader("particle_explosion_large");
               simLoaderDict[EffectType.EXPLOSION_BULLET.name] = createSimLoader("particle_explosion_bullet");
               simLoaderDict[EffectType.EXPLOSION_POISON_LARGE.name] = createSimLoader("particle_explosion_poison_large");
               simLoaderDict[EffectType.EXPLOSION_ELECTRIC.name] = createSimLoader("particle_explosion_electric_large");
               simLoaderDict[EffectType.EXPLOSION_ELECTRIC_SMALL.name] = createSimLoader("particle_explosion_electric_small");
               simLoaderDict[EffectType.EXPLOSION_SUPPLY.name] = createSimLoader("particle_explosion_supply");
               simLoaderDict[EffectType.EXPLOSION_SMALL.name] = createSimLoader("particle_explosion_shard");
               simLoaderDict[EffectType.EXPLOSION_SMOKE.name] = createSimLoader("particle_explosion_smoke");
               simLoaderDict[EffectType.EXPLOSION_DEMONIC.name] = createSimLoader("particle_explosion_demonic");
               simLoaderDict[EffectType.FIRE.name] = createSimLoader("particle_fire");
               simLoaderDict[EffectType.EXPLOSION_FIRE_END.name] = createSimLoader("particle_explosion_fire_end");
               simLoaderDict[EffectType.DRAGON_FIRE_STEP.name] = createSimLoader("particle_fire");
               simLoaderDict[EffectType.UNIT_TRAIL.name] = createSimLoader("smoke_small_white_par");
            }
         }
         §_-X1j§.§_-12n§.addEventListener(Event.COMPLETE,onSceneCreated);
      }
      
      private static function onSceneCreated(param1:Event) : void
      {
         var _loc2_:§_-D8§ = param1.target as §_-D8§;
         _loc2_.removeEventListener(Event.COMPLETE,onSceneCreated);
         frontParticleCanvas = new Sprite();
         backParticleCanvas = new Sprite();
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(frontParticleCanvas,SortingLayer.EFFECT_LAYER);
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(backParticleCanvas,SortingLayer.GRAVE_LAYER);
      }
      
      public static function clearAfterBattle() : void
      {
         if(improvedFxEnabled)
         {
         }
         frontParticleCanvas.removeFromParent(true);
         backParticleCanvas.removeFromParent(true);
      }
      
      private static function simPlayerDestroy(param1:AbstractSimPlayer) : void
      {
         param1.dispose();
      }
      
      private static function createSimLoader(param1:String) : StardustCustomSimLoaderLoader
      {
         var _loc2_:StardustCustomSimLoaderLoader = new StardustCustomSimLoaderLoader(param1,Application.assetManager);
         var _loc3_:ByteArray = Application.assetManager.getByteArray(param1);
         _loc2_.loadSim(_loc3_);
         return _loc2_;
      }
      
      private static function particleExplosion(param1:StardustCustomSimLoaderLoader, param2:int, param3:int, param4:uint = 0) : void
      {
         var _loc5_:ProjectValueObject = param1.createProjectInstance();
         var _loc6_:StationarySimPlayer = stationaryPlayerPool.getObject();
         _loc6_.playerPoolLink = stationaryPlayerPool;
         _loc6_.maxLifetime = 1.5;
         _loc6_.activate();
         _loc6_.setProject(_loc5_);
         _loc6_.setRenderTarget(frontParticleCanvas);
         _loc6_.setPosition(param2,param3);
         simPlayersManager.addSimPlayer(_loc6_);
         _loc6_.addEventListener(Event.COMPLETE,onStardustProjectComplete);
         Starling.juggler.delayCall(_loc6_.deactivate,1.5);
      }
      
      public static function createSoul(param1:String, param2:int, param3:int) : void
      {
         if(DISABLED)
         {
            return;
         }
         var _loc4_:§_-di§ = §_-K1t§.buildArmature(param1);
         WorldClock.clock.add(_loc4_);
         _loc4_.display.x = param2;
         _loc4_.display.y = param3;
         _loc4_.playAnimation("play",0,-1,1);
         _loc4_.addEventListener(§_-s1d§.COMPLETE,removeSoul);
         _loc4_.addEventListener(Event.REMOVED_FROM_STAGE,removeSoul);
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(_loc4_.display,SortingLayer.EFFECT_LAYER);
      }
      
      public static function removeSoul(param1:Event) : void
      {
         var _loc2_:§_-di§ = param1.target as §_-di§;
         _loc2_.removeEventListener(§_-s1d§.COMPLETE,removeSoul);
         _loc2_.removeEventListener(Event.REMOVED_FROM_STAGE,removeSoul);
         removeArmatureOnRemovedFromStage(param1);
      }
      
      public static function createDragonbonesAnimationEffect(param1:EffectType, param2:int, param3:int, param4:uint = 0, param5:§_-t1R§ = null) : void
      {
         var _loc7_:IPooledEffect = null;
         if(DISABLED)
         {
            return;
         }
         var _loc6_:StardustCustomSimLoaderLoader = simLoaderDict[param1.name];
         if(_loc6_)
         {
            particleExplosion(_loc6_,param2,param3,param4);
         }
         else
         {
            _loc7_ = effectObjectsPool.createObject(param1);
            _loc7_.init(param2,param3,[param4]);
         }
         if(param5 != null)
         {
            §_-h2B§.play(param5);
         }
      }
      
      public static function explodeParticles(param1:int, param2:int, param3:int) : void
      {
         if(DISABLED)
         {
            return;
         }
      }
      
      public static function explodeParticleTrails(param1:int, param2:int, param3:int, param4:uint) : void
      {
         if(DISABLED)
         {
            return;
         }
      }
      
      public static function attachTrail(param1:§_-F3o§, param2:int, param3:Number, param4:uint, param5:uint, param6:Number = 0.997) : VertexTrail
      {
         var _loc7_:IPooledEffect = effectObjectsPool.createObject(EffectType.TRAJECTORY_TRAIL);
         _loc7_.attach(param1,[param2,param3,param4,param6,param5]);
         return _loc7_ as VertexTrail;
      }
      
      public static function attachParticleTrailWithColor(param1:ParticleEffectType, param2:§_-F3o§, param3:uint, param4:uint, param5:Number) : ParticleTrail
      {
         var _loc6_:ParticleTrail = Effects.attachParticleTrail(param1,param2);
         _loc6_.startColor.fromRgb(param3);
         _loc6_.endColor.fromRgb(param4);
         _loc6_.startColor.alpha = _loc6_.endColor.alpha = param5;
         return _loc6_;
      }
      
      public static function attachParticleTrail(param1:ParticleEffectType, param2:§_-F3o§) : ParticleTrail
      {
         var _loc3_:ParticleTrail = new ParticleTrail(param1,param2);
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(_loc3_,SortingLayer.UNIT_LAYER,0);
         return _loc3_;
      }
      
      public static function attachStardustParticle(param1:§_-F3o§, param2:EffectType) : ITrail
      {
         var _loc3_:StardustCustomSimLoaderLoader = simLoaderDict[param2.name];
         var _loc4_:ProjectValueObject = _loc3_.createProjectInstance();
         var _loc5_:MovableSimPlayer = movablePlayerPool.getObject();
         _loc5_.playerPoolLink = movablePlayerPool;
         _loc5_.setProject(_loc4_);
         if(param1.§_-V2p§.index < SortingLayer.UNIT_LAYER.index)
         {
            _loc5_.setRenderTarget(backParticleCanvas);
         }
         else
         {
            _loc5_.setRenderTarget(frontParticleCanvas);
         }
         _loc5_.maxLifetime = 0;
         _loc5_.activate();
         simPlayersManager.addSimPlayer(_loc5_);
         _loc5_.addEventListener(Event.COMPLETE,onStardustProjectComplete);
         var _loc6_:ITrail = new StardustParticleTrail(_loc5_);
         _loc6_.attachTo(param1);
         return _loc6_;
      }
      
      private static function onStardustProjectComplete(param1:Event) : void
      {
         var _loc2_:AbstractSimPlayer = param1.target as AbstractSimPlayer;
         _loc2_.removeEventListener(Event.COMPLETE,onStardustProjectComplete);
         if(simPlayersManager)
         {
            simPlayersManager.removeSimPlayer(_loc2_);
         }
         if(_loc2_.playerPoolLink)
         {
            _loc2_.playerPoolLink.putObject(_loc2_);
         }
         var _loc3_:ProjectValueObject = _loc2_.getProject();
         _loc3_.destroy();
      }
      
      public static function attachShadowTrail(param1:§_-F3o§, param2:String, param3:uint = 1) : void
      {
         if(DISABLED)
         {
            return;
         }
         new ShadowTrail(param1,param2,param3);
      }
      
      public static function createParticleEffect(param1:ParticleEffectType, param2:Number = -1, param3:int = 0, param4:int = 0) : ParticleEffect
      {
         var _loc5_:ParticleEffect = new ParticleEffect(param1,param2);
         _loc5_.x = param3;
         _loc5_.y = param4;
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(_loc5_,SortingLayer.EFFECT_LAYER);
         return _loc5_;
      }
      
      public static function createParticleCloudEffect(param1:ColorArgb, param2:int = 0, param3:int = 0) : ParticleCloudEffect
      {
         var _loc4_:ParticleCloudEffect = new ParticleCloudEffect(param1);
         _loc4_.x = param2;
         _loc4_.y = param3;
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(_loc4_,SortingLayer.EFFECT_LAYER);
         return _loc4_;
      }
      
      public static function lightningOnObjects(param1:§_-F3o§, param2:§_-F3o§, param3:uint, param4:uint, param5:uint, param6:uint) : void
      {
         if(DISABLED)
         {
            return;
         }
         LightingEffect.createFromPool().fromObjectToObject(param1,param2,param3,param4,param5,param6);
      }
      
      public static function lightningOnObject(param1:§_-F3o§, param2:int, param3:int, param4:uint, param5:uint, param6:uint, param7:uint) : void
      {
         if(DISABLED)
         {
            return;
         }
         LightingEffect.createFromPool().fromObjectToPoint(param1,param2,param3,param4,param5,param6,param7);
      }
      
      public static function lightning(param1:int, param2:int, param3:int, param4:int, param5:uint, param6:uint, param7:uint, param8:uint, param9:uint) : void
      {
         if(DISABLED || §_-X1j§.§_-12n§.scene == null)
         {
            return;
         }
         LightingEffect.createFromPool().fromPointToPoint(param1,param2,param3,param4,param5,param6,param7,param8,param9);
      }
      
      public static function putSimpleUncontrolledDisappearingCrossMark(param1:int, param2:int, param3:uint = 0, param4:Boolean = false) : void
      {
         var _loc5_:§_-di§ = createSimpleCrossMark();
         putCrossMark(_loc5_,param1,param2,param3,param4);
         _loc5_.addEventListener(Event.REMOVED_FROM_STAGE,removeArmatureOnRemovedFromStage);
      }
      
      public static function putSimpleDisappearingCrossMark(param1:int, param2:int, param3:uint = 0, param4:Boolean = false) : §_-di§
      {
         var _loc5_:§_-di§ = createSimpleCrossMark();
         putCrossMark(_loc5_,param1,param2,param3,param4);
         return _loc5_;
      }
      
      public static function putSimpleCrossMark(param1:int, param2:int) : §_-di§
      {
         return putCrossMark(createSimpleCrossMark(),param1,param2);
      }
      
      public static function putArrowWithCrossMark(param1:int, param2:int, param3:Boolean) : §_-di§
      {
         return putCrossMark(createMarkWithArrow(false,param3),param1,param2);
      }
      
      public static function createMarkWithArrow(param1:Boolean, param2:Boolean = false) : §_-di§
      {
         var _loc3_:§_-di§ = §_-K1t§.buildArmature("GuidingBazookaCursor",§_-K1t§.§_-Y1c§,§_-K1t§.§_-t1V§);
         _loc3_.display.scale = §_-K1t§.§_-31c§;
         if(param1)
         {
            _loc3_.getBone(CROSS_ARROW_NAME).visible = false;
         }
         else if(!param2)
         {
            _loc3_.§_-I3v§(CROSS_ARROW_NAME,"right");
         }
         return _loc3_;
      }
      
      private static function putCrossMark(param1:§_-di§, param2:int, param3:int, param4:uint = 0, param5:Boolean = false) : §_-di§
      {
         var display:DisplayObject = null;
         var mark:§_-di§ = param1;
         var x:int = param2;
         var y:int = param3;
         var frames:uint = param4;
         var disposeAfter:Boolean = param5;
         display = mark.display;
         display.x = x;
         display.y = y;
         WorldClock.clock.add(mark);
         mark.playAnimation(§_-K1t§.§_-Y1c§,0,-1,0);
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(display,SortingLayer.POINTERS_LAYER);
         if(frames != 0)
         {
            §_-Ia§.§_-33o§(frames,function():void
            {
               mark.removeEventListener(Event.REMOVED_FROM_STAGE,removeArmatureOnRemovedFromStage);
               §_-X1j§.§_-12n§.scene.removeElement(display);
               WorldClock.clock.remove(mark);
               if(disposeAfter)
               {
                  display.dispose();
                  mark.dispose();
               }
            });
         }
         return mark;
      }
      
      private static function createSimpleCrossMark() : §_-di§
      {
         return createMarkWithArrow(true);
      }
      
      public static function removeArmatureOnRemovedFromStage(param1:Event) : void
      {
         var _loc2_:§_-di§ = param1.target as §_-di§;
         _loc2_.removeEventListener(Event.REMOVED_FROM_STAGE,removeArmatureOnRemovedFromStage);
         removeArmature(_loc2_);
      }
      
      public static function removeArmature(param1:§_-di§) : void
      {
         if(!param1)
         {
            return;
         }
         WorldClock.clock.remove(param1);
         if(param1.display)
         {
            param1.display.removeFromParent();
         }
         param1.dispose();
      }
      
      public static function createAnimationEffect(param1:String, param2:int = -1) : §_-di§
      {
         if(DISABLED)
         {
            return null;
         }
         var _loc3_:§_-di§ = §_-K1t§.buildArmature(§_-K1t§.§_-Im§ + param1,§_-K1t§.§_-Y1c§,§_-K1t§.§_-t1V§);
         if(!_loc3_)
         {
            _loc3_ = §_-K1t§.buildArmature(§_-K1t§.§_-Im§ + param1,§_-K1t§.§_-Y1c§);
         }
         if(!_loc3_)
         {
            _loc3_ = §_-K1t§.buildArmature(param1,§_-K1t§.§_-Y1c§);
         }
         if(_loc3_)
         {
            _loc3_.display.scaleX = _loc3_.display.scaleY = §_-K1t§.§_-31c§;
         }
         var _loc4_:DisplayObject = _loc3_.display;
         _loc4_.width = param2 == -1 ? _loc4_.width : param2;
         _loc4_.height = param2 == -1 ? _loc4_.height : param2;
         return _loc3_;
      }
      
      public static function playClip(param1:Number, param2:Number, param3:§_-di§, param4:Number = 1, param5:Boolean = true, param6:Function = null, param7:SortingLayer = null) : §_-di§
      {
         var x:Number = param1;
         var y:Number = param2;
         var armature:§_-di§ = param3;
         var scale:Number = param4;
         var autoRemoveClip:Boolean = param5;
         var callback:Function = param6;
         var layer:SortingLayer = param7;
         if(DISABLED)
         {
            return null;
         }
         armature.display.scale = scale;
         armature.animation.play();
         armature.display.x = x;
         armature.display.y = y;
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(armature.display,layer ? layer : SortingLayer.EFFECT_LAYER);
         WorldClock.clock.add(armature);
         armature.addEventListener(§_-s1d§.COMPLETE,function(param1:Event):void
         {
            if(autoRemoveClip)
            {
               removeArmature(armature);
            }
            if(callback != null)
            {
               callback.call();
            }
         });
         return armature;
      }
      
      public static function createAndPlayAnimationEffect(param1:String, param2:int = -1, param3:String = "") : §_-di§
      {
         if(DISABLED)
         {
            return null;
         }
         var _loc4_:§_-di§ = createAnimationEffect(param1,param2);
         WorldClock.clock.add(_loc4_);
         if(param3 == "")
         {
            _loc4_.animation.play();
         }
         else
         {
            _loc4_.playAnimation(param3);
         }
         return _loc4_;
      }
      
      public static function attachAndPlayAnimationEffect(param1:§_-01J§, param2:String, param3:int = -1, param4:String = "") : §_-di§
      {
         if(DISABLED)
         {
            return null;
         }
         var _loc5_:§_-di§ = createAndPlayAnimationEffect(param2,param3);
         param1.graphics.§_-O1i§(§_-K2s§.§_-r2F§,_loc5_.display,0,0,true,false);
         return _loc5_;
      }
      
      public static function detachAnimationEffect(param1:§_-01J§, param2:§_-di§) : void
      {
         removeArmature(param2);
      }
      
      public static function heal(param1:int, param2:§_-F3o§) : void
      {
         if(DISABLED)
         {
            return;
         }
         floatingText("+" + param1.toString(),Effects.HEAL_COLOR,param2.getPhysicModel().x,param2.getPhysicModel().y - 35);
         new ParticleAura("heal","red_cross",param2);
      }
      
      public static function aura(param1:§_-01J§, param2:String, param3:int, param4:Function = null, param5:Boolean = false) : void
      {
         var armature:§_-di§ = null;
         var listener:Function = null;
         var dummy:§_-F3o§ = null;
         var ft:FrameTimer = null;
         var unit:§_-01J§ = param1;
         var className:String = param2;
         var duration:int = param3;
         var callback:Function = param4;
         var unstable:Boolean = param5;
         if(DISABLED)
         {
            return;
         }
         armature = attachAndPlayAnimationEffect(unit,className);
         armature.playAnimation(§_-K1t§.§_-Y1c§,0);
         armature.advanceTime(0);
         if(unstable)
         {
            dummy = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
         }
         listener = function(param1:Event):void
         {
            if(ft)
            {
               ft.removeEventListener(§_-03k§.§_-cE§,listener);
            }
            else
            {
               armature.removeEventListener(§_-s1d§.COMPLETE,listener);
            }
            if(armature)
            {
               detachAnimationEffect(unit,armature);
               armature = null;
            }
            unit = null;
            if(dummy)
            {
               dummy.disposed = true;
            }
            if(callback != null)
            {
               callback();
            }
         };
         if(duration == -1)
         {
            armature.addEventListener(§_-s1d§.COMPLETE,listener);
         }
         else
         {
            ft = new FrameTimer(duration,1);
            ft.addEventListener(§_-03k§.§_-cE§,listener);
            ft.start();
         }
         §_-Zd§.once(unit as EventDispatcher,§_-L3Q§.DISPOSED,function(param1:Event):void
         {
            if(ft)
            {
               ft.stop();
            }
            detachAnimationEffect(unit,armature);
            armature = null;
            if(Boolean(dummy) && !dummy.disposed)
            {
               dummy.disposed = true;
            }
         });
      }
      
      public static function createStormCloud() : §_-di§
      {
         var _loc1_:§_-di§ = §_-K1t§.buildArmature(§_-K1t§.§_-Im§ + "StormCloud","appearance",§_-K1t§.§_-t1V§);
         _loc1_.display.scaleX = _loc1_.display.scaleY = §_-K1t§.§_-31c§;
         return _loc1_;
      }
      
      public static function makeStarlingTextWithFont(param1:String, param2:String, param3:String, param4:uint = 20) : Sprite
      {
         var _loc5_:ITextRenderer = §_-R2g§.createTextRenderer(param1,param4,uint(param3),TextFormatAlign.CENTER);
         _loc5_.text = param2;
         _loc5_.minHeight = param4 + TEXT_FIELD_OFFSET;
         var _loc6_:Sprite = new Sprite();
         _loc6_.addChild(_loc5_ as DisplayObject);
         return _loc6_;
      }
      
      public static function makeStarlingText(param1:String, param2:String, param3:uint = 20) : Sprite
      {
         return makeStarlingTextWithFont(BitmapFonts.GROBOLD_WITH_SHADOWS,param1,param2,param3);
      }
      
      public static function tweenedText(param1:String, param2:String, param3:int, param4:int, param5:uint = 16) : void
      {
         if(DISABLED)
         {
            return;
         }
         var _loc6_:IPooledEffect = effectObjectsPool.createObject(EffectType.TEXT);
         _loc6_.init(param3,param4,[param1,param5,uint(param2),0.75]);
         TweenLite.killTweensOf(_loc6_);
         TweenLite.to(_loc6_,1.5,{
            "y":param4 - 100 - Math.random() * 50,
            "ease":Back.easeOut
         });
         TweenLite.to(_loc6_,1.5,{
            "x":param3 + (Math.random() > 0.5 ? 1 : -1) * 100 + Math.random() * 100 - 50,
            "ease":Linear.easeNone
         });
      }
      
      public static function floatingText(param1:String, param2:String, param3:int, param4:int, param5:uint = 16, param6:String = "GROBOLD_WITH_SHADOWS", param7:int = 50) : void
      {
         if(DISABLED)
         {
            return;
         }
         var _loc8_:IPooledEffect = effectObjectsPool.createObject(EffectType.TEXT);
         _loc8_.init(param3,param4,[param1,param5,uint(param2),0.75,param6,param7]);
         TweenLite.killTweensOf(_loc8_);
         TweenLite.to(_loc8_,1.5,{
            "y":(_loc8_ as TextEffect).y - 100,
            "ease":Linear.easeNone
         });
      }
      
      public static function floatingClip(param1:String, param2:int, param3:int, param4:uint = 0) : void
      {
         var texture:Texture;
         var display:DisplayObject = null;
         var scaleX:Number = NaN;
         var scaleY:Number = NaN;
         var scale:Number = NaN;
         var ft:FrameTimer = null;
         var clipName:String = param1;
         var x:int = param2;
         var y:int = param3;
         var maxSize:uint = param4;
         if(DISABLED)
         {
            return;
         }
         texture = Application.assetManager.getTexture(clipName);
         if(texture)
         {
            display = new Image(texture);
            scaleX = maxSize / display.width;
            scaleY = maxSize / display.height;
            scale = Math.min(scaleX,scaleY);
            display.width *= scale;
            display.height *= scale;
            display.x = x;
            display.y = y;
            §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(display,SortingLayer.EFFECT_LAYER);
            ft = new FrameTimer(1,30);
            ft.addEventListener(§_-03k§.§_-836§,function(param1:§_-03k§):void
            {
               display.y -= 3;
            });
            ft.addEventListener(§_-03k§.§_-cE§,function(param1:§_-03k§):void
            {
               §_-X1j§.§_-12n§.scene.removeElement(display);
            });
            ft.start();
         }
      }
      
      public static function showMonsterEffect(param1:int, param2:int) : void
      {
         var monsterName:String;
         var monsterTexture:Texture;
         var timeline:TimelineMax;
         var monsterImage:Image = null;
         var x:int = param1;
         var y:int = param2;
         if(!§_-X1j§.§_-12n§.scene.gameInterface)
         {
            return;
         }
         monsterName = §_-q2v§.MONSTER + §_-X1j§.§_-82T§.§_-I9§().§_-b2§();
         monsterTexture = Application.assetManager.getTexture(monsterName);
         if(monsterTexture)
         {
            monsterImage = new Image(monsterTexture);
            monsterImage.alignPivot();
         }
         monsterImage.x = Application.§_-i1N§.stageWidth * 0.5 / §_-X1j§.§_-12n§.scene.gameInterface.getContainer().scaleX;
         monsterImage.y = Application.§_-i1N§.stageHeight * 0.5 / §_-X1j§.§_-12n§.scene.gameInterface.getContainer().scaleX;
         monsterImage.scaleX = monsterImage.scaleY = 0.02;
         §_-X1j§.§_-12n§.scene.gameInterface.§_-72v§(monsterImage);
         timeline = new TimelineMax({"onComplete":function onCompleteTimelineMax():void
         {
            if(monsterImage)
            {
               monsterImage.removeFromParent(true);
               monsterImage = null;
               timeline = null;
            }
         }});
         timeline.append(new TweenLite(monsterImage,0.5,{
            "scaleX":0.8,
            "scaleY":0.8,
            "ease":Elastic.easeOut
         }));
         timeline.append(new TweenMax(monsterImage,0.5,{
            "scaleX":0.1,
            "scaleY":0.1,
            "ease":Elastic.easeIn
         }));
      }
   }
}

