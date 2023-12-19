import Greetings from 0x0687bd376fdc0bbf;

pub fun main(add: Address): Greetings.GreetingInfo
{
  let publicCap = getAccount(add).getCapability(Greetings.publicGreetingsStoragePath).borrow<&Greetings.Greeting{Greetings.IGreetingPublic}>() ?? panic("Can't find public path!");
  return publicCap.getGreeting();
}