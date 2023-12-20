import HelloWorld from 0x0687bd376fdc0bbf;

transaction(greeting: String) {
  prepare(signer: AuthAccount) {

  }

  execute {
    HelloWorld.changeGreeting(newGreeting: greeting);
  }
}