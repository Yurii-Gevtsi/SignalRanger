package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-21p§.§_-4w§;
   import §_-62§.§_-L1r§;
   import §_-73d§.§_-x2C§;
   import §_-91B§.§_-QK§;
   import §_-Bv§.§_-G3P§;
   import §_-Ly§.§_-81L§;
   import §_-YF§.§_-q2v§;
   import §_-l1K§.§_-d2j§;
   import §_-l1K§.§_-vO§;
   import §_-n1w§.*;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-r1C§.§_-h2B§;
   import §_-u13§.MD2;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u13§.§_-F3§;
   import com.hurlant.util.der.Sequence;
   import com.hurlant.util.der.§_-cz§;
   import com.hurlant.util.der.§_-l1b§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.LayoutGroup;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.TiledColumnsLayout;
   import feathers.skins.ImageSkin;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   
   public class §_-P4§
   {
      
      private static const §_-O16§:String = "#4BAB02";
      
      private static const §_-Nk§:String = "#6F5742";
      
      private var §_-n2p§:DisplayObjectContainer;
      
      public function §_-P4§(param1:DisplayObjectContainer, param2:§_-L1r§, param3:CraftedEquipFamily = null)
      {
         super();
         this.§_-n2p§ = param1;
         var _loc4_:TiledColumnsLayout = TiledColumnsLayout((param1 as LayoutGroup).layout);
         _loc4_.tileHorizontalAlign = HorizontalAlign.LEFT;
         _loc4_.horizontalAlign = HorizontalAlign.LEFT;
         _loc4_.requestedColumnCount = 2;
         if(param3)
         {
            this.§_-e3§(param2,param3);
         }
         else
         {
            this.§_-R12§(param2);
         }
      }
      
      private static function §_-R1s§(param1:CraftedEquipFamily) : Dictionary
      {
         var _loc3_:§_-4w§ = null;
         var _loc2_:Dictionary = new Dictionary();
         for each(_loc3_ in param1.§_-42S§().§_-b2I§())
         {
            _loc2_[_loc3_.getType().getName()] = _loc3_;
         }
         return _loc2_;
      }
      
      private static function §_-e2g§(param1:CraftedEquipFamily, param2:int) : Dictionary
      {
         var _loc4_:§_-QK§ = null;
         var _loc5_:Vector.<§_-4w§> = null;
         var _loc6_:§_-4w§ = null;
         var _loc3_:Dictionary = new Dictionary();
         for each(_loc4_ in param1.§_-46§())
         {
            if(_loc4_.§_-62n§() == param2)
            {
               _loc5_ = _loc4_.§_-V1v§().§_-b2I§();
               for each(_loc6_ in _loc5_)
               {
                  _loc3_[_loc6_.getType().getName()] = _loc6_;
               }
               return _loc3_;
            }
         }
         return _loc3_;
      }
      
      private static function §_-2V§(param1:String, param2:Boolean) : String
      {
         return "<font color=\'" + (param2 ? §_-O16§ : §_-Nk§) + "\'>" + param1 + "</font> ";
      }
      
      private function §_-R12§(param1:§_-L1r§) : void
      {
         var _loc3_:§_-4w§ = null;
         var _loc4_:§_-qB§ = null;
         var _loc2_:Vector.<§_-4w§> = param1.§_-V1v§().§_-b2I§();
         for each(_loc3_ in _loc2_)
         {
            if(_loc3_.§_-I2a§())
            {
               _loc4_ = this.§_-X11§();
               _loc4_.init(_loc3_.getType().§_-M2c§(),§_-s2a§.§_-K3t§(_loc3_.getType().§_-23r§()) + ":",_loc3_.§_-I2a§(),_loc3_.getType().§_-5B§());
               this.§_-n2p§.addChild(_loc4_.§_-tt§());
            }
         }
      }
      
      private function §_-e3§(param1:§_-L1r§, param2:CraftedEquipFamily) : void
      {
         var _loc6_:§_-4w§ = null;
         var _loc8_:§_-4w§ = null;
         var _loc9_:§_-qB§ = null;
         var _loc3_:Dictionary = §_-R1s§(param2);
         var _loc4_:Dictionary = §_-e2g§(param2,param1.getId());
         var _loc5_:Vector.<§_-4w§> = param1.§_-V1v§().§_-b2I§();
         var _loc7_:String = "";
         for each(_loc6_ in _loc5_)
         {
            if(_loc6_.§_-I2a§())
            {
               _loc7_ = "";
               _loc8_ = _loc3_[_loc6_.getType().getName()];
               if(_loc8_)
               {
                  _loc7_ += §_-2V§((_loc8_ as §_-4w§).§_-I2a§(),false);
               }
               _loc8_ = _loc4_[_loc6_.getType().getName()];
               if(_loc8_)
               {
                  _loc7_ += §_-2V§((_loc8_ as §_-4w§).§_-I2a§(true),true);
               }
               _loc9_ = this.§_-X11§();
               _loc9_.init(_loc6_.getType().§_-M2c§(),§_-s2a§.§_-K3t§(_loc6_.getType().§_-23r§()) + ":",_loc7_,_loc6_.getType().§_-5B§());
               this.§_-n2p§.addChild(_loc9_.§_-tt§());
            }
         }
      }
      
      public function dispose() : void
      {
         this.§_-n2p§.dispose();
         this.§_-n2p§ = null;
      }
      
      private function §_-X11§() : §_-qB§
      {
         var _loc1_:DisplayObjectContainer = §_-YQ§.§_-qM§(§_-q2v§.§_-p2R§,§_-q2v§.§_-A2T§);
         return new §_-qB§(DisplayObjectContainer(§_-YQ§.getClip("template",_loc1_)));
      }
   }
}

