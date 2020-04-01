using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIE_ABConverter
{
    public partial class ABConverter : Form
    {
        public ABConverter()
        {
            this.InitializeComponent();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtTo.Text);
        }

        string table = "fZodR9XQDSUm21yCkr6zBqiveYah8bt4xsWpHnJE7jL5VG3guMTKNPAwcF";
        int[] tr = new int[255];
        int[] s = {11,10,3,8,4,6};
        int xor = 177451812;
        BigInteger add = 8728348608;
        Regex regexAV = new Regex("^([aA][vV]){0,1}([0-9]+)$");
        Regex regexBV = new Regex("^([bB][vV]){0,1}([fZodR9XQDSUm21yCkr6zBqiveYah8bt4xsWpHnJE7jL5VG3guMTKNPAwcF]{10})$");
        private void txtFrom_TextChanged(object sender, EventArgs e)
        {
            txtTo.BackColor = Color.FromKnownColor(KnownColor.Window);
            if (txtFrom.Text == "")
            {
                txtTo.Text = "";
                return;
            }
            if (regexAV.IsMatch(txtFrom.Text))
            {
                try
                {
                    txtTo.Text = this.AV2BV(BigInteger.Parse(regexAV.Match(txtFrom.Text).Groups[2].Value.ToString()));
                }
                catch
                {
                    txtTo.Text = "转换失败，请检查输入数据！";
                    txtTo.BackColor = Color.Red;
                }
            }
            else if (regexBV.IsMatch(txtFrom.Text))
            {
                try
                {
                    txtTo.Text = this.BV2AV(txtFrom.Text);
                }
                catch
                {
                    txtTo.Text = "转换失败，请检查输入数据！";
                    txtTo.BackColor = Color.Red;
                }
            }
            else
            {
                txtTo.Text = "输入格式不正确，请检查！";
                txtTo.BackColor = Color.Red;
            }
        }
        string BV2AV(string x)
        {
            BigInteger r = 0;
            for (var i = 0; i < 6; i++)
            {
                r += tr[x[s[i]]] * BigInteger.Pow(58, i);
            }
            return "av" + ((r - add) ^ BigInteger.Parse(xor.ToString())).ToString();
        }

        string AV2BV(BigInteger x)
        {
            x = (x ^ xor) + add;
            string r = "BV1  4 1 7  ";
            List<char> rr = r.ToList();
            for (int i = 0; i < 6; i++)
            {
                rr[s[i]] = table[(int)(x / BigInteger.Pow(58, i) % 58)];
            }
            string result = "";
            for (int i = 0; i < rr.Count; i++)
            {
                result += rr[i];
            }
            return result;
        }

        private void ABConverter_Load(object sender, EventArgs e)
        {
            for (var i = 0; i < table.Length; i++)
            {
                tr[table[i]] = i;
            }
        }

        private void txtTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                txtTo.SelectAll();
            }
        }
    }
}
