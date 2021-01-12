using Facebook;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FacebookAppForm
{
    public partial class CoronaForm : FacebookGeneralForm
    {
        private FacadeCoronaForm m_FacadeCoronaForm = new FacadeCoronaForm();
        private List<User> m_FriendListWhoWasWithMeInPast2Weeks;

        public CoronaForm() :
            base()
        {
            InitializeComponent();
        }

        public override void InitializeForm()
        {

        }

        private void friendShowReportButton_Click(object sender, EventArgs e)
        {
            m_FriendListWhoWasWithMeInPast2Weeks = new List<User>();
            try
            {
                m_FriendListWhoWasWithMeInPast2Weeks.AddRange(m_FacadeCoronaForm.FoundedPeople());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void moveToList_Click(object sender, EventArgs e)
        {

            foreach (String s in m_CheckListBoxFriendOfCovid.CheckedItems)
            {
                m_ListBoxCoronaSend.Invoke(new Action(() => m_ListBoxCoronaSend.Items.Add(s)));
            }
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            List<User> friendLists = new List<User>();
            foreach (User friend in m_FacadeCoronaForm.FetchFriends())
            {
                friendLists.Add(friend);
            }

            StringBuilder builder = new StringBuilder();
            foreach (String s in m_ListBoxCoronaSend.Items)
            {
                foreach (User user in friendLists)
                {
                    if (s.Equals(user.Name))
                    {
                        builder.AppendLine(string.Format("Name : {0}  Email : {1} ", user.Name, user.Email));
                    }
                }
            }

            MessageBox.Show(builder.ToString());
        }

        private void CoronaForm_Load(object sender, EventArgs e)
        {

        }
    }
}
