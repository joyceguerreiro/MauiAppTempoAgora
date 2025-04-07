using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Net;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica se o campo está vazio
                if (string.IsNullOrWhiteSpace(txt_cidade.Text))
                {
                    lbl_res.Text = "Por favor, preencha o nome da cidade.";
                    return;
                }

                // Verifica conexão com a internet
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("Sem conexão", "Verifique sua conexão com a internet e tente novamente.", "OK");
                    return;
                }

                // Busca os dados da API
                Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                if (t != null)
                {
                    string dados_previsao = $"Descrição do clima: {t.description} \n" +
                                          $"Visibilidade: {t.visibility} \n" +
                                          $"Latitude: {t.lat} \n" +
                                          $"Longitude: {t.lon} \n" +
                                          $"Velocidade do vento: {t.speed} \n" +
                                          $"Nascer do Sol: {t.sunrise} \n" +
                                          $"Por do Sol: {t.sunset} \n" +
                                          $"Temp Máx: {t.temp_max} \n" +
                                          $"Temp Min: {t.temp_min} \n";

                    lbl_res.Text = dados_previsao;
                }
                else
                {
                    lbl_res.Text = "Sem dados de previsão para esta cidade.";
                }
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == HttpStatusCode.NotFound)
            {
                await DisplayAlert("Cidade não encontrada", "Não foi possível encontrar informações para a cidade informada.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
            }
        }
    }
}