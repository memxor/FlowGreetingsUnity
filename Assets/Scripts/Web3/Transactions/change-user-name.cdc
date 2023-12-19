import Profile from 0xb67c73ff54a4c730;

transaction(name: String)
{
  prepare(acct: AuthAccount) 
  {
    let userInfo = acct.borrow<&Profile.UserProfile>(from: Profile.storageProfileStoragePath) ?? panic("Can't borrow the file from storage!");
    userInfo.updateUserName(name);
  }
}