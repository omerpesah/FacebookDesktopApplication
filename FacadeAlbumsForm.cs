using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;

namespace FacebookAppForm
{
    class FacadeAlbumsForm
    {
        FacebookUserUtiles m_FacebookUserUtiles = FacebookUserUtiles.GetInstance;
        private IEnumerable<Album> m_Albums = null;

        public event Action AlbumsHandler = null;

        public FacadeAlbumsForm()
        {

        }

        public IEnumerable<Album> Albums
        {
            get
            {
                return m_Albums;
            }
        }
        internal void FetchUserAlbum()
        {
            m_Albums = m_FacebookUserUtiles.FetchUserAlbum();
            OnAlbumsReady();
        }

        internal Album FetchAlbumByName(string i_AlbumToFetch)
        {
            return m_FacebookUserUtiles.FetchAlbumByName(i_AlbumToFetch);
        }

        internal string FetchPhotoUrl(Album i_CurrAlbum, string i_UrlToShow)
        {
            return m_FacebookUserUtiles.FetchPhotoUrl(i_CurrAlbum, i_UrlToShow);
        }

        protected virtual void OnAlbumsReady()
        {
            if (AlbumsHandler != null)
            {
                AlbumsHandler.Invoke();
            }
        }
    }
}
