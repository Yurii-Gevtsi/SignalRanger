package ru.pragmatix.wormix.fx
{
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   
   public class EffectsPool
   {
      
      private static var instance:EffectsPool;
      
      private var cache:Dictionary;
      
      public function EffectsPool()
      {
         super();
         instance = this;
         this.cache = new Dictionary();
      }
      
      public static function getInstance() : EffectsPool
      {
         if(instance == null)
         {
            return new EffectsPool();
         }
         return instance;
      }
      
      public function createObject(param1:EffectType) : IPooledEffect
      {
         var _loc2_:IPooledEffect = null;
         var _loc3_:Array = this.getEffectPool(param1.name);
         if(_loc3_.length)
         {
            _loc2_ = _loc3_.pop();
            if(_loc2_ is §_-F3o§)
            {
               (_loc2_ as §_-F3o§).clip.visible = true;
            }
         }
         else
         {
            _loc2_ = param1.createInstance();
         }
         return _loc2_;
      }
      
      public function putObject(param1:IPooledEffect, param2:EffectType) : void
      {
         this.getEffectPool(param2.name).push(param1);
      }
      
      public function createPoolOfObjects(param1:EffectType, param2:int = 100) : void
      {
         var _loc3_:* = param2;
         while(_loc3_--)
         {
            this.putObject(param1.createInstance(),param1);
         }
      }
      
      private function getEffectPool(param1:String, param2:Boolean = true) : Array
      {
         var _loc3_:Array = this.cache[param1];
         if(_loc3_ == null)
         {
            _loc3_ = [];
            this.cache[param1] = _loc3_;
         }
         return _loc3_;
      }
   }
}

