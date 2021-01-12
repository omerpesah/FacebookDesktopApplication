using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FacebookAppForm
{
    class FacadeTopFanForm
    {
        FacebookUserUtiles m_FacebookUserUtiles = FacebookUserUtiles.GetInstance;

        public Func<IDictionary<User,int>, User, int, bool> Test { get; set; }
        public Func<IEnumerable<PostedItem>, List<User>> Choise { get; set; }

        public FacadeTopFanForm()
        {

        }

        public User GetResultFromTheTest(Dictionary<User, int> i_Collection)
        {
            int maxCounter = int.MinValue;
            User UserToReturn = null;

            foreach (User UserVar in i_Collection.Keys)
            {
                if (Test.Invoke(i_Collection, UserVar, maxCounter))
                {
                    UserToReturn = UserVar;
                    maxCounter = i_Collection[UserVar];
                }
            }

            return UserToReturn;
        }

        internal Dictionary<User, int> CalcPostedItemsDict(IEnumerable<PostedItem> i_collection)
        {
           return m_FacebookUserUtiles.UpdateLikesToDict(Choise.Invoke(i_collection), new Dictionary<User, int>());
        }

        internal IEnumerable<PostedItem> FetchAllPosts()
        {
            return m_FacebookUserUtiles.FetchUserPosts();
        }

        internal ICollection<PostedItem> FetchAllPhotos()
        {
            List<Photo> photosToReturn = new List<Photo>();
            foreach(Album album in m_FacebookUserUtiles.FetchUserAlbum())
            {
                    photosToReturn.AddRange(album.Photos);
            }
            return photosToReturn as ICollection<PostedItem>;
        }
    }
}
