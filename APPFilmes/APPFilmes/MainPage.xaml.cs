using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Networking.Connectivity;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace APPFilmes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /* ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~   Alterações a serem feitas(TODO):   ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     * 
    ** 1. Corrigir o lista_ItemClick, usar como exemplo o programa do Banco, em interface Universal(UWP)
    ** 1-1. Olhar no site da API o melhor modelo para realizar a busca dentro dela, levando em conta que você já possui dois dados que podem ser utilizados na busca.
    ** 2. Melhorar os detalhes no design, tendo em conta que a primeira coluna está muito grande, se comparada a segunda.
    ** 3. Corrigir o design do botão de busca, junto com o TextBox, que, ao diminuir o tamanho da tela, os elementos começam a se unir.
    ** 4. Excluir eventos não utilizados.
    ** 5. Tratar erros.
    ** 6. Alterar o design para exibir o poster (pesquisar o elemento e como fazer o binding).
    ** 7. Adicionar o evento ao apertar o enter na busca.
    ** 8. Mudar o "OK" no botão de busca para um icone de lupa (pesquisar sobre).
    ** 9. Limpar tags no xaml;
    ** 10. Aplicar atributos comuns no contexto global do xaml (page)
     * 10: Apesar de ter aplicado os atributos fontsize e foreground no contexto global, ainda há diversos elementos utilizando estes atributos sem necessidade.
     * 11. Limpar tags comuns;
     * 12. Estudar estilos;
     * 13. Pesquisar maneiras para adicionar scroll no "Details"
    ** 14. Corrigir erros de quando o filme não possui algum dado.
    ** 14-1. Criar métodos de acesso a dados numéricos.
    ** 15. Pensar em Favoritos ou outra estrutura pra preencher o listview antes da busca.
     * 16.  Implementar favoritos (método de remover, checar se ja é favorito)
    * ;;;
     * 17. ++Implementar um botão para voltar para a HomePage(apresentando os favoritos).
     * 18. ++Implementar um método para fazer os requests (manda a url por parametro(string) e retorna o response(string))
     * 18-1. ++Try catch checando se há conexão com a internet, ou se a url retornou algum erro.
     * 19. ++Mudar o design, adicionando uma AppBar na parte de baixo do programa (ref. http://developerpublish.com/uwp-tips-tricks-1-appbar-and-default-behavior/)
     * 19-1++Na AppBar deve constar os botões de home, Favorite (ou Remove Favorite, se já cadastrado).
     * 20. ++Exibir ano no ListView.
     * 21. ++Mudar cor da letra dos detalhes para uma mais transparente ou mais escura.(Para facilitar, utilizar o style).
     * ++21: Apenas a parte que não muda deve trocar a cor. Os dados do filme, o "Details" e o titulo do programa devem ficar destacados com uma cor mais clara (preferencialmente o branco).
     * 22-1. ++Melhorar icone do filme (buscar por icones sem cor de fundo).
     * 22. Adicionar um icone de filme nos detalhes, mas apenas exibir quando for um filme.
     * 23. ++Aplicar styles através de ResourceDictionary.
     * 24. ++Limpeza da MainPage (xaml e cs).
     * 25. ++Tamanho do icone do filme pelo xaml.
     * 26. ++Adicionar uma tooltip no icone do filme que exibe o type.
     * 27. ++A checagem se há internet deve ocorrer apenas se der erro na requisição.
     * 28. Se a checagem retornar que não há conexão com internet deve ser criada uma toast informando o usuário que ele não possui conexão.
     * 29. Implementar eventos se o usuário apertar a seta para baixo ou para cima para mudar o filme na lista.
     * 30. ++Encontrar outros itens para serie, jogo
     * 31. ++Placeholder na busca
     * 32. ++Mudar o OkBusca_Click para utilizar apenas uma string para realizar as operações.
     * 33. Pensar em como incluir temporada e episodio
     * 34. Passar pag listview
     * * Adicionar textblock que exibirá, quando possuir, o total e a pagina atual. Ex.:("Pagina 2/20");
        ** Implementar nos eventos para modificar o texto deste textblock.
        ** O textblock não deve aparecer na home.
        * Implementar a busca por season e episode;
        * Crash ao apertar em voltar após fazer a busca.
        * A url continua linkada ao textblock de busca, logo, se eu digitar algo e apertar proximo, ele vai mudar a busca, podendo ocorrer crashs. 
        * Ao voltar pra home e clicar em proxima pagina/pagina anterior ele refaz a busca.
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  CORRIGIR!!!!~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        * Modificar a cor do botão de favoritar/desfavoritar;
        * Identar e limpar o MainPage.xaml.
        * Adição de mais styles no xaml.

        Correções:
     * 
     */
    public sealed partial class MainPage : Page
    {
        int i = 1;
        string data;
        double paginasmax;
        string busca;
        string episode;
        string season;

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            string data = await Favorites.ReadFavorites();
            if (!string.IsNullOrEmpty(data))
                lista.ItemsSource = Favorites.GetFavorites(data);
            StackSearch.Visibility = Visibility.Collapsed;
            SeasonS.Visibility = Visibility.Collapsed;
            EpisodeS.Visibility = Visibility.Collapsed;
            Voltapag.Visibility = Visibility.Collapsed;
            Avpag.Visibility = Visibility.Collapsed;
            lista.SetValue(Grid.RowSpanProperty, 2);
            //lista rowspan 2
        }
        public void Bind(FilmesJson Filme)
        {
            this.DataContext = null;
            this.DataContext = Filme;
            rating.Children.Clear();
            try
            {
                Debug.WriteLine(Filme.Imdbrating);
                float i = (float)Math.Round(Convert.ToDecimal(Filme.Imdbrating)) / 2;
                for (float j = 1; i >= j; i--)
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri("ms-appx:///Assets/star-full-icon.png", UriKind.Absolute));
                    img.Width = 30;
                    img.Height = 30;
                    img.Stretch = Stretch.UniformToFill;
                    rating.Children.Add(img);
                }
                if (i >= 0.5)
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri("ms-appx:///Assets/star-half-icon.png", UriKind.Absolute));
                    img.Width = 15;
                    img.Height = 30;
                    img.Stretch = Stretch.UniformToFill;
                    rating.Children.Add(img);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async void lista_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                FilmesJson filme = (FilmesJson)e.ClickedItem;
                Debug.WriteLine(filme.Title);
                string data = await GetResponseFromUrl(string.Format("http://www.omdbapi.com/?i={0}&plot=full", filme.ImdbId));
                FilmesJson movie = JsonConvert.DeserializeObject<FilmesJson>(data);
                Bind(movie);
                FilmesJson fjson = (FilmesJson)DataContext;
                bool x = await Favorites.IsFavorite(fjson);
                IconFavorito(fjson);
            }
            catch
            {
                bool k = IsInternet();
                if (k == true)
                {

                }
                else
                {
                    NotificacaoNet();

                }
            }
        }
        private void OK_keydownS(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                BotaoBusca_Click(this, new RoutedEventArgs());
            }
        }
        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FilmesJson fjson = (FilmesJson)DataContext;
            bool x = await Favorites.IsFavorite(fjson);
            if (x)
                await Favorites.UnFavorite(fjson);
            else
                await Favorites.AddFavorite(fjson);
            IconFavorito(fjson);
        }
        private async void AppBarButton_Click_Home(object sender, RoutedEventArgs e)
        {
            string data = await Favorites.ReadFavorites();
            if (!string.IsNullOrEmpty(data))
                lista.ItemsSource = Favorites.GetFavorites(data);
            this.DataContext = null;
            rating.Children.Clear();
            IconFilme.Source = null;
            Voltapag.Visibility = Visibility.Collapsed;
            Avpag.Visibility = Visibility.Collapsed;
            lista.SetValue(Grid.RowSpanProperty, 2);
            Pagina.Text = "";
            busca = "";
        }
        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
        public async Task<string> GetResponseFromUrl(string Url)
        {
            var uri = new Uri(Url);
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }
        public void NotificacaoNet()
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[1].AppendChild(toastXml.CreateTextNode("Voce esta offline "));
            String imagePath = "///Assets/Wi-Fi-52.png";
            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
        private async void Voltapag_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (busca.Equals(""))
            {
            }
            else
            {
                if (i <= paginasmax & i > 1)
                {
                    i = i - 1;
                    string data = await GetResponseFromUrl(string.Format("http://www.omdbapi.com/?s={0}&page={1}", busca, i));
                    string raw = JObject.Parse(data).SelectToken("Search").ToString();
                    List<FilmesJson> movieList = JsonConvert.DeserializeObject<List<FilmesJson>>(raw);
                    lista.ItemsSource = movieList;
                    Pagina.Text = i + "/" + paginasmax;
                }
            }
            }
            catch
            {
            }
        }
        private async void Avpag_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (busca.Equals(""))
            {
            }
            else
            {
                if (i < paginasmax)
                {
                    i = i + 1;
                    string data = await GetResponseFromUrl(string.Format("http://www.omdbapi.com/?s={0}&page={1}", busca, i));
                    string raw = JObject.Parse(data).SelectToken("Search").ToString();
                    List<FilmesJson> movieList = JsonConvert.DeserializeObject<List<FilmesJson>>(raw);
                    lista.ItemsSource = movieList;
                    Pagina.Text = i + "/" + paginasmax;
                }
            }
            }
            catch
            {

            }
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
        private async void IconFavorito(FilmesJson filme)
        {
            if (await Favorites.IsFavorite(filme))
            {
                Iconfav.Tag = "";
                Iconfav.Content = "Unfavorite";
            }
            else
            {
                Iconfav.Tag = "";
                Iconfav.Content = "Favorite";
            }
        }
        private async void BotaoBusca_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                i = 1;
                season = SeasonS.Text;
                episode = EpisodeS.Text;
                busca = BuscaS.Text;
                if (season.Equals("") || episode.Equals(""))
                {
                    data = await GetResponseFromUrl(string.Format("http://www.omdbapi.com/?t={0}&plot=full", busca));
                }
                else
                {
                    data = await GetResponseFromUrl(string.Format("http://www.omdbapi.com/?t={0}&Season={1}&Episode={2}", busca, season, episode));
                }
                FilmesJson movie = JsonConvert.DeserializeObject<FilmesJson>(data);
                Bind(movie);
                data = await GetResponseFromUrl(string.Format("http://www.omdbapi.com/?s={0}", busca));
                string raw = JObject.Parse(data).SelectToken("Search").ToString();
                List<FilmesJson> movieList = JsonConvert.DeserializeObject<List<FilmesJson>>(raw);
                lista.ItemsSource = movieList;
                FilmesJson fjson = (FilmesJson)DataContext;
                bool x = await Favorites.IsFavorite(fjson);
                paginasmax = Math.Ceiling(Convert.ToDouble(JObject.Parse(data)["totalResults"].ToString()) / 10);
                Pagina.Text = i + "/" + paginasmax;
                IconFavorito(fjson);
                Voltapag.Visibility = Visibility.Visible;
                Avpag.Visibility = Visibility.Visible;
                lista.SetValue(Grid.RowSpanProperty, 2);
            }
            catch
            {
                bool k = IsInternet();
                if (k == true)
                {
                }
                else
                {
                    NotificacaoNet();
                }
            }

        }
        private void ToggBusca_Click(object sender, RoutedEventArgs e)
        {
            if (StackSearch.Visibility == Visibility.Visible)
            {
                StackSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                StackSearch.Visibility = Visibility.Visible;
                MySplitView.IsPaneOpen = true;
            }
        }
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (SeasonS.Visibility == Visibility.Visible)
            {
                SeasonS.Visibility = Visibility.Collapsed;
                EpisodeS.Visibility = Visibility.Collapsed;
            }
            else
            {
                SeasonS.Visibility = Visibility.Visible;
                EpisodeS.Visibility = Visibility.Visible;
            }
        }
        private void MySplitView_PaneClosed(SplitView sender, object args)
        {
            if (StackSearch.Visibility == Visibility.Visible)
            {
                StackSearch.Visibility = Visibility.Collapsed;
                    SeasonS.Visibility = Visibility.Collapsed;
                EpisodeS.Visibility = Visibility.Collapsed;
            }
        }
    }
}




