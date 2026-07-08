package ru.pragmatix.wormix.model.items.craft
{
   import §_-21p§.§_-4w§;
   import §_-62§.*;
   import §_-TF§.§_-y2n§;
   import com.greensock.loading.LoaderStatus;
   import config.§_-B1K§;
   
   public class §_-IE§ extends §_-W1r§
   {
      
      public var prevUpgradeId:int = -1;
      
      public var upgradeId:int;
      
      public var reagents:Vector.<§_-o6§>;
      
      public var level:int = 0;
      
      public var availableFromStart:Boolean = true;
      
      public var §_-j0§:§_-6S§;
      
      public function §_-IE§()
      {
         super();
      }
      
      override public function initFromConfig(param1:Object) : void
      {
         super.initFromConfig(param1);
         if(param1.prevUpgradeId)
         {
            this.prevUpgradeId = param1.prevUpgradeId;
         }
         if(param1.upgradeId)
         {
            this.upgradeId = param1.upgradeId;
         }
         if(param1.reagents)
         {
            this.reagents = param1.reagents;
         }
         if(param1.level)
         {
            this.level = param1.level;
         }
         if(param1.availableFromStart)
         {
            this.availableFromStart = param1.availableFromStart;
         }
      }
      
      override public function §_-n1W§() : int
      {
         return §_-y2n§.§_-81U§;
      }
   }
}

