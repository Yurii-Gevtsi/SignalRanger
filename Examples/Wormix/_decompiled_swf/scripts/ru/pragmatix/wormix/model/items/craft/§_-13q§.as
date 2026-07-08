package ru.pragmatix.wormix.model.items.craft
{
   import §_-62§.§_-O2H§;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import feathers.core.IPopUpManager;
   
   public class §_-13q§
   {
      
      public var §_-R2N§:String;
      
      public var root:§_-6S§;
      
      public function §_-13q§(param1:§_-O2H§)
      {
         super();
         this.root = new §_-6S§();
         this.root.level = 0;
         this.root.§_-HS§ = null;
         this.root.weapon = param1;
      }
      
      public function get §_-A39§() : §_-O2H§
      {
         return this.root.weapon;
      }
   }
}

