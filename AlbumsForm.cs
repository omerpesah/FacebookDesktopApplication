using FacebookWrapper.ObjectModel;
using System;
using System.Threading;

namespace FacebookAppForm
{
    public partial class AlbumsForm : FacebookGeneralForm
    {
        private FacadeAlbumsForm m_FacadeAlbumsForm = new FacadeAlbumsForm();

        public AlbumsForm()
            : base()
        {
            InitializeComponent();
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            m_FacadeAlbumsForm.AlbumsHandler += dataReady;
            Thread threadFetchData = new Thread(() => m_FacadeAlbumsForm.FetchUserAlbum());
            threadFetchData.Start();
        }

        private void dataReady()
        {
            this.Invoke(new Action(addDataBinding));
        }

        private void addDataBinding()
        {
            albumBindingSource.DataSource = m_FacadeAlbumsForm.Albums;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void albumListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            photoBindingSource.DataSource = (albumBindingSource.Current as Album).Photos;
        }

        private void albumBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void albumBindingNavigator_RefreshItems(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void AlbumsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
