package ru.pragmatix.wormix
{
   import §_-82a§.§_-c2B§;
   import §_-82a§.§_-j1O§;
   import §_-A1K§.§_-TA§;
   import §_-F39§.§_-43a§;
   import §_-H2g§.§_-di§;
   import §_-dS§.§_-D3a§;
   import §_-fo§.§_-r5§;
   import §_-hO§.§_-02h§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-52k§;
   import §_-w2i§.§_-Zd§;
   import dragonBones.animation.WorldClock;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.MovieClip;
   import flash.display.StageQuality;
   import flash.display3D.Context3DTextureFormat;
   import flash.events.Event;
   import flash.filters.GlowFilter;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.setTimeout;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.LightingEffect;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-93l§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.scene.SceneBuilder;
   import ru.pragmatix.wormix.scene.SceneGround;
   import ru.pragmatix.wormix.scene.SceneTiledLayer;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.scene.Weather;
   import ru.pragmatix.wormix.scene.camera.SceneCamera;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionsHandler;
   import ru.pragmatix.wormix.scene.tile.GeneratedTilesData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.scene.water.Water;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-vB§;
   import starling.animation.IAnimatable;
   import starling.core.Starling;
   import starling.display.BlendMode;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Scene extends Sprite implements IAnimatable, §_-w1N§
   {
      
      private static var backTilesAtlas:§_-c2B§;
      
      private static const TILE_SIZE:uint = 9;
      
      public static const TILE_WIDTH:uint = 1 << TILE_SIZE;
      
      public static const TILE_HEIGHT:uint = 1 << TILE_SIZE;
      
      public static const TILE_WIDTH_REVERT:Number = 1 / TILE_WIDTH;
      
      public static const TILE_HEIGHT_REVERT:Number = 1 / TILE_HEIGHT;
      
      private static const BACKGROUND_SCALE:Number = 2.5;
      
      public var sceneCamera:SceneCamera;
      
      public var gameInterface:§_-Q2v§;
      
      public var backParallaxLayer:Sprite;
      
      public var gameParallaxLayer:Sprite;
      
      private var backgroundLayer:SceneTiledLayer;
      
      private var groundBackLayer:SceneTiledLayer;
      
      private var groundLayer:SceneGround;
      
      private var objectsLayer:Sprite;
      
      private var cloudsLayer:SceneTiledLayer;
      
      private var tiledLayers:Array;
      
      private var water:Water;
      
      private var waterLevel:§_-TA§;
      
      private var _sceneW:int = 0;
      
      private var _sceneH:int = 0;
      
      public var stable:Boolean;
      
      private var _gameObjectProcessor:§_-93l§;
      
      private var weather:Weather;
      
      private var activeUnitPointer:§_-43a§;
      
      private var defaultQuality:String;
      
      private var curMapRecord:§_-D3a§;
      
      private var noMapGround:Boolean;
      
      private var indestructibleGround:Boolean;
      
      private var permanentZones:Vector.<Rectangle>;
      
      private var created:Boolean;
      
      private var nextTurnEvent:§_-02h§;
      
      public var needUpdateVisibleObjects:Boolean = false;
      
      private var textureFormat:String;
      
      private var _processingWaterIncrease:Boolean = false;
      
      private var callbackOnCreate:Function;
      
      private var createFromResourceCallback:Function;
      
      private var mapSource:MovieClip;
      
      private var sceneRestoreCallback:Function = null;
      
      private var boundsRect:Rectangle;
      
      private var _interactionsHandler:SceneInteractionsHandler;
      
      public function Scene()
      {
         super();
         this.created = false;
         this.boundsRect = Pool.getRectangle();
         this._interactionsHandler = new SceneInteractionsHandler(this);
         if(Starling.current.stage3D == null || Starling.current.context == null || Starling.current.context.driverInfo == "Disposed")
         {
            Starling.current.stage3D.addEventListener(flash.events.Event.CONTEXT3D_CREATE,this.onContextCreated,false,0,true);
         }
         else
         {
            this.onContextCreated();
         }
      }
      
      public function isCreated() : Boolean
      {
         return this.created;
      }
      
      private function onContextCreated(param1:flash.events.Event = null) : void
      {
         §_-h2B§.§_-s2U§();
         if(§_-vB§.§_-z1v§())
         {
            return;
         }
         Starling.current.stage3D.removeEventListener(flash.events.Event.CONTEXT3D_CREATE,this.onContextCreated);
         if(this.created)
         {
            §_-h2B§.§_-l2z§();
            this.create(this.curMapRecord,this.noMapGround,this.indestructibleGround);
         }
      }
      
      public function deactivate() : void
      {
         if(this.groundLayer != null)
         {
            this.backgroundLayer.visible = false;
            this.groundLayer.visible = false;
            this.objectsLayer.visible = false;
            this._interactionsHandler.deactivate();
         }
      }
      
      public function restore(param1:Function = null) : void
      {
         this.sceneRestoreCallback = param1;
         if(this.created && this.groundLayer != null)
         {
            Starling.juggler.delayCall(this.updateGroundImages,0.25);
         }
         else
         {
            this.runRestoreCallback();
         }
      }
      
      private function turnOnGraphics() : void
      {
         if(this.created)
         {
            this.backgroundLayer.visible = true;
            this.groundLayer.visible = true;
            this.objectsLayer.visible = true;
         }
         this.runRestoreCallback();
      }
      
      private function runRestoreCallback() : void
      {
         if(this.sceneRestoreCallback != null)
         {
            this.sceneRestoreCallback();
            this.sceneRestoreCallback = null;
         }
      }
      
      public function create(param1:§_-D3a§, param2:Boolean = false, param3:Boolean = false, param4:Function = null) : void
      {
         this.created = true;
         this.curMapRecord = param1;
         this.indestructibleGround = param3;
         this.callbackOnCreate = param4;
         if(§_-vB§.§_-z1v§())
         {
            return;
         }
         this.textureFormat = Context3DTextureFormat.BGRA_PACKED;
         this.activeUnitPointer = new §_-43a§();
         this.weather = new Weather();
         this.defaultQuality = param1.quality;
         if(param1.className)
         {
            this.createFromResource(param1,param2,this.onCreatedFromResources);
         }
         else
         {
            this.onCreatedFromResources(null);
         }
      }
      
      private function onCreatedFromResources(param1:MovieClip) : void
      {
         var _loc2_:§_-D3a§ = this.curMapRecord;
         this.permanentZones = new Vector.<Rectangle>();
         if(_loc2_.water)
         {
            this.water = new Water();
            if(_loc2_.water.height != null && _loc2_.water.x != null && _loc2_.water.y != null)
            {
               this.water.x = _loc2_.water.x;
               this.water.y = _loc2_.water.y + _loc2_.indentHeight;
               this.water.height = _loc2_.water.height + 50;
               this.water.width = this._sceneW;
            }
            if(Boolean(_loc2_.water.color) && Boolean(_loc2_.water.secondaryColor))
            {
               this.water.setColor(_loc2_.water.color,_loc2_.water.secondaryColor);
            }
            this.addElementToCertainSortingLayer(this.water,SortingLayer.WATER_LAYER);
            this.waterLevel = new §_-TA§(this.water.y);
         }
         else
         {
            this.waterLevel = new §_-TA§(this._sceneH);
         }
         this.groundLayer.addEventListener(starling.events.Event.ADDED_TO_STAGE,this.onAddedToStage);
         this.backParallaxLayer = new Sprite();
         addChild(this.backParallaxLayer);
         this.backParallaxLayer.addChild(this.backgroundLayer);
         this.gameParallaxLayer = new Sprite();
         addChild(this.gameParallaxLayer);
         if(this.groundBackLayer)
         {
            this.gameParallaxLayer.addChild(this.groundBackLayer);
         }
         this.gameParallaxLayer.addChild(this.groundLayer);
         this.gameParallaxLayer.addChild(this.objectsLayer);
         if(this.cloudsLayer)
         {
            this.addElementToCertainSortingLayer(this.cloudsLayer,SortingLayer.CLOUDS_LAYER);
         }
         this.gameInterface = new §_-Q2v§();
         this.gameInterface.addEventListener(§_-Q2v§.CLOUDS_VISIBILITY_CHANGED,this.onCloudsVisibilityChanged);
         this.onCloudsVisibilityChanged();
         this.sceneCamera = new SceneCamera(this,this.gameInterface);
         this.sceneCamera.addParallaxLayer(this.gameParallaxLayer,1,this._sceneW,this._sceneH);
         this.sceneCamera.addParallaxLayer(this.backParallaxLayer,0.05,this._sceneW,this._sceneH);
         this.tiledLayers = [];
         this.tiledLayers.push(this.groundLayer);
         if(this.groundBackLayer)
         {
            this.tiledLayers.push(this.groundBackLayer);
         }
         if(this.cloudsLayer)
         {
            this.tiledLayers.push(this.cloudsLayer);
         }
         Starling.juggler.add(this);
         dispatchEvent(new starling.events.Event(starling.events.Event.COMPLETE));
         if(this.callbackOnCreate != null)
         {
            this.callbackOnCreate(this);
            this.callbackOnCreate = null;
         }
      }
      
      private function onCloudsVisibilityChanged(param1:starling.events.Event = null) : void
      {
         if(this.cloudsLayer)
         {
            this.getSortingLayer(SortingLayer.CLOUDS_LAYER).visible = this.gameInterface.§_-N2u§.cloudsVisibilitySwitch.§_-X2D§;
         }
      }
      
      public function init(param1:Boolean, param2:Boolean, param3:Boolean, param4:Array, param5:Boolean) : void
      {
         var online:Boolean = param1;
         var showChat:Boolean = param2;
         var tabsEnabled:Boolean = param3;
         var squads:Array = param4;
         var backpackEnabled:Boolean = param5;
         if(!this.gameInterface)
         {
            return;
         }
         this.gameInterface.init(squads,showChat,online,tabsEnabled);
         this.sceneCamera.init();
         if(!backpackEnabled)
         {
            §_-X1j§.§_-12n§.scene.gameInterface.§_-gu§();
         }
         §_-Zd§.once(this,§_-L3Q§.SCENE_STABLE,function():void
         {
            dispatchEvent(new §_-02h§(0));
         });
         this.nextTurnEvent = new §_-02h§(0);
         LightingEffect.initPool();
         this.sceneCamera.activateControls();
      }
      
      public function advanceTime(param1:Number) : void
      {
         §_-MN§.§_-73M§();
      }
      
      private function createFromResource(param1:§_-D3a§, param2:Boolean, param3:Function) : void
      {
         this.noMapGround = param2;
         this.createFromResourceCallback = param3;
         this.mapSource = §_-r5§.§_-tF§(param1.className);
         Starling.current.nativeStage.quality = StageQuality.BEST;
         var _loc4_:Point = SceneBuilder.getMapDimensions(this.mapSource,param1);
         this._sceneW = _loc4_.x;
         this._sceneH = _loc4_.y;
         Pool.putPoint(_loc4_);
         var _loc5_:flash.display.DisplayObject = param2 ? new MovieClip() : this.mapSource.getChildByName("ground");
         var _loc6_:GeneratedTilesData = SceneBuilder.generateGroundTileDataArray(_loc5_,this._sceneW,this._sceneH,this.curMapRecord,TILE_WIDTH,TILE_HEIGHT);
         this.groundLayer = new SceneGround(this._sceneW,this._sceneH,!this.indestructibleGround);
         §_-52k§.single(this.groundLayer,starling.events.Event.COMPLETE,this.onGroundInitComplete);
         this.groundLayer.init(_loc6_);
      }
      
      private function onGroundInitComplete(param1:starling.events.Event) : void
      {
         backTilesAtlas = §_-j1O§.§_-n1P§(TILE_WIDTH / 4,TILE_HEIGHT / 4);
         backTilesAtlas.init();
         var _loc2_:BitmapData = SceneBuilder.createBackgroundBitmapData(this.mapSource,this.curMapRecord,BACKGROUND_SCALE);
         this._sceneW = _loc2_.width * BACKGROUND_SCALE;
         this._sceneH = _loc2_.height * BACKGROUND_SCALE;
         this.backgroundLayer = new SceneTiledLayer(this._sceneW,this._sceneH,TILE_WIDTH / 4,TILE_HEIGHT / 4,BACKGROUND_SCALE);
         var _loc3_:Array = [];
         SceneBuilder.generateTileDataArray(_loc2_,TileType.BACKGROUND,TILE_WIDTH / 4,TILE_HEIGHT / 4,_loc3_);
         backTilesAtlas.§_-637§(_loc3_);
         _loc2_.dispose();
         var _loc4_:Point = Pool.getPoint();
         var _loc5_:BitmapData = SceneBuilder.createBackLayerBitmapDataWithOffset(this.mapSource,this.curMapRecord,_loc4_);
         if(_loc5_)
         {
            this.groundBackLayer = new SceneTiledLayer(this._sceneW,this._sceneH,TILE_WIDTH / 4,TILE_HEIGHT / 4);
            this.groundBackLayer.x = _loc4_.x;
            this.groundBackLayer.y = _loc4_.y;
            SceneBuilder.generateTileDataArray(_loc5_,TileType.GROUND_BACK,TILE_WIDTH / 4,TILE_HEIGHT / 4,_loc3_);
            backTilesAtlas.§_-637§(_loc3_);
            _loc5_.dispose();
         }
         var _loc6_:BitmapData = SceneBuilder.createCloudsLayerBitmapDataWithOffset(this.mapSource,this.curMapRecord,_loc4_);
         if(_loc6_)
         {
            this.cloudsLayer = new SceneTiledLayer(this._sceneW,this._sceneH,TILE_WIDTH / 4,TILE_HEIGHT / 4);
            this.cloudsLayer.x = _loc4_.x;
            this.cloudsLayer.y = _loc4_.y;
            SceneBuilder.generateTileDataArray(_loc6_,TileType.CLOUDS,TILE_WIDTH / 4,TILE_HEIGHT / 4,_loc3_);
            backTilesAtlas.§_-637§(_loc3_);
            _loc6_.dispose();
         }
         backTilesAtlas.§_-p1F§(this.onAtlasesCreated);
         Pool.putPoint(_loc4_);
      }
      
      private function onAtlasesCreated() : void
      {
         §_-52k§.single(backTilesAtlas,starling.events.Event.COMPLETE,this.onBackTilesInitComplete);
         backTilesAtlas.§_-T1X§(this.backgroundLayer,TileType.BACKGROUND);
      }
      
      private function onBackTilesInitComplete(param1:starling.events.Event) : void
      {
         this.backgroundLayer.blendMode = BlendMode.NONE;
         if(this.groundBackLayer)
         {
            §_-52k§.single(backTilesAtlas,starling.events.Event.COMPLETE,this.onGroundBackTilesInitComplete);
            backTilesAtlas.§_-T1X§(this.groundBackLayer,TileType.GROUND_BACK);
         }
         else
         {
            this.onGroundBackTilesInitComplete(null);
         }
      }
      
      private function onGroundBackTilesInitComplete(param1:starling.events.Event) : void
      {
         §_-52k§.single(backTilesAtlas,starling.events.Event.COMPLETE,this.onCloudsTilesInitComplete);
         backTilesAtlas.§_-T1X§(this.cloudsLayer,TileType.CLOUDS);
      }
      
      private function onCloudsTilesInitComplete(param1:starling.events.Event) : void
      {
         backTilesAtlas.§_-42P§();
         this.initSceneLayers();
      }
      
      private function initSceneLayers() : void
      {
         var _loc1_:Sprite = null;
         var _loc2_:SortingLayer = null;
         this.objectsLayer = new Sprite();
         for each(_loc2_ in SortingLayer.getAllLayers())
         {
            _loc1_ = new Sprite();
            _loc1_.name = _loc2_.name;
            _loc1_.touchable = _loc2_.touchable;
            this.objectsLayer.addChild(_loc1_);
         }
         Starling.current.nativeStage.quality = this.defaultQuality;
         if(this.createFromResourceCallback != null)
         {
            this.createFromResourceCallback(this.mapSource);
            this.createFromResourceCallback = null;
            this.mapSource = null;
         }
      }
      
      public function shutDown() : void
      {
         if(this.gameInterface)
         {
            this.gameInterface.§_-c1v§(false);
         }
         if(this.sceneCamera)
         {
            this.sceneCamera.deactivateControls();
         }
      }
      
      private function getSortingLayer(param1:SortingLayer) : DisplayObjectContainer
      {
         var _loc2_:int = 0;
         if(this.objectsLayer.numChildren > 0)
         {
            _loc2_ = this.objectsLayer.numChildren > param1.index ? param1.index : 0;
            return this.objectsLayer.getChildAt(_loc2_) as DisplayObjectContainer;
         }
         return null;
      }
      
      public function getWaterHeight() : uint
      {
         return this._sceneH - this.waterLevel.value;
      }
      
      public function getWaterLevel() : uint
      {
         return this.waterLevel.value;
      }
      
      public function showArrowOnUnit(param1:§_-01J§) : void
      {
         this.activeUnitPointer.showArrowOnUnit(param1);
      }
      
      public function deactivateUnitUI(param1:§_-01J§) : void
      {
         this.gameInterface.deactivateUnit(param1);
      }
      
      public function activateUnitUI(param1:§_-01J§) : void
      {
         this.activeUnitPointer.showArrowOnUnit(param1);
         this.gameInterface.activateUnit(param1);
         var _loc2_:DisplayObjectContainer = this.getSortingLayer(SortingLayer.UNIT_LAYER);
         var _loc3_:int = _loc2_.getChildIndex(param1.clip);
         var _loc4_:int = _loc2_.numChildren;
         _loc2_.swapChildrenAt(_loc3_,_loc4_ - 1);
      }
      
      public function showUIClip(param1:String, param2:int, param3:int) : Sprite
      {
         var _loc4_:Sprite = §_-YQ§.§_-qM§(param1);
         _loc4_.x = param2;
         _loc4_.y = param3;
         this.addElementToCertainSortingLayer(_loc4_,SortingLayer.POINTERS_LAYER);
         return _loc4_;
      }
      
      public function showDragonBonedClip(param1:String, param2:int, param3:int) : §_-di§
      {
         var _loc4_:§_-di§ = §_-K1t§.buildArmature(param1);
         _loc4_.display.x = param2;
         _loc4_.display.y = param3;
         this.addElementToCertainSortingLayer(_loc4_.display,SortingLayer.POINTERS_LAYER);
         WorldClock.clock.add(_loc4_);
         _loc4_.playAnimation("play",0,-1,0);
         return _loc4_;
      }
      
      public function hideClip(param1:starling.display.DisplayObject) : void
      {
         this.removeElement(param1);
      }
      
      public function hideUIClip(param1:starling.display.DisplayObject) : void
      {
         if(!param1)
         {
            return;
         }
         this.removeElement(param1);
         param1.dispose();
         param1 = null;
      }
      
      public function hideDragonBonedClip(param1:§_-di§) : void
      {
         if(!param1)
         {
            return;
         }
         if(WorldClock.clock.contains(param1))
         {
            WorldClock.clock.remove(param1);
         }
         this.removeElement(param1.display);
         param1.dispose();
      }
      
      public function drawDamage(param1:int, param2:int, param3:int) : void
      {
         this.groundLayer.drawDamage(param1,param2,param3);
         var _loc4_:§_-eX§ = §_-X1j§.§_-12n§ ? §_-X1j§.§_-12n§.gameMaster : null;
         if(!_loc4_)
         {
            return;
         }
         var _loc5_:Boolean = Boolean(_loc4_.activeSquad) && _loc4_.activeSquad.isLocal();
         if(_loc5_)
         {
            §_-X1j§.§_-12n§.battleStats.§_-K3h§(int(Math.floor(param3 * param3)));
         }
      }
      
      public function drawRectangleDamage(param1:Number, param2:Number, param3:Number, param4:Number) : void
      {
         this.groundLayer.drawRectDamage(param1,param2,param3,param4);
         var _loc5_:§_-eX§ = §_-X1j§.§_-12n§ ? §_-X1j§.§_-12n§.gameMaster : null;
         if(!_loc5_)
         {
            return;
         }
         var _loc6_:Boolean = Boolean(_loc5_.activeSquad) && _loc5_.activeSquad.isLocal();
         if(_loc6_)
         {
            §_-X1j§.§_-12n§.battleStats.§_-K3h§(int(Math.floor((param3 - param1) * (param4 - param2))));
         }
      }
      
      public function drawRotatedRectDamage(param1:Number, param2:Number, param3:Number, param4:Number, param5:Number) : void
      {
         this.groundLayer.drawRotatedRectDamage(param1,param2,param3,param4,param5);
         var _loc6_:§_-eX§ = §_-X1j§.§_-12n§ ? §_-X1j§.§_-12n§.gameMaster : null;
         if(!_loc6_)
         {
            return;
         }
         var _loc7_:Boolean = Boolean(_loc6_.activeSquad) && _loc6_.activeSquad.isLocal();
         if(_loc7_)
         {
            §_-X1j§.§_-12n§.battleStats.§_-K3h§(int(Math.floor(param3 * param4)));
         }
      }
      
      public function addClipToGround(param1:flash.display.DisplayObject, param2:Number, param3:Number, param4:Boolean = false) : void
      {
         if(param4)
         {
            this.addPermanentZone(Pool.getRectangle(param2,param3,param1.width,param1.height));
         }
         this.groundLayer.addClipToGround(param1,param2,param3);
      }
      
      public function removeClipFromGround(param1:flash.display.DisplayObject, param2:Number, param3:Number) : void
      {
         var _loc4_:GlowFilter = new GlowFilter(0,1,6,6,20,1);
         param1.filters = [_loc4_];
         this.groundLayer.removeClipFromGround(param1,param2,param3);
      }
      
      public function addPermanentZone(param1:Rectangle) : void
      {
         this.permanentZones.push(param1);
      }
      
      public function hitTestPoint(param1:int, param2:int) : Boolean
      {
         return this.groundLayer.hitTestPoint(param1,param2) || this.hitTestPermanent(param1,param2);
      }
      
      public function hitTestPermanent(param1:int, param2:int) : Boolean
      {
         var _loc3_:int = int(this.permanentZones.length);
         while(--_loc3_ > -1)
         {
            if(this.permanentZones[_loc3_].contains(param1,param2))
            {
               return true;
            }
         }
         return false;
      }
      
      public function moveClouds() : void
      {
         if(!this.cloudsLayer)
         {
            return;
         }
         this.cloudsLayer.x += Force.WIND.ax;
         if(this.cloudsLayer.x > this._sceneW)
         {
            this.cloudsLayer.x = -this.cloudsLayer.width;
         }
         else if(this.cloudsLayer.x + this.cloudsLayer.width < 0)
         {
            this.cloudsLayer.x = this._sceneW;
         }
      }
      
      private function onAddedToStage(param1:starling.events.Event) : void
      {
         this.parent.addChild(this.gameInterface.getContainer());
      }
      
      public function addElementToCertainSortingLayer(param1:starling.display.DisplayObject, param2:SortingLayer, param3:int = -1) : starling.display.DisplayObject
      {
         var _loc4_:DisplayObjectContainer = this.getSortingLayer(param2);
         if(param3 > -1)
         {
            return _loc4_.addChildAt(param1,Math.min(param3,_loc4_.numChildren));
         }
         return _loc4_.addChild(param1);
      }
      
      public function removeElement(param1:starling.display.DisplayObject, param2:Boolean = false) : starling.display.DisplayObject
      {
         if(!param1)
         {
            return null;
         }
         var _loc3_:DisplayObjectContainer = param1.parent;
         if(_loc3_)
         {
            return _loc3_.removeChild(param1,param2);
         }
         return null;
      }
      
      public function hasElementIntoCertainSortingLayer(param1:starling.display.DisplayObject, param2:SortingLayer) : Boolean
      {
         var _loc3_:DisplayObjectContainer = this.getSortingLayer(param2);
         if(_loc3_)
         {
            return _loc3_.contains(param1);
         }
         return false;
      }
      
      override public function get width() : Number
      {
         return this._sceneW;
      }
      
      override public function get height() : Number
      {
         return this._sceneH;
      }
      
      public function increaseWaterHeight(param1:int) : void
      {
         var changePos:Function;
         var updatePhys:Function;
         var objects:Vector.<§_-F3o§>;
         var delta:int = param1;
         this.groundLayer.shiftGround(delta);
         if(this.groundBackLayer)
         {
            this.groundBackLayer.y += delta;
         }
         this._processingWaterIncrease = true;
         changePos = function(param1:§_-F3o§):Boolean
         {
            param1.y += delta;
            param1.§_-q1Q§();
            return false;
         };
         updatePhys = function(param1:§_-F3o§):Boolean
         {
            param1.§_-Y2I§.checkBounds();
            param1.nextState();
            return false;
         };
         objects = this._gameObjectProcessor.cache.§_-a1N§();
         §_-F2L§.§_-11m§(objects,changePos);
         §_-F2L§.§_-11m§(objects,updatePhys);
         this._processingWaterIncrease = false;
         §_-F2L§.§_-11m§(this.permanentZones,function(param1:Rectangle):Boolean
         {
            param1.y += delta;
            return false;
         });
      }
      
      public function get processingWaterIncrease() : Boolean
      {
         return this._processingWaterIncrease;
      }
      
      public function makeUnstabilizer() : §_-F3o§
      {
         var _loc1_:§_-F3o§ = new §_-F3o§();
         _loc1_.stable = false;
         this.addGameObject(_loc1_);
         return _loc1_;
      }
      
      public function addGameObject(param1:§_-F3o§) : void
      {
         this._gameObjectProcessor.§_-L2X§(param1);
      }
      
      public function updateOnEnterFrame() : void
      {
         var _loc3_:SceneTiledLayer = null;
         var _loc1_:Rectangle = this.sceneCamera.cameraRect.rect;
         var _loc2_:int = int(this.tiledLayers.length);
         while(--_loc2_ > -1)
         {
            _loc3_ = this.tiledLayers[_loc2_];
            _loc3_.update(_loc1_);
         }
      }
      
      public function updateWeather() : void
      {
         this.weather.update();
         this.moveClouds();
      }
      
      public function hideUIAndEffects() : void
      {
         this.gameInterface.§_-W2U§();
         this.getSortingLayer(SortingLayer.EFFECT_LAYER).visible = false;
      }
      
      override public function dispose() : void
      {
         var _loc1_:Rectangle = null;
         Starling.juggler.remove(this);
         LightingEffect.clearPool();
         §_-MN§.§_-sN§();
         this.created = false;
         if(this.gameInterface)
         {
            this.gameInterface.dispose();
            this.gameInterface = null;
         }
         if(this.water)
         {
            if(this.water.parent)
            {
               this.water.parent.removeChild(this.water);
            }
            this.water.destroy();
            this.water = null;
         }
         if(this.cloudsLayer)
         {
            this.cloudsLayer.removeFromParent(true);
            this.cloudsLayer = null;
         }
         this.backgroundLayer.removeFromParent(true);
         if(this.groundBackLayer)
         {
            this.groundBackLayer.removeFromParent(true);
         }
         this.groundLayer.removeFromParent(true);
         this.objectsLayer.dispose();
         if(this.curMapRecord)
         {
            this.curMapRecord = null;
         }
         if(this.weather)
         {
            this.weather.dispose();
            this.weather = null;
         }
         if(this._interactionsHandler)
         {
            this._interactionsHandler.dispose();
            this._interactionsHandler = null;
         }
         if(backTilesAtlas)
         {
            backTilesAtlas.clearAll();
         }
         this.activeUnitPointer.dispose();
         this.sceneCamera.dispose();
         super.dispose();
         §_-K1t§.§_-LC§();
         Pool.putRectangle(this.boundsRect);
         this.boundsRect = null;
         if(this.permanentZones)
         {
            for each(_loc1_ in this.permanentZones)
            {
               Pool.putRectangle(_loc1_);
               _loc1_ = null;
            }
            this.permanentZones.length = 0;
            this.permanentZones = null;
         }
      }
      
      public function getMapRecord() : §_-D3a§
      {
         return this.curMapRecord;
      }
      
      public function bringToTop(param1:§_-F3o§) : void
      {
         var _loc2_:DisplayObjectContainer = null;
         if(param1.clip)
         {
            _loc2_ = this.getSortingLayer(SortingLayer.UNIT_LAYER);
            _loc2_.setChildIndex(param1.clip,_loc2_.numChildren - 1);
         }
      }
      
      public function bringToBottom(param1:§_-F3o§) : void
      {
         if(param1.clip)
         {
            this.getSortingLayer(SortingLayer.UNIT_LAYER).setChildIndex(param1.clip,0);
         }
      }
      
      public function setLayerIndex(param1:§_-F3o§, param2:SortingLayer, param3:int) : void
      {
         if(param1.clip)
         {
            this.getSortingLayer(param2).setChildIndex(param1.clip,param3);
         }
      }
      
      public function getLayerIndex(param1:§_-F3o§, param2:SortingLayer) : int
      {
         if(param1.clip)
         {
            return this.getSortingLayer(param2).getChildIndex(param1.clip);
         }
         return -1;
      }
      
      public function setWeather(param1:uint) : void
      {
         this.weather.change(param1);
      }
      
      public function gotoNextTurn(param1:§_-TA§, param2:Boolean = true) : void
      {
         if(this.gameInterface.§_-Fb§.isOpened)
         {
            this.gameInterface.§_-Fb§.close();
         }
         Starling.juggler.delayCall(this.updateGroundImages,0.25);
         if(param2)
         {
            this.nextTurnEvent.§_-225§ = param1.value;
            this.dispatchEvent(this.nextTurnEvent);
         }
      }
      
      private function updateGroundImages() : void
      {
         if(this.groundLayer)
         {
            this.groundLayer.refreshDamagedTiles();
         }
         if(this.sceneRestoreCallback != null)
         {
            setTimeout(this.turnOnGraphics,100);
         }
      }
      
      public function get gameObjectProcessor() : §_-93l§
      {
         return this._gameObjectProcessor;
      }
      
      public function set gameObjectProcessor(param1:§_-93l§) : void
      {
         this._gameObjectProcessor = param1;
      }
      
      public function get sceneW() : int
      {
         return this._sceneW;
      }
      
      public function set sceneW(param1:int) : void
      {
         this._sceneW = param1;
      }
      
      public function get sceneH() : int
      {
         return this._sceneH;
      }
      
      public function set sceneH(param1:int) : void
      {
         this._sceneH = param1;
      }
      
      public function stagePointToGroundPoint(param1:Point) : Point
      {
         return this.groundLayer.globalToLocal(param1);
      }
      
      public function get interactionsHandler() : SceneInteractionsHandler
      {
         return this._interactionsHandler;
      }
   }
}

