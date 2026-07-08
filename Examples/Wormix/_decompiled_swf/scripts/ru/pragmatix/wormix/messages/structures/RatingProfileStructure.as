package ru.pragmatix.wormix.messages.structures
{
   import feathers.core.ITextRenderer;
   
   public class RatingProfileStructure extends SimpleProfileStructure
   {
      
      public var dailyRating:int;
      
      public var oldPlace:int;
      
      public function RatingProfileStructure()
      {
         super();
      }
   }
}

