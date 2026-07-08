package ru.pragmatix.wormix.gui.menu.arena.boss
{
   import §_-21p§.§_-4w§;
   import §_-23P§.§_-V2T§;
   import §_-52L§.§_-u2x§;
   import §_-62§.§_-G1p§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.*;
   import §_-L2F§.§_-D2p§;
   import §_-P1z§.*;
   import §_-RE§.§_-D3v§;
   import §_-YF§.§_-q2v§;
   import §_-b19§.§_-ip§;
   import §_-c2O§.§_-03I§;
   import §_-nZ§.§_-41f§;
   import §_-nZ§.§_-6z§;
   import §_-z1g§.§_-y1S§;
   import com.greensock.loading.LoaderStatus;
   import feathers.controls.ImageLoader;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.model.items.craft.§_-13q§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.ScaleMode;
   
   public class §_-K1P§ extends §_-x2u§
   {
      
      protected static var §_-n1o§:int = -1;
      
      public function §_-K1P§()
      {
         var _loc1_:DisplayObjectContainer = §_-YQ§.§_-qM§(§_-q2v§.§_-TW§,§_-q2v§.§_-h2y§);
         super(_loc1_,§_-ip§,§_-41f§);
      }
      
      override protected function §_-QB§() : Array
      {
         var _loc1_:Array = [];
         var _loc2_:Vector.<§_-y1S§> = this.§_-A3E§().§_-A2A§(0);
         var _loc3_:int = int(_loc2_.length);
         var _loc4_:uint = 0;
         while(_loc4_ < _loc3_)
         {
            _loc1_.push(_loc2_[_loc4_]);
            _loc4_++;
         }
         return _loc1_;
      }
      
      override protected function §_-p2N§(param1:Event) : void
      {
         this.§_-m7§((§_-SR§ as §_-ip§).selectedIndex);
         if(§_-SR§ as §_-ip§)
         {
            this.§_-P1I§(§_-i2C§,(§_-SR§ as §_-ip§).selectedItem);
         }
      }
      
      public function §_-P1I§(param1:§_-03I§, param2:§_-D2p§) : void
      {
         if(!param2)
         {
            return;
         }
         var _loc3_:String = §_-6z§.§_-l2I§(param2);
         if(_loc3_ == §_-6z§.§_-D1p§)
         {
            param1.§_-x2i§(param2);
         }
         if(_loc3_ == §_-6z§.COMPLETED)
         {
            param1.§_-42T§(param2);
         }
         if(_loc3_ == §_-6z§.AVAILABLE)
         {
            param1.§_-23j§(param2);
         }
         if(_loc3_ == §_-6z§.UNAVAILABLE)
         {
            param1.§_-511§(param2);
         }
         if(_loc3_ == §_-6z§.§_-v1N§)
         {
            param1.§_-t1i§(param2);
         }
      }
      
      protected function §_-A3E§() : §_-D3v§
      {
         return §_-N1V§.§_-I39§.§_-X1P§();
      }
      
      override protected function §_-I1p§() : int
      {
         return §_-n1o§;
      }
      
      override protected function §_-m7§(param1:int) : void
      {
         §_-n1o§ = param1;
      }
   }
}

