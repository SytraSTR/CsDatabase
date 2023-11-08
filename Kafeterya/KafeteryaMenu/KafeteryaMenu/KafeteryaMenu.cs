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
    public partial class KafeteryaMenu : Form
    {
        public KafeteryaMenu()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Yenile();
        }

        int Mouse_Move;
        int Mouse_X;
        int Mouse_Y;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse_Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Mouse_Move = 0;
        }

        static string ConString = "Data Source=DESKTOP-9578CDS\\SYTRASANDRO;Initial Catalog=CafeteriaDB;Integrated Security=True";
        SqlConnection Connect = new SqlConnection(ConString);

        SqlDataAdapter Da;
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
                        "Select Count(*) From " +
                        "CafeteriaMenu Where FoodName = @fName";
                    SqlCommand CmdSorgu = new SqlCommand(Sorgu, Connect);
                    CmdSorgu.Parameters.Add("@fName",
                        SqlDbType.VarChar).Value = fName;

                    Connect.Open();
                    int SorguSonucu = (int)CmdSorgu.ExecuteScalar();
                    Connect.Close();

                    if (SorguSonucu == 0)
                    {
                        string Kayit =
                            "Insert Into " +
                            "CafeteriaMenu (FoodIncentives, FoodName) " +
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
                        string Read = "Select * From CafeteryaMenu " +
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
            string fName1 = "";
            fInc = radioButton1.Checked ? radioButton1.Text :
                      radioButton2.Checked ? radioButton2.Text :
                      radioButton3.Checked ? radioButton3.Text :
                      radioButton4.Checked ? radioButton4.Text :
                      radioButton5.Checked ? radioButton5.Text : "";

            fName1 = textBox1.Text;

            if ((radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked || radioButton5.Checked) &&
                (dataGridView1.SelectedRows.Count == 1 || dataGridView1.Rows.Count == 1))
            {
                string Update = "Update CafeteriaMenu Set FoodIncentives = @fInc,  FoodName = @fName1 " +
                    "Where FoodName = @fName";
                SqlCommand CmdGuncelle = new SqlCommand(Update, Connect);

                CmdGuncelle.Parameters.AddWithValue("@fInc", fInc);

                if (dataGridView1.SelectedRows.Count == 1)
                {
                    if (MessageBox.Show(dataGridView1.SelectedRows[0].Cells[0].Value + " | " +
                        dataGridView1.SelectedRows[0].Cells[1].Value +
                        " Düzenlemek istediğinize emin misiniz?".ToString(),
                        "İşlem onayı", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        CmdGuncelle.Parameters.AddWithValue("@fName", dataGridView1.SelectedRows[0].Cells[1].Value);

                        if (textBox1.Text != "" && textBox1.Text != "Lütfen tür seçiniz" && textBox1.Text != "Yiyecek adı" && textBox1.Text != "İçecek adı")
                        {
                            CmdGuncelle.Parameters.AddWithValue("@fName1", fName1);
                        }
                        else
                        {
                            CmdGuncelle.Parameters.AddWithValue("@fName1", dataGridView1.SelectedRows[0].Cells[1].Value);
                        }

                        Connect.Open();
                        int EtkilenenSatirSayisi = CmdGuncelle.ExecuteNonQuery();
                        Connect.Close();
                        if (EtkilenenSatirSayisi > 0)
                        {
                            MessageBox.Show(dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + " Düzenlenmiştir.");

                            Sil();
                        }
                        else
                        {
                            MessageBox.Show("Düzenleme işlemi iptal edilmiştir.");
                        }
                    }
                    else
                    {
                        MessageBox.Show(dataGridView1.SelectedRows[0].Cells[1].Value.ToString() +
                            " Düzenlenemedi, Belirtilen kayıt bulunamadı veya bir hata oluştu.");
                    }
                }
                else
                {
                    if (MessageBox.Show(dataGridView1.Rows[0].Cells[0].Value + " | " +
                        dataGridView1.Rows[0].Cells[1].Value +
                        " Düzenlemek istediğinize emin misiniz?".ToString(),
                        "İşlem onayı", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        CmdGuncelle.Parameters.AddWithValue("@fName", dataGridView1.Rows[0].Cells[1].Value);

                        if (textBox1.Text != "" && textBox1.Text != "Lütfen tür seçiniz" && textBox1.Text != "Yiyecek adı" && textBox1.Text != "İçecek adı")
                        {
                            CmdGuncelle.Parameters.AddWithValue("@fName1", fName1);
                        }
                        else
                        {
                            CmdGuncelle.Parameters.AddWithValue("@fName1", dataGridView1.Rows[0].Cells[1].Value);
                        }

                        Connect.Open();
                        int EtkilenenSatirSayisi = CmdGuncelle.ExecuteNonQuery();
                        Connect.Close();
                        if (EtkilenenSatirSayisi > 0)
                        {
                            MessageBox.Show(dataGridView1.Rows[0].Cells[1].Value.ToString() + " Düzenlenmiştir.");

                            Sil();
                        }
                        else
                        {
                            MessageBox.Show("Düzenleme işlemi iptal edilmiştir.");
                        }
                    }
                    else
                    {
                        MessageBox.Show(dataGridView1.Rows[0].Cells[1].Value.ToString() +
                            " Düzenlenemedi, Belirtilen kayıt bulunamadı veya bir hata oluştu.");
                    }
                }
            }
            else if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked && !radioButton4.Checked && !radioButton5.Checked)
            {
                MessageBox.Show("Lütfen tür seçiniz");
            }
            else if (dataGridView1.Rows.Count > 1)
            {
                MessageBox.Show("Birden fazla satır gözüküyor, Bu nedenle düzenleme işlemi iptal edilmiştir.");
            }
            else if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Satır Bulunamamıştır.");
            }
            else if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seçili Satır Bulunamamıştır.");
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

            if (dataGridView1.Rows.Count == 1 || dataGridView1.SelectedRows.Count == 1)
            {
                string Delete = "Delete From CafeteriaMenu " +
                    "Where FoodName = @fName";

                SqlCommand CmdDelete = new SqlCommand(Delete, Connect);


                if (dataGridView1.SelectedRows.Count == 1)
                {
                    if (MessageBox.Show(dataGridView1.SelectedRows[0].Cells[0].Value + " | " +
                        dataGridView1.SelectedRows[0].Cells[1].Value +
                        " Silmek istediğinize emin misiniz?".ToString(),
                        "İşlem onayı", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fName = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                        CmdDelete.Parameters.AddWithValue("@fName",
                                fName);

                        Connect.Open();
                        int EtkilenenSatirSayisi = CmdDelete.ExecuteNonQuery();
                        Connect.Close();

                        if (EtkilenenSatirSayisi > 0)
                        {
                            MessageBox.Show(dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + " Silinmiştir.");

                            Sil();
                        }
                        else
                        {
                            MessageBox.Show("Silme işlemi iptal edilmiştir.");
                        }
                    }
                    else
                    {
                        MessageBox.Show(dataGridView1.SelectedRows[0].Cells[1].Value.ToString() +
                            " Silinemedi, Belirtilen kayıt bulunamadı veya bir hata oluştu.");
                    }
                }
                else if (MessageBox.Show(dataGridView1.Rows[0].Cells[0].Value + " | " +
                    dataGridView1.Rows[0].Cells[1].Value +
                    " Silmek istediğinize emin misiniz?".ToString(),
                    "İşlem onayı", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fName = dataGridView1.Rows[0].Cells[1].Value.ToString();
                    CmdDelete.Parameters.AddWithValue("@fName",
                    fName);

                    Connect.Open();
                    int EtkilenenSatirSayisi = CmdDelete.ExecuteNonQuery();
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
            else if (dataGridView1.Rows.Count > 1)
            {
                MessageBox.Show("Birden fazla satır gözüküyor, Bu nedenle silme işlemi iptal edilmiştir.");
            }
            else if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Satır Bulunamamıştır.");
            }
            else if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seçili Satır Bulunamamıştır.");
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

            dataGridView1.ClearSelection();
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
            Da = new SqlDataAdapter("Select * From CafeteriaMenu", Connect);
            Connect.Open();
            Dt = new DataTable();
            Da.Fill(Dt);
            dataGridView1.DataSource = Dt;
            Connect.Close();

            dataGridView1.ClearSelection();
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
