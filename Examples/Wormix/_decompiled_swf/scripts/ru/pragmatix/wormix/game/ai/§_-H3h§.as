package ru.pragmatix.wormix.game.ai
{
   import §_-73d§.§_-v1o§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-cZ§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class §_-H3h§
   {
      
      private var fs:Array = [];
      
      public function §_-H3h§()
      {
         super();
      }
      
      public function chain(param1:Function, param2:Object, ... rest) : §_-H3h§
      {
         var func:Function = param1;
         var fThis:Object = param2;
         var args:Array = rest;
         var f:Function = function():void
         {
            var _loc3_:* = undefined;
            var _loc2_:Array = [arguments.callee.next];
            for each(_loc3_ in args)
            {
               _loc2_.push(_loc3_);
            }
            func.apply(fThis,_loc2_);
         };
         f["next"] = this.next;
         this.fs.push(f);
         return this;
      }
      
      private function next() : void
      {
         this.fs.shift()();
      }
      
      public function run() : void
      {
         this.next();
      }
   }
}

