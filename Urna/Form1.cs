using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Urna
{
    public partial class Form1 : Form
    {
        private string c_voto = "";
        private int etapa = 1;

        private Dictionary<string, Tuple<string, string, string>> c_prefeitos;
        private Dictionary<string, Tuple<string, string, string, string>> c_presidente;

        private Label nome_label;
        private Label nome_partido;
        private Label nome_vice;
        private PictureBox picbox_foto;

        public Form1(){
            init_comp();
            init_candidatos();
            init_iface();
            update_cargo();
        }

        private void init_candidatos(){
            c_prefeitos = new Dictionary<string, Tuple<string, string, string>>(){
                { "45", Tuple.Create("PSDB", "Rodrigo Martins", "45") },
                { "50", Tuple.Create("PSOL", "Felipe Antunes", "50") },
                { "40", Tuple.Create("PSB", "Marcelo Diniz", "40") },
                { "10", Tuple.Create("Republicanos", "César Rocha", "10") },
                { "13", Tuple.Create("PT", "Jorge Vieira", "13") }
            };
            c_presidente = new Dictionary<string, Tuple<string, string, string, string>>(){
                { "13", Tuple.Create("PT", "Antônio Ferreira", "13", "Roberto Amaral") },
                { "22", Tuple.Create("PL", "Renato Barros", "22", "Álvaro Souza") },
                { "15", Tuple.Create("MDB", "Camila Teixeira", "15", "Juliana Lopes") },
                { "12", Tuple.Create("PDT", "Rogério Farias", "12", "Patrícia Maia") },
                { "30", Tuple.Create("NOVO", "Eduardo Martins", "30", "Henrique Vidal") }
            };
        }
        private void init_iface(){
            nome_label = new Label();
            nome_label.Location = new Point(300, 80);
            nome_label.Size = new Size(300, 20);
            nome_label.Text = "Nome:";
            this.Controls.Add(nome_label);
            nome_partido = new Label();
            nome_partido.Location = new Point(300, 110);
            nome_partido.Size = new Size(300, 20);
            nome_partido.Text = "Partido:";
            this.Controls.Add(nome_partido);
            nome_vice = new Label();
            nome_vice.Location = new Point(300, 140);
            nome_vice.Size = new Size(300, 20);
            nome_vice.Text = "Vice:";
            this.Controls.Add(nome_vice);
            picbox_foto = new PictureBox();
            picbox_foto.Location = new Point(300, 170);
            picbox_foto.Size = new Size(100, 120);
            picbox_foto.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(picbox_foto);
            Button buttonVerCandidatos = new Button();
            buttonVerCandidatos.Text = "Ver Candidatos";
            buttonVerCandidatos.Location = new Point(300, 300);
            buttonVerCandidatos.Size = new Size(150, 40);
            buttonVerCandidatos.Click += buttonVerCandidatos_Click;
            this.Controls.Add(buttonVerCandidatos);
            buttonVerCandidatos.BringToFront();
        }
        private void update_cargo(){
            update_pcbox2("", "", "", "", false);
        }
        private void update_tela(){
            textBoxDisplay.Text = c_voto;

            if (c_voto.Length == 2)
                update_info_candidato();
            else
                limpa_info();
        }
        private void update_info_candidato(){
            if (etapa == 1) {
                if (c_prefeitos.ContainsKey(c_voto)){
                    var dados = c_prefeitos[c_voto];
                    nome_label.Text = "Nome: " + dados.Item2;
                    nome_partido.Text = "Partido: " + dados.Item1;
                    nome_vice.Text = "Vice: N/A";
                    load_foto(dados.Item3, "prefeito");
                    update_pcbox2(dados.Item2, dados.Item1, dados.Item3, "", true);
                }
                else{
                    nome_label.Text = "Nome: Número inválido";
                    nome_partido.Text = "Partido:";
                    nome_vice.Text = "Vice:";
                    picbox_foto.Image = null;
                    update_pcbox2("", "", "", "", false);
                }
            }
            else{
                if (c_presidente.ContainsKey(c_voto)){
                    var dados = c_presidente[c_voto];
                    nome_label.Text = "Nome: " + dados.Item2;
                    nome_partido.Text = "Partido: " + dados.Item1;
                    nome_vice.Text = "Vice: " + dados.Item4;
                    load_foto(dados.Item3, "presidente");
                    update_pcbox2(dados.Item2, dados.Item1, dados.Item3, dados.Item4, true);
                }
                else{
                    nome_label.Text = "Nome: Número inválido";
                    nome_partido.Text = "Partido:";
                    nome_vice.Text = "Vice:";
                    picbox_foto.Image = null;
                    update_pcbox2("", "", "", "", false); // todo: ver se não conflita com x func
                }
            }
        }
        private void load_foto(string numero, string tipoCargo){
            try{
                string caminho = $"{numero}_{tipoCargo}.jpg";
                picbox_foto.Image = Image.FromFile(caminho);
            }
            catch{
                picbox_foto.Image = null;
            }
        }
        private void limpa_info(){
            nome_label.Text = "Nome:";
            nome_partido.Text = "Partido:";
            nome_vice.Text = "Vice:";
            picbox_foto.Image = null;

            update_pcbox2("", "", "", "", false);
        }
        private void update_pcbox2(string nome, string partido, string numero, string vice, bool candidato_valido){
            Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            using (Graphics g = Graphics.FromImage(bmp)) {
                g.Clear(Color.FromArgb(224, 224, 224));

                Font fonteTitulo = new Font("Arial", 20, FontStyle.Bold);
                Brush pincelTextoTitulo = Brushes.Black;
                string cargo = etapa == 1 ? "PREFEITO" : "PRESIDENTE";

                SizeF tamanhoTexto = g.MeasureString(cargo, fonteTitulo);
                PointF pontoCentralizado = new PointF((pictureBox2.Width - tamanhoTexto.Width) / 2, 10);
                g.DrawString(cargo, fonteTitulo, pincelTextoTitulo, pontoCentralizado);

                if (candidato_valido){
                    Font fonte = new Font("Arial", 14, FontStyle.Bold);
                    Brush pincelTexto = Brushes.Black;
                    g.DrawString("Nome: " + nome, fonte, pincelTexto, new PointF(10, 50));
                    g.DrawString("Partido: " + partido, fonte, pincelTexto, new PointF(10, 80));
                    g.DrawString("Número: " + numero, fonte, pincelTexto, new PointF(10, 110));
                    if (etapa == 2 && !string.IsNullOrEmpty(vice)){
                        g.DrawString("Vice: " + vice, fonte, pincelTexto, new PointF(10, 140));
                    }
                    try{
                        string tipoCargo = etapa == 1 ? "prefeito" : "presidente";
                        string caminhoImagem = $"{numero}_{tipoCargo}.jpg";
                        Image imagem = Image.FromFile(caminhoImagem);
                        g.DrawImage(imagem, new Rectangle(450, 10, 150, 200));
                    }
                    catch{
                        g.DrawString("Foto não encontrada", fonte, Brushes.Red, new PointF(400, 10));
                    }
                }
                else if (c_voto.Length == 2){
                    Font fonte = new Font("Arial", 14, FontStyle.Bold);
                    g.DrawString("Número inválido", fonte, Brushes.Red, new PointF(10, 50));
                }
            }
            pictureBox2.Image = bmp;
        }
        private void numeroButton_Click(object sender, EventArgs e){
            if (c_voto.Length < 2){
                c_voto += ((Button)sender).Text;
                update_tela();
            }
        }
        private void buttonCorrige_Click(object sender, EventArgs e){
            c_voto = "";
            update_tela();
        }
        private void buttonConfirma_Click(object sender, EventArgs e){
            if (c_voto == ""){
                MessageBox.Show("Nenhum número digitado.");
                return;
            }

            if (c_voto == "BRANCO"){
                MessageBox.Show("Voto em branco confirmado para " + (etapa == 1 ? "Prefeito" : "Presidente"));
            }
            else{
                bool valido = etapa == 1 ? c_prefeitos.ContainsKey(c_voto) : c_presidente.ContainsKey(c_voto);

                if (valido){
                    string nome = etapa == 1
                        ? c_prefeitos[c_voto].Item2
                        : c_presidente[c_voto].Item2;

                    MessageBox.Show("Voto confirmado para " + nome + " - " + (etapa == 1 ? "Prefeito" : "Presidente"));
                }
                else{
                    MessageBox.Show("Número inválido. Candidato inexistente.");
                    return;
                }
            }

            c_voto = "";

            if (etapa == 1){
                etapa = 2;
                update_cargo();
                limpa_info();
                textBoxDisplay.Text = "";
                pictureBox2.Image = null;
                update_pcbox2("", "", "", "", false);
            }
            else{
                MessageBox.Show("Fim da votação. Obrigado por votar!");
                Application.Exit();
            }
        }
        private void buttonBranco_Click(object sender, EventArgs e){
            c_voto = "BRANCO";
            textBoxDisplay.Text = c_voto;
            limpa_info();
            pictureBox2.Image = null;
            update_cargo();
        }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void buttonVerCandidatos_Click(object sender, EventArgs e){
            string mensagem = "Candidatos a Prefeito:\n";
            foreach (var candidato in c_prefeitos){
                mensagem += $"{candidato.Value.Item2} ({candidato.Value.Item1}) - Número: {candidato.Key}\n";
            }

            mensagem += "\nCandidatos a Presidente:\n";
            foreach (var candidato in c_presidente){
                mensagem += $"{candidato.Value.Item2} ({candidato.Value.Item1}) - Número: {candidato.Key} - Vice: {candidato.Value.Item4}\n";
            }
            MessageBox.Show(mensagem, "Lista de Candidatos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
