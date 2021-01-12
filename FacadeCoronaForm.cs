using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacebookAppForm
{
    class FacadeCoronaForm
    {
        FacebookUserUtiles m_FacebookUserUtiles = FacebookUserUtiles.GetInstance;

        public FacadeCoronaForm()
        {

        }

        public IEnumerable<User> FoundedPeople()
        {
            List<User> friendListWhoWasWithMeInPast2Weeks = new List<User>();
            int amountOfPeoplePhoto = FetchAllUsersAreTaggedWithMeInPhoto().Count();
            int amountOfPeopleCheckIn = FetchAllUsersAreWasWithMeInSameCheckins().Count();

            foreach (Photo photo in FetchAllUsersAreTaggedWithMeInPhoto())
            {
                if (amountOfPeoplePhoto > 0)
                {
                    if (photo.CreatedTime > DateTime.Now.AddDays(-14))
                    {
                        foreach (PhotoTag photoTag in photo.Tags)
                        {
                            yield return photoTag.User;
                            amountOfPeoplePhoto--;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            foreach (User user in FetchAllUsersAreWasWithMeInSameCheckins())
            {
                if (amountOfPeopleCheckIn > 0)
                {
                    yield return user;
                    amountOfPeopleCheckIn--;
                }
                else
                {
                    break;
                }
            }
        }
    

        internal IEnumerable<Photo> FetchAllUsersAreTaggedWithMeInPhoto()
        {
            return m_FacebookUserUtiles.FetchAllUsersAreTaggedWithMeInPhoto();
        }

        internal IEnumerable<User> FetchAllUsersAreWasWithMeInSameCheckins()
        {
            return m_FacebookUserUtiles.FetchAllUsersAreWasWithMeInSameCheckins();
        }

        internal FacebookObjectCollection<User> FetchFriends()
        {
            return m_FacebookUserUtiles.User.Friends;
        }
    }
}
