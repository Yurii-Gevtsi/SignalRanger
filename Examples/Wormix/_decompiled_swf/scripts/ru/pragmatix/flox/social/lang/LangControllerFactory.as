package ru.pragmatix.flox.social.lang
{
   import §_-CF§.§_-p2U§;
   
   public class LangControllerFactory
   {
      
      public function LangControllerFactory()
      {
         super();
      }
      
      public static function create(param1:§_-p2U§) : ILangController
      {
         switch(param1)
         {
            case §_-p2U§.§_-qr§:
               return new EnLangController();
            case §_-p2U§.§_-w13§:
               return new RuLangController();
            default:
               return new EnLangController();
         }
      }
   }
}

