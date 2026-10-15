package ru.pragmatix.flox.social.lang
{
   public class RuLangController extends AbstractLangController
   {
      
      public function RuLangController()
      {
         super();
      }
      
      override protected function getPluralIndex(param1:int) : int
      {
         return param1 % 10 == 1 && param1 % 100 != 11 ? 0 : (param1 % 10 >= 2 && param1 % 10 <= 4 && (param1 % 100 < 10 || param1 % 100 >= 20) ? 1 : 2);
      }
   }
}

