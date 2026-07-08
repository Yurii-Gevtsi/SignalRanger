package ru.pragmatix.wormix.gui
{
   import §_-F39§.*;
   import §_-S§.*;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.BasicButton;
   import feathers.controls.Button;
   import feathers.controls.ScrollBar;
   import feathers.core.PopUpManager;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.structures.PvpProfileStructure;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.messages.structures.UnitStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.structures.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.utils.Pool;
   
   public class §_-eP§
   {
      
      public function §_-eP§()
      {
         super();
      }
      
      public static function §_-G3p§() : ScrollBar
      {
         var scrollBar:ScrollBar = new ScrollBar();
         scrollBar.thumbFactory = function():BasicButton
         {
            var _loc1_:BasicButton = insideScrollBarFactory();
            _loc1_.pivotX = -_loc1_.defaultSkin.width;
            return _loc1_;
         };
         scrollBar.liveDragging = true;
         return scrollBar;
      }
      
      public static function §_-M1B§() : ScrollBar
      {
         var _loc1_:ScrollBar = new ScrollBar();
         _loc1_.thumbFactory = insideScrollBarFactory;
         _loc1_.liveDragging = true;
         return _loc1_;
      }
      
      private static function insideScrollBarFactory() : BasicButton
      {
         var _loc1_:Button = new Button();
         var _loc2_:Image = new Image(Application.assetManager.getTexture("scrollbar_btn_default"));
         _loc2_.scale9Grid = Pool.getRectangle(4,4,24,3);
         _loc2_.width = 10;
         _loc2_.height = 30;
         _loc1_.defaultSkin = _loc2_;
         return _loc1_;
      }
   }
}

