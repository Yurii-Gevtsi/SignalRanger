package ru.pragmatix.wormix.scene
{
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.Shape;
   import flash.display3D.Context3DTextureFormat;
   import flash.geom.Matrix;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.scene.tile.GeneratedTilesData;
   import ru.pragmatix.wormix.scene.tile.GroundTile;
   import ru.pragmatix.wormix.scene.tile.TileData;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.core.Starling;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class SceneGround extends SceneTiledLayer
   {
      
      private static var damageShape:Shape;
      
      private static var rectDamageShape:Shape;
      
      private static const DAMAGE_IMAGE_RADIUS:int = 50;
      
      private static const DAMAGE_IMAGE_RADIUS_REVERT:Number = 1 / DAMAGE_IMAGE_RADIUS;
      
      private var worldBounds:Rectangle;
      
      private var tilesInRowNum:int;
      
      private var tilesInColumnNum:int;
      
      private var groundTileVec:Vector.<GroundTile>;
      
      private var groundTileDic:Dictionary;
      
      private var outdatedGroundTileVec:Vector.<GroundTile>;
      
      private var groundTilesForBundleDraw:Vector.<GroundTile>;
      
      private var imageForSizeUpdate:Vector.<Image>;
      
      private var imagesForSizeUpdateSize:int = 0;
      
      private var helperPoint:Point;
      
      private var destructable:Boolean;
      
      private var damageImage:Image;
      
      private var damageImageBitmapData:BitmapData;
      
      private var damageImageTexture:Texture;
      
      private var rectDamageImage:Image;
      
      private var rectDamageImageBitmapData:BitmapData;
      
      private var rectDamageImageTexture:Texture;
      
      private var groundShift:int = 0;
      
      private var needToUpdateChildsAfterGroundShift:Boolean = false;
      
      private var needPerformBundledDraw:Boolean = false;
      
      private var tick:int = 0;
      
      private var groundTileCreationCall:uint;
      
      public function SceneGround(param1:int, param2:int, param3:Boolean)
      {
         this.worldBounds = Pool.getRectangle(0,0,param1,param2);
         super(this.worldBounds.width,this.worldBounds.height,Scene.TILE_WIDTH,Scene.TILE_HEIGHT);
         this.helperPoint = Pool.getPoint();
         this.destructable = param3;
         this.outdatedGroundTileVec = new Vector.<GroundTile>(0);
         this.groundTilesForBundleDraw = new Vector.<GroundTile>(0);
         this.imageForSizeUpdate = new Vector.<Image>(0);
         SystemUtil.executeWhenApplicationIsActive(this.prepareDamageImages);
      }
      
      private function prepareDamageImages() : void
      {
         this.prepareCircleDamageImage();
         this.prepareRectDamageImage();
      }
      
      private function prepareCircleDamageImage() : void
      {
         var damageImageShape:Shape;
         damageShape = new Shape();
         damageShape.graphics.beginFill(0);
         damageShape.graphics.drawCircle(0,0,1);
         damageShape.graphics.endFill();
         damageImageShape = new Shape();
         damageImageShape.graphics.beginFill(16711680);
         damageImageShape.graphics.drawCircle(DAMAGE_IMAGE_RADIUS,DAMAGE_IMAGE_RADIUS,DAMAGE_IMAGE_RADIUS);
         damageImageShape.graphics.endFill();
         this.damageImageBitmapData = new BitmapData(Math.ceil(damageImageShape.width),Math.ceil(damageImageShape.height),true,0);
         this.damageImageBitmapData.draw(damageImageShape);
         this.damageImageTexture = Texture.fromBitmapData(this.damageImageBitmapData,false);
         this.damageImageTexture.root.onRestore = function():void
         {
            damageImageTexture.root.uploadBitmapData(damageImageBitmapData);
         };
         this.damageImage = new Image(this.damageImageTexture);
      }
      
      private function prepareRectDamageImage() : void
      {
         var damageImageShape:Shape;
         rectDamageShape = new Shape();
         rectDamageShape.graphics.beginFill(0);
         rectDamageShape.graphics.drawRect(0,0,1,1);
         rectDamageShape.graphics.endFill();
         damageImageShape = new Shape();
         damageImageShape.graphics.beginFill(16711680);
         damageImageShape.graphics.drawRect(0,0,DAMAGE_IMAGE_RADIUS * 2,DAMAGE_IMAGE_RADIUS * 2);
         damageImageShape.graphics.endFill();
         this.rectDamageImageBitmapData = new BitmapData(Math.ceil(damageImageShape.width),Math.ceil(damageImageShape.height),true,0);
         this.rectDamageImageBitmapData.draw(damageImageShape);
         this.rectDamageImageTexture = Texture.fromBitmapData(this.rectDamageImageBitmapData,false);
         this.rectDamageImageTexture.root.onRestore = function():void
         {
            rectDamageImageTexture.root.uploadBitmapData(rectDamageImageBitmapData);
         };
         this.rectDamageImage = new Image(this.rectDamageImageTexture);
      }
      
      override public function dispose() : void
      {
         var _loc1_:GroundTile = null;
         super.dispose();
         for each(_loc1_ in this.groundTileVec)
         {
            _loc1_.dispose();
         }
         this.groundTileVec = null;
         this.outdatedGroundTileVec = null;
         this.groundTilesForBundleDraw = null;
         this.imageForSizeUpdate = null;
         Pool.putPoint(this.helperPoint);
         this.helperPoint = null;
         Pool.putRectangle(this.worldBounds);
         this.worldBounds = null;
         this.damageImageTexture.dispose();
         this.damageImageBitmapData.dispose();
         this.damageImage.dispose();
         this.rectDamageImageTexture.dispose();
         this.rectDamageImageBitmapData.dispose();
         this.rectDamageImage.dispose();
      }
      
      public function init(param1:GeneratedTilesData) : void
      {
         this.tilesInRowNum = param1.numHorizontalTiles;
         this.tilesInColumnNum = param1.numVerticalTiles;
         this.groundTileVec = new Vector.<GroundTile>();
         this.groundTileDic = new Dictionary();
         this.groundTileCreationCall = Starling.juggler.repeatCall(this.createGroundTile,0.01,0,param1.tileDataArray);
      }
      
      private function createGroundTile(param1:Array) : void
      {
         var _loc3_:GroundTile = null;
         var _loc4_:int = 0;
         var _loc2_:TileData = param1.pop();
         if(_loc2_)
         {
            _loc3_ = new GroundTile(_loc2_);
            addChild(_loc3_.image);
            _loc4_ = this.convertWorldPosIntoIndex(_loc3_.x,_loc3_.y);
            this.groundTileDic[_loc4_] = _loc3_;
            this.groundTileVec.push(_loc3_);
         }
         else
         {
            Starling.juggler.removeByID(this.groundTileCreationCall);
            dispatchEventWith(Event.COMPLETE);
         }
      }
      
      override public function update(param1:Rectangle) : void
      {
         if(this.needPerformBundledDraw && this.tick++ > 0)
         {
            this.tick = 0;
            this.performBundledDrawInGroundTiles();
         }
         super.update(param1);
      }
      
      private function performBundledDrawInGroundTiles() : void
      {
         var _loc1_:int = int(this.groundTilesForBundleDraw.length);
         var _loc2_:int = _loc1_;
         while(--_loc2_ > -1)
         {
            this.groundTilesForBundleDraw[_loc2_].performBundledDraw(this.damageImage);
         }
         this.needPerformBundledDraw = false;
         this.groundTilesForBundleDraw.splice(0,_loc1_);
      }
      
      override public function refresh() : void
      {
         var groundTile:GroundTile = null;
         var image:Image = null;
         try
         {
            if(this.needToUpdateChildsAfterGroundShift)
            {
               for each(groundTile in this.groundTileVec)
               {
                  this.updateImage(groundTile.image);
               }
               this.needToUpdateChildsAfterGroundShift = false;
            }
            else if(this.imagesForSizeUpdateSize > 0)
            {
               for each(image in this.imageForSizeUpdate)
               {
                  this.updateImage(image);
               }
               this.imageForSizeUpdate.splice(0,this.imagesForSizeUpdateSize);
               this.imagesForSizeUpdateSize = 0;
            }
         }
         catch(e:*)
         {
         }
         super.refresh();
      }
      
      private function updateImage(param1:Image) : void
      {
         if(param1.parent)
         {
            updateChild(param1);
         }
         else
         {
            removeChild(param1);
            addChild(param1);
         }
      }
      
      public function shiftGround(param1:int) : void
      {
         var _loc2_:GroundTile = null;
         var _loc5_:int = 0;
         this.groundShift += param1;
         var _loc3_:int = int(this.groundTileVec.length);
         var _loc4_:int = _loc3_;
         while(--_loc4_ > -1)
         {
            _loc2_ = this.groundTileVec[_loc4_];
            if(_loc2_)
            {
               _loc2_.y += param1;
               if(_loc2_.y > this.worldBounds.height)
               {
                  _loc2_.clear();
                  _loc2_.y -= this.tilesInColumnNum * Scene.TILE_HEIGHT;
               }
            }
         }
         _loc4_ = _loc3_;
         while(--_loc4_ > -1)
         {
            _loc2_ = this.groundTileVec[_loc4_];
            if(_loc2_)
            {
               _loc5_ = this.convertWorldPosIntoIndex(_loc2_.x,_loc2_.y);
               this.groundTileDic[_loc5_] = _loc2_;
            }
         }
         this.needToUpdateChildsAfterGroundShift = true;
      }
      
      public function hitTestPoint(param1:int, param2:int) : Boolean
      {
         var _loc3_:int = 0;
         var _loc4_:GroundTile = null;
         if(this.worldBounds.contains(param1,param2))
         {
            _loc3_ = this.convertWorldPosIntoIndex(param1,param2);
            _loc4_ = this.groundTileDic[_loc3_];
            return (Boolean(_loc4_)) && _loc4_.hitTest(param1,param2);
         }
         return false;
      }
      
      public function drawDamage(param1:int, param2:int, param3:int) : void
      {
         var _loc4_:Number = NaN;
         if(this.destructable)
         {
            _loc4_ = param3 * DAMAGE_IMAGE_RADIUS_REVERT;
            this.damageImage.scaleX = this.damageImage.scaleY = _loc4_;
            this.damageImage.x = param1 - param3;
            this.damageImage.y = param2 - param3;
            damageShape.scaleX = damageShape.scaleY = param3;
            damageShape.x = param1;
            damageShape.y = param2;
            this.drawObject(damageShape,this.damageImage,true,true);
         }
      }
      
      public function drawRectDamage(param1:int, param2:int, param3:int, param4:int) : void
      {
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         if(this.destructable)
         {
            _loc5_ = param3 - param1;
            _loc6_ = param4 - param2;
            this.prepareRectDamage(param1,param2,_loc5_,_loc6_,0);
            this.drawObject(rectDamageShape,this.rectDamageImage,true,false);
         }
      }
      
      public function drawRotatedRectDamage(param1:int, param2:int, param3:int, param4:int, param5:Number) : void
      {
         if(this.destructable)
         {
            this.prepareRectDamage(param1,param2,param3,param4,param5);
            this.drawObject(rectDamageShape,this.rectDamageImage,true);
         }
      }
      
      private function prepareRectDamage(param1:int, param2:int, param3:int, param4:int, param5:Number) : void
      {
         this.rectDamageImage.rotation = 0;
         this.rectDamageImage.width = param3;
         this.rectDamageImage.height = param4;
         this.rectDamageImage.rotation = param5;
         this.rectDamageImage.x = param1;
         this.rectDamageImage.y = param2;
         rectDamageShape.rotation = 0;
         rectDamageShape.width = param3;
         rectDamageShape.height = param4;
         rectDamageShape.rotation = param5 * MathUtil.§_-s1S§;
         rectDamageShape.x = param1;
         rectDamageShape.y = param2;
      }
      
      public function addClipToGround(param1:DisplayObject, param2:Number, param3:Number) : void
      {
         this.drawObjectOnGround(param1,param2,param3,false);
      }
      
      public function removeClipFromGround(param1:DisplayObject, param2:Number, param3:Number) : void
      {
         this.drawObjectOnGround(param1,param2,param3,true);
      }
      
      private function drawObjectOnGround(param1:DisplayObject, param2:Number, param3:Number, param4:Boolean) : void
      {
         param1.x = param2;
         param1.y = param3;
         var _loc5_:Rectangle = param1.getBounds(Starling.current.nativeStage);
         var _loc6_:BitmapData = new BitmapData(Math.ceil(_loc5_.width),Math.ceil(_loc5_.height),true,0);
         var _loc7_:Matrix = param1.transform.matrix.clone();
         _loc7_.translate(-_loc5_.left,-_loc5_.top);
         _loc6_.draw(param1,_loc7_);
         var _loc8_:Image = new Image(Texture.fromBitmapData(_loc6_,false,false,1,Context3DTextureFormat.BGRA_PACKED));
         _loc8_.x = _loc5_.left;
         _loc8_.y = _loc5_.top;
         this.drawObject(param1,_loc8_,param4);
      }
      
      private function drawObject(param1:DisplayObject, param2:Image, param3:Boolean = true, param4:Boolean = false) : void
      {
         var _loc5_:Rectangle = null;
         var _loc10_:GroundTile = null;
         var _loc11_:int = 0;
         var _loc13_:int = 0;
         _loc5_ = param2.bounds;
         var _loc6_:int = this.convert_worldX_to_tileX(Math.max(_loc5_.left,0));
         var _loc7_:int = this.convert_worldX_to_tileX(Math.min(_loc5_.right,this.worldBounds.right));
         var _loc8_:int = this.convert_worldY_to_tileY(Math.max(_loc5_.top,0));
         var _loc9_:int = this.convert_worldY_to_tileY(Math.min(_loc5_.bottom,this.worldBounds.bottom));
         var _loc12_:int = _loc6_;
         while(_loc12_ <= _loc7_)
         {
            _loc13_ = _loc8_;
            while(_loc13_ <= _loc9_)
            {
               _loc11_ = this.convertTilePosIntoIndex(_loc12_,_loc13_);
               _loc10_ = this.groundTileDic[_loc11_];
               if(!param4)
               {
                  _loc10_.drawObjectInImage(param2,param3);
               }
               else
               {
                  _loc10_.saveForBundledDraw(param2);
                  this.groundTilesForBundleDraw.push(_loc10_);
                  this.needPerformBundledDraw = true;
               }
               _loc10_.drawObjectInBitmapData(param1,_loc5_,param3);
               if(this.outdatedGroundTileVec.indexOf(_loc10_) == -1)
               {
                  this.outdatedGroundTileVec.push(_loc10_);
               }
               this.enqueueToChildUpdate(_loc10_);
               _loc13_++;
            }
            _loc12_++;
         }
      }
      
      public function refreshDamagedTiles() : void
      {
         var _loc2_:GroundTile = null;
         var _loc1_:int = int(this.outdatedGroundTileVec.length);
         while(--_loc1_ > -1)
         {
            _loc2_ = this.outdatedGroundTileVec.pop();
            if(!_loc2_.isRefreshed())
            {
               _loc2_.refresh();
            }
            this.enqueueToChildUpdate(_loc2_);
         }
      }
      
      private function convertWorldPosIntoIndex(param1:int, param2:int) : int
      {
         return this.convertTilePosIntoIndex(this.convert_worldX_to_tileX(param1),this.convert_worldY_to_tileY(param2));
      }
      
      private function convert_worldX_to_tileX(param1:int) : int
      {
         return Math.floor(param1 * Scene.TILE_WIDTH_REVERT);
      }
      
      private function convert_worldY_to_tileY(param1:int) : int
      {
         return Math.floor((param1 - this.groundShift + Scene.TILE_HEIGHT) * Scene.TILE_HEIGHT_REVERT);
      }
      
      private function convertTilePosIntoIndex(param1:int, param2:int) : int
      {
         return param1 * 10000 + param2;
      }
      
      private function enqueueToChildUpdate(param1:GroundTile) : void
      {
         var _loc2_:int = this.imageForSizeUpdate.indexOf(param1.image);
         if(_loc2_ == -1)
         {
            this.imageForSizeUpdate.push(param1.image);
            ++this.imagesForSizeUpdateSize;
         }
      }
   }
}

