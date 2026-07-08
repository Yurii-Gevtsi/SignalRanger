package ru.pragmatix.flox.social.lang
{
   import ru.pragmatix.flox.social.model.IUserProfile;
   import starling.events.Event;
   
   public class AbstractLangController implements ILangController
   {
      
      public function AbstractLangController()
      {
         super();
      }
      
      public function getDisplayName(param1:IUserProfile) : String
      {
         var _loc2_:String = param1.getSocialData().getFirstName();
         var _loc3_:String = param1.getSocialData().getLastName();
         return (_loc2_ ? _loc2_ : "") + " " + (_loc3_ ? _loc3_ : "");
      }
      
      public function getPluralForm(param1:int, param2:Array) : String
      {
         if(param2.length == 0)
         {
            return "";
         }
         var _loc3_:int = this.getPluralIndex(param1);
         return _loc3_ < param2.length ? param2[_loc3_] : param2[0];
      }
      
      protected function getPluralIndex(param1:int) : int
      {
         return 0;
      }
   }
}

