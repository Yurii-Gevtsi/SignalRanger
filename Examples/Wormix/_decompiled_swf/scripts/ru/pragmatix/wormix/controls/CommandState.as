package ru.pragmatix.wormix.controls
{
   public class CommandState
   {
      
      public static const BEGIN:CommandState = new CommandState("BEGIN");
      
      public static const END:CommandState = new CommandState("END");
      
      public var name:String;
      
      public function CommandState(param1:String)
      {
         super();
         this.name = param1;
      }
   }
}

