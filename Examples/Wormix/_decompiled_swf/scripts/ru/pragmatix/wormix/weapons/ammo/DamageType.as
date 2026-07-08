package ru.pragmatix.wormix.weapons.ammo
{
   public class DamageType
   {
      
      public static const PHYSICAL:DamageType = new DamageType("physical");
      
      public static const AIR:DamageType = new DamageType("air");
      
      public static const DRILL:DamageType = new DamageType("drill");
      
      public static const FIRE:DamageType = new DamageType("fire");
      
      public static const SHOCK:DamageType = new DamageType("shock");
      
      public static const COLD:DamageType = new DamageType("ice");
      
      public static const POISON:DamageType = new DamageType("poison");
      
      public static const CHEMICAL:DamageType = new DamageType("chemical");
      
      private var type:String;
      
      public function DamageType(param1:String)
      {
         super();
         this.type = param1;
      }
      
      public function toString() : String
      {
         return this.type;
      }
   }
}

