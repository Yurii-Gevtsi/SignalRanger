package ru.pragmatix.wormix.scene
{
   public class ObjectType
   {
      
      public static const GAME_OBJECT:ObjectType = new ObjectType(0);
      
      public static const BACK_ENVIRONMENT:ObjectType = new ObjectType(1);
      
      public static const FRONT_ENVIRONMENT:ObjectType = new ObjectType(2);
      
      private var _id:int;
      
      public function ObjectType(param1:int)
      {
         super();
         this._id = param1;
      }
   }
}

