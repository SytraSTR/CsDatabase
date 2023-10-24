using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        static string ConString =
            "Data Source=DESKTOP-5J370CT\\SYTRA;" +
            "Initial Catalog=Kafeterya;Integrated Security=True";
        SqlConnection Baglanti = new SqlConnection(ConString);

        SqlDataAdapter Da = new SqlDataAdapter();
        DataSet Ds = new DataSet();

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
                    SqlCommand Command = new SqlCommand(Sorgu, Baglanti);
                    Command.Parameters.Add("@fName",
                        SqlDbType.VarChar).Value = fName;

                    Baglanti.Open();
                    int SorguSonucu = (int)Command.ExecuteScalar();
                    Baglanti.Close();

                    if (SorguSonucu == 0)
                    {
                        string Kayit =
                            "INSERT INTO " +
                            "KafeteryaMenu (FoodIncentives, FoodName) " +
                            "VALUES (@fInc, @FoodName)";
                        SqlCommand Command1 = new SqlCommand(Kayit, Baglanti);

                        Command1.Parameters.Add("@fInc",
                            SqlDbType.VarChar).Value = fInc;
                        Command1.Parameters.Add("@FoodName",
                            SqlDbType.VarChar).Value = fName;

                        Baglanti.Open();
                        Command1.ExecuteNonQuery();
                        Baglanti.Close();

                        MessageBox.Show("Menü kaydedilmiştir.", "Bilgi");
                        Sil();
                    }
                    else
                    {
                        MessageBox.Show(textBox1.Text + " zaten mevcut.");
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

            fName = textBox1.Text;
            string Sorgu = "SELECT COUNT(*) FROM KafeteryaMenu WHERE FoodName = @fName";
            SqlCommand Command = new SqlCommand(Sorgu, Baglanti);
            Command.Parameters.Add("@fName",
                SqlDbType.VarChar).Value = fName;

            Baglanti.Open();
            int SorguSonucu = (int)Command.ExecuteScalar();
            Baglanti.Close();

            if (SorguSonucu == 1)
            {
                string Update = "UPDATE kafeteryaMenu set FoodIncentives=@fInc " +
                    "WHERE FoodName=@fName";
                SqlCommand komut = new SqlCommand(Update, Baglanti);

                komut.Parameters.AddWithValue("@fInc", fInc);
                komut.Parameters.AddWithValue("@fName", fName);

                Baglanti.Open();
                komut.ExecuteNonQuery();
                Baglanti.Close();

                MessageBox.Show("Müşteri Bilgileri Güncellendi.");
            }
            else
            {
                MessageBox.Show("Kayıt Bulunamamıştır");
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

        private void radioButtonAll_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = radioButton1.Checked || radioButton2.Checked ||
                radioButton3.Checked || radioButton4.Checked ||
                radioButton5.Checked ? true : false;

            textBox1.Text = radioButton1.Checked ?
                "İçecek adı" : "Yiyecek adı";
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

        }

        public void Sil()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;

            textBox1.Text = "Lütfen tür seçiniz";
            textBox1.ForeColor = Color.DimGray;
            textBox1.Enabled = false;

            ActiveControl = null;
        }


    }
}
