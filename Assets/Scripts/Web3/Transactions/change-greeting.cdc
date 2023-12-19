import Greetings from 0x0687bd376fdc0bbf;

transaction(greeting: String)
{
  prepare(acct: AuthAccount) 
  {
    let userInfo = acct.borrow<&Greetings.Greeting>(from: Greetings.storageGreetingsStoragePath) ?? panic("Can't borrow the file from storage!");
    userInfo.updateGreeting(greeting);
  }
}