package ru.pragmatix.wormix.scene
{
   public class SortingLayer
   {
      
      public static const WEATHER_LAYER:SortingLayer = new SortingLayer("WEATHER_LAYER",0);
      
      public static const GRAVE_LAYER:SortingLayer = new SortingLayer("GRAVE_LAYER",1);
      
      public static const DROPPED_HAT_LAYER:SortingLayer = new SortingLayer("DROPPED_HAT_LAYER",2,true);
      
      public static const UNIT_LAYER:SortingLayer = new SortingLayer("UNIT_LAYER",3,true);
      
      public static const AMMO_LAYER:SortingLayer = new SortingLayer("AMMO_LAYER",4);
      
      public static const EFFECT_LAYER:SortingLayer = new SortingLayer("EFFECT_LAYER",5,false,false);
      
      public static const CLOUDS_LAYER:SortingLayer = new SortingLayer("CLOUDS_LAYER",6);
      
      public static const WATER_LAYER:SortingLayer = new SortingLayer("WATER_LAYER",7);
      
      public static const UNIT_INFO_LAYER:SortingLayer = new SortingLayer("UNIT_INFO_LAYER",8);
      
      public static const POINTERS_LAYER:SortingLayer = new SortingLayer("POINTERS_LAYER",9,true);
      
      public static const TEXT_EFFECT_LAYER:SortingLayer = new SortingLayer("TEXT_EFFECT_LAYER",10,false,false);
      
      private static var _layers:Array = [WEATHER_LAYER,GRAVE_LAYER,DROPPED_HAT_LAYER,UNIT_LAYER,AMMO_LAYER,EFFECT_LAYER,CLOUDS_LAYER,WATER_LAYER,UNIT_INFO_LAYER,POINTERS_LAYER,TEXT_EFFECT_LAYER];
      
      private var _name:String;
      
      private var _index:int;
      
      private var _touchable:Boolean;
      
      private var _updateVisibility:Boolean;
      
      public function SortingLayer(param1:String, param2:int, param3:Boolean = false, param4:Boolean = true)
      {
         super();
         this._name = param1;
         this._index = param2;
         this._touchable = param3;
         this._updateVisibility = param4;
      }
      
      public static function getAllLayers() : Array
      {
         return _layers;
      }
      
      public function get name() : String
      {
         return this._name;
      }
      
      public function get index() : int
      {
         return this._index;
      }
      
      public function get touchable() : Boolean
      {
         return this._touchable;
      }
      
      public function get updateVisibility() : Boolean
      {
         return this._updateVisibility;
      }
   }
}

