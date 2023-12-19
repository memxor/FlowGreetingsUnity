pub contract Greetings
{
    access(contract) var totalGreetingsCount: UInt64;

    pub var publicGreetingsStoragePath: PublicPath;
    pub var storageGreetingsStoragePath: StoragePath;

    pub resource interface IGreetingPublic
    {
        pub let greetingNumber: UInt64;
        pub var greeting: String;
        
        pub fun getGreeting(): GreetingInfo;
    }

    pub resource Greeting : IGreetingPublic
    {
        pub let greetingNumber: UInt64;
        pub var greeting: String;

        pub fun getGreeting(): GreetingInfo
        {
            return GreetingInfo(self.greetingNumber, self.greeting);
        }

        pub fun updateGreeting(_ greeting: String)
        {
            self.greeting = greeting;
        }

        init(_ greetingNumber: UInt64, _ greeting: String)
        {
            self.greetingNumber = greetingNumber;
            self.greeting = greeting;
        }
    }

    pub fun createGreeting(_ greeting: String): @Greeting
    {
        let newGreeting <- create Greeting(self.totalGreetingsCount, greeting);
        self.totalGreetingsCount = self.totalGreetingsCount + 1;
        return <- newGreeting;
    }

    pub struct GreetingInfo
    {
        pub let greetingNumber: UInt64;
        pub let greeting: String;

        init(_ greetingNumber: UInt64, _ greeting: String)
        {
            self.greetingNumber = greetingNumber;
            self.greeting = greeting;
        }
    }

    init()
    {
        self.totalGreetingsCount = 0;

        self.publicGreetingsStoragePath = /public/greeting;
        self.storageGreetingsStoragePath = /storage/greeting;
    }
}