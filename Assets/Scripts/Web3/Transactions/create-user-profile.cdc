import Profile from 0xb67c73ff54a4c730;

transaction(name: String)
{
  prepare(acct: AuthAccount) 
  {
    let newUserProfile <- Profile.createUserProfile(name, acct.address.toString());
    acct.save(<- newUserProfile, to: Profile.storageProfileStoragePath);
    acct.link<&Profile.UserProfile{Profile.IUserProfilePublic}>(Profile.publicProfileStoragePath, target: Profile.storageProfileStoragePath);
  }
}