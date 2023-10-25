using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KafeteryaMenu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int MMove;
        int Mouse_X;
        int Mouse_Y;
        private void Form1_Load(object sender, EventArgs e)
        {
            Yenile();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            MMove = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            MMove = 0;
        }

        static string ConString =
            "Data Source=DESKTOP-9578CDS\\SYTRASANDRO;" +
            "Initial Catalog=Kafeterya;Integrated Security=True";
        SqlConnection Connect = new SqlConnection(ConString);

        SqlDataAdapter Da;
        DataSet Ds;
        DataTable Dt; 

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string fInc = "";
                string fName = "";

                fInc = radioButton1.Checked ? radioButton1.Text :
                          radioButton2.Checked ? radioButton2.Text :
                          radioButton3.Checked ? radioButton3.Text :
                          radioButton4.Checked ? radioButton4.Text :
                          radioButton5.Checked ? radioButton5.Text : "";

                fName = textBox1.Text;

                if (fInc != "" && fName != "")
                {
                    string Sorgu =
                        "SELECT COUNT(*) FROM " +
                        "KafeteryaMenu WHERE FoodName = @fName";
                    SqlCommand CmdSorgu = new SqlCommand(Sorgu, Connect);
                    CmdSorgu.Parameters.Add("@fName",
                        SqlDbType.VarChar).Value = fName;

                    Connect.Open();
                    int SorguSonucu = (int)CmdSorgu.ExecuteScalar();
                    Connect.Close();
                    
                    if (SorguSonucu == 0)
                    {
                        string Kayit =
                            "INSERT INTO " +
                            "KafeteryaMenu (FoodIncentives, FoodName) " +
                            "VALUES (@fInc, @FoodName)";
                        SqlCommand CmdKayit = new SqlCommand(Kayit, Connect);

                        CmdKayit.Parameters.Add("@fInc",
                            SqlDbType.VarChar).Value = fInc;
                        CmdKayit.Parameters.Add("@FoodName",
                            SqlDbType.VarChar).Value = fName;

                        Connect.Open();
                        CmdKayit.ExecuteNonQuery();
                        Connect.Close();
                        
                        MessageBox.Show(fInc + " | " + fName + 
                            " Menüye kaydedilmiştir.", "Bilgi");
                        Sil();
                    }
                    else
                    {
                        string Read = "SELECT * FROM KafeteryaMenu " +
                            "Where FoodName = @fName";
                        
                        SqlCommand ComReader = new SqlCommand(Read, Connect);
                        ComReader.Parameters.Add("@fName",
                            SqlDbType.VarChar).Value = fName;

                        Connect.Open();
                        SqlDataReader DataReader = ComReader.ExecuteReader();
                        DataReader.Read();
                        MessageBox.Show(DataReader["FoodIncentives"] + " | " + fName + " zaten mevcut.");
                        DataReader.Close();
                        Connect.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Eksik bilgi sebebiyle kaydedilmemiştir.", "Bilgi");
                }
            }
            catch (Exception Hata)
            {
                MessageBox.Show("Bir hatayla karşılaştınız;" + Hata.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string fInc = "";
            string fName = "";

            fInc = radioButton1.Checked ? radioButton1.Text :
                      radioButton2.Checked ? radioButton2.Text :
                      radioButton3.Checked ? radioButton3.Text :
                      radioButton4.Checked ? radioButton4.Text :
                      radioButton5.Checked ? radioButton5.Text : "";

            fName = textBox2.Text;

            string Sorgu = "SELECT COUNT(*) FROM KafeteryaMenu WHERE FoodName = @fName";
            SqlCommand CmdSorgu = new SqlCommand(Sorgu, Connect);
            CmdSorgu.Parameters.Add("@fName",
                SqlDbType.VarChar).Value = fName;

            Connect.Open();
            int SorguSonucu = (int)CmdSorgu.ExecuteScalar();
            Connect.Close();

            if (SorguSonucu == 1)
            {
                string Sorgu1 = "SELECT COUNT(*) FROM KafeteryaMenu " +
                    "WHERE FoodName = @fName1 AND Foodıncentives = @fInc1";
                SqlCommand CmdSorgu1 = new SqlCommand(Sorgu1, Connect);
                CmdSorgu1.Parameters.Add("@fName1",
                    SqlDbType.VarChar).Value = fName;
                CmdSorgu1.Parameters.Add("@fInc1",
                    SqlDbType.VarChar).Value = fInc;

                Connect.Open();
                int SorguSonucu1 = (int)CmdSorgu1.ExecuteScalar();
                Connect.Close();

                if (SorguSonucu1 == 0)
                {
                    string Update = "UPDATE kafeteryaMenu set FoodIncentives = @fInc " +
                    "WHERE FoodName = @fName";
                    SqlCommand CmdGuncelle = new SqlCommand(Update, Connect);

                    CmdGuncelle.Parameters.AddWithValue("@fInc", fInc);
                    CmdGuncelle.Parameters.AddWithValue("@fName", fName);

                    Connect.Open();
                    CmdGuncelle.ExecuteNonQuery();
                    Connect.Close();

                    string Read = "SELECT * FROM KafeteryaMenu " +
                            "Where FoodName = @fName";

                    SqlCommand ComReader = new SqlCommand(Read, Connect);
                    ComReader.Parameters.Add("@fName",
                        SqlDbType.VarChar).Value = fName;

                    Connect.Open();
                    SqlDataReader DataReader = ComReader.ExecuteReader();
                    DataReader.Read();
                    MessageBox.Show(DataReader["FoodIncentives"] + " | " + fName + " Güncellenmiştir.");
                    DataReader.Close();
                    Connect.Close();

                    Sil();
                }
                else
                {
                    MessageBox.Show(fInc + " | " + fName + " Zaten doğru şekilde kaydedilmiştir");
                }
            }
            else
            {
                MessageBox.Show("Kayıt Bulunamamıştır");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string fInc = "";
            string fName = "";

            fInc = radioButton1.Checked ? radioButton1.Text :
                      radioButton2.Checked ? radioButton2.Text :
                      radioButton3.Checked ? radioButton3.Text :
                      radioButton4.Checked ? radioButton4.Text :
                      radioButton5.Checked ? radioButton5.Text : "";

            fName = textBox2.Text;

            if (dataGridView1.Rows.Count == 1 && dataGridView1.Rows[0].Cells[1].Value != null)
            {
                string Delete = "DELETE FROM KafeteryaMenu " +
                    "WHERE FoodName = @fName";

                SqlCommand ComSil = new SqlCommand(Delete, Connect);
                ComSil.Parameters.AddWithValue("@fName", 
                    dataGridView1.Rows[0].Cells[1].Value.ToString());

                if (MessageBox.Show(dataGridView1.Rows[0].Cells[0].Value + " | " +
                    dataGridView1.Rows[0].Cells[1].Value + 
                    " Silmek istediğinize emin misiniz?".ToString(),
                    "Silme", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    Connect.Open();
                    int EtkilenenSatirSayisi = ComSil.ExecuteNonQuery();
                    Connect.Close();
                    if (EtkilenenSatirSayisi > 0)
                    {
                        MessageBox.Show(dataGridView1.Rows[0].Cells[1].Value.ToString() + " Silinmiştir.");

                        Sil();
                    }
                    else
                    {
                        MessageBox.Show("Silme işlemi iptal edilmiştir.");
                    }
                }
                else
                {
                    MessageBox.Show(dataGridView1.Rows[0].Cells[1].Value.ToString() + 
                        " Silinemedi. Belirtilen kayıt bulunamadı veya bir hata oluştu.");
                }
            }
            else
            {
                MessageBox.Show("Sadece 0. indeks gözüküyor, bu nedenle silemezsiniz.");
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text == "Yiyecek adı" ||
                textBox1.Text == "İçecek adı" ? "" : textBox1.Text;

            textBox1.ForeColor = textBox1.Text != "İçecek adı" ||
                textBox1.Text != "Yiyecek adı" ? Color.Black : Color.DimGray;
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text == "" ? textBox1.Text =
                radioButton1.Checked ? "İçecek adı" : "Yiyecek adı" : textBox1.Text;

            textBox1.ForeColor = textBox1.Text == "İçecek adı" ||
                textBox1.Text == "Yiyecek adı" ? Color.DimGray : Color.Black;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = textBox1.Text.First().ToString().ToUpper() +
                    textBox1.Text.Substring(1).ToLower();

                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Text = textBox2.Text.First().ToString().ToUpper() +
                    textBox2.Text.Substring(1).ToLower();

                textBox2.SelectionStart = textBox2.Text.Length;
            }

            DataView Dv = Dt.DefaultView;

            string filterExpression = string.Format("FoodName LIKE '{0}%'", textBox2.Text);
            Dv.RowFilter = filterExpression;

            dataGridView1.DataSource = Dv;
        }
        
        private void textBox2_Enter(object sender, EventArgs e)
        {
            label1.Visible = textBox2.Text == "" ? false : false;
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {

            label1.Visible = textBox2.Text == "" ? true : false;
        }

        private void radioButtonAll_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = radioButton1.Checked || radioButton2.Checked ||
                radioButton3.Checked || radioButton4.Checked ||
                radioButton5.Checked ? true : false;

            textBox1.Text = radioButton1.Checked ?
                "İçecek adı" : "Yiyecek adı";

            textBox1.ForeColor = Color.DimGray;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Kapatmak istediğinize emin misiniz?",
                "Kapatma onayı", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            { Close(); }
        }

        public void Yenile()
        {
            Da = new SqlDataAdapter("Select * From KafeteryaMenu", Connect);
            Connect.Open();
            Dt = new DataTable();
            Da.Fill(Dt);
            dataGridView1.DataSource = Dt;
            Connect.Close();
        }

        public void Sil()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;

            textBox1.Text = "Lütfen tür seçiniz";
            textBox1.ForeColor = Color.Gray;
            textBox1.Enabled = false;

            textBox2.Text = "";

            label1.Visible = true;

            ActiveControl = null;

            Yenile();
        }
    }
}
