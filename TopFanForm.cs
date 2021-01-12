using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace FacebookAppForm
{
    public partial class TopFanForm : FacebookGeneralForm
    {
        private FacadeTopFanForm m_FacadeTopFanForm = new FacadeTopFanForm();
        internal Dictionary<User, int> m_PhotoLikeCounter;
        internal Dictionary<User, int> m_PostsLikeCounter;
        internal Dictionary<User, int> m_GeneralTopFanDict;
        private bool m_AllAges;
        private User.eGender m_genderFilter;
        private bool m_AllGenderSelected;
        private int m_MinAge;
        private int m_MaxAge;
        private List<PostedItem> m_GeneralCollection;
        private User m_PostItem;

        public TopFanForm()
        {
            InitializeComponent();
            m_FacadeTopFanForm.Choise = (postedItems) =>
            {
                List<User> users = new List<User>();
                foreach(PostedItem item in postedItems)
                {
                    foreach(User user in item.LikedBy)
                    {
                        users.Add(user);
                    }
                }
                return users;
            };
            m_FacadeTopFanForm.Test = (dictionary, user, maxCounter) => dictionary[user] > maxCounter;
        }

        private void m_CloseButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void m_PhotoFanButton_Click(object sender, EventArgs e)
        {
            if (m_PhotoLikeCounter == null)
            {
                m_PhotoLikeCounter = new Dictionary<User, int>();
            }
            try
            {
                applyFilters();
                m_PhotoLikeCounter = m_FacadeTopFanForm.CalcPostedItemsDict(m_FacadeTopFanForm.FetchAllPhotos());
                initInformation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void initInformation()
        {
            this.Invoke(new Action(() => m_FansListBox.Items.Clear()));
            Thread threadFetchData = new Thread(new ThreadStart(fetchData));
            threadFetchData.Start();
        }
        private void fetchData()
        {
            m_PostItem = m_FacadeTopFanForm.GetResultFromTheTest(m_PhotoLikeCounter);
            m_FansListBox.Invoke(new Action(() => m_FansListBox.Items.Add(string.Format("{0} {1}", m_PostItem.Name, m_PhotoLikeCounter[m_PostItem]))));
        }

        private void m_PostFanButton_Click(object sender, EventArgs e)
        {
            if (m_PostsLikeCounter == null)
            {
                m_PostsLikeCounter = new Dictionary<User, int>();
            }
            try
            {
                applyFilters();
                m_PostsLikeCounter = m_FacadeTopFanForm.CalcPostedItemsDict(m_FacadeTopFanForm.FetchAllPosts());
                initInformation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void applyFilters()
        {
            m_AllAges = m_AllAgesCheckBox.Checked;
            if (!m_AllAges)
            {

                m_MinAge = (int)m_MinAgeUpDown.Value;
                m_MaxAge = (int)m_MaxAgeUpDown.Value;
                if (m_MinAge > m_MaxAge)
                {
                    throw new Exception("Wrong age range");
                }
            }
            if (m_GenderComboBox.SelectedItem != null)
            {
                if (!m_GenderComboBox.SelectedItem.Equals("All"))
                {
                    m_genderFilter = (User.eGender)Enum.Parse(typeof(User.eGender), (string)m_GenderComboBox.SelectedItem);
                }
                else
                {
                    m_AllGenderSelected = true;
                }
            }
            else
            {
                throw new Exception("Please select gender filter from the list!");
            }
        }

        private void m_GeneralFanButton_Click(object sender, EventArgs e)
        {
            if (m_GeneralTopFanDict == null)
            {
                m_GeneralTopFanDict = new Dictionary<User, int>();
            }
            try
            {
                applyFilters();
                m_GeneralCollection.AddRange(m_FacadeTopFanForm.FetchAllPhotos());
                m_GeneralCollection.AddRange(m_FacadeTopFanForm.FetchAllPosts());
                m_GeneralTopFanDict = m_FacadeTopFanForm.CalcPostedItemsDict(m_GeneralCollection);
                initInformation();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_AllAgesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool checkBoxResult = true;
            if (!m_AllAgesCheckBox.Checked)
            {
                checkBoxResult = true;
            }
            else
            {
                checkBoxResult = false;
            }
            foreach (Control control in m_AgeGroupBox.Controls)
            {
                control.Enabled = checkBoxResult;
            }
        }

        private int checkUserAge(User i_User)
        {
            DateTime today = DateTime.Today;
            DateTime birthday = DateTime.Parse(i_User.Birthday);
            int age = today.Year - birthday.Year;

            return age;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void m_LikeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_FacadeTopFanForm.Choise = (postedItems) =>
            {
                List<User> users = new List<User>();
                foreach (PostedItem item in postedItems)
                {
                    foreach (User user in item.LikedBy)
                    {
                        users.Add(user);
                    }
                }
                return users;
            };
        }

        private void m_CommentRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_FacadeTopFanForm.Choise = (postedItems) =>
            {
                List<User> users = new List<User>();
                foreach (PostedItem item in postedItems)
                {
                    foreach (Comment comment in item.Comments)
                    {
                        users.Add(comment.From);
                    }
                }
                if(users.Count == 0)
                {
                    throw new Exception("There are No comments for all post");
                }
                return users;
            };
        }
    }
}
