import Greetings from 0x0687bd376fdc0bbf;

transaction(greeting: String)
{
  prepare(acct: AuthAccount) 
  {
    let newUserProfile <- Greetings.createGreeting(greeting);
    acct.save(<- newUserProfile, to: Greetings.storageGreetingsStoragePath);
    acct.link<&Greetings.Greeting{Greetings.IGreetingPublic}>(Greetings.publicGreetingsStoragePath, target: Greetings.storageGreetingsStoragePath);
  }
}