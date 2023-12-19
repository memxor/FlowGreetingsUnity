import Profile from 0xb67c73ff54a4c730;

pub fun main(add: Address): Profile.UserProfileInfo
{
  let publicCap = getAccount(add).getCapability(Profile.publicProfileStoragePath).borrow<&Profile.UserProfile{Profile.IUserProfilePublic}>() ?? panic("Can't find public path!");
  return publicCap.getUserProfileInfo();
}