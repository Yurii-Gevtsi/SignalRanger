package ru.pragmatix.flox.social.lang
{
   public class EnLangController extends AbstractLangController
   {
      
      public function EnLangController()
      {
         super();
      }
      
      override protected function getPluralIndex(param1:int) : int
      {
         return param1 == 1 ? 0 : 1;
      }
   }
}

