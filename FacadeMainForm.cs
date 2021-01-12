using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;

namespace FacebookAppForm
{
    class FacadeMainForm
    {
        FacebookUserUtiles m_FacebookUserUtiles = FacebookUserUtiles.GetInstance;
        private IEnumerable<User> m_FriendList;

        public event Action FriendListHandler;


        public FacadeMainForm()
        {

        }

        public IEnumerable<User> FriendList
        {
            get
            {
                return m_FriendList;
            }
        }

        internal string FetchProfilePicture()
        {
            return m_FacebookUserUtiles.FetchPhotoProfilePictureUrl();
        }

        internal Album FetchCoverPhoto(string i_coverPhotoHebrew)
        {
            return m_FacebookUserUtiles.FetchAlbumByName(i_coverPhotoHebrew);
        }

        internal void FetchFriends()
        {
            m_FriendList = m_FacebookUserUtiles.FetchAllFriendList();
            OnFriendListReady();
        }

        protected virtual void OnFriendListReady()
        {
            if (FriendListHandler != null)
            {
                FriendListHandler.Invoke();
            }
        }

    }
}
