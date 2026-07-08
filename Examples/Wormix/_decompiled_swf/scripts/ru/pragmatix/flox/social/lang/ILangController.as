package ru.pragmatix.flox.social.lang
{
   import ru.pragmatix.flox.social.model.IUserProfile;
   
   public interface ILangController
   {
      
      function getDisplayName(param1:IUserProfile) : String;
      
      function getPluralForm(param1:int, param2:Array) : String;
   }
}

