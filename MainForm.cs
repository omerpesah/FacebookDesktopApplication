using FacebookWrapper.ObjectModel;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Threading;

namespace FacebookAppForm
{
    public partial class MainForm : FacebookGeneralForm
    {
        private FacadeMainForm m_FacadeMainForm = new FacadeMainForm();
        private const string coverPhotoHebrew = "תמונות נושא";
        private const string coverPhotoEnglish = "Cover Photos";
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            if (!LoginForm.s_LoginSec)
            {
                changeForm(typeof(LoginForm));
                if (LoginForm.s_LoginSec)
                {
                    Thread threadBasicInfo = new Thread(() =>
                    {

                        loadBasicDetails();

                    });
                    threadBasicInfo.Start();
                }
                else
                {
                    this.Dispose();
                }
            }
        }

        private void loadBasicDetails()
        {
            profilePicture.LoadAsync(m_FacadeMainForm.FetchProfilePicture());
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, profilePicture.Width - 3, profilePicture.Height - 3);
            Region rg = new Region(gp);
            profilePicture.Invoke(new Action(() => profilePicture.Region = rg));

            try
            {
                if (m_FacadeMainForm.FetchCoverPhoto(coverPhotoHebrew) != null)
                {
                    Album album = m_FacadeMainForm.FetchCoverPhoto(coverPhotoHebrew);
                    m_CoverPhoto.LoadAsync(album.Photos[0].PictureNormalURL);
                }
                else if (m_FacadeMainForm.FetchCoverPhoto(coverPhotoEnglish) != null)
                {
                    Album album = m_FacadeMainForm.FetchCoverPhoto(coverPhotoEnglish);
                    m_CoverPhoto.LoadAsync(album.Photos[0].PictureNormalURL);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                initFriendList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load, please check app access permissions");
                Application.Exit();
            }
        }

        private void initFriendList()
        {
            m_FacadeMainForm.FriendListHandler += initDataBinding;
            Thread threadFetchFriendList = new Thread(() => m_FacadeMainForm.FetchFriends());
            threadFetchFriendList.Start();
        }

        private void initDataBinding()
        {
            this.Invoke(new Action(() => friendsBindingSource.DataSource = m_FacadeMainForm.FriendList));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void changeForm(Type i_AppForm)
        {
            FacebookGeneralForm facebookGeneralForm = FactoryFacebookForm.CreateInstance(i_AppForm);
            if (facebookGeneralForm != null)
            {
                facebookGeneralForm.InitializeForm();
                this.Visible = false;
                facebookGeneralForm.StartPosition = FormStartPosition.CenterParent;
                facebookGeneralForm.ShowDialog();
                this.Visible = true;
            }
            else
            {
                MessageBox.Show("Upload Form EEROR!");
            }

        }

        private void postButton_Click(object sender, EventArgs e)
        {
            changeForm(typeof(PostForm));
        }

        private void eventButton_Click(object sender, EventArgs e)
        {
            changeForm(typeof(EventForm));
        }

        private void albumsButton_Click(object sender, EventArgs e)
        {
            changeForm(typeof(AlbumsForm));
        }

        private void aboutMeButton_Click(object sender, EventArgs e)
        {
            changeForm(typeof(AboutMeForm));
        }


        private void coronaFeature_Click(object sender, EventArgs e)
        {
            changeForm(typeof(CoronaForm));
        }

        private void topFanFeruteButton_Click(object sender, EventArgs e)
        {
            changeForm(typeof(TopFanForm));
        }

        private void friendListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_FacadeMainForm.FriendList != null)
            {
                foreach (User user in m_FacadeMainForm.FriendList)
                {
                    if (user.Equals(m_FriendListBox.SelectedItem))
                    {
                        Thread threadFetchFriendPic = new Thread(() => m_PictureBoxFriend.LoadAsync(user.PictureNormalURL));
                        threadFetchFriendPic.Start();
                    }
                }
            }
        }

        private void nameLabel_Click(object sender, EventArgs e)
        {

        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
