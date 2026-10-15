package ru.pragmatix.wormix.game
{
   import §_-21p§.§_-4w§;
   import §_-9e§.§_-73e§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-U11§;
   import §_-G3w§.*;
   import §_-Y1O§.*;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-T2N§;
   import §_-m1g§.*;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-sQ§.§_-V1n§;
   import com.greensock.loading.*;
   import feathers.controls.ImageLoader;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import flash.display.StageOrientation;
   import flash.events.Event;
   import flash.events.StageOrientationEvent;
   import flash.events.TimerEvent;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.pvp.messages.ex.WidenSearch;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class §_-QF§
   {
      
      public var §_-13r§:Array = [];
      
      public var §_-c2i§:Array = [];
      
      public var §_-j1i§:§_-7u§ = new §_-7u§(false);
      
      public var §_-b2a§:§_-7u§ = new §_-7u§(false);
      
      private var settings:§_-73e§;
      
      public function §_-QF§(param1:§_-73e§)
      {
         super();
         this.settings = param1;
      }
      
      public function record(param1:int, param2:UserProfile, param3:int = 1) : void
      {
         var _loc4_:Boolean = false;
         var _loc5_:WeaponStructure = null;
         if(this.settings.§_-52M§)
         {
            if(this.settings.§_-31H§.length > 0)
            {
               for each(_loc5_ in this.settings.§_-31H§)
               {
                  if(param1 == _loc5_.getId())
                  {
                     _loc4_ = true;
                     break;
                  }
               }
            }
            else
            {
               _loc4_ = true;
            }
            if(_loc4_)
            {
               if(Application.userProfile.equals(param2))
               {
                  this.§_-u2O§(param1,this.§_-13r§,param3);
               }
               this.§_-u2O§(param1,this.§_-c2i§,param3);
            }
         }
      }
      
      private function §_-u2O§(param1:int, param2:Array, param3:int = 1) : void
      {
         var _loc4_:WeaponStructure = null;
         for each(_loc4_ in param2)
         {
            if(_loc4_.getId() == param1)
            {
               _loc4_.addCount(param3);
               return;
            }
         }
         if(param3 > 0)
         {
            param2.push(new WeaponStructure(param1,param3));
         }
      }
      
      public function reset() : void
      {
         this.§_-j1i§.value = false;
         this.§_-b2a§.value = false;
         this.§_-c2i§ = [];
      }
   }
}

